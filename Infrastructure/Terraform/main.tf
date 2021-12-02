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

resource "aws_dynamodb_table" "booking_table" {
    name            = "booking"
    billing_mode    = "PAY_PER_REQUEST"
	stream_enabled   = true
	stream_view_type = "NEW_AND_OLD_IMAGES"
    hash_key        = "PartitionKey"
    range_key	    = "Id"

    attribute {
        name = "PartitionKey"
        type = "S"
    }

    attribute {
        name = "Id"
        type = "S"
    }

    attribute {
        name = "Data"
        type = "S"
    }

    global_secondary_index {
        name               = "DataIndex"
		hash_key           = "PartitionKey"
        range_key          = "Data"
		projection_type	   = "ALL"
    }

    point_in_time_recovery {
        enabled = true
    }
