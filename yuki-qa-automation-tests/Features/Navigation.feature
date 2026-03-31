Feature: Common E2E Navigation Tests
    Feature: Navigation Through All Pages
    As a user
    I want to navigate through all pages using the menu
    So that I can access different sections of the application

    Background:
        Given I navigate to "http://localhost:5000"

    @smoke
    @navigation
    Scenario: Navigate to invoices page from home page
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page should be displayed
        And the navigation menu should be visible
        When I navigate to invoices page using menu
        Then the invoices page should be displayed
        And the invoice table should be visible

    @smoke
    @navigation
    Scenario: Navigate to privacy page from home page
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page should be displayed
        When I navigate to privacy page using menu
        Then element with selector "h1" should be visible

    @regression
    @navigation
    Scenario: Navigate from invoices back to home page
        Given I navigate to the invoices page
        When I navigate to home page using menu
        Then the home page should be displayed

    @regression
    @navigation
    Scenario: Navigate from privacy back to invoices page
        Given I navigate to the invoices page
        When I navigate to privacy page using menu
        Then element with selector "h1" should be visible
        When I navigate to invoices page using menu
        Then the invoices page should be displayed

    @regression
    @navigation
    Scenario: Complete navigation cycle through all pages
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page should be displayed
        When I navigate to invoices page using menu
        Then the invoices page should be displayed
        When I navigate to privacy page using menu
        Then element with selector "h1" should be visible
        When I navigate to home page using menu
        Then the home page should be displayed
