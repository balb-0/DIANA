Feature: Compliance Service
    As a compliance officer
    I want to retrieve client compliance counts
    So that I can monitor and report on compliance metrics

Background:
    Given I have a compliance service with mocked dependencies

Scenario: Get conforming clients count successfully
    Given the repository returns 150 conforming clients
    When I request the count of conforming clients
    Then I should receive 150 as the result
    And the conforming clients repository method should be called once
    And the non-conforming clients repository method should not be called
    And an information log should be recorded

Scenario: Get non-conforming clients count successfully
    Given the repository returns 75 non-conforming clients
    When I request the count of non-conforming clients
    Then I should receive 75 as the result
    And the non-conforming clients repository method should be called once
    And the conforming clients repository method should not be called
    And an information log should be recorded

Scenario: Handle repository exception gracefully
    Given the repository throws a database exception with message "Database connection failed"
    When I request the count of conforming clients
    Then an InvalidOperationException should be thrown with message "Database connection failed"
    And an error log should be recorded for service

Scenario Outline: Handle various client count scenarios
    Given the repository returns <count> <client_type> clients
    When I request the count of <client_type> clients
    Then I should receive <count> as the result
    And an information log should be recorded

    Examples:
    | client_type    | count |
    | conforming     | 0     |
    | non-conforming | 0     |
    | conforming     | 1000  |
    | non-conforming | 999   |
