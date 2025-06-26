import React, { useEffect, useState } from 'react';
import { View, Text, ActivityIndicator, TouchableOpacity, Alert } from 'react-native';
import { useStatistics } from '@/hooks/useStatistics';
import { EstatisticasCategoria } from '@/services/statisticsService';

interface StatCardProps {
    categoria: 'Conservador' | 'Moderado' | 'Sofisticado';
    onPress?: () => void;
}

export const StatCard: React.FC<StatCardProps> = ({ categoria, onPress }) => {
    const { getEstatisticasCategoria, isLoading } = useStatistics();
    const [stats, setStats] = useState<EstatisticasCategoria>({ total: 0, conformes: 0, inconformes: 0 });
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadStats = async () => {
            setLoading(true);
            try {
                const estatisticas = await getEstatisticasCategoria(categoria);
                setStats(estatisticas);
            } catch (error) {
                console.error(`Erro ao carregar stats para ${categoria}:`, error);
            } finally {
                setLoading(false);
            }
        };

        loadStats();
    }, [categoria]);

    const porcentagemConformidade = stats.total > 0 ? ((stats.conformes / stats.total) * 100).toFixed(1) : '0';

    const handlePress = () => {
        if (onPress) {
            onPress();
        } else {
            Alert.alert(
                `Estatísticas - ${categoria}`,
                `Total: ${stats.total}\nConformes: ${stats.conformes}\nInconformes: ${stats.inconformes}\nTaxa de Conformidade: ${porcentagemConformidade}%`
            );
        }
    };

    if (loading) {
        return (
            <View className="bg-white rounded-lg p-4 m-2 shadow flex-1 justify-center items-center min-h-[120px]">
                <ActivityIndicator size="small" color="#001E62" />
                <Text className="text-xs text-gray-500 mt-2">Carregando {categoria}...</Text>
            </View>
        );
    }

    return (
        <TouchableOpacity
            className="bg-white rounded-lg p-4 m-2 shadow flex-1 min-h-[120px]"
            onPress={handlePress}
        >
            <Text className="text-lg font-bold text-azul-escuro-principal mb-2">{categoria}</Text>

            <View className="flex-1 justify-center">
                <Text className="text-2xl font-bold text-center text-azul-escuro-principal">
                    {stats.conformes}
                    <Text className="text-sm font-normal"> de </Text>
                    {stats.total}
                </Text>
                <Text className="text-xs text-center text-gray-600 mt-1">
                    em conformidade
                </Text>
            </View>

            <View className="flex-row justify-between items-center mt-2">
                <Text className="text-xs text-gray-500">Taxa: {porcentagemConformidade}%</Text>
                <View className="flex-row space-x-1">
                    <View className="w-2 h-2 bg-green-500 rounded-full" />
                    <Text className="text-xs text-green-600">{stats.conformes}</Text>
                    <View className="w-2 h-2 bg-red-500 rounded-full ml-2" />
                    <Text className="text-xs text-red-600">{stats.inconformes}</Text>
                </View>
            </View>
        </TouchableOpacity>
    );
};

// Exemplo de uso em um componente maior
export const DashboardStatistics: React.FC = () => {
    return (
        <View className="p-4">
            <Text className="text-xl font-bold text-center mb-4">Conformidade por Perfil</Text>

            <View className="flex-row">
                <StatCard categoria="Conservador" />
                <StatCard categoria="Moderado" />
            </View>

            <View className="flex-row">
                <StatCard categoria="Sofisticado" />
                <View className="flex-1 m-2" /> {/* Spacer */}
            </View>
        </View>
    );
};
