import { View, Text, TouchableOpacity } from 'react-native';
import { ReactNode } from 'react';
import {
    Check,
    ExclamationMark,
    ArrowSquareOut,
} from "phosphor-react-native";

type StatusConformidadeTipo = 'azul' | 'vermelho';

interface StatusConformidadeProps {
    corFundo: StatusConformidadeTipo;
    tipo: string;
    corQuadrado: string;
    quantidade: number;
    total: number;
    onPress?: () => void;
}

const statusConfig: Record<StatusConformidadeTipo, {
    corTexto: string;
    corQuadrado: string;
    icone: ReactNode;
    categoria: 'regulares' | 'inconformes';
    bgColor: string;
}> = {
    azul: {
        corTexto: 'text-azul-claro-principal',
        icone: <Check size={38} color="#AEC3F5" weight="bold" />,
        categoria: 'regulares',
        bgColor: 'bg-azul-escuro-principal',
        corQuadrado: '#AEC3F5',
    },
    vermelho: {
        corTexto: 'text-vermelho-secundario',
        icone: <ExclamationMark size={38} color="#E9A5A5" weight="bold" />,
        categoria: 'inconformes',
        bgColor: 'bg-vermelho-principal',
        corQuadrado: '#E9A5A5',

    },
};

export default function StatusConformidade({
    corFundo,
    tipo,
    quantidade,
    total,
    onPress,
}: StatusConformidadeProps) {
    const isConforme = tipo === "Conformidade";

    const config = statusConfig[corFundo];

    return (
        <TouchableOpacity
            onPress={onPress}
            className={`p-3 pb-1.5 rounded-xl ${config.bgColor} justify-center items-center w-[45%] aspect-[174/101] shadow-clicaveis`}
        >
            <View className="flex-row items-start justify-between w-full">
                <View className=''>
                    <Text className={`${config.corTexto} text-sm font-light mb-[-4px]`}>Carteiras {isConforme ? "em" : ""}</Text>
                    <Text className={`font-bold text-lg ${config.corTexto}`}>{tipo}</Text>
                </View>
                <ArrowSquareOut size={14} color={`${config.corQuadrado}`} weight="bold" />
            </View>
            <View className='flex-row items-center justify-between w-full h-fit'>
                <Text className={`${config.corTexto}`}>
                    <Text className="text-xl font-bold">{quantidade}</Text>
                    <Text className="text-[10px] font-light"> de </Text>
                    <Text className="text-sm font-bold">{total}</Text>
                </Text>
                <View className="">{config.icone}</View>
            </View>
        </TouchableOpacity>
    );
}