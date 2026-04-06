import { ArrowRight, Car, MapPin, Search, Users } from "lucide-react";
import { useTranslations } from "next-intl";
import Head from "next/head";
import HowItWorks from "./pages/howItWorks";
import WhyChooseUs from "./pages/whyChooseUs";
import Testimonials from "./pages/testimonials";
import CTA from "./pages/cta";

export default function Home() {
  const t = useTranslations("Homepage");

  return (
    <div className="pt-[50px]">
      <div>
        <Head>
          <title>{t("title")}</title>
          <meta name="description" content={t("description")} />
        </Head>

        <div className="min-h-screen bg-gray-950 relative overflow-hidden">
          <div
            className="absolute inset-0 bg-cover bg-center bg-no-repeat"
            style={{
              backgroundImage: 'url("https://www.topgear.com/sites/default/files/news-listicle/image/2023/09/LEAD.jpg")',
            }}
          >
            <div className="absolute inset-0 bg-gradient-to-b from-gray-950/80 via-gray-950/70 to-gray-950/90"></div>
          </div>

          <div className="relative z-10 container mx-auto px-4 sm:px-6 lg:px-8">
            <header className="pt-8 pb-4">
              <nav className="flex justify-center">
                <span className="text-cyan-400 font-semibold tracking-wider text-sm uppercase">
                  {t("header")}
                </span>
              </nav>
            </header>

            <main className="flex flex-col items-center justify-center min-h-[70vh] text-center">
              <h1 className="text-5xl sm:text-6xl lg:text-7xl font-bold text-white mb-6 leading-tight">
                {t.rich("main.headline", {
                  highlight: (chunks) => (
                    <span className="text-cyan-400">{chunks}</span>
                  ),
                })}
              </h1>

              <p className="text-gray-300 text-lg sm:text-xl max-w-3xl mb-10 leading-relaxed">
                {t("main.subtext")}
              </p>

              <div className="flex flex-col sm:flex-row gap-4 mb-16">
                <button className="group bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-semibold py-4 px-8 rounded-xl flex items-center justify-center gap-3 transition-all duration-300 transform hover:scale-105 shadow-lg shadow-cyan-400/25">
                  <Search className="w-5 h-5" />
                  {t("main.buttons.browse")}
                </button>

                <button className="group bg-gray-900/80 hover:bg-gray-800 backdrop-blur-sm border border-gray-700 text-white font-semibold py-4 px-8 rounded-xl flex items-center justify-center gap-3 transition-all duration-300 transform hover:scale-105">
                  {t("main.buttons.getStarted")}
                  <ArrowRight className="w-5 h-5 group-hover:translate-x-1 transition-transform" />
                </button>
              </div>

              <div className="grid grid-cols-3 gap-8 sm:gap-16 max-w-3xl w-full">
                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <Car className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    {t("main.stats.cars")}
                  </div>
                  <div className="text-gray-400 text-sm">{t("main.statsLabels.cars")}</div>
                </div>

                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <Users className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    {t("main.stats.users")}
                  </div>
                  <div className="text-gray-400 text-sm">{t("main.statsLabels.users")}</div>
                </div>

                <div className="flex flex-col items-center">
                  <div className="bg-cyan-400/10 p-4 rounded-2xl mb-3">
                    <MapPin className="w-8 h-8 text-cyan-400" />
                  </div>
                  <div className="text-3xl sm:text-4xl font-bold text-white mb-1">
                    {t("main.stats.cities")}
                  </div>
                  <div className="text-gray-400 text-sm">{t("main.statsLabels.cities")}</div>
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