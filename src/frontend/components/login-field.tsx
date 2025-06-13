import React, { useState } from "react";
import { View, Alert } from "react-native";
import { EmailInput } from "./email-input";
import { PasswordInput } from "./password-input";
import { AuthToggle } from "./auth-toggle";
import { LoginButton } from "./login-button";
import { useRouter, useLocalSearchParams } from "expo-router";
import { RememberMeCheckbox } from "./remember-me-checkbox";
import { useAuth } from "@/hooks/useAuth";
import { useAuth as useAuthContext } from "@/app/contexts/AuthProvider";

export function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const { login: apiLogin, isLoading, error } = useAuth();
  const { login: authLogin } = useAuthContext();
  const router = useRouter();
  const { mode } = useLocalSearchParams();

  const handleLogin = async () => {
    if (!email || !password) {
      Alert.alert("Erro", "Por favor, preencha todos os campos");
      return;
    }

    // First make the API call
    const apiResult = await apiLogin({ email, password });

    if (apiResult.success) {
      // If API call succeeds, update auth context
      const authResult = await authLogin(email, password);
      if (authResult.success) {
        router.replace("/");
      } else {
        Alert.alert("Erro", "Erro ao atualizar estado de autenticação");
      }
    } else {
      Alert.alert("Erro", apiResult.message || "Erro ao fazer login");
    }
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
        <LoginButton onPress={handleLogin} isLoading={isLoading} />
      </View>
    </View>
  );
}

export default LoginForm;
