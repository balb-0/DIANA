Feature: Client Service
    As a financial advisor
    I want to retrieve client risk profiles
    So that I can provide suitable investment recommendations

Background:
    Given I have a client service with mocked dependencies

Scenario: Get client risk profile successfully
    Given the repository returns risk profile "Conservador" for client "12345"
    When I request the risk profile for client "12345"
    Then I should receive "Conservador" as the result
    And the client repository method should be called once with client ID "12345"
    And an information log should be recorded for service

Scenario: Get aggressive client risk profile successfully
    Given the repository returns risk profile "Agressivo" for client "67890"
    When I request the risk profile for client "67890"
    Then I should receive "Agressivo" as the result
    And the client repository method should be called once with client ID "67890"
    And an information log should be recorded for service

Scenario: Handle client not found in repository
    Given the repository returns null for client "99999"
    When I request the risk profile for client "99999"
    Then I should receive null as the result
    And the client repository method should be called once with client ID "99999"
    And an information log should be recorded for service

Scenario: Handle repository exception gracefully
    Given the repository throws a database exception with message "Database connection failed"
    When I request the risk profile for client "12345"
    Then an InvalidOperationException should be thrown with message "Database connection failed"
    And an error log should be recorded for service

Scenario: Handle invalid client ID format
    Given I have a client service with mocked dependencies
    When I request the risk profile for client ""
    Then an ArgumentException should be thrown with message "Client ID cannot be null or empty"
    And an error log should be recorded for service

Scenario Outline: Handle various client risk profiles
    Given the repository returns risk profile "<risk_profile>" for client "<client_id>"
    When I request the risk profile for client "<client_id>"
    Then I should receive "<risk_profile>" as the result
    And the client repository method should be called once with client ID "<client_id>"
    And an information log should be recorded for service

    Examples:
    | client_id | risk_profile |
    | 11111     | Conservador  |
    | 22222     | Moderado     |
    | 33333     | Agressivo    |
    | 44444     | Conservador  |
    | 55555     | Moderado     |
