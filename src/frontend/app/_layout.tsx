import { BottomNavigation } from "@/components/nav-tabs";
import Welcome from "@/components/welcome";
import { Slot, usePathname } from "expo-router";
import "../global.css";
import { CarteirasProvider } from "../contexts/CarteirasProvider";
import { ClientesProvider } from "../contexts/ClientesProvider";

export default function Layout() {
  const pathname = usePathname();
  const hideBottomNav = ["/login", "/signup"].includes(pathname);
  const hideWelcome =
    ["/login", "/signup", "/wallet", "/profile"].includes(pathname) ||
    pathname.startsWith("/categoria/") ||
    pathname.startsWith("/investor/");
    
  return (
    <ClientesProvider>
      <CarteirasProvider>
        {!hideWelcome && <Welcome user="João" />}
        <Slot />
        {!hideBottomNav && <BottomNavigation />}
      </CarteirasProvider>
    </ClientesProvider>
  );
}
