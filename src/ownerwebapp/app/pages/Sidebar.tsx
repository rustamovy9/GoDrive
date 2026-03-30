"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
    LayoutDashboard,
    Building2,
    Car,
    Calendar,
    LogOut,
    CarFront,
    Bell,
    BotOff,
    BotMessageSquare,
} from "lucide-react";

export default function Sidebar() {
    const pathname = usePathname();
    const token = localStorage.getItem("token");

    const menuItems = [
        {
            name: "Dashboard",
            href: "/",
            icon: LayoutDashboard,
        },
        {
            name: "Company",
            href: "/company",
            icon: Building2,
        },
        {
            name: "My Cars",
            href: "/cars",
            icon: Car,
        },
        {
            name: "Bookings",
            href: "/bookings",
            icon: Calendar,
        },
        {
            name: "Notifications",
            href: "/notifications",
            icon: Bell
        },
        {
            name: "Chat Ai",
            href: "/chat",
            icon: BotMessageSquare
        }
    ];

    const handleSignOut = () => {
        localStorage.removeItem("token");
        document.cookie = "token=; path=/; max-age=0";
        window.location.href = "/login";
    };

    if (!token) {
        return null
    }

    return (
        <aside className="w-64 bg-zinc-950 border-r border-zinc-800 min-h-screen flex flex-col">
            <div className="p-6 border-b border-zinc-800">
                <Link href="/" className="flex items-center gap-2">
                    <CarFront className="text-cyan-400" size={28} />
                    <span className="text-xl font-bold text-white">
                        Go<span className="text-cyan-400">Drive</span>
                    </span>
                </Link>
            </div>

            <nav className="flex-1 p-4 space-y-2">
                {menuItems.map((item) => {
                    const Icon = item.icon;
                    const isActive = pathname === item.href;

                    return (
                        <Link
                            key={item.name}
                            href={item.href}
                            className={`flex items-center gap-3 px-4 py-3 rounded-lg transition-all ${isActive
                                    ? "bg-cyan-500/10 text-cyan-400 border border-cyan-500/20"
                                    : "text-zinc-400 hover:bg-zinc-900 hover:text-white"
                                }`}
                        >
                            <Icon size={20} />
                            <span className="font-medium">{item.name}</span>
                        </Link>
                    );
                })}
            </nav>

            <div className="p-4 border-t border-zinc-800">
                <button
                    onClick={handleSignOut}
                    className="flex items-center gap-3 px-4 py-3 w-full rounded-lg text-zinc-400 hover:bg-zinc-900 hover:text-white transition-all"
                >
                    <LogOut size={20} />
                    <span className="font-medium">Sign Out</span>
                </button>
            </div>
        </aside>
    );
}