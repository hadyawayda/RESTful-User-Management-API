# Installation

To run this project locally, install Visual Studio and .Net runtime, and postgresQL. Create a new database and create a new table using this query:

`CREATE TABLE Users (
	id SERIAL PRIMARY KEY,
	username VARCHAR(256) UNIQUE NOT NULL,
	email VARCHAR(256) UNIQUE NOT NULL,
	hash TEXT NOT NULL,
	created TIMESTAMP WITH TIME ZONE NOT NULL,
	updated TIMESTAMP WITH TIME ZONE NOT NULL);`

Next, launch the "Dynamic Eye.sln" solution, then modify the connection string found in appsettings.json to include your username and password, and install the required dependencies using nuget package manager:
Microsoft.EntityFrameworkCore
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.AspNetCore.Authentication.JwtBearer

Finally run the project by clicking on the green arrow at the top of Visual Studio. Swagger will launch in the browser, and will allow testing of every API Endpoint.

# API Documentation

## Get Users

Endpoint: GET /users
Query Parameters:
id (optional): The unique identifier of the user.
username (optional): The username of the user.
email (optional): The email address of the user.
Description: Fetches users from the database. You can filter the results by supplying an id, username, or email. If no parameters are provided, it returns all users.
Responses:
200 OK: Successfully retrieved users. Returns a list of users.
404 Not Found: No users found matching the criteria.

## Create User

Endpoint: POST /users
Body:
username: The username of the new user.
email: The email address of the new user.
hash: The password hash of the new user.
Description: Creates a new user with the provided details. created and updated timestamps are set automatically.
Responses:
200 OK: User successfully created. Returns a message.
400 Bad Request: A user with the given username or email already exists.

## Update User

Endpoint: PUT /users/{id}
Path Parameter:
id: The unique identifier of the user to update.
Body:
username: (Optional) The new username of the user.
email: (Optional) The new email address of the user.
hash: (Optional) The new password hash of the user.
Description: Updates the details of an existing user identified by id.
Responses:
200 OK: User successfully updated. Returns a message.
404 Not Found: User not found.
500 Internal Server Error: Error updating user.

## Delete User

Endpoint: DELETE /users/{id}
Path Parameter:
id: The unique identifier of the user to delete.
Description: Deletes the user identified by id.
Responses:
200 OK: User successfully deleted. Returns a message.
404 Not Found: User not found.

# TODO:

### implement unit testing (maybe in swagger?)

### validate inputs...

### add rate limiting maybe?
