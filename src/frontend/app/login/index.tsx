import { View } from "react-native";
import { TitleHeader } from "@/components/title-header";
import { LoginForm } from "@/components/login-field";

function Login() {
  return (
    <View className="flex-1 bg-[#c3d5ff]">
      <TitleHeader />
      <View className="flex-1 items-center justify-start">
        <LoginForm />
      </View>
    </View>
  );
}

export default Login;
