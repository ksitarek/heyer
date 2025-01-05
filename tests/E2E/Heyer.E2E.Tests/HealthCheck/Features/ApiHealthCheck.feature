Feature: API Health Check
    As a developer
    I want to check the health of the APIs
    So that I can ensure the APIs are working correctly

    @E2E
    Scenario: Heyer API is healthy
        Given the Heyer API is running
        When I check the healthcheck endpoint
        Then the API should be healthy

    @E2E
    Scenario: Storage API is healthy
        Given the Storage API is running
        When I check the healthcheck endpoint
        Then the API should be healthy
