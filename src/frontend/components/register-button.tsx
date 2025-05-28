import { TouchableOpacity, Text } from "react-native";
import { SignIn } from "phosphor-react-native";

interface RegisterButtonProps {
  onPress: () => void;
}

export const RegisterButton: React.FC<RegisterButtonProps> = ({ onPress }) => {
  return (
    <TouchableOpacity
      onPress={onPress}
      className="w-[348px] h-[56px] bg-[#001E62] rounded-[10px] items-center justify-center flex-row"
      activeOpacity={0.7}
    >
      <Text className="text-white text-base font-medium">Cadastrar</Text>
      <SignIn size={24} color="#AEC3F5" style={{ marginLeft: 8 }} />
    </TouchableOpacity>
  );
};

export default RegisterButton;
