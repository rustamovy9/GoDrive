"use client";

import React, { useEffect, useState } from "react";
import i18n from "../../../shared/i18n/i18n";

interface Language {
    code: string;
    label: string;
    flag: string;
}

const languages: Language[] = [
    { code: "en", label: "English", flag: "https://flagcdn.com/w40/us.png" },
    { code: "ru", label: "Русский", flag: "https://flagcdn.com/w40/ru.png" },
    { code: "tj", label: "Тоҷикӣ", flag: "https://flagcdn.com/w40/tj.png" },
];

export default function SelectLanguage() {
    const [currentLang, setCurrentLang] = useState("en");
    const [open, setOpen] = useState(false);

    useEffect(() => {
        const savedLang = localStorage.getItem("lang") || "en";
        i18n.changeLanguage(savedLang);
        setCurrentLang(savedLang);
    }, []);

    useEffect(() => {
        const handler = (lng: string) => setCurrentLang(lng);

        i18n.on("languageChanged", handler);
        return () => {
            i18n.off("languageChanged", handler);
        };
    }, []);

    const changeLanguage = (lang: string) => {
        i18n.changeLanguage(lang);
        localStorage.setItem("lang", lang);
        setOpen(false);
    };

    const current = languages.find((l) => l.code === currentLang);

    return (
        <div className="relative inline-block">
            <button
                onClick={() => setOpen(!open)}
                className="flex items-center gap-2 bg-gray-900 text-white px-4 py-2 rounded-xl hover:bg-gray-800 transition"
            >
                <img src={current?.flag} alt={current?.label} className="w-5 h-5 rounded" />
                <span>{current?.label}</span>
            </button>

            {open && (
                <div className="absolute mt-2 w-40 bg-gray-900 rounded-xl shadow-lg border border-gray-700 z-50">
                    {languages.map((lang) => (
                        <div
                            key={lang.code}
                            onClick={() => changeLanguage(lang.code)}
                            className="flex items-center gap-2 px-4 py-2 hover:bg-gray-800 cursor-pointer first:rounded-t-xl last:rounded-b-xl"
                        >
                            <img src={lang.flag} alt={lang.label} className="w-5 h-5 rounded" />
                            <span>{lang.label}</span>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}