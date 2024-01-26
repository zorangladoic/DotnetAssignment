# Readme

## FlowrSpot Project

### Assignment Info
You are going to create a working version of a product called FlowerSpot.
The app is used for flower spotting while hiking, traveling, etc. Users can check out
different flowers, their details, and sightings as well as add their own. Think of it as
Instagram, but only for flower spotters.
Your task is to go through the requirements and determine what needs to be done and
implement it. If you think the task is trivial, you can just leave a TODO comment in the
code on what changes you would implement.
The assignment usually takes a couple of hours. Focus on what you believe is most
important and instead of aiming for perfection, demonstrate your familiarity with different
paradigms and let us know what you would improve if you had more time. Anything that
is not specifically written is up to you to decide. Keep it simple.

### Requirements
You will build a backend for a RESTful API as a .NET web app. Ideally, the latest stable
version. The solution to this assignment should represent your level of expertise and we
trust that you will solve it yourself. You must be able to explain the decisions that you
made while planning and developing this app. Communication is JSON over HTTP, the
database is PostgreSQL and the use of Docker is a plus.
The project will be submitted via git. Write your commit messages so it is clear what
changes they contain and push regularly.
Please make sure that the app supports the following user stories:
1. User model
Expose an endpoint that enables user registration. We want to persist the following data
points about users: username, password, email. Implement request validation that
makes the most sense to you.
2. Flowers
Expose endpoints to get and create flowers (name, image ref, description).
3. Authentication
Each request except for user registration and retrieval of flowers should only be allowed
to registered i.e. authenticated users. To achieve this it is sufficient to implement Basic
Authentication (Wiki on Basic Access Authentication), but any other form of security for
the exposed endpoints will also be acceptable. There is no need to implement
authorization but a basic authorization configuration will count as a bonus point.
4. Sightings
Expose sighting endpoint (longitude, latitude, user ref, flower ref, image ref). Operations
on this endpoint include getting, creating and deleting a sighting. Only users who
created a sighting can also delete it.
5. Likes
Expose endpoints for likes. A user can like a sighting and unlike (delete) it. Along with
likes implementation also extend sighting endpoint with the counter of the number of
likes of a sighting. Users can only delete their own likes, not from others.
Quote of the day
When a user creates a sighting, we want to add a random motivational quote to the
entity and return it as part of a response. To get a quote you have to call quotes.rest and
get the free quote-of-the-day.

### Additional recommendations
Pay special attention to the construction of the REST API and add different layers of
tests. Application is only handling image references, not the images themself. We
assume that it is the front-end teams responisility to integrate with some image storing
service and only provide us with image references. There is no need to have high code
coverage with tests, but rather focus on the demonstration of different principles. Use
README.md file to notify a reviewer about anything you believe is relevant.
We are not looking for a perfect app or 100% test coverage. It is more important to us
that design ideas are properly demonstrated to provide a base for a deeper technical
discussion that might come later in the process.

## Development analysis

Based on requirements, I’m going to develop .NET Backend solution for the given task with latest stable version: .NET 8, Using PostgreSQL Database, Entity Framework ORM, Code First modeling.

For the solution architecture, I’ll be using Clean Architecture to have strong separation of concerns and to have adaptability for future changes. This will enhance maintainability, testability and scaleability of the application, especially when application get complex over time.

I will be adding automated tests: Unit test and Integration tests to avoid errors in the code and logic while developing the application, extending it with new features, or fixing a bug.

I am using an Agile approach to break down the initial problem into smaller tasks. I will be using the BDD Gherkin syntax to define the features of the our system. 

## BDD User Stories

### Feature 1
#### Scenario: Anonymous user is allowed to register
    Given I am an anonymous user
    When I access the User POST endpoint
    Then I can register a new User

### Feature 2
#### Scenario: Anonymous user can get a list of flowers
    Given I am an anonymous user
    When I access the Flower GET endpoint
    Then I can see list of flowers

### Feature 3
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Flower POST endpoint
    Then I get access denied response

#### Scenario: A logged in user can add new flowers
    Given I am a logged in user
    When I access the Flower POST endpoint
    Then I can create a new Flower

### Feature 4
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Sighting GET endpoint
    Then I get access denied response

#### Scenario: A logged in user can get a list of sightings
    Given I am a logged in user
    When I access the Sighting GET endpoint
    Then I can see list of sightings

### Feature 5
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Sighting CREATE endpoint
    Then I get access denied response

#### Scenario: A logged in user can add new sighting
    Given I am a logged in user
    When I access the Sighting CREATE endpoint
    Then I can create a new Sighting
    And I get random motivational quote in response

### Feature 6
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Sighting DELETE endpoint
    Then I get access denied response

#### Scenario: A logged in user can remove sighting
    Given I am a logged in user
    When I access the Sighting DELETE endpoint
    Then I can delete a Sighting I created

### Feature 7
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Like POST endpoint
    Then I get access denied response

#### Scenario: A logged in user can remove sighting
    Given I am a logged in user
    When I access the Like POST endpoint
    Then I can create a Like

### Feature 8
#### Scenario: Anonymous access is now allowed
    Given I am an anonymous user
    When I access the Like DELETE endpoint
    Then I get access denied response

#### Scenario: A logged in user can remove sighting
    Given I am a logged in user
    When I access the Like DELETE endpoint
    Then I can delete a Like I created