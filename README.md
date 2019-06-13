# TodoList

I took the standard approach using the default template supplied by Microsoft for creating and MVC application. The project structure is simple for a small-scale application, all the layers contained in the same project library. 

Using the MVC design pattern, this pattern is good for separating concerns for a layered architecture structure. The DAL consists of the Data & Model folders, this also holds the class ‘SeedDB.cs’ for seeding the database with test the user and some to-do items for the user.

The controller is holding the business logic and database querying. Using dependency injection to inject the ApplicationDbContext in to the controller I can query the in-memory database. The logic is at controller level and is used for authentication and authorization the signed in user is they say they are.

The presentation layer containing the View folder with all the necessary views for each corresponding action. I used some JavaScript for validation & the checkbox check.
Usually when creating projects these layers would be separated as class libraries. This clean architecture structure would see a class library for BLL, DAL, and unit tests. The unit testing of functions, methods and controller actions.

If given more time, 
- I would have put more effort in to the design, rather than using the standard out of the box MVC template by displaying tasks as a tabled view.
- Separated the DAL & BLL layers in to individual class libraries. 
- Added a class library for unit testing of the functionality with the controller actions and functions.

