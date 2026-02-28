import { Star } from 'lucide-react';

const testimonials = [
    {
        name: 'Sarah J.',
        rating: 5,
        text: 'Absolutely amazing experience. The car was spotless and drove like a dream!',
    },
    {
        name: 'Mike T.',
        rating: 4,
        text: 'Great service, smooth booking process. Will definitely use again.',
    },
    {
        name: 'Emily R.',
        rating: 5,
        text: 'Premium quality all the way. The best rental experience I\'ve had.',
    },
];

export default function Testimonials() {
    return (
        <section className="bg-gray-950 py-20 px-4 sm:px-6 lg:px-8">
            <div className="max-w-7xl mx-auto">
                <div className="text-center mb-16">
                    <h2 className="text-cyan-400 font-semibold tracking-wider text-sm uppercase mb-3">
                        TESTIMONIALS
                    </h2>
                    <h3 className="text-4xl sm:text-5xl font-bold text-white">
                        What Our Users Say
                    </h3>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 max-w-6xl mx-auto">
                    {testimonials.map((testimonial, index) => (
                        <div
                            key={index}
                            className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-2xl p-8 hover:border-cyan-400/30 transition-all duration-300"
                        >
                            <div className="flex gap-1 mb-4">
                                {[...Array(5)].map((_, i) => (
                                    <Star
                                        key={i}
                                        className={`w-5 h-5 ${i < testimonial.rating
                                                ? 'fill-yellow-400 text-yellow-400'
                                                : 'fill-gray-700 text-gray-700'
                                            }`}
                                    />
                                ))}
                            </div>

                            <p className="text-gray-400 mb-6 leading-relaxed">
                                "{testimonial.text}"
                            </p>

                            <p className="text-white font-semibold">
                                {testimonial.name}
                            </p>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}