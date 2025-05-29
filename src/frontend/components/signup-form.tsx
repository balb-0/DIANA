import { View } from "react-native";
import { EmailInput } from "./email-input";
import { useState } from "react";
import { AuthToggle } from "./auth-toggle";
import { useRouter } from "expo-router";
import { PasswordFields } from "./password-fields";
import { RegisterButton } from "./register-button";

export const SignupForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const router = useRouter();

  const handleSignup = () => {
    // Handle signup logic here
    console.log("Signup with:", { email, password, confirmPassword });
  };

  const handleToggle = (mode: "login" | "register") => {
    if (mode === "login") {
      router.replace("/");
    }
  };

  return (
    <View className="w-full items-center justify-center gap-8">
      <AuthToggle initialMode="register" onToggle={handleToggle} />
      <View className="w-full items-center gap-4">
        <EmailInput value={email} onChangeText={setEmail} />
        <PasswordFields
          password={password}
          confirmPassword={confirmPassword}
          onPasswordChange={setPassword}
          onConfirmPasswordChange={setConfirmPassword}
        />
      </View>
      <RegisterButton onPress={handleSignup} />
    </View>
  );
};

export default SignupForm;
