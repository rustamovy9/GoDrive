"use client";

import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import { Car, Menu, X, User, ChevronDown } from "lucide-react";
import { useLocale, useTranslations } from "next-intl";

export default function Navbar() {
    const pathname = usePathname();
    const router = useRouter();
    const [open, setOpen] = useState(false);
    const [langOpen, setLangOpen] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const locale = useLocale();
    const t = useTranslations("Navbar");

    useEffect(() => {
        const token = localStorage.getItem("token");
        setIsLoggedIn(!!token);
    }, []);

    const linkStyle = (path: string) =>
        `transition-all duration-300 font-medium ${pathname === path ? "text-cyan-400" : "text-gray-400 hover:text-white"
        }`;

    const languages = [
        { code: "en", name: "En", flag: "https://flagcdn.com/w40/us.png" },
        { code: "ru", name: "Ru", flag: "https://flagcdn.com/w40/ru.png" },
        { code: "tj", name: "Tj", flag: "https://flagcdn.com/w40/tj.png" },
    ];

    const currentLang = languages.find(l => l.code === locale) || languages[0];

    const changeLanguage = (newLocale: string) => {
        const pathSegments = pathname.split("/");
        if (["en", "tj", "ru"].includes(pathSegments[1])) {
            pathSegments[1] = newLocale;
        } else {
            pathSegments.splice(1, 0, newLocale);
        }
        router.push(pathSegments.join("/") || `/${newLocale}`);
        setLangOpen(false);
        document.cookie = `NEXT_LOCALE=${newLocale}; path=/; max-age=31536000`;
    };

    return (
        <nav className="w-full bg-[#161b22]/95 backdrop-blur-md border-b border-[#30363d] fixed top-0 z-50">
            <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
                <Link href={`/${locale}`} className="flex items-center gap-2 text-2xl font-bold">
                    <Car className="text-cyan-400 w-7 h-7" />
                    <span className="text-white">
                        Go<span className="text-cyan-400">Drive</span>
                    </span>
                </Link>

                <div className="hidden md:flex gap-10 text-lg items-center">
                    <Link href={`/${locale}`} className={linkStyle(`/${locale}`)}>
                        {t("home")}
                    </Link>
                    <Link href={`/${locale}/cars`} className={linkStyle(`/${locale}/cars`)}>
                        {t("browseCars")}
                    </Link>
                    <Link href={`/${locale}/about`} className={linkStyle(`/${locale}/about`)}>
                        {t("about")}
                    </Link>
                    <Link href={`/${locale}/contact`} className={linkStyle(`/${locale}/contact`)}>
                        {t("contact")}
                    </Link>
                    <Link href={`/${locale}/chat`} className={linkStyle(`/${locale}/chat`)}>
                        {t("chatAI")}
                    </Link>
                    <Link href="https://godrive-ownertj.vercel.app/">
                        <button className="bg-blue-700 text-white font-bold py-2 px-6 rounded-lg hover:bg-blue-800 transition-colors">
                            {t("owner")}
                        </button>
                    </Link>

                    <div className="relative">
                        <button
                            onClick={() => setLangOpen(!langOpen)}
                            className="flex items-center gap-3 bg-[#1a1f2e] hover:bg-[#252b3b] px-4 py-2.5 rounded-xl transition-all border border-[#2a3142]"
                        >
                            <div className="w-8 h-8 rounded-full overflow-hidden flex items-center justify-center bg-gray-700">
                                <img
                                    src={currentLang.flag}
                                    alt={currentLang.name}
                                    className="w-full h-full object-cover"
                                />
                            </div>
                            <span className="text-white font-semibold text-sm">{currentLang.name}</span>
                            <ChevronDown
                                className={`w-4 h-4 text-gray-400 transition-transform duration-200 ${langOpen ? "rotate-180" : ""}`}
                            />
                        </button>

                        {langOpen && (
                            <div className="absolute right-0 top-full mt-2 bg-[#1a1f2e] border border-[#2a3142] rounded-xl shadow-2xl min-w-[140px] overflow-hidden z-50">
                                {languages.map(lang => (
                                    <button
                                        key={lang.code}
                                        onClick={() => changeLanguage(lang.code)}
                                        className={`w-full flex items-center gap-3 px-4 py-3 transition-all ${locale === lang.code ? "bg-[#252b3b]" : "hover:bg-[#252b3b]"
                                            }`}
                                    >
                                        <div className="w-7 h-7 rounded-full overflow-hidden flex items-center justify-center bg-gray-700">
                                            <img
                                                src={lang.flag}
                                                alt={lang.name}
                                                className="w-full h-full object-cover"
                                            />
                                        </div>
                                        <span
                                            className={`font-semibold text-sm ${locale === lang.code ? "text-cyan-400" : "text-gray-300"
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

                <div className="hidden md:flex items-center gap-6">
                    {isLoggedIn ? (
                        <Link href={`/${locale}/myprofile`} className="text-white hover:text-cyan-400 transition">
                            <User className="w-7 h-7" />
                        </Link>
                    ) : (
                        <>
                            <Link href={`/${locale}/login`} className="text-white hover:text-cyan-400 transition">
                                {t("login")}
                            </Link>
                            <Link
                                href={`/${locale}/signup`}
                                className="bg-cyan-400 text-[#161b22] px-6 py-2 rounded-xl font-semibold hover:bg-cyan-500 transition"
                            >
                                {t("signup")}
                            </Link>
                        </>
                    )}
                </div>

                <button onClick={() => setOpen(!open)} className="md:hidden text-white">
                    {open ? <X size={28} /> : <Menu size={28} />}
                </button>
            </div>

            {open && (
                <div className="md:hidden bg-[#161b22] px-6 pb-6 flex flex-col gap-5 text-lg border-t border-[#30363d] transition-all duration-300">
                    <Link href={`/${locale}`} className={linkStyle(`/${locale}`)}>
                        {t("home")}
                    </Link>
                    <Link href={`/${locale}/cars`} className={linkStyle(`/${locale}/cars`)}>
                        {t("browseCars")}
                    </Link>
                    <Link href={`/${locale}/about`} className={linkStyle(`/${locale}/about`)}>
                        {t("about")}
                    </Link>
                    <Link href={`/${locale}/contact`} className={linkStyle(`/${locale}/contact`)}>
                        {t("contact")}
                    </Link>
                    <Link href={`/${locale}/chat`} className={linkStyle(`/${locale}/chat`)}>
                        {t("chatAI")}
                    </Link>
                    <Link href="https://godrive-ownertj.vercel.app/">
                        <button className="bg-blue-700 text-white font-bold py-2 px-6 rounded-lg hover:bg-blue-800 transition-colors">
                            {t("owner")}
                        </button>
                    </Link>

                    <div className="border-t border-[#30363d] pt-4">
                        <p className="text-gray-400 text-sm mb-3">Забон / Language / Язык</p>
                        <div className="flex gap-3">
                            {languages.map(lang => (
                                <button
                                    key={lang.code}
                                    onClick={() => changeLanguage(lang.code)}
                                    className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-colors ${locale === lang.code
                                        ? "bg-cyan-400 text-[#161b22]"
                                        : "bg-[#30363d] text-white hover:bg-[#3d444d]"
                                        }`}
                                >
                                    <div className="w-6 h-6 rounded-full overflow-hidden">
                                        <img
                                            src={lang.flag}
                                            alt={lang.name}
                                            className="w-full h-full object-cover"
                                        />
                                    </div>
                                    <span className="font-medium">{lang.name}</span>
                                </button>
                            ))}
                        </div>
                    </div>

                    {isLoggedIn ? (
                        <Link href={`/${locale}/myprofile`} className="text-white hover:text-cyan-400 transition">
                            {t("myProfile")}
                        </Link>
                    ) : (
                        <>
                            <Link href={`/${locale}/login`} className="text-white hover:text-cyan-400 transition">
                                {t("login")}
                            </Link>
                            <Link
                                href={`/${locale}/signup`}
                                className="bg-cyan-400 text-[#161b22] px-5 py-2 rounded-lg w-fit font-semibold hover:bg-cyan-500 transition"
                            >
                                {t("signup")}
                            </Link>
                        </>
                    )}
                </div>
            )}
        </nav>
    );
}