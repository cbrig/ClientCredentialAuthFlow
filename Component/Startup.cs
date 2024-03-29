﻿using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component.Security;
using Microsoft.AspNetCore.Authorization;

namespace Component
{
    public class Startup
    {
        private static readonly string _authority = "";
        private static readonly string _wellKnownConfiguration = $"{_authority}/.well-known/openid-configuration";
        private static readonly string _securityType = "oauth2";

        public Startup(IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(_authority)) throw new Exception("StartUp - Authority is empty");
            if (string.IsNullOrEmpty(_wellKnownConfiguration)) throw new Exception("StartUp - Well known configuration URL is empty");
            if (string.IsNullOrEmpty(_securityType)) throw new Exception("StartUp - Security Type is empty");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(_wellKnownConfiguration, new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever());
            var discoveryDocument = configurationManager.GetConfigurationAsync().Result;
            var signingKeys = discoveryDocument.SigningKeys;

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = _authority,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeys = signingKeys,
                        RequireSignedTokens = true
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("read", policy => policy.Requirements.Add(new HasScopeRequirement("API/read", _authority)));
                    options.AddPolicy("write", policy => policy.Requirements.Add(new HasScopeRequirement("API/write", _authority)));
                    options.AddPolicy("delete", policy => policy.Requirements.Add(new HasScopeRequirement("API/delete", _authority)));
                });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                options.AddSecurityDefinition(_securityType, new OAuth2Scheme
                {
                    Type = _securityType,
                    Flow = "application",
                    TokenUrl = _authority
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { _securityType, Enumerable.Empty<string>() }
                });
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}