"use client";

import { useState } from "react";
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
    BotMessageSquare,
    Menu
} from "lucide-react";

export default function Sidebar() {
    const pathname = usePathname();
    const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;
    const [isOpen, setIsOpen] = useState(false);

    const menuItems = [
        { name: "Dashboard", href: "/", icon: LayoutDashboard },
        { name: "Company", href: "/company", icon: Building2 },
        { name: "My Cars", href: "/cars", icon: Car },
        { name: "Bookings", href: "/bookings", icon: Calendar },
        { name: "Notifications", href: "/notifications", icon: Bell },
        { name: "Chat Ai", href: "/chat", icon: BotMessageSquare },
    ];

    const handleSignOut = () => {
        localStorage.removeItem("token");
        document.cookie = "token=; path=/; max-age=0";
        window.location.href = "/login";
    };

    if (!token) return null;

    return (
        <>
            <button
                onClick={() => setIsOpen(true)}
                className="md:hidden fixed top-4 left-4 z-50 p-2 bg-zinc-900 rounded-lg text-white shadow-lg"
            >
                <Menu size={24} />
            </button>

            <div
                className={`fixed inset-0 z-40 bg-black/70 transition-opacity md:hidden ${isOpen ? "opacity-100 visible" : "opacity-0 invisible"}`}
                onClick={() => setIsOpen(false)}
            />

            <aside className={`fixed z-50 top-0 left-0 w-64 h-full bg-zinc-950 border-r border-zinc-800 flex flex-col transform transition-transform
                ${isOpen ? "translate-x-0" : "-translate-x-full"} md:translate-x-0 md:relative md:flex`}
            >
                <div className="p-6 border-b border-zinc-800 flex justify-between items-center">
                    <Link href="/" className="flex items-center gap-2">
                        <CarFront className="text-cyan-400" size={28} />
                        <span className="text-xl font-bold text-white">
                            Go<span className="text-cyan-400">Drive</span>
                        </span>
                    </Link>
                    <button onClick={() => setIsOpen(false)} className="md:hidden text-zinc-400">
                        ✕
                    </button>
                </div>

                <nav className="flex-1 p-4 space-y-2">
                    {menuItems.map((item) => {
                        const Icon = item.icon;
                        const isActive = pathname === item.href;

                        return (
                            <Link
                                key={item.name}
                                href={item.href}
                                onClick={() => setIsOpen(false)}
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
                    <Link href="https://godrivetj.vercel.app/">
                        <button className="bg-blue-700 text-white font-bold py-2 px-6 rounded-lg hover:bg-blue-800 transition-colors">
                            User
                        </button>
                    </Link>
                    <button
                        onClick={handleSignOut}
                        className="flex items-center gap-3 px-4 py-3 w-full rounded-lg text-zinc-400 hover:bg-zinc-900 hover:text-white transition-all"
                    >
                        <LogOut size={20} />
                        <span className="font-medium">Sign Out</span>
                    </button>
                </div>
            </aside>
        </>
    );
}