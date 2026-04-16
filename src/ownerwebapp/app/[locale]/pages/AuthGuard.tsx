"use client";

import { useEffect, useState } from "react";
import { useRouter, usePathname, useSearchParams } from "next/navigation";

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [checked, setChecked] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");

    const isLoginPage = pathname.includes("/login");

    if (!token) {
      if (!isLoginPage) {
        router.replace(`/login?from=${pathname}`);
        return;
      }

      setChecked(true);
      return;
    }

    if (token && isLoginPage) {
      const from = searchParams.get("from");

      if (from && from !== pathname) {
        router.replace(from);
      } else {
        router.replace("/");
      }
      return;
    }

    setChecked(true);
  }, [pathname, router, searchParams]);

  if (!checked) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-black">
        <div className="text-cyan-400 text-xl animate-pulse">
          Checking authentication...
        </div>
      </div>
    );
  }

  return <>{children}</>;
}