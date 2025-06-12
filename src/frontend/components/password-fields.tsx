import { View } from "react-native";
import { PasswordInput } from "./password-input";

interface PasswordFieldsProps {
  password: string;
  confirmPassword: string;
  onPasswordChange: (text: string) => void;
  onConfirmPasswordChange: (text: string) => void;
}

export const PasswordFields: React.FC<PasswordFieldsProps> = ({
  password,
  confirmPassword,
  onPasswordChange,
  onConfirmPasswordChange,
}) => {
  return (
    <View className="w-full items-center gap-4">
      <PasswordInput
        value={password}
        onChangeText={onPasswordChange}
        placeholder="Digite sua senha"
      />
      <PasswordInput
        value={confirmPassword}
        onChangeText={onConfirmPasswordChange}
        placeholder="Repita sua senha"
      />
    </View>
  );
};

export default PasswordFields;
