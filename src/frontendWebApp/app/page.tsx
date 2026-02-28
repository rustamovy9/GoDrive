import { ArrowRight, Car, MapPin, Search, Users } from "lucide-react";
import Head from "next/head";
import HowItWorks from "./pages/howItWorks";
import WhyChooseUs from "./pages/whyChooseUs";
import Testimonials from "./pages/testimonials";
import CTA from "./pages/cta";

export default function Home() {
  return (
    <div className="mt-[50px] ">
      <div>
        <Head>
          <title>Premium Car Rental - Drive Your Dream Car</title>
          <meta name="description" content="Access hundreds of premium vehicles from trusted owners" />
        </Head>

        <div className="min-h-screen bg-gray-950 relative overflow-hidden">
          <div
            className="absolute inset-0 bg-cover bg-center bg-no-repeat"
            style={{
              backgroundImage: 'url("https://www.reliancegeneral.co.in/siteassets/rgiclassets/images/blogs-images/top-5-fastest-cars-in-the-world2.webp")',
            }}
          >
            <div className="absolute inset-0 bg-gradient-to-b from-gray-950/80 via-gray-950/70 to-gray-950/90"></div>
          </div>

          <div className="relative z-10 container mx-auto px-4 sm:px-6 lg:px-8">
            <header className="pt-8 pb-4">
              <nav className="flex justify-center">
                <span className="text-cyan-400 font-semibold tracking-wider text-sm uppercase">
                  Premium Car Rental
                </span>
              </nav>
            </header>

            <main className="flex flex-col items-center justify-center min-h-[70vh] text-center">
              <h1 className="text-5xl sm:text-6xl lg:text-7xl font-bold text-white mb-6 leading-tight">
                Drive Your{' '}
                <span className="text-cyan-400">Dream Car</span>
                <br />
                Today
              </h1>

              <p className="text-gray-300 text-lg sm:text-xl max-w-3xl mb-10 leading-relaxed">
                Access hundreds of premium vehicles from trusted owners.
                Book instantly, drive anywhere.
              </p>

              <div className="flex flex-col sm:flex-row gap-4 mb-16">
                <button className="group bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-4 px-8 rounded-xl flex items-center justify-center gap-3 transition-all duration-300 transform hover:scale-105 shadow-lg shadow-cyan-400/25">
                  <Search className="w-5 h-5" />
                  Browse Cars
                </button>

                <button className="group bg-gray-900/80 hover:bg-gray-800 backdrop-blur-sm border border-gray-700 text-white font-semibold py-4 px-8 rounded-xl flex items-center justify-center gap-3 transition-all duration-300 transform hover:scale-105">
                  Get Started
                  <ArrowRight className="w-5 h-5 group-hover:translate-x-1 transition-transform" />
                </button>
              </div>

              <div className="grid grid-cols-3 gap-8 sm:gap-16 max-w-3xl w-full">
                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <Car className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    500+
                  </div>
                  <div className="text-gray-400 text-sm">Cars</div>
                </div>

                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <Users className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    10K+
                  </div>
                  <div className="text-gray-400 text-sm">Users</div>
                </div>

                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <MapPin className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    50+
                  </div>
                  <div className="text-gray-400 text-sm">Cities</div>
                </div>
              </div>
            </main>
          </div>

          <div className="absolute bottom-0 left-0 right-0 h-32 bg-gradient-to-t from-gray-950 to-transparent"></div>
        </div>
      </div>
      <HowItWorks />
      <WhyChooseUs />
      <Testimonials />
      <CTA />
    </div>
  );
}
