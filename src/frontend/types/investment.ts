export interface Investment {
  id: string;
  clientId: string;
  productName: string;
  productType: string;
  investmentValue: number;
  applicationDate: string;
  profitability: number;
  currentValue: number;
  status: 'ACTIVE' | 'CANCELLED' | 'MATURED';
  maturityDate?: string;
  createdAt: string;
  updatedAt: string;
  valorTotal: number;
  ativo?: {
    tipo: string;
    subgrupo: string;
  };
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
  statusCode: number;
}

export interface InvestmentSummary {
  totalInvestments: number;
  totalValue: number;
  investmentsByType: { [key: string]: number };
  investmentsBySubgroup: { [key: string]: number };
}