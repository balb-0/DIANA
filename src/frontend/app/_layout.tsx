import "../global.css";
import { Slot, usePathname } from "expo-router";
import Welcome from "@/components/welcome"
import { BottomNavigation } from "@/components/nav-tabs";

export default function Layout() {
  const pathname = usePathname();
  const hideBottomNav = ["/login", "/signup"].includes(pathname);
  const hideWelcome = ["/login", "/signup", "/wallet", "/profile", "/categoria/Moderado", "/categoria/Sofisticado", "/categoria/Conservador", "/categoria/Todas",].includes(pathname);

  return (
    <>
      {!hideWelcome && <Welcome user="João" />}
      <Slot />
      {!hideBottomNav && <BottomNavigation />}
    </>
  );
}
