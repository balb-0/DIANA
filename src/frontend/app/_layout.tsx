import { BottomNavigation } from "@/components/nav-tabs";
import Welcome from "@/components/welcome";
import { Slot, usePathname } from "expo-router";
import "../global.css";
import { CarteirasProvider } from "./contexts/CarteirasProvider";
import { ClientesProvider } from "./contexts/ClientesProvider";
import { AuthProvider, useAuth } from "./contexts/AuthProvider";
import { AuthGuard } from "@/components/auth-guard";

function LayoutContent() {
  const pathname = usePathname();
  const { user } = useAuth();
  const hideBottomNav = ["/login", "/signup"].includes(pathname);
  const hideWelcome =
    ["/login", "/signup", "/wallet", "/profile"].includes(pathname) ||
    pathname.startsWith("/categoria/") ||
    pathname.startsWith("/investor/");

  return (
    <ClientesProvider tipoRisco="geral">
      <CarteirasProvider>
        <AuthGuard>
          {!hideWelcome && <Welcome user={user || "Usuário"} />}
          <Slot />
          {!hideBottomNav && <BottomNavigation />}
        </AuthGuard>
      </CarteirasProvider>
    </ClientesProvider>
  );
}

export default function Layout() {
  return (
    <AuthProvider>
      <LayoutContent />
    </AuthProvider>
  );
}
