import { TouchableOpacity, Text } from "react-native";
import { SignIn } from "phosphor-react-native";

interface LoginButtonProps {
  onPress: () => void;
  text?: string;
}

export const LoginButton: React.FC<LoginButtonProps> = ({ onPress }) => {
  return (
    <TouchableOpacity
      onPress={onPress}
      activeOpacity={0.8}
      className="bg-[#001E62] w-[256px] h-[72px] rounded-2xl flex-row items-center justify-center"
    >
      <Text className="text-[#AEC3F5] text-2xl font-semibold mr-2">Entrar</Text>
      <SignIn size={24} weight="bold" color="#AEC3F5" />
    </TouchableOpacity>
  );
};

export default LoginButton;
