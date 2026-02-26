import { Link } from "react-router-dom";

export function HomePage() {
  return (
    <section className="relative overflow-hidden">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center min-h-[calc(100vh-12rem)] py-12 lg:py-0">
          <div className="space-y-8">
            <div className="space-y-4">
              <h1 className="text-5xl font-bold text-gray-900 tracking-tight leading-tight">
                Rent the right car, every time
              </h1>
              <p className="text-xl text-gray-600 max-w-xl">
                Simple booking, transparent pricing. Find and drive the perfect car for your trip.
              </p>
            </div>
            <div className="flex flex-wrap gap-4">
              <Link
                to="/cars"
                className="inline-flex items-center justify-center bg-blue-600 hover:bg-blue-700 text-white rounded-lg px-6 py-3 font-medium transition-colors duration-200"
              >
                Browse Cars
              </Link>
              <Link
                to="/login"
                className="inline-flex items-center justify-center border border-gray-300 rounded-lg px-6 py-3 font-medium text-gray-700 hover:bg-gray-100 transition-colors duration-200"
              >
                Login
              </Link>
              <Link
                to="/register"
                className="inline-flex items-center justify-center border border-gray-300 rounded-lg px-6 py-3 font-medium text-gray-700 hover:bg-gray-100 transition-colors duration-200"
              >
                Register
              </Link>
            </div>
          </div>
          <div className="relative hidden lg:block">
            <div className="aspect-[4/3] rounded-2xl bg-gradient-to-br from-blue-50 to-gray-100 overflow-hidden shadow-xl">
              <div className="absolute inset-0 flex items-center justify-center">
                <svg
                  className="w-48 h-48 text-gray-300"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                  aria-hidden
                >
                  <path d="M18.92 6.01C18.72 5.42 18.16 5 17.5 5h-11c-.66 0-1.22.42-1.42 1.01L3 12v8c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-1h12v1c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-8l-2.08-5.99zM6.5 16c-.83 0-1.5-.67-1.5-1.5S5.67 13 6.5 13s1.5.67 1.5 1.5S7.33 16 6.5 16zm11 0c-.83 0-1.5-.67-1.5-1.5s.67-1.5 1.5-1.5 1.5.67 1.5 1.5-.67 1.5-1.5 1.5zM5 11l1.5-4.5h11L19 11H5z" />
                </svg>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
