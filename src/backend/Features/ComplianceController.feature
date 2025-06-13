Feature: Compliance API Controller
    As a client application
    I want to access compliance data through REST API
    So that I can retrieve client compliance counts

Background:
    Given I have a compliance controller with mocked dependencies

Scenario: Get conforming clients count via API
    Given the compliance service returns 200 conforming clients
    When I call the GET clients-count endpoint with nonconforming parameter as false
    Then I should receive an HTTP 200 OK response
    And the response body should contain the count 200
    And the compliance service should be called once with nonconforming as false

Scenario: Get non-conforming clients count via API
    Given the compliance service returns 85 non-conforming clients
    When I call the GET clients-count endpoint with nonconforming parameter as true
    Then I should receive an HTTP 200 OK response
    And the response body should contain the count 85
    And the compliance service should be called once with nonconforming as true

Scenario: Handle service exception in API
    Given the compliance service throws an exception with message "Service unavailable"
    When I call the GET clients-count endpoint with nonconforming parameter as false
    Then I should receive an HTTP 500 Internal Server Error response
    And the response body should contain the message "Internal server error"
    And an error log should be recorded for controller
    And the compliance service should be called once with nonconforming as false

Scenario: API endpoint with default parameter
    Given the compliance service returns 150 conforming clients
    When I call the GET clients-count endpoint without nonconforming parameter
    Then I should receive an HTTP 200 OK response
    And the response body should contain the count 150
    And the compliance service should be called once with nonconforming as false

Scenario Outline: API handles various count scenarios
    Given the compliance service returns <count> <client_type> clients
    When I call the GET clients-count endpoint with nonconforming parameter as <nonconforming>
    Then I should receive an HTTP 200 OK response
    And the response body should contain the count <count>
    And the compliance service should be called once with nonconforming as <nonconforming>

    Examples:
    | client_type    | count | nonconforming |
    | conforming     | 0     | false         |
    | non-conforming | 0     | true          |
    | conforming     | 500   | false         |
    | non-conforming | 300   | true          |
