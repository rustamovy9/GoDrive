"use client";

import { Car, Users, Award, Heart, Shield, Zap } from "lucide-react";
import Link from "next/link";
import { useTranslations } from "next-intl";
import CTA from "../pages/cta";

export default function AboutPage() {
    const t = useTranslations("About");

    const values = [
        {
            icon: Shield,
            title: t("values.items.trust.title"),
            description: t("values.items.trust.description"),
        },
        {
            icon: Heart,
            title: t("values.items.customer.title"),
            description: t("values.items.customer.description"),
        },
        {
            icon: Zap,
            title: t("values.items.innovation.title"),
            description: t("values.items.innovation.description"),
        },
        {
            icon: Award,
            title: t("values.items.quality.title"),
            description: t("values.items.quality.description"),
        },
    ];

    const stats = [
        { label: t("stats.customers"), value: "10K+" },
        { label: t("stats.cars"), value: "500+" },
        { label: t("stats.cities"), value: "50+" },
        { label: t("stats.years"), value: "5+" },
    ];

    return (
        <div className="min-h-screen mt-[20px] bg-gray-950">

            <section className="relative py-20 px-4 sm:px-6 lg:px-8 overflow-hidden">
                <div className="max-w-7xl mx-auto text-center">
                    <h1 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        {t("hero.subtitle")}
                    </h1>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-bold text-white mb-6">
                        {t("hero.title")}
                    </h2>
                    <p className="text-gray-400 text-lg sm:text-xl max-w-3xl mx-auto leading-relaxed">
                        {t("hero.description")}
                    </p>
                </div>
            </section>

            <section className="py-20 px-4 sm:px-6 lg:px-8 bg-gray-900/30">
                <div className="max-w-7xl mx-auto">
                    <div className="relative">
                        <div className="absolute inset-0 bg-gradient-to-r from-cyan-400/5 to-purple-600/5 rounded-3xl"></div>
                        <div className="relative grid grid-cols-1 lg:grid-cols-2 gap-12 items-center p-8 lg:p-12">
                            <div>
                                <h3 className="text-3xl sm:text-4xl font-bold text-white mb-6">
                                    {t("mission.title")}
                                </h3>
                                <div className="space-y-4">
                                    <p className="text-gray-400 text-lg leading-relaxed">
                                        {t("mission.text1")}
                                    </p>
                                    <p className="text-gray-400 text-lg leading-relaxed">
                                        {t("mission.text2")}
                                    </p>
                                </div>
                                <div className="mt-8 flex flex-wrap gap-4">
                                    <Link
                                        href="/browse"
                                        className="inline-flex items-center gap-2 bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-3 px-8 rounded-xl transition-all duration-300 transform hover:scale-105"
                                    >
                                        {t("mission.browse")}
                                    </Link>
                                    <Link
                                        href="/about"
                                        className="inline-flex items-center gap-2 bg-gray-800 hover:bg-gray-700 text-white font-semibold py-3 px-8 rounded-xl border border-gray-700 transition-all duration-300"
                                    >
                                        {t("mission.learn")}
                                    </Link>
                                </div>
                            </div>
                            <div className="relative">
                                <div className="absolute -inset-4 bg-gradient-to-r from-cyan-400/20 to-purple-600/20 rounded-3xl blur-xl"></div>
                                <div className="relative aspect-square rounded-3xl overflow-hidden">
                                    <img
                                        src="https://m.atcdn.co.uk/ect/media/w600/4fbc64ae0e0b480697a3fde1e008a1ba.jpg"
                                        alt="Mission"
                                        className="w-full h-full object-cover"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section className="py-20 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto">
                    <div className="text-center mb-16">
                        <h3 className="text-3xl sm:text-4xl font-bold text-white mb-4">
                            {t("values.title")}
                        </h3>
                        <p className="text-gray-400 text-lg max-w-2xl mx-auto">
                            {t("values.subtitle")}
                        </p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                        {values.map((value, index) => (
                            <div
                                key={index}
                                className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8 hover:border-cyan-400/50 transition-all duration-300 group"
                            >
                                <div className="bg-gray-800 group-hover:bg-cyan-400/10 rounded-xl p-4 w-fit mb-6 transition-colors duration-300">
                                    <value.icon className="w-8 h-8 text-cyan-400" />
                                </div>
                                <h4 className="text-xl font-semibold text-white mb-3">
                                    {value.title}
                                </h4>
                                <p className="text-gray-400 leading-relaxed">
                                    {value.description}
                                </p>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            <section className="py-20 px-4 sm:px-6 lg:px-8 bg-gray-900/30">
                <div className="max-w-7xl mx-auto">
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
                        {stats.map((stat, index) => (
                            <div key={index} className="text-center">
                                <div className="text-4xl sm:text-5xl font-bold text-cyan-400 mb-2">
                                    {stat.value}
                                </div>
                                <div className="text-gray-400 text-sm sm:text-base">
                                    {stat.label}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            <section className="py-20 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto">
                    <div className="text-center mb-16">
                        <h3 className="text-3xl sm:text-4xl font-bold text-white mb-4">
                            {t("why.title")}
                        </h3>
                        <p className="text-gray-400 text-lg max-w-2xl mx-auto">
                            {t("why.subtitle")}
                        </p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        <div className="bg-gray-900/50 border border-gray-800 rounded-2xl p-8">
                            <Users className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                {t("why.community.title")}
                            </h4>
                            <p className="text-gray-400">
                                {t("why.community.description")}
                            </p>
                        </div>

                        <div className="bg-gray-900/50 border border-gray-800 rounded-2xl p-8">
                            <Award className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                {t("why.premium.title")}
                            </h4>
                            <p className="text-gray-400">
                                {t("why.premium.description")}
                            </p>
                        </div>

                        <div className="bg-gray-900/50 border border-gray-800 rounded-2xl p-8">
                            <Shield className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                {t("why.insured.title")}
                            </h4>
                            <p className="text-gray-400">
                                {t("why.insured.description")}
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            
            <section>
                <CTA />
            </section>

        </div>
    );
}