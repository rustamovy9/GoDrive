"use client";

import { Search, Clock, Zap } from "lucide-react";
import { useTranslations } from "next-intl";

export default function HowItWorks() {
    const t = useTranslations("HowItWorks");

    const steps = [
        {
            icon: Search,
            title: t("steps.findCar.title"),
            description: t("steps.findCar.description"),
        },
        {
            icon: Clock,
            title: t("steps.bookInstantly.title"),
            description: t("steps.bookInstantly.description"),
        },
        {
            icon: Zap,
            title: t("steps.hitRoad.title"),
            description: t("steps.hitRoad.description"),
        },
    ];

    return (
        <section className="bg-gray-950 py-20 px-4 sm:px-6 lg:px-8">
            <div className="max-w-7xl mx-auto">
                <div className="text-center mb-16">
                    <h2 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        {t("sectionSubtitle")}
                    </h2>
                    <h3 className="text-4xl sm:text-5xl font-bold text-white">
                        {t("sectionTitle")}
                    </h3>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                    {steps.map((step, index) => (
                        <div
                            key={index}
                            className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8 hover:border-cyan-400/50 transition-all duration-300 group"
                        >
                            <div className="flex justify-center mb-6">
                                <div className="bg-gray-800 group-hover:bg-cyan-400/10 rounded-xl p-4 transition-colors duration-300">
                                    <step.icon className="w-8 h-8 text-cyan-400" />
                                </div>
                            </div>

                            <h4 className="text-xl font-semibold text-white text-center mb-4">
                                {step.title}
                            </h4>

                            <p className="text-gray-400 text-center leading-relaxed">
                                {step.description}
                            </p>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}