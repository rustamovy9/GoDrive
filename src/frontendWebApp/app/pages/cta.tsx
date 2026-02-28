export default function CTA() {
    return (
        <section className="bg-gray-950 py-20 px-4 sm:px-6 lg:px-8">
            <div className="max-w-5xl mx-auto">
                <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-3xl p-12 sm:p-16 text-center hover:border-cyan-400/30 transition-all duration-300">
                    <h2 className="text-4xl sm:text-5xl font-bold text-white mb-6">
                        Ready to Drive?
                    </h2>

                    <p className="text-gray-400 text-lg sm:text-xl max-w-2xl mx-auto mb-10 leading-relaxed">
                        Join thousands of happy drivers. Sign up today and get your first ride.
                    </p>

                    <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
                        <button className="w-full sm:w-auto bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-4 px-10 rounded-xl transition-all duration-300 transform hover:scale-105 shadow-lg shadow-cyan-400/25">
                            Create Account
                        </button>

                        <button className="w-full sm:w-auto bg-gray-800 hover:bg-gray-700 text-white font-semibold py-4 px-10 rounded-xl border border-gray-700 transition-all duration-300 transform hover:scale-105">
                            Browse Cars
                        </button>
                    </div>
                </div>
            </div>
        </section>
    );
}