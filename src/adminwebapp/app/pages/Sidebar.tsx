"use client";

import { useEffect, useState } from "react";
import { usePathname } from "next/navigation";
import Link from "next/link";
import {
  Users,
  Car,
  Star,
  Settings,
  LayoutDashboard,
  LogOut,
  Menu,
  X,
  Bell,
  BotMessageSquare,
} from "lucide-react";

export default function Sidebar() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [userName, setUserName] = useState("User");
  const [open, setOpen] = useState(false);

  const pathname = usePathname();

  useEffect(() => {
    const token = localStorage.getItem("token");
    const user = localStorage.getItem("user");

    setIsAuthenticated(!!token);

    if (user) {
      try {
        const parsed = JSON.parse(user);
        setUserName(parsed.name || parsed.email || "User");
      } catch {
        setUserName("User");
      }
    }
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    document.cookie = "token=; path=/; max-age=0";

    window.location.href = "/login";
  };

  if (!isAuthenticated) return null;

  const getLinkClass = (href: string) => {
    const base =
      "flex items-center gap-3 px-3 py-2 rounded-lg transition-all duration-300 text-sm font-medium";

    const isActive = pathname === href;

    if (isActive) {
      return `${base}
      bg-cyan-500/10
      text-cyan-400
      border border-cyan-500/30
      shadow-[0_0_20px_rgba(34,211,238,0.35)]`;
    }

    return `${base}
    text-zinc-400
    hover:text-white
    hover:bg-zinc-800/50
    hover:pl-4`;
  };

  return (
    <div className="md:ml-[220px] ">
      <button
        onClick={() => setOpen(true)}
        className="lg:hidden fixed top-4 left-4 z-50 bg-zinc-900 p-2 rounded-lg border border-zinc-800"
      >
        <Menu size={22} />
      </button>

      {open && (
        <div
          onClick={() => setOpen(false)}
          className="fixed inset-0 bg-black/50 z-40 lg:hidden"
        />
      )}

      <aside
        className={`
        fixed top-0 left-0 h-screen w-56
        bg-zinc-950
        border-r border-zinc-800
        p-4
        flex flex-col
        z-50
        transform transition-transform duration-300
        ${open ? "translate-x-0" : "-translate-x-full"}
        lg:translate-x-0
        `}
      >
        <button
          onClick={() => setOpen(false)}
          className="lg:hidden absolute top-4 right-4 text-zinc-400"
        >
          <X size={22} />
        </button>

        <div className="flex items-center gap-2 font-bold text-cyan-400 mb-8 px-2">
          <div className="bg-cyan-500/10 p-2 rounded-lg">
            <Car size={20} />
          </div>
          <span className="text-lg tracking-wide">GoDrive</span>
        </div>

        <nav className="space-y-1 flex-1">
          <Link href="/" className={getLinkClass("/")}>
            <LayoutDashboard size={18} />
            Dashboard
          </Link>

          <Link href="/users" className={getLinkClass("/users")}>
            <Users size={18} />
            Users
          </Link>

          <Link href="/cars" className={getLinkClass("/cars")}>
            <Car size={18} />
            Cars
          </Link>

          <Link href="/reviews" className={getLinkClass("/reviews")}>
            <Star size={18} />
            Reviews
          </Link>

          <Link href="/settings" className={getLinkClass("/settings")}>
            <Settings size={18} />
            Settings
          </Link>

          <Link href="/notifications" className={getLinkClass("/notifications")}>
            <Bell size={18} />
            Notifications
          </Link>
          
          <Link href="/chat" className={getLinkClass("/chat")}>
            <BotMessageSquare size={18} />
            Chat Ai
          </Link>
        </nav>

        <div className="mt-auto pt-4 border-t border-zinc-800/50">
          <button
            onClick={handleLogout}
            className="w-full flex items-center gap-3 px-3 py-2 rounded-lg text-zinc-400 hover:text-red-400 hover:bg-red-500/10 transition text-sm"
          >
            <LogOut size={18} />
            Sign Out
          </button>
        </div>
      </aside>
    </div>
  );
}