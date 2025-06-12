import React from "react";
import { View, Text, Image, Dimensions, ImageBackground } from "react-native";
import { User, Phone, Envelope } from "phosphor-react-native";

interface ProfileHeaderProps {
  name?: string;
  phone?: string;
  email?: string;
  avatar?: string;
  height?: number;
}

export const ProfileHeader: React.FC<ProfileHeaderProps> = ({
  name = "Assessor Master Blaster",
  phone = "(11) 97070-7070",
  email = "assessormasterblaster@yahoo.com.br",
  avatar,
  height = 300,
}) => {
  return (
    <View className="relative">
      {/* Background Image */}
      <ImageBackground
        source={{ uri: "/head.png" }}
        className="absolute inset-0"
        style={{ height }}
        resizeMode="cover"
      >
        <Image
          source={require("../assets/head.png")}
          style={{
            position: "absolute",
            bottom: 70,
            width: Dimensions.get("window").width,
            height: 300,
            left: 0,
            resizeMode: "stretch",
          }}
        />
      </ImageBackground>

      {/* Content */}
      <View className="flex justify-center px-6" style={{ height }}>
        <View className="flex-row items-center gap-2">
          {/* Avatar */}
          <View>
            <View className="w-[76px] h-[76px] bg-[#AEC3F5] justify-center items-center rounded-[100000px] p-[26px]">
              {avatar ? (
                <Image
                  source={{ uri: avatar }}
                  className="w-full h-full rounded-full"
                  resizeMode="cover"
                />
              ) : (
                <User size={24} color="#001E62" weight="regular" />
              )}
            </View>
          </View>

          {/* Info Section */}
          <View className="flex flex-1 gap-2">
            {/* Name */}
            <Text className="text-[#AEC3F5] text-[18px] font-bold">{name}</Text>

            {/* Phone */}
            <View className="flex-row items-center">
              <View className="bg-[#AEC3F5] rounded-md px-[8px] py-[2px] flex-row items-center">
                <Phone size={18} color="#001E62" weight="fill" />
                <Text className="text-[#001E62] text-[12px] ml-1 font-bold">
                  {phone}
                </Text>
              </View>
            </View>

            {/* Email */}
            <View className="flex-row items-center">
              <View className="bg-[#AEC3F5]  rounded-md px-[8px] py-[2px] flex-row items-center">
                <Envelope size={18} color="#001E62" weight="fill" />
                <Text
                  className="text-[#001E62] text-[12px] ml-1 font-bold"
                  numberOfLines={1}
                  ellipsizeMode="tail"
                >
                  {email}
                </Text>
              </View>
            </View>
          </View>
        </View>
      </View>
    </View>
  );
};

export default ProfileHeader;
