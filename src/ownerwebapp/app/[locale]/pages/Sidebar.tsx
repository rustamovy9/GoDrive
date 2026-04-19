"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useTranslations, useLocale } from "next-intl";

import {
    LayoutDashboard,
    Building2,
    Car,
    Calendar,
    LogOut,
    CarFront,
    Bell,
    BotMessageSquare,
    Menu,
    ChevronDown
} from "lucide-react";

export default function Sidebar() {
    const pathname = usePathname();
    const router = useRouter();
    const locale = useLocale();
    const t = useTranslations("Sidebar");

    const [isOpen, setIsOpen] = useState(false);
    const [langOpen, setLangOpen] = useState(false);
    const [token, setToken] = useState<string | null>(null);

    useEffect(() => {
        if (typeof window !== "undefined") {
            setToken(localStorage.getItem("token"));
        }
    }, []);

    const languages = [
        { code: "en", name: "En", flag: "https://flagcdn.com/w40/us.png" },
        { code: "ru", name: "Ru", flag: "https://flagcdn.com/w40/ru.png" },
        { code: "tj", name: "Tj", flag: "https://flagcdn.com/w40/tj.png" },
    ];

    const currentLang =
        languages.find((l) => l.code === locale) || languages[0];

    const changeLanguage = (newLocale: string) => {
        const pathSegments = pathname.split("/");

        if (["en", "tj", "ru"].includes(pathSegments[1])) {
            pathSegments[1] = newLocale;
        } else {
            pathSegments.splice(1, 0, newLocale);
        }

        router.push(pathSegments.join("/") || `/${newLocale}`);
        document.cookie = `NEXT_LOCALE=${newLocale}; path=/; max-age=31536000`;
    };

    const menuItems = [
        { name: t("dashboard"), href: `/${locale}`, icon: LayoutDashboard },
        { name: t("company"), href: `/${locale}/company`, icon: Building2 },
        { name: t("cars"), href: `/${locale}/cars`, icon: Car },
        { name: t("bookings"), href: `/${locale}/bookings`, icon: Calendar },
        { name: t("notifications"), href: `/${locale}/notifications`, icon: Bell },
        { name: t("chat"), href: `/${locale}/chat`, icon: BotMessageSquare },
    ];

    const handleSignOut = () => {
        localStorage.removeItem("token");
        document.cookie = "token=; path=/; max-age=0";
        window.location.href = `/${locale}/login`;
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
                className={`fixed inset-0 z-40 bg-black/70 transition-opacity md:hidden ${isOpen ? "opacity-100 visible" : "opacity-0 invisible"
                    }`}
                onClick={() => setIsOpen(false)}
            />

            <aside
                className={`fixed z-50 top-0 left-0 w-64 h-full bg-zinc-950 border-r border-zinc-800 flex flex-col transform transition-transform
                ${isOpen ? "translate-x-0" : "-translate-x-full"}
                md:translate-x-0 md:relative md:flex`}
            >
                <div className="p-6 border-b border-zinc-800 flex justify-between items-center">
                    <Link href={`/${locale}`} className="flex items-center gap-2">
                        <CarFront className="text-cyan-400" size={28} />
                        <span className="text-xl font-bold text-white">
                            Go<span className="text-cyan-400">Drive</span>
                        </span>
                    </Link>

                    <button
                        onClick={() => setIsOpen(false)}
                        className="md:hidden text-zinc-400"
                    >
                        ✕
                    </button>
                </div>

                <nav className="flex-1 p-4 space-y-2">
                    {menuItems.map((item) => {
                        const Icon = item.icon;
                        const isActive = pathname === item.href;

                        return (
                            <Link
                                key={item.href}
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

                <div className="p-4 border-t border-zinc-800 space-y-4">

                    <div>
                        <p className="text-zinc-400 text-sm mb-2">
                            Забон / Language / Язык
                        </p>

                        <div className="relative">
                            <button
                                onClick={() => setLangOpen(!langOpen)}
                                className="w-full flex items-center justify-between gap-3 bg-[#1a1f2e] hover:bg-[#252b3b] px-4 py-3 rounded-xl transition-all border border-[#2a3142]"
                            >
                                <div className="flex items-center gap-3">
                                    <img
                                        src={currentLang.flag}
                                        className="w-7 h-7 rounded-full"
                                    />
                                    <span className="text-white font-semibold text-sm">
                                        {currentLang.name}
                                    </span>
                                </div>

                                <ChevronDown
                                    className={`w-4 h-4 text-gray-400 transition-transform ${langOpen ? "rotate-180" : ""
                                        }`}
                                />
                            </button>

                            {langOpen && (
                                <div className="absolute bottom-full mb-2 w-full bg-[#1a1f2e] border border-[#2a3142] rounded-xl shadow-xl overflow-hidden z-50">
                                    {languages.map((lang) => (
                                        <button
                                            key={lang.code}
                                            onClick={() => {
                                                changeLanguage(lang.code);
                                                setLangOpen(false);
                                            }}
                                            className={`w-full flex items-center gap-3 px-4 py-3 transition-all ${locale === lang.code
                                                    ? "bg-[#252b3b]"
                                                    : "hover:bg-[#252b3b]"
                                                }`}
                                        >
                                            <img
                                                src={lang.flag}
                                                className="w-6 h-6 rounded-full"
                                            />
                                            <span
                                                className={`text-sm font-medium ${locale === lang.code
                                                        ? "text-cyan-400"
                                                        : "text-gray-300"
                                                    }`}
                                            >
                                                {lang.name}
                                            </span>
                                        </button>
                                    ))}
                                </div>
                            )}
                        </div>
                    </div>

                    <Link href="https://godrivetj.vercel.app/">
                        <button className="w-full bg-blue-700 text-white font-bold py-2 px-6 rounded-lg hover:bg-blue-800 transition-colors">
                            {t("user")}
                        </button>
                    </Link>

                    <button
                        onClick={handleSignOut}
                        className="flex items-center gap-3 px-4 py-3 w-full rounded-lg text-zinc-400 hover:bg-zinc-900 hover:text-white transition-all"
                    >
                        <LogOut size={20} />
                        <span className="font-medium">{t("signOut")}</span>
                    </button>
                </div>
            </aside>
        </>
    );
}