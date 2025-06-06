import { render } from "@testing-library/react-native";
import { ProfileHeader } from "./profile-header";

describe("ProfileHeader", () => {
  it("renders default profile information", () => {
    const { getByText } = render(<ProfileHeader />);

    expect(getByText("Assessor Master Blaster")).toBeTruthy();
    expect(getByText("(11) 97070-7070")).toBeTruthy();
    expect(getByText("assessormasterblaster@yahoo.com.br")).toBeTruthy();
  });
});
