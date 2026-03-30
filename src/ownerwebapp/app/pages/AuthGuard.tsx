"use client";

import { useEffect, useState } from "react";
import { useRouter, usePathname } from "next/navigation";

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const pathname = usePathname();
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    const checkAuth = () => {
      const token = localStorage.getItem("token");

      if (!token && pathname !== "/login") {
        router.push(`/login?from=${pathname}`);
      } else {
        setIsChecking(false);
      }
    };

    checkAuth();
  }, [pathname, router]);

  if (isChecking) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-black">
        <div className="text-cyan-400 text-xl">Checking authentication...</div>
      </div>
    );
  }

  return <>{children}</>;
}