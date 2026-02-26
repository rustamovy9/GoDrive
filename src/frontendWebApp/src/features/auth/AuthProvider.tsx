import { useEffect, type ReactNode } from "react";
import { useAuthStore } from "./useAuth";

export function AuthProvider({ children }: { children: ReactNode }) {
  const fetchUser = useAuthStore((s) => s.fetchUser);
  useEffect(() => {
    fetchUser();
  }, [fetchUser]);
  return <>{children}</>;
}
