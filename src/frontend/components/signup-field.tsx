import { View } from "react-native";
import { EmailInput } from "./email-input";
import { PasswordInput } from "./password-input";
import { useState } from "react";
import { AuthToggle } from "./auth-toggle";
import { RegisterButton } from "./register-button";
import { useRouter } from "expo-router";

export const SignupForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();

  const handleRegister = () => {
    // Handle registration logic here
    console.log("Register with:", { email, password });
  };

  const handleToggle = (mode: "login" | "register") => {
    if (mode === "login") {
      router.replace("/login");
    }
  };

  return (
    <View className="w-full items-center justify-center gap-8">
      <AuthToggle initialMode="register" onToggle={handleToggle} />
      <View className="w-full items-center gap-4">
        <EmailInput value={email} onChangeText={setEmail} />
        <PasswordInput value={password} onChangeText={setPassword} />
      </View>
      <RegisterButton onPress={handleRegister} />
    </View>
  );
};

export default SignupForm;
