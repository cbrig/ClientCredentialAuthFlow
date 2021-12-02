variable "aws_region" {
  description = "The AWS region to deploy to (e.g. eu-central-1)"
  default = "eu-central-1"
}

variable "contact_tag" {
  description = "The name of the person as a contact."
}

variable "application_name" {
  description = "The name of the application."
  default = "ClientCredentialsExample"
}


variable "user_pool_name" {
  description = "The name of the user pool all the resources will use."
  default = "ClientCredentialsExample"
}

variable "resource_server_name" {
  description = "The name of component with the data."
  default = "Component1"
}

variable "resource_server_id" {
  description = "The name of component with the data."
  default = "API"
}

variable "user_pool_client" {
  description = "The name of the client call the component. In this Case it is the BFF."
  default = "BFF1"
}

variable "user_pool_domain" {
  description = "The unique domain used to get tokens. This will have to be available in AWS."
}
