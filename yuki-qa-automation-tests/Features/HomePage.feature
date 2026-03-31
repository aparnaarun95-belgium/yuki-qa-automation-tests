Feature: Home Page E2E Tests
    As a user
    I want to verify the home page functionality
    So that I can ensure the application is working correctly

    @smoke
    @ui
    Scenario: Verify home page loads successfully
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page should be displayed
        And the URL should contain "localhost:5000"

    @smoke
    @ui
    Scenario: Verify welcome heading is visible
        Given I navigate to the home page
        When I wait for the home page to load
        Then the welcome heading should be visible
        And the home page should be displayed

    @regression
    @ui
    Scenario: Verify home page title is correct
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page title should be "Home Page - yuki_qa_automation_frontend"
