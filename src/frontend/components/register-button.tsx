import { TouchableOpacity, Text } from "react-native";
import { SignIn } from "phosphor-react-native";

interface RegisterButtonProps {
  onPress: () => void;
}

export const RegisterButton: React.FC<RegisterButtonProps> = ({ onPress }) => {
  return (
    <TouchableOpacity
      onPress={onPress}
      className="bg-[#001E62] w-[256px] h-[72px] rounded-2xl flex-row items-center justify-center"
      activeOpacity={0.7}
    >
      <Text className="text-azul-claro-principal text-2xl font-semibold">Cadastrar</Text>
      <SignIn size={24} color="#AEC3F5" weight="bold" style={{ marginLeft: 8 }} />
    </TouchableOpacity>
  );
};

export default RegisterButton;
