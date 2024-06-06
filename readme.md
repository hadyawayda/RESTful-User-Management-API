# Description

This application follows an MVP architecture and contains a REFTful API with authentication using JWT tokens.
It contains two primary functionalities: a User Controller which handles incoming HTTP requests, and an Authentication controller which returns a JWT token upon successful login.
Passwords are hashed using BCrypt and stored as a hash string in the database.
Expired JWT tokens are handled, as well as invalid tokens, by a middleware (JwtExceptionHandler). Token expiry set to 60 minutes, value can be changed inside appsettings.json

# Installation

To run this project locally, install Visual Studio and .Net runtime, and postgresQL. Create a new database called "UsersDb" and create a new table using this query:

`CREATE TABLE Users (
	id SERIAL PRIMARY KEY,
	username VARCHAR(256) UNIQUE NOT NULL,
	email VARCHAR(256) UNIQUE NOT NULL,
	hash TEXT NOT NULL,
	created TIMESTAMP WITH TIME ZONE NOT NULL,
	updated TIMESTAMP WITH TIME ZONE NOT NULL);`

Next, launch the "Dynamic Eye.sln" solution, then modify the connection string found in appsettings.json to include your postgres username and password, and install the required dependencies using nuget package manager:

`BCrypt.Net-Next`
`Microsoft.AspNetCore.Authentication.JwtBearer`
`Microsoft.EntityFrameworkCore`
`Npgsql.EntityFrameworkCore.PostgreSQL`
`Swashbuckle.AspNetCore`
`Swashbuckle.AspNetCore.Filters`

Finally run the project by clicking on the green arrow at the top of Visual Studio. Swagger will launch in the browser, and will allow testing of every API Endpoint.

# API Documentation

## Auth

Endpoint: POST auth/login
Body:
email: The email address of the user.
password: The password used for authentication. It is hashed using BCrypt and compared to the hash found in the database.
Description: Returns a JWT token upon successful login.
Responses:
200 OK: Successfully logged in. Returns an object containing the JWT token.
401 Unauthorized: Invalid email or password.

## Get Users (requires authentication)

Endpoint: GET /users
Query Parameters:
id (optional): The unique identifier of the user.
username (optional): The username of the user.
email (optional): The email address of the user.
Description: Fetches users from the database.
You can filter the results by supplying an id, username, or email as query parameters.
If no parameters are provided, it returns all users.
Responses:
200 OK: Successfully retrieved users. Returns a list of users.
404 Not Found: No users found matching the criteria.

## Create User

Endpoint: POST /users/register
Body:
username: The username of the new user.
email: The email address of the new user.
password: The password of the new user.
Description: Creates a new user with the provided details. created and updated timestamps are set automatically.
Responses:
200 OK: User successfully created. Returns a the created user object, while removing the password.
400 Bad Request: A user with the given username or email already exists.

## Update User (requires authentication)

Endpoint: PUT /users/update
Body:
username: The new username of the user.
email: The new email address of the user.
password: The password of the new user.
Description: Updates the details of an existing user identified by id.
Responses:
200 OK: User successfully updated. Returns a message.
404 Not Found: User not found.
500 Internal Server Error: Error updating user.

## Delete User (requires authentication)

Endpoint: DELETE /users/delete
Description: Deletes the user identified by id from the database.
Responses:
200 OK: User successfully deleted. Returns a message.
404 Not Found: User not found.
