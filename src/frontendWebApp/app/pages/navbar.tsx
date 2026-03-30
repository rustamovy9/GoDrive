"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useState, useEffect } from "react";
import { Car, Menu, X, User } from "lucide-react";
// import SelectLanguage from "./SelectLanguage";

export default function Navbar() {
    const pathname = usePathname();
    const [open, setOpen] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem("token");
        setIsLoggedIn(!!token);
    }, []);

    const linkStyle = (path: string) =>
        `transition-all duration-300 font-medium ${pathname === path
            ? "text-cyan-400"
            : "text-gray-400 hover:text-white"
        }`;

    return (
        <nav className="w-full bg-[#161b22]/95 backdrop-blur-md border-b border-[#30363d] fixed top-0 z-50">
            <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
                <Link href="/" className="flex items-center gap-2 text-2xl font-bold">
                    <Car className="text-cyan-400 w-7 h-7" />
                    <span className="text-white">
                        Go<span className="text-cyan-400">Drive</span>
                    </span>
                </Link>

                <div className="hidden md:flex gap-10 text-lg">
                    <Link href="/" className={linkStyle("/")}>
                        Home
                    </Link>
                    <Link href="/cars" className={linkStyle("/cars")}>
                        Browse Cars
                    </Link>
                    <Link href="/about" className={linkStyle("/about")}>
                        About
                    </Link>
                    <Link href="/contact" className={linkStyle("/contact")}>
                        Contact
                    </Link>
                    <Link href="/chat" className={linkStyle("/chat")}>
                        Chat AI
                    </Link>
                    {/* <SelectLanguage /> */}
                </div>

                <div className="hidden md:flex items-center gap-6">
                    {isLoggedIn ? (
                        <Link
                            href="/myprofile"
                            className="text-white hover:text-cyan-400 transition"
                        >
                            <User className="w-7 h-7" />
                        </Link>
                    ) : (
                        <>
                            <Link
                                href="/login"
                                className="text-white hover:text-cyan-400 transition"
                            >
                                Log in
                            </Link>

                            <Link
                                href="/signup"
                                className="bg-cyan-400 text-[#161b22] px-6 py-2 rounded-xl font-semibold hover:bg-cyan-500 transition"
                            >
                                Sign up
                            </Link>
                        </>
                    )}
                </div>

                <button
                    onClick={() => setOpen(!open)}
                    className="md:hidden text-white"
                >
                    {open ? <X size={28} /> : <Menu size={28} />}
                </button>
            </div>

            {open && (
                <div className="md:hidden bg-[#161b22] px-6 pb-6 flex flex-col gap-5 text-lg border-t border-[#30363d]">
                    <Link href="/" className={linkStyle("/")}>
                        Home
                    </Link>
                    <Link href="/cars" className={linkStyle("/cars")}>
                        Browse Cars
                    </Link>
                    <Link href="/about" className={linkStyle("/about")}>
                        About
                    </Link>
                    <Link href="/contact" className={linkStyle("/contact")}>
                        Contact
                    </Link>
                    <Link href="/chat" className={linkStyle("/chat")}>
                        Chat AI
                    </Link>
                    {/* <SelectLanguage /> */}

                    {isLoggedIn ? (
                        <Link
                            href="/myprofile"
                            className="text-white hover:text-cyan-400 transition"
                        >
                            My Profile
                        </Link>
                    ) : (
                        <>
                            <Link href="/login" className="text-white hover:text-cyan-400 transition">
                                Log in
                            </Link>
                            <Link
                                href="/signup"
                                className="bg-cyan-400 text-[#161b22] px-5 py-2 rounded-lg w-fit font-semibold hover:bg-cyan-500 transition"
                            >
                                Sign up
                            </Link>
                        </>
                    )}
                </div>
            )}
        </nav>
    );
}