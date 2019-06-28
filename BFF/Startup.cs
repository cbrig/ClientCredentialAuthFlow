using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net.Http;

namespace BFF
{
    public class Startup
    {
        private readonly string _tokenAddress = "https://chrisbriggstest.auth.eu-central-1.amazoncognito.com/oauth2/token";
        private readonly string _clientId = "1kb7f149f05v5dpd1q1jt7j2q0";
        private readonly string _clientSecret = "1v9l0h33c1c2muroh74jghrssq5ll766h6he9snrlbj2merm0uig";
        private readonly Uri _componentUrl = new Uri("https://localhost:5003");

        public Startup(IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(_tokenAddress)) throw new Exception("StartUp - Token address is empty");
            if (string.IsNullOrEmpty(_clientId)) throw new Exception("StartUp - ClientId is empty");
            if (string.IsNullOrEmpty(_clientSecret)) throw new Exception("StartUp - Client Secret is empty");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IComponentService, ComponentService>(client =>
            {
                var tokenResponse = new HttpClient().RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _tokenAddress,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret
                }).Result;

                client.BaseAddress = _componentUrl;
                client.SetBearerToken(tokenResponse.AccessToken);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}