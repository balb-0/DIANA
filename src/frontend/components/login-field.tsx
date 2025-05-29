import { View } from "react-native";
import { EmailInput } from "./email-input";
import { PasswordInput } from "./password-input";
import { useState } from "react";
import { AuthToggle } from "./auth-toggle";
import { LoginButton } from "./login-button";
import { useRouter, useLocalSearchParams } from "expo-router";
import { RememberMeCheckbox } from "./remember-me-checkbox";

export const LoginForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const router = useRouter();
  const { mode } = useLocalSearchParams();

  const handleLogin = () => {
    // Handle login logic here
    console.log("Login with:", { email, password, rememberMe });
  };

  const handleToggle = (mode: "login" | "register") => {
    if (mode === "register") {
      router.replace("/signup");
    }
  };

  return (
    <View className="w-full items-center justify-center gap-8">
      <AuthToggle
        initialMode={(mode as "login" | "register") || "login"}
        onToggle={handleToggle}
      />
      <View className="w-full items-center gap-4">
        <EmailInput value={email} onChangeText={setEmail} />
        <PasswordInput value={password} onChangeText={setPassword} />
      </View>
      <View className="items-center gap-4">
        <RememberMeCheckbox
          checked={rememberMe}
          onToggle={() => setRememberMe(!rememberMe)}
        />
        <LoginButton onPress={handleLogin} />
      </View>
    </View>
  );
};

export default LoginForm;
