import React, { useEffect, useState } from 'react';
import { View, Text, ActivityIndicator, ScrollView } from 'react-native';
import { useStatistics } from '@/hooks/useStatistics';
import { EstatisticasCompletas } from '@/services/statisticsService';

export const EstatisticasDebugView: React.FC = () => {
    const { isLoading, error, getEstatisticasCompletas } = useStatistics();
    const [stats, setStats] = useState<EstatisticasCompletas | null>(null);

    useEffect(() => {
        const loadStats = async () => {
            const estatisticas = await getEstatisticasCompletas();
            setStats(estatisticas);
        };

        loadStats();
    }, []);

    if (isLoading) {
        return (
            <View className="flex-1 justify-center items-center p-4">
                <ActivityIndicator size="large" color="#001E62" />
                <Text className="mt-2 text-gray-600">Carregando estatísticas...</Text>
            </View>
        );
    }

    if (error) {
        return (
            <View className="flex-1 justify-center items-center p-4">
                <Text className="text-red-500 text-center">{error}</Text>
            </View>
        );
    }

    if (!stats) {
        return null;
    }

    return (
        <ScrollView className="flex-1 p-4">
            <Text className="text-2xl font-bold text-center mb-6">Estatísticas de Conformidade</Text>

            {/* Estatísticas Gerais */}
            <View className="bg-white rounded-lg p-4 mb-4 shadow">
                <Text className="text-lg font-bold mb-2">📊 Visão Geral</Text>
                <Text>Total de Clientes: {stats.todas.total}</Text>
                <Text className="text-green-600">Conformes: {stats.todas.conformes}</Text>
                <Text className="text-red-600">Inconformes: {stats.todas.inconformes}</Text>
                <Text>Taxa de Conformidade: {stats.todas.total > 0 ? ((stats.todas.conformes / stats.todas.total) * 100).toFixed(1) : 0}%</Text>
            </View>

            {/* Conservador */}
            <View className="bg-blue-50 rounded-lg p-4 mb-4">
                <Text className="text-lg font-bold mb-2">🏛️ Conservador</Text>
                <Text>Total: {stats.conservador.total}</Text>
                <Text className="text-green-600">Conformes: {stats.conservador.conformes}</Text>
                <Text className="text-red-600">Inconformes: {stats.conservador.inconformes}</Text>
                <Text>Taxa: {stats.conservador.total > 0 ? ((stats.conservador.conformes / stats.conservador.total) * 100).toFixed(1) : 0}%</Text>
            </View>

            {/* Moderado */}
            <View className="bg-yellow-50 rounded-lg p-4 mb-4">
                <Text className="text-lg font-bold mb-2">⚖️ Moderado</Text>
                <Text>Total: {stats.moderado.total}</Text>
                <Text className="text-green-600">Conformes: {stats.moderado.conformes}</Text>
                <Text className="text-red-600">Inconformes: {stats.moderado.inconformes}</Text>
                <Text>Taxa: {stats.moderado.total > 0 ? ((stats.moderado.conformes / stats.moderado.total) * 100).toFixed(1) : 0}%</Text>
            </View>

            {/* Sofisticado */}
            <View className="bg-green-50 rounded-lg p-4 mb-4">
                <Text className="text-lg font-bold mb-2">📈 Sofisticado</Text>
                <Text>Total: {stats.sofisticado.total}</Text>
                <Text className="text-green-600">Conformes: {stats.sofisticado.conformes}</Text>
                <Text className="text-red-600">Inconformes: {stats.sofisticado.inconformes}</Text>
                <Text>Taxa: {stats.sofisticado.total > 0 ? ((stats.sofisticado.conformes / stats.sofisticado.total) * 100).toFixed(1) : 0}%</Text>
            </View>
        </ScrollView>
    );
};
