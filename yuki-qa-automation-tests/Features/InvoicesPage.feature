Feature: Invoices Page E2E Tests
    Feature: Invoices Page E2E Tests
    As a user
    I want to verify the invoices page functionality
    So that I can manage and view invoices correctly

    @smoke
    @invoices
    Scenario: Navigate to invoices page and verify it loads
        Given I navigate to the home page
        When I wait for the home page to load
        Then the home page should be displayed
        When I navigate to invoices page using menu
        Then the invoices page should be displayed
        And the invoice table should be visible

    @smoke
    @invoices
    Scenario: Retrieve specific invoice amount I634
        Given I navigate to the invoices page
        When I wait for the invoices page to load
        Then invoice "I634" should exist
        And the invoice "I634" should have amount "423.99 EUR"

    @regression
    @invoices
    Scenario: Verify sum of all invoices matches summary row
        Given I navigate to the invoices page
        When I wait for the invoices page to load
        Then the invoices page should be displayed
        And the invoice table should be visible
        And the sum of all invoices should match the summary amount

    @regression
    @invoices
    Scenario: Verify invoice count on page
        Given I navigate to the invoices page
        When I wait for the invoices page to load
        Then the invoices page should contain at least 1 invoices

    @regression
    @invoices
    Scenario: Navigate away from invoices and back
        Given I navigate to the invoices page
        When I navigate to home page using menu
        Then the home page should be displayed
        When I navigate to invoices page using menu
        Then the invoices page should be displayed
        And the invoice table should be visible

