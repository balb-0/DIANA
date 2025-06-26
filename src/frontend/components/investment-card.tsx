import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Investment } from '../types/investment';

interface InvestmentCardProps {
  investment: Investment;
}

export const InvestmentCard: React.FC<InvestmentCardProps> = ({ investment }) => {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'ACTIVE':
        return '#10B981';
      case 'CANCELLED':
        return '#EF4444';
      case 'MATURED':
        return '#F59E0B';
      default:
        return '#6B7280';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'ACTIVE':
        return 'Ativo';
      case 'CANCELLED':
        return 'Cancelado';
      case 'MATURED':
        return 'Vencido';
      default:
        return status;
    }
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <Text style={styles.productName}>{investment.productName}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(investment.status) }]}>
          <Text style={styles.statusText}>{getStatusText(investment.status)}</Text>
        </View>
      </View>
      
      <Text style={styles.productType}>{investment.productType}</Text>
      
      <View style={styles.valueContainer}>
        <View style={styles.valueItem}>
          <Text style={styles.valueLabel}>Valor Investido</Text>
          <Text style={styles.valueText}>
            {formatCurrency(investment.investmentValue)}
          </Text>
        </View>
        
        <View style={styles.valueItem}>
          <Text style={styles.valueLabel}>Valor Atual</Text>
          <Text style={styles.valueText}>
            {formatCurrency(investment.currentValue)}
          </Text>
        </View>
      </View>
      
      <View style={styles.detailsContainer}>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>Aplicação</Text>
          <Text style={styles.detailText}>
            {formatDate(investment.applicationDate)}
          </Text>
        </View>
        
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>Rentabilidade</Text>
          <Text style={[styles.detailText, { color: investment.profitability >= 0 ? '#10B981' : '#EF4444' }]}>
            {investment.profitability.toFixed(2)}%
          </Text>
        </View>
      </View>
      
      {investment.maturityDate && (
        <View style={styles.maturityContainer}>
          <Text style={styles.maturityLabel}>Vencimento: </Text>
          <Text style={styles.maturityDate}>
            {formatDate(investment.maturityDate)}
          </Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    padding: 16,
    marginBottom: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  productName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1F2937',
    flex: 1,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
  },
  statusText: {
    fontSize: 10,
    fontWeight: '500',
    color: '#FFFFFF',
  },
  productType: {
    fontSize: 14,
    color: '#6B7280',
    marginBottom: 12,
  },
  valueContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 12,
  },
  valueItem: {
    flex: 1,
  },
  valueLabel: {
    fontSize: 12,
    color: '#6B7280',
    marginBottom: 2,
  },
  valueText: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1F2937',
  },
  detailsContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  detailItem: {
    flex: 1,
  },
  detailLabel: {
    fontSize: 12,
    color: '#6B7280',
    marginBottom: 2,
  },
  detailText: {
    fontSize: 14,
    fontWeight: '500',
    color: '#1F2937',
  },
  maturityContainer: {
    flexDirection: 'row',
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#E5E7EB',
  },
  maturityLabel: {
    fontSize: 12,
    color: '#6B7280',
  },
  maturityDate: {
    fontSize: 12,
    fontWeight: '500',
    color: '#1F2937',
  },
});