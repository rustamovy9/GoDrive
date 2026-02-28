import { Car, Users, Award, Heart, Shield, Zap } from 'lucide-react';
import Link from 'next/link';

export default function AboutPage() {
    const values = [
        {
            icon: Shield,
            title: 'Trust & Safety',
            description: 'Every vehicle and owner is thoroughly verified for your peace of mind.',
        },
        {
            icon: Heart,
            title: 'Customer First',
            description: 'Your satisfaction is our priority. We are here 24/7 to support you.',
        },
        {
            icon: Zap,
            title: 'Innovation',
            description: 'Using cutting-edge technology to make car rental simple and fast.',
        },
        {
            icon: Award,
            title: 'Quality',
            description: 'Only premium vehicles make it to our curated collection.',
        },
    ];

    const stats = [
        { label: 'Happy Customers', value: '10K+' },
        { label: 'Premium Cars', value: '500+' },
        { label: 'Cities Covered', value: '50+' },
        { label: 'Years Experience', value: '5+' },
    ];

    return (
        <div className="min-h-screen mt-[20px] bg-gray-950">
            <section className="relative py-20 px-4 sm:px-6 lg:px-8 overflow-hidden">
                <div className="max-w-7xl mx-auto text-center">
                    <h1 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        ABOUT US
                    </h1>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-bold text-white mb-6">
                        Revolutionizing Car Rental
                    </h2>
                    <p className="text-gray-400 text-lg sm:text-xl max-w-3xl mx-auto leading-relaxed">
                        GoDrive is a premium car rental platform that connects drivers with
                        exceptional vehicles from trusted owners. We make luxury accessible.
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
                                    Our Mission
                                </h3>
                                <div className="space-y-4">
                                    <p className="text-gray-400 text-lg leading-relaxed">
                                        We believe everyone deserves access to premium vehicles without the
                                        hassle of traditional rental companies. Our platform eliminates
                                        paperwork, long queues, and hidden fees.
                                    </p>
                                    <p className="text-gray-400 text-lg leading-relaxed">
                                        By connecting car owners with drivers, we create a community where
                                        luxury is accessible, affordable, and enjoyable for everyone.
                                    </p>
                                </div>
                                <div className="mt-8 flex flex-wrap gap-4">
                                    <Link
                                        href="/browse"
                                        className="inline-flex items-center gap-2 bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-3 px-8 rounded-xl transition-all duration-300 transform hover:scale-105"
                                    >
                                        Browse Our Cars
                                    </Link>
                                    <Link
                                        href="/about"
                                        className="inline-flex items-center gap-2 bg-gray-800 hover:bg-gray-700 text-white font-semibold py-3 px-8 rounded-xl border border-gray-700 transition-all duration-300"
                                    >
                                        Learn More
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
                            Our Core Values
                        </h3>
                        <p className="text-gray-400 text-lg max-w-2xl mx-auto">
                            These principles guide everything we do at GoDrive
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
                            Why Choose GoDrive?
                        </h3>
                        <p className="text-gray-400 text-lg max-w-2xl mx-auto">
                            We are more than just a car rental platform
                        </p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8">
                            <Users className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                Community Driven
                            </h4>
                            <p className="text-gray-400 leading-relaxed">
                                Built by drivers, for drivers. We understand what matters most when
                                you are behind the wheel.
                            </p>
                        </div>

                        <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8">
                            <Award className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                Premium Quality
                            </h4>
                            <p className="text-gray-400 leading-relaxed">
                                Every vehicle in our collection meets strict quality standards.
                                No compromises, only the best.
                            </p>
                        </div>

                        <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8">
                            <Shield className="w-12 h-12 text-cyan-400 mb-6" />
                            <h4 className="text-xl font-semibold text-white mb-3">
                                Fully Insured
                            </h4>
                            <p className="text-gray-400 leading-relaxed">
                                Every rental comes with comprehensive insurance coverage. Drive with
                                complete peace of mind.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            <section className="py-20 px-4 sm:px-6 lg:px-8">
                <div className="max-w-5xl mx-auto">
                    <div className="bg-gradient-to-br from-cyan-400/10 to-purple-600/10 border border-cyan-400/20 rounded-3xl p-12 sm:p-16 text-center">
                        <h3 className="text-3xl sm:text-4xl font-bold text-white mb-6">
                            Ready to Experience GoDrive?
                        </h3>
                        <p className="text-gray-400 text-lg max-w-2xl mx-auto mb-10">
                            Join thousands of satisfied drivers and discover a new way to rent
                            premium vehicles.
                        </p>
                        <div className="flex flex-col sm:flex-row gap-4 justify-center">
                            <Link
                                href="/signup"
                                className="bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-4 px-10 rounded-xl transition-all duration-300 transform hover:scale-105"
                            >
                                Create Account
                            </Link>
                            <Link
                                href="/browse"
                                className="bg-gray-800 hover:bg-gray-700 text-white font-semibold py-4 px-10 rounded-xl border border-gray-700 transition-all duration-300"
                            >
                                Browse Cars
                            </Link>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    );
}