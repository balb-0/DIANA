import React from 'react';
import { Text } from 'react-native';
import { render, waitFor } from '@testing-library/react-native';
import { ClientesProvider, useClientes } from '../ClientesProvider';

const TestComponent = () => {
    const { clientes, clientesPorPerfil, clientesPorStatus } = useClientes();

    return (
        <>
            <Text testID="totalClientes">{clientes.length}</Text>
            <Text testID="conservadores">{clientesPorPerfil.Conservador.length}</Text>
            <Text testID="regulares">{clientesPorStatus.Regulares.length}</Text>
        </>
    );
};


describe('ClientesProvider', () => {
    it('classifica corretamente os clientes por perfil e status', async () => {
        const { getByTestId } = render(
            <ClientesProvider>
                <TestComponent />
            </ClientesProvider>
        );

        await waitFor(() => {
            expect(getByTestId('totalClientes').props.children).toBe(3);
            expect(getByTestId('conservadores').props.children).toBe(1);
            expect(getByTestId('regulares').props.children).toBe(2);
        }, { timeout: 2000 });
    });
});
