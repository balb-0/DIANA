import { TouchableOpacity, Text } from "react-native";
import { ArrowLeft } from "phosphor-react-native";
import { useRouter } from "expo-router";

interface BackButtonProps {
  onPress?: () => void;
}

export const BackButton: React.FC<BackButtonProps> = ({ onPress }) => {
  const router = useRouter();

  const handlePress = () => {
    if (onPress) {
      onPress();
    } else {
      router.back();
    }
  };

  return (
    <TouchableOpacity
      onPress={handlePress}
      className="flex-row items-center bg-[#AEC3F5] px-6 py-3 rounded-full"
      activeOpacity={0.8}
    >
      <ArrowLeft size={18} color="#001E62" weight="bold" />
      <Text className="text-[#001E62] text-xl font-medium ml-2">Voltar</Text>
    </TouchableOpacity>
  );
};

export default BackButton;
