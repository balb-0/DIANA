import { usePathname, useRouter } from 'expo-router';
import React from 'react';
import { ActivityIndicator, View } from 'react-native';
import { useAuth } from '../contexts/AuthProvider';

interface AuthGuardProps {
  children: React.ReactNode;
}

export function AuthGuard({ children }: AuthGuardProps) {
  const { isAuthenticated, isLoading } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  // Public routes that don't require authentication
  const publicRoutes = ['/login', '/signup'];
  const isPublicRoute = publicRoutes.includes(pathname);

  React.useEffect(() => {
    if (!isLoading && !isAuthenticated && !isPublicRoute) {
      router.replace('/login');
    }
  }, [isAuthenticated, isLoading, isPublicRoute, router]);

  // Show loading screen while checking authentication
  if (isLoading) {
    return (
      <View className="flex-1 justify-center items-center bg-azul-claro-secundario">
        <ActivityIndicator size="large" color="#0000ff" />
      </View>
    );
  }

  // Allow access to public routes or authenticated users
  if (isPublicRoute || isAuthenticated) {
    return <>{children}</>;
  }

  // Show loading while redirecting
  return (
    <View className="flex-1 justify-center items-center bg-azul-claro-secundario">
      <ActivityIndicator size="large" color="#0000ff" />
    </View>
  );
}
