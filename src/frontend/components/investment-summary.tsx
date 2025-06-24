import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { InvestmentSummary as SummaryType } from '../types/investment';

interface InvestmentSummaryProps {
  summary: SummaryType;
}

export const InvestmentSummary: React.FC<InvestmentSummaryProps> = ({ summary }) => {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Resumo dos Investimentos</Text>
      
      <View style={styles.summaryGrid}>
        <View style={styles.summaryCard}>
          <Text style={styles.cardValue}>{summary.totalInvestments}</Text>
          <Text style={styles.cardLabel}>Total de Investimentos</Text>
        </View>
        
        <View style={styles.summaryCard}>
          <Text style={styles.cardValue}>{formatCurrency(summary.totalValue)}</Text>
          <Text style={styles.cardLabel}>Valor Total</Text>
        </View>
      </View>

      {Object.keys(summary.investmentsByType).length > 0 && (
        <View style={styles.typeBreakdown}>
          <Text style={styles.breakdownTitle}>Por Tipo:</Text>
          {Object.entries(summary.investmentsByType).map(([tipo, valor]) => (
            <View key={tipo} style={styles.breakdownItem}>
              <Text style={styles.breakdownLabel}>{tipo}</Text>
              <Text style={styles.breakdownValue}>{formatCurrency(valor)}</Text>
            </View>
          ))}
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    padding: 16,
    marginBottom: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 3,
  },
  title: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1F2937',
    marginBottom: 16,
  },
  summaryGrid: {
    flexDirection: 'row',
    gap: 12,
    marginBottom: 16,
  },
  summaryCard: {
    flex: 1,
    backgroundColor: '#F3F4F6',
    padding: 16,
    borderRadius: 8,
    alignItems: 'center',
  },
  cardValue: {
    fontSize: 20,
    fontWeight: '700',
    color: '#1F2937',
    marginBottom: 4,
  },
  cardLabel: {
    fontSize: 12,
    color: '#6B7280',
    textAlign: 'center',
  },
  typeBreakdown: {
    borderTopWidth: 1,
    borderTopColor: '#E5E7EB',
    paddingTop: 12,
  },
  breakdownTitle: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1F2937',
    marginBottom: 8,
  },
  breakdownItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 4,
  },
  breakdownLabel: {
    fontSize: 14,
    color: '#6B7280',
  },
  breakdownValue: {
    fontSize: 14,
    fontWeight: '500',
    color: '#1F2937',
  },
});