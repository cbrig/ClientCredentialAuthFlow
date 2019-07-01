output "bff_token_address" {
value = "${local.endpoint}"
}

output "bff_client_id" {
	value = "${aws_cognito_user_pool_client.BFF.id}"
}

output "bff_client_secret" {
	value = "${aws_cognito_user_pool_client.BFF.client_secret}"
}

output "component_authority" {
value = "${local.endpoint}"
}