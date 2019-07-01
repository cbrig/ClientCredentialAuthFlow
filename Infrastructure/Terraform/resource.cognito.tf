resource "aws_cognito_user_pool" "pool" {
name = "${var.user_pool_name}"
}

resource "aws_cognito_resource_server" "Component" {
identifier = "${var.resource_server_id}"
name = "${var.resource_server_name}"
user_pool_id = "${aws_cognito_user_pool.pool.id}"

scope {
scope_name = "read"
scope_description = "read-${var.resource_server_name}"
}

scope {
scope_name = "write"
scope_description = "write-${var.resource_server_name}"
}

scope {
scope_name = "delete"
scope_description = "delete-${var.resource_server_name}"
}
}

resource "aws_cognito_user_pool_client" "BFF" {
name = "${var.resource_server_name}"
user_pool_id = "${aws_cognito_user_pool.pool.id}"
allowed_oauth_flows = ["client_credentials"] 
generate_secret = true 
allowed_oauth_scopes = ["API/read", "API/write"]
allowed_oauth_flows_user_pool_client = true 
}

resource "aws_cognito_user_pool_domain" "main" {
domain = "${var.user_pool_domain}"
user_pool_id = "${aws_cognito_user_pool.pool.id}"
}
