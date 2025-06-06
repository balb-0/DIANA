Feature: Profile Header Display
  As a user
  I want to see my profile information in the header
  So that I can verify my personal details are correct

  Scenario: Viewing default profile information
    Given I am on the profile page
    When the profile header component loads
    Then I should see the default name "Assessor Master Blaster"
    And I should see the default phone "(11) 97070-7070"
    And I should see the default email "assessormasterblaster@yahoo.com.br"
    And I should see a default avatar icon 