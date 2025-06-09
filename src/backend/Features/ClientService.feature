Feature: Investment Risk Evaluation
  As a financial advisor
  I want to evaluate clients' investment risk profiles
  So that I can provide suitable investment recommendations

Background:
  Given the risk evaluation service has all dependencies mocked

Scenario: Evaluate low-risk client profile successfully
  Given the client has declared risk level "Conservative"
    And the client's portfolio matches low volatility investments
    And the historical risk calculation returns "Low"
  When I evaluate the client's risk profile
  Then the final calculated risk should be "Low"
    And the declared risk should match the calculated risk
    And a risk report should be generated
    And the log should include "Risk profile evaluated for client ID"

Scenario: Evaluate client with mismatched declared and calculated risk
  Given the client has declared risk level "Aggressive"
    And the client's portfolio is made only of government bonds
    And the historical risk calculation returns "Low"
  When I evaluate the client's risk profile
  Then the final calculated risk should be "Low"
    And a discrepancy warning log should be recorded
    And the advisor should be notified for manual review

Scenario: Handle exception during risk calculation
  Given the risk calculation service throws a "TimeoutException" with message "Risk engine timeout"
  When I evaluate the client's risk profile
  Then a ServiceUnavailableException should be raised with message "Risk engine timeout"
    And an error log should be recorded with severity "High"
    And no notification should be sent to the advisor

Scenario Outline: Evaluate multiple client profiles successfully
  Given the client has declared risk level "<declared_risk>"
    And the client's portfolio has "<portfolio_type>" investments
    And the historical risk calculation returns "<calculated_risk>"
  When I evaluate the client's risk profile
  Then the final calculated risk should be "<calculated_risk>"
    And the declared risk should <match_result> the calculated risk

  Examples:
    | declared_risk | portfolio_type        | calculated_risk | match_result |
    | Conservative   | government bonds      | Low             | match         |
    | Moderate       | mixed                 | Medium          | match         |
    | Aggressive     | only stocks           | High            | match         |
    | Conservative   | only stocks           | High            | not match     |
