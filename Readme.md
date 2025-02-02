# Coffee Bean API

A .NET Core Web API for managing coffee beans, featuring a "Bean of the Day" functionality and comprehensive bean management capabilities.

## Technology Choices

### Backend Framework

- **.NET 8.0**: Latest LTS version offering improved performance and modern features
- **ASP.NET Core Web API**: Robust framework for building HTTP services
- **Entity Framework Core**: ORM for database operations with code-first approach

### Database

- **SQL Server**: Reliable relational database with excellent Entity Framework Core support
- **In-Memory Database (for testing)**: Enables fast, isolated testing without external dependencies

### Testing

- **xUnit**: Modern testing framework for .NET
- **Entity Framework Core InMemory**: For database testing without actual database dependencies
- **Moq**: Mocking framework for unit tests

### Development Tools

- **Swagger/OpenAPI**: Automatic API documentation and testing interface
- **Entity Framework Migrations**: Database version control and schema management

## Project Structure

The solution consists of two main projects:

1. `CoffeeBeanAPI`: Main API project containing controllers, models, and data access
2. `CoffeeBeanAPI.Tests`: Test project with unit tests for controllers

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore dependencies/Build Solution
4. Update database by running the below in the package manager console

```
update-database
```

### Running the Application

1. Start the API using Visual Studio
2. Access Swagger UI and following Swagger docs

### Running Tests

- Run all tests from the tests tab in Visual Studio

## API Endpoints

### Beans Controller

- `GET /api/beans`: Get all beans
- `GET /api/beans/{id}`: Get specific bean
- `POST /api/beans`: Create new bean
- `PUT /api/beans/{id}`: Update bean
- `DELETE /api/beans/{id}`: Delete bean
- `GET /api/beans/search`: Search beans with filters

### Bean of the Day Controller

- `GET /api/beanoftheday`: Get today's featured bean

## Database Seeding

The application includes automatic database seeding with sample coffee bean data. The seeder runs automatically in development mode when the application starts.

## Testing Strategy

The test project includes comprehensive unit tests for both controllers:

- Bean controller tests cover CRUD operations and search functionality
- Bean of the Day controller tests verify daily selection logic and state management

Test coverage includes:

- Basic CRUD operations
- Search functionality with multiple parameters
- Bean of the Day selection logic
- Edge cases and error handling

## Notes

- The \_id property in the JSON data has been discarded as mongoDB \_id is incompatible with guids in .net without adding additional padding bits. This is was done to reduce complexity but can be retained as an additional property.
- The API uses UTC dates for Bean of the Day functionality to ensure consistent behavior across time zones
- Price filtering is implemented to handle UK currency format (£)
- Database schema includes unique constraints on relevant fields
- All endpoints return appropriate HTTP status codes and error messages
