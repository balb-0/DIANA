import { render, RenderAPI } from "@testing-library/react-native";
import { ProfileHeader } from "@/components/profile-header";
import { User } from "phosphor-react-native";

describe("Profile Header Display", () => {
  describe("Viewing default profile information", () => {
    let component: RenderAPI;

    // Given
    beforeEach(() => {
      // I am on the profile page
      // Setup is handled by component render
    });

    // When
    beforeEach(() => {
      // the profile header component loads
      component = render(<ProfileHeader />);
    });

    // Then
    test("I should see the default name", () => {
      const nameElement = component.getByText("Assessor Master Blaster");
      expect(nameElement).toBeTruthy();
    });

    // And
    test("I should see the default phone", () => {
      const phoneElement = component.getByText("(11) 97070-7070");
      expect(phoneElement).toBeTruthy();
    });

    // And
    test("I should see the default email", () => {
      const emailElement = component.getByText(
        "assessormasterblaster@yahoo.com.br"
      );
      expect(emailElement).toBeTruthy();
    });

    // And
    test("I should see a default avatar icon", () => {
      const userIcon = component.UNSAFE_getByType(User);
      expect(userIcon).toBeTruthy();
    });
  });
});
