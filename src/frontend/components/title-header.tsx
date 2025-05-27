import { View, Text, Dimensions, Image } from "react-native";

interface TitleHeaderProps {
  title?: string;
}

export const TitleHeader: React.FC<TitleHeaderProps> = ({
  title = "DIANA",
}) => {
  const screenWidth = Dimensions.get("window").width;

  return (
    <View style={{ width: screenWidth, height: 320 }}>
      {/* Bottom curve image */}
      <Image
        source={require("../assets/head.png")}
        style={{
          position: "absolute",
          bottom: 50,
          width: screenWidth,
          height: 300,
          left: 0,
          resizeMode: "stretch",
        }}
      />
      {/* Title text */}
      <View className="items-center justify-center h-full">
        <Text className="text-[#AEC3F5] text-7xl tracking-[20px] font-bold">
          {title}
          <Text className="text-[#FF7CA1]">.</Text>
        </Text>
      </View>
    </View>
  );
};

export default TitleHeader;
