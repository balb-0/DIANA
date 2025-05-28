import { View, Text, TouchableOpacity } from "react-native";
import { BottomNavigation } from "@/components/nav-tabs";
import { useRouter } from "expo-router";
import { BackButton } from "../components/back-button";

export default function Teste() {
  const router = useRouter();

  const handleLogin = () => {
    router.push("/login");
  };

  return (
    <View className="flex-1 bg-white">
      <View className="flex-1 items-center justify-center">
        <BackButton />
        <TouchableOpacity onPress={handleLogin}>
          <Text className="text-2xl font-bold">Login</Text>
        </TouchableOpacity>
      </View>
      <BottomNavigation />
    </View>
  );
}
