import React from 'react';
import {
  View,
  Text,
  ScrollView,
  RefreshControl,
  Alert,
  TouchableOpacity,
  StyleSheet,
} from 'react-native';
import { useInvestments } from '../hooks/useInvestments';
import { InvestmentCard } from '../components/investment-card';
import { InvestmentSummary } from '../components/investment-summary';
import { LoadingScreen } from '../components/loading-screen';
import { ErrorMessage } from '../components/error-message';

interface InvestmentsScreenProps {
  clientId: string;
  onNavigateToLogin?: () => void;
}

export const InvestmentsScreen: React.FC<InvestmentsScreenProps> = ({ 
  clientId, 
  onNavigateToLogin 
}) => {
  const {
    investments,
    summary,
    loading,
    error,
    refetch,
    clearError,
    isAuthenticated
  } = useInvestments(clientId);

  const handleRetry = () => {
    clearError();
    refetch();
  };

  const handleLoginRedirect = () => {
    if (onNavigateToLogin) {
      onNavigateToLogin();
    } else {
      Alert.alert(
        'Sessão Expirada',
        'Sua sessão expirou. Faça login novamente.',
        [{ text: 'OK' }]
      );
    }
  };

  if (!isAuthenticated) {
    return (
      <View style={styles.centerContainer}>
        <View style={styles.sessionExpiredBox}>
          <Text style={styles.sessionExpiredTitle}>
            Sessão Expirada
          </Text>
          <Text style={styles.sessionExpiredMessage}>
            Sua sessão expirou. Faça login novamente para continuar.
          </Text>
          
          <TouchableOpacity
            onPress={handleLoginRedirect}
            style={styles.loginButton}
          >
            <Text style={styles.loginButtonText}>
              Fazer Login
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    );
  }

  if (loading) {
    return <LoadingScreen message="Carregando investimentos..." />;
  }

  if (error) {
    return (
      <ErrorMessage
        message={error}
        onRetry={handleRetry}
      />
    );
  }

  if (investments.length === 0) {
    return (
      <View style={styles.centerContainer}>
        <Text style={styles.noInvestmentsTitle}>
          Nenhum investimento encontrado
        </Text>
        <Text style={styles.noInvestmentsMessage}>
          Este cliente ainda não possui investimentos cadastrados.
        </Text>
        <TouchableOpacity
          onPress={refetch}
          style={styles.refreshButton}
        >
          <Text style={styles.refreshButtonText}>Atualizar</Text>
        </TouchableOpacity>
      </View>
    );
  }

  return (
    <ScrollView
      style={styles.scrollView}
      refreshControl={
        <RefreshControl
          refreshing={loading}
          onRefresh={refetch}
          colors={['#3B82F6']}
        />
      }
    >
      <View style={styles.container}>
        {/* Header com informações do usuário */}
        <View style={styles.header}>
          <Text style={styles.headerTitle}>
            Meus Investimentos
          </Text>
          <View style={styles.clientBadge}>
            <Text style={styles.clientBadgeText}>
              Cliente: {clientId}
            </Text>
          </View>
        </View>

        {/* Resumo dos Investimentos */}
        {summary && <InvestmentSummary summary={summary} />}

        {/* Lista de Investimentos */}
        <View style={styles.investmentsList}>
          <Text style={styles.investmentsListTitle}>
            Detalhes dos Investimentos ({investments.length})
          </Text>
          
          {investments.map((investment) => (
            <InvestmentCard
              key={investment.id}
              investment={investment}
            />
          ))}
        </View>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  centerContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F9FAFB',
    padding: 24,
  },
  sessionExpiredBox: {
    backgroundColor: '#FFFBEB',
    borderWidth: 1,
    borderColor: '#FDE68A',
    borderRadius: 8,
    padding: 24,
    width: '100%',
    maxWidth: 400,
  },
  sessionExpiredTitle: {
    color: '#D97706',
    textAlign: 'center',
    fontSize: 18,
    fontWeight: '600',
    marginBottom: 8,
  },
  sessionExpiredMessage: {
    color: '#92400E',
    textAlign: 'center',
    marginBottom: 16,
  },
  loginButton: {
    backgroundColor: '#D97706',
    paddingVertical: 12,
    paddingHorizontal: 24,
    borderRadius: 8,
  },
  loginButtonText: {
    color: '#FFFFFF',
    textAlign: 'center',
    fontWeight: '600',
  },
  noInvestmentsTitle: {
    fontSize: 24,
    fontWeight: '700',
    color: '#374151',
    marginBottom: 16,
  },
  noInvestmentsMessage: {
    color: '#6B7280',
    textAlign: 'center',
    marginBottom: 24,
  },
  refreshButton: {
    backgroundColor: '#3B82F6',
    paddingHorizontal: 24,
    paddingVertical: 12,
    borderRadius: 8,
  },
  refreshButtonText: {
    color: '#FFFFFF',
    fontWeight: '600',
  },
  scrollView: {
    flex: 1,
    backgroundColor: '#F9FAFB',
  },
  container: {
    padding: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 24,
  },
  headerTitle: {
    fontSize: 24,
    fontWeight: '700',
    color: '#1F2937',
  },
  clientBadge: {
    backgroundColor: '#DBEAFE',
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: 16,
  },
  clientBadgeText: {
    color: '#1E40AF',
    fontSize: 12,
    fontWeight: '500',
  },
  investmentsList: {
    marginTop: 24,
  },
  investmentsListTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#374151',
    marginBottom: 16,
  },
});