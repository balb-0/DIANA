import "../global.css";
import { Slot, usePathname } from "expo-router";
import { BottomNavigation } from "@/components/nav-tabs";

export default function Layout() {
  const pathname = usePathname();

  // Se quiser ocultar o BottomNavigation em páginas específicas (ex: login, signup):
  const hideBottomNav = ["/login", "/signup"].includes(pathname);

  return (
    <>
      <Slot />
      {!hideBottomNav && <BottomNavigation />}
    </>
  );
}
