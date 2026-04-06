'use client';

import { useState } from 'react';
import { Mail, Phone, MapPin, Clock, Send } from 'lucide-react';
import CTA from '../pages/cta';
import { useTranslations } from 'next-intl';

export default function ContactPage() {
    const t = useTranslations("Contact");

    const [formData, setFormData] = useState({
        name: '',
        email: '',
        subject: '',
        message: '',
    });

    const handleChange = (e: any) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = (e: any) => {
        e.preventDefault();
        console.log('Contact form:', formData);
        setFormData({ name: '', email: '', subject: '', message: '' });
    };

    const contactInfo = [
        {
            icon: Mail,
            title: t("info.email"),
            value: 'yusufrstmv9@gmail.com',
            href: 'mailto:yusufrstmv9@gmail.com',
        },
        {
            icon: Phone,
            title: t("info.phone"),
            value: '+992 203617007',
            href: 'tel:+992203617007',
        },
        {
            icon: MapPin,
            title: t("info.address"),
            value: t("info.addressValue"),
            href: '#',
        },
        {
            icon: Clock,
            title: t("info.hours"),
            value: t("info.hoursValue"),
            href: '#',
        },
    ];

    return (
        <div className="min-h-screen mt-[20px] bg-gray-950">

            <section className="py-20 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto text-center">
                    <h1 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        {t("header.small")}
                    </h1>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-bold text-white mb-6">
                        {t("header.title")}
                    </h2>
                    <p className="text-gray-400 text-lg sm:text-xl max-w-3xl mx-auto leading-relaxed">
                        {t("header.desc")}
                    </p>
                </div>
            </section>

            <section className="py-12 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-16">
                        {contactInfo.map((info, index) => (
                            <div key={index} className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-6 hover:border-cyan-400/50 transition-all duration-300 group">
                                <div className="bg-gray-800 group-hover:bg-cyan-400/10 rounded-xl p-3 w-fit mb-4">
                                    <info.icon className="w-6 h-6 text-cyan-400" />
                                </div>
                                <h3 className="text-white font-semibold mb-2">{info.title}</h3>
                                <a href={info.href} className="text-gray-400 hover:text-cyan-400 text-sm">
                                    {info.value}
                                </a>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            <section className="py-12 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto grid grid-cols-1 lg:grid-cols-2 gap-8">

                    <div className="bg-gray-900/50 border border-gray-800 rounded-2xl p-8">
                        <h3 className="text-2xl font-bold text-white mb-6">
                            {t("form.title")}
                        </h3>

                        <form onSubmit={handleSubmit} className="space-y-5">

                            <input
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder={t("form.name")}
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white"
                                required
                            />

                            <input
                                name="email"
                                type="email"
                                value={formData.email}
                                onChange={handleChange}
                                placeholder={t("form.email")}
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white"
                                required
                            />

                            <select
                                name="subject"
                                value={formData.subject}
                                onChange={handleChange}
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white"
                                required
                            >
                                <option value="">{t("form.select")}</option>
                                <option value="general">{t("form.general")}</option>
                                <option value="booking">{t("form.booking")}</option>
                                <option value="support">{t("form.support")}</option>
                                <option value="partnership">{t("form.partner")}</option>
                                <option value="other">{t("form.other")}</option>
                            </select>

                            <textarea
                                name="message"
                                value={formData.message}
                                onChange={handleChange}
                                placeholder={t("form.message")}
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white"
                                required
                            />

                            <button className="w-full bg-cyan-400 text-gray-950 font-semibold py-3 rounded-xl flex justify-center gap-2">
                                <Send className="w-5 h-5" />
                                {t("form.send")}
                            </button>

                        </form>
                    </div>

                    <div className="bg-gray-900/50 border border-gray-800 rounded-2xl flex items-center justify-center text-center p-8">
                        <div>
                            <MapPin className="w-16 h-16 text-cyan-400 mx-auto mb-4" />
                            <h4 className="text-xl font-semibold text-white mb-2">
                                {t("visit.title")}
                            </h4>
                            <p className="text-gray-400">
                                {t("visit.address")}
                            </p>
                        </div>
                    </div>

                </div>
            </section>

            <CTA />
        </div>
    );
}