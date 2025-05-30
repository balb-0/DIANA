import * as React from 'react';
import { fireEvent, render } from '@testing-library/react-native';
import StatusConformidade from '../components/quantidadeConformidade';

jest.mock('phosphor-react-native', () => ({
  Check: () => null,
  ExclamationMark: () => null,
  ArrowSquareOut: () => null,
}));


describe('StatusConformidade Component', () => {
  const mockOnPress = jest.fn();

  it('deve renderizar corretamente com corFundo azul e tipo Conformidade', () => {
    const { getByText } = render(
      <StatusConformidade
        corFundo="azul"
        tipo="Conformidade"
        corQuadrado="#AEC3F5"
        quantidade={5}
        total={10}
        onPress={mockOnPress}
      />
    );

    expect(getByText(/Carteiras em/)).toBeTruthy();
    expect(getByText('Conformidade')).toBeTruthy();
    expect(getByText('5')).toBeTruthy();
    expect(getByText('de')).toBeTruthy();
    expect(getByText('10')).toBeTruthy();
  });

  it('deve renderizar corretamente com corFundo vermelho e tipo diferente de Conformidade', () => {
    const { getByText } = render(
      <StatusConformidade
        corFundo="vermelho"
        tipo="OutroTipo"
        corQuadrado="#E9A5A5"
        quantidade={3}
        total={7}
      />
    );

    expect(getByText(/^Carteiras$/)).toBeTruthy();
    expect(getByText('OutroTipo')).toBeTruthy();
    expect(getByText('3')).toBeTruthy();
    expect(getByText('de')).toBeTruthy();
    expect(getByText('7')).toBeTruthy();
  });

  it('deve chamar onPress quando o componente for clicado', () => {
    // IMPORTANTE: Adicione testID="touchable" ao TouchableOpacity no componente
    // <TouchableOpacity testID="touchable" ...>
    const { getByTestId } = render(
      <StatusConformidade
        corFundo="azul"
        tipo="Conformidade"
        corQuadrado="#AEC3F5"
        quantidade={5}
        total={10}
        onPress={mockOnPress}
      />
    );

    fireEvent.press(getByTestId('touchable'));
    expect(mockOnPress).toHaveBeenCalledTimes(1);
  });
});
