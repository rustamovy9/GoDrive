import { Shield, Clock, Zap, Star } from 'lucide-react';

const features = [
    {
        icon: Shield,
        title: 'Fully Insured',
        description: 'Every rental comes with comprehensive insurance coverage.',
    },
    {
        icon: Clock,
        title: '24/7 Support',
        description: 'Round-the-clock customer support whenever you need it.',
    },
    {
        icon: Zap,
        title: 'Instant Booking',
        description: 'No waiting. Confirm your car in seconds.',
    },
    {
        icon: Star,
        title: 'Verified Owners',
        description: 'All car owners are verified and reviewed by the community.',
    },
];

export default function WhyChooseUs() {
    return (
        <section className="bg-gray-950 py-20 px-4 sm:px-6 lg:px-8">
            <div className="max-w-7xl mx-auto">
                <div className="text-center mb-16">
                    <h2 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        WHY GODRIVE
                    </h2>
                    <h3 className="text-4xl sm:text-5xl font-bold text-white">
                        Built for Modern Drivers
                    </h3>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 max-w-5xl mx-auto">
                    {features.map((feature, index) => (
                        <div
                            key={index}
                            className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8 hover:border-cyan-400/50 transition-all duration-300 group"
                        >
                            <div className="flex justify-center mb-6">
                                <div className="bg-gray-800 group-hover:bg-cyan-400/10 rounded-xl p-4 transition-colors duration-300">
                                    <feature.icon className="w-8 h-8 text-cyan-400" />
                                </div>
                            </div>

                            <h4 className="text-xl font-semibold text-white text-center mb-3">
                                {feature.title}
                            </h4>

                            <p className="text-gray-400 text-center leading-relaxed">
                                {feature.description}
                            </p>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}