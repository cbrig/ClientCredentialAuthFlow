provider "aws" {
  region = "${var.aws_region}"
  version = "~> 2.2"
}

terraform {
required_version = ">= 0.11.13"
}

locals {
	endpoint = "https://${aws_cognito_user_pool_domain.main.domain}.auth.${var.aws_region}.amazoncognito.com/oauth2/token"
	resource_end_point = "https://cognito-idp.${var.aws_region}.amazonaws.com/${aws_cognito_user_pool.pool.id}"
  	common_tags = {
		Created-by = "Terraform"
		Contact = "${var.contact_tag}"
		ApplicationName = "${var.application_name}"
	}
}