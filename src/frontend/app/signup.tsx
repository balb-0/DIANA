import { View } from "react-native";
import { TitleHeader } from "@/components/title-header";
import { SignupForm } from "@/components/signup-form";

function Signup() {
  return (
    <View className="flex-1 bg-[#c3d5ff]">
      <TitleHeader />
      <View className="flex-1 items-center justify-start ">
        <SignupForm />
      </View>
    </View>
  );
}

export default Signup;
