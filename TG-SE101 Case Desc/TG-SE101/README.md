# E-Commerce Microservice Assessment

## Overview

This assessment is designed to evaluate your ability to structure and organize a basic e-commerce microservice project. The provided code is a starting point, and your task is to refactor and organize it into a fully structured solution, ensuring that best practices are followed.

## Task

You are provided with a single file containing an incomplete and basic implementation of an e-commerce microservice. Your task is to:

1. **Create a Visual Studio Solution**: Set up a solution that is properly organized, with appropriate project separation (for example, by organizing entities, database, services, and controllers into distinct projects if necessary).
   
2. **Migrate the Code**: Refactor the provided code into a proper architecture, maintaining SOLID principles, separation of concerns, and clean code practices. You can make use of design patterns where applicable.

3. **Implement a Proper Startup**: Ensure the solution uses a proper startup mechanism, with dependency injection where necessary. Organize and configure services like Entity Framework DbContext and repositories accordingly.

4. **Apply Migrations**: Ensure that database migrations are correctly implemented using Entity Framework. This should include seeding data (optional).

5. **Improve Error Handling**: Add proper error handling and logging where needed.

## Guidelines

- **Architecture**: Use best practices for organizing the solution. You are free to choose how you want to structure the projects (e.g., separating business logic, database, and API layers).
  
- **Frameworks**: You are expected to use ASP.NET Core for the application framework and Entity Framework Core for data access. Feel free to make use of any additional libraries if necessary.

- **Database**: The code is currently set up to use a local SQL Server instance. You can adjust the connection string as needed.

- **Documentation**: Ensure your code is well-commented and documented.

## Bonus Points

- Implementing Swagger for API documentation.
- Dockerizing the application.
- Providing additional validation logic for the entities.

## Deliverables

- A database migration and seed script.
- A brief description of your architectural decisions in the form of a `DesignChoices.md` file.
- Instructions on how to run the project locally, including any necessary environment setup.

## Submission

Once you're done with the implementation, please ensure that:

- All the requirements are met.
- Commit history is clear and informative.

Good luck!

