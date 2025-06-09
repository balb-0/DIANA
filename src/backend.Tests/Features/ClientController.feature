Feature: Client API Controller
    As a client application
    I want to access client risk profile data through REST API
    So that I can retrieve client risk profiles for investment recommendations

Background:
    Given I have a client controller with mocked dependencies

Scenario: Get client risk profile via API successfully
    Given the client service returns risk profile "Conservador" for client "12345"
    When I call the GET client risk profile endpoint with client ID "12345"
    Then I should receive an HTTP 200 OK response
    And the response body should contain client ID "12345"
    And the response body should contain risk profile "Conservador"
    And the client service should be called once with client ID "12345"

Scenario: Get aggressive client risk profile via API successfully
    Given the client service returns risk profile "Agressivo" for client "67890"
    When I call the GET client risk profile endpoint with client ID "67890"
    Then I should receive an HTTP 200 OK response
    And the response body should contain client ID "67890"
    And the response body should contain risk profile "Agressivo"
    And the client service should be called once with client ID "67890"

Scenario: Handle client not found via API
    Given the client service returns null for client "99999"
    When I call the GET client risk profile endpoint with client ID "99999"
    Then I should receive an HTTP 404 Not Found response
    And the response body should contain the message "Cliente não encontrado."
    And the client service should be called once with client ID "99999"

Scenario: Handle service exception in API
    Given the client service throws an exception with message "Database connection failed"
    When I call the GET client risk profile endpoint with client ID "12345"
    Then I should receive an HTTP 500 Internal Server Error response
    And the response body should contain the message "Internal server error"
    And an error log should be recorded for controller
    And the client service should be called once with client ID "12345"

Scenario: Handle invalid client ID format in API
    Given I have a client controller with mocked dependencies
    When I call the GET client risk profile endpoint with client ID ""
    Then I should receive an HTTP 400 Bad Request response
    And the response body should contain the message "Invalid client ID format"

Scenario Outline: API handles various client risk profile scenarios
    Given the client service returns risk profile "<risk_profile>" for client "<client_id>"
    When I call the GET client risk profile endpoint with client ID "<client_id>"
    Then I should receive an HTTP 200 OK response
    And the response body should contain client ID "<client_id>"
    And the response body should contain risk profile "<risk_profile>"
    And the client service should be called once with client ID "<client_id>"

    Examples:
    | client_id | risk_profile |
    | 11111     | Conservador  |
    | 22222     | Moderado     |
    | 33333     | Agressivo    |
    | 44444     | Conservador  |
    | 55555     | Moderado     |
