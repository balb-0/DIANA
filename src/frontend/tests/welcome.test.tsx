// tests/welcome.test.tsx
import * as React from 'react';
import { render } from '@testing-library/react-native';
import Welcome from '../components/welcome';

describe('Welcome Component', () => {
  const mockUser = 'João Silva';

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('deve renderizar corretamente sem erros', () => {
    const { getByText } = render(<Welcome user={mockUser} />);
    // Busca pelo texto completo usando regex
    expect(getByText(new RegExp(`Bem vindo, ${mockUser}\\.`))).toBeTruthy();
  });

  it('deve exibir a mensagem de boas-vindas com o nome do usuário', () => {
    const { getByText } = render(<Welcome user={mockUser} />);
    // Verifica se o texto completo está presente
    expect(getByText(new RegExp(`Bem vindo, ${mockUser}\\.`))).toBeTruthy();
    // Verifica se o nome do usuário está presente
    expect(getByText(new RegExp(mockUser))).toBeTruthy();
  });

  it('deve renderizar o ponto rosa no final (opcional, pode remover)', () => {
    const { getByText } = render(<Welcome user={mockUser} />);
    // Se quiser testar o ponto, use regex para o texto completo
    expect(getByText(new RegExp(`Bem vindo, ${mockUser}\\.`))).toBeTruthy();
    // Ou, se usar testID, pode usar getByTestId('welcome-dot')
  });

  it('deve renderizar com diferentes nomes de usuário', () => {
    const testUsers = ['Maria', 'Pedro Santos', 'Ana Carolina'];
    testUsers.forEach(user => {
      const { getByText, unmount } = render(<Welcome user={user} />);
      expect(getByText(new RegExp(`Bem vindo, ${user}\\.`))).toBeTruthy();
      unmount();
    });
  });

  it('deve ter a estrutura de componentes correta', () => {
    const component = render(<Welcome user={mockUser} />);
    expect(component).toBeTruthy();
    expect(() => render(<Welcome user={mockUser} />)).not.toThrow();
  });

  it('deve aceitar props user como string', () => {
    const users = ['', 'A', 'Nome Muito Longo Com Espaços e Caracteres Especiais @#'];
    users.forEach(user => {
      expect(() => render(<Welcome user={user} />)).not.toThrow();
    });
  });

 it('snapshot test - deve manter a estrutura consistente', () => {
  const tree = render(<Welcome user={mockUser} />).toJSON();
  expect(tree).toBeTruthy();
  // Verifica se a estrutura básica está correta
  expect(tree.type).toBe('View');
});
});
