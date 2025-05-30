import { ProfileHeader } from "@/components/profile-header";
import { View } from "react-native";

export default function Profile() {
  return (
    <View className="h-full w-full justify-between bg-azul-claro-secundario">
      <ProfileHeader />
    </View>
  );
}
