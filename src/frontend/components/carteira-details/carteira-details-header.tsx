import React from "react";
import { View, Text, ImageBackground } from "react-native";
import {
  Wallet,
  Phone,
  Envelope,
  IdentificationCard,
  Cake,
} from "phosphor-react-native";
import { BackButton } from "../back-button";

interface CarteiraDetailsHeaderProps {
  name?: string;
  email?: string;
  phone?: string;
  cpf?: string;
  birthDate?: string;
  height?: number;
}

export const CarteiraDetailsHeader: React.FC<CarteiraDetailsHeaderProps> = ({
  name = "Fulano da Silva Soares de S. Martins",
  email = "fulanodesilva@gmail.com",
  phone = "(11) 97070-7070",
  cpf = "111.111.111-11",
  birthDate = "19/11/2002",
  height = 300,
}) => {
  return (
    <View className="relative">
      {/* Background Image */}
      <ImageBackground
        source={require("../../assets/head.png")}
        className="absolute inset-0"
        style={{
          height: 220,
          width: "100%",
        }}
        resizeMode="stretch"
      ></ImageBackground>

      {/* Back Button */}
      <View className="absolute right-8 top-16 z-10">
        <BackButton />
      </View>

      {/* Content */}
      <View className="flex justify-center px-6" style={{ height }}>
        {/* Header with icon and name */}
        <View className="flex-row items-center gap-2 mb-3">
          <View className="justify-center items-center">
            <Wallet size={24} color="#AEC3F5" weight="regular" />
          </View>
          <View>
            <Text className="text-[#AEC3F5] text-xl font-bold">{name}</Text>
          </View>
        </View>

        {/* Info Section */}
        <View className="flex gap-2">
          {/* Email and Phone */}
          <View className="flex-row items-center gap-2 flex-wrap">
            <View className="bg-[#AEC3F5] rounded-md px-[8px] py-[2px] flex-row items-center">
              <Envelope size={18} color="#001E62" weight="fill" />
              <Text
                className="text-[#001E62] text-[12px] ml-1 font-bold"
                numberOfLines={1}
                ellipsizeMode="tail"
              >
                {email}
              </Text>
            </View>
            <View className="bg-[#AEC3F5] rounded-md px-[8px] py-[2px] flex-row items-center">
              <Phone size={18} color="#001E62" weight="fill" />
              <Text className="text-[#001E62] text-[12px] ml-1 font-bold">
                {phone}
              </Text>
            </View>
          </View>

          {/* CPF and Birth Date */}
          <View className="flex-row items-center gap-2">
            <View className="bg-[#AEC3F5] rounded-md px-[8px] py-[2px] flex-row items-center">
              <IdentificationCard size={18} color="#001E62" weight="fill" />
              <Text className="text-[#001E62] text-[12px] ml-1 font-bold">
                {cpf}
              </Text>
            </View>
            <View className="bg-[#AEC3F5] rounded-md px-[8px] py-[2px] flex-row items-center">
              <Cake size={18} color="#001E62" weight="fill" />
              <Text className="text-[#001E62] text-[12px] ml-1 font-bold">
                {birthDate}
              </Text>
            </View>
          </View>
        </View>
      </View>
    </View>
  );
};

export default CarteiraDetailsHeader;
