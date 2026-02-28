import { Search, Clock, Zap } from 'lucide-react';

const steps = [
    {
        icon: Search,
        title: 'Find Your Car',
        description: 'Browse our curated collection of premium vehicles by location, category, and price.',
    },
    {
        icon: Clock,
        title: 'Book Instantly',
        description: 'Select your dates and confirm your booking in seconds. No paperwork needed.',
    },
    {
        icon: Zap,
        title: 'Hit the Road',
        description: 'Pick up your car and enjoy a seamless driving experience with 24/7 support.',
    },
];

export default function HowItWorks() {
    return (
        <section className="bg-gray-950 py-20 px-4 sm:px-6 lg:px-8">
            <div className="max-w-7xl mx-auto">
                <div className="text-center mb-16">
                    <h2 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        SIMPLE PROCESS
                    </h2>
                    <h3 className="text-4xl sm:text-5xl font-bold text-white">
                        How It Works
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