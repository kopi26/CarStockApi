# Car Stock API

A C# Web API for dealers to manage their car stock.

## Technologies

* C#
* ASP.NET Core
* FastEndpoints
* Dapper
* SQLite
* JWT Authentication
* BCrypt

## How to Run

1. Clone the repository.
2. Open the `CarStockApi` project in Visual Studio.
3. Restore the NuGet packages.
4. Build the project.
5. Run the application.

The application uses a local SQLite database (`carstock.db`). The database schema is automatically initialised when the application starts, so no manual database or table creation is required.

## Test Login Accounts

| Username | Password    |
| -------- | ----------- |
| dealer1  | password123 |
| dealer2  | password456 |

## API Endpoints

| Method | Endpoint                                 | Description                 |
| ------ | ---------------------------------------- | --------------------------- |
| POST   | `/auth/login`                            | Login and receive JWT token |
| POST   | `/cars`                                  | Add a car                   |
| GET    | `/cars`                                  | List cars                   |
| GET    | `/cars/{id}`                             | Get a car                   |
| GET    | `/cars/search?make={make}&model={model}` | Search by make and model    |
| PUT    | `/cars/{id}/stock`                       | Update car stock            |
| DELETE | `/cars/{id}`                             | Delete a car                |

## Authentication

The car endpoints require JWT authentication.

After logging in, use the returned token in the request header:

`Authorization: Bearer YOUR_TOKEN`

Each dealer can only access and modify their own cars and stock. Dealer ownership is determined from the authenticated JWT token rather than from the API request.

## Validation and Error Handling

The API validates request data such as:

* Required fields
* Car year
* Non-negative stock
* Required search parameters

The API also includes global error handling for unexpected errors.

## Database

The application uses SQLite with Dapper and SQL queries.

The database schema is automatically initialised when the application starts using the SQL script in `Data/schema.sql`.

The submitted repository also includes `carstock.db` with the tested database data.

The database contains:

* Dealers
* Cars

## Project Structure

```text
CarStockApi
├── Data
├── Endpoints
├── Models
├── Properties
├── Program.cs
├── appsettings.json
├── CarStockApi.csproj
├── carstock.db
├── README.md
└── .gitignore
```
