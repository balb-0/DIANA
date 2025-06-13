import { SignIn } from "phosphor-react-native";
import { ActivityIndicator, Text, TouchableOpacity } from "react-native";

interface LoginButtonProps {
  onPress: () => void;
  isLoading?: boolean;
  text?: string;
}

export const LoginButton: React.FC<LoginButtonProps> = ({ onPress, isLoading }) => {
  return (
    <TouchableOpacity
      onPress={onPress}
      activeOpacity={0.8}
      disabled={isLoading} // Desabilita o botão enquanto carrega
      className="bg-[#001E62] w-[256px] h-[72px] rounded-2xl flex-row items-center justify-center"
    >
      {isLoading ? (
        <ActivityIndicator color="#AEC3F5" size="small" />
      ) : (
        <>
          <Text className="text-[#AEC3F5] text-2xl font-semibold mr-2">Entrar</Text>
          <SignIn size={24} weight="bold" color="#AEC3F5" />
        </>
      )}
    </TouchableOpacity>
  );
};

export default LoginButton;
