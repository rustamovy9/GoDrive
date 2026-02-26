import { Link } from "react-router-dom";
import { UserMenu } from "../UserMenu/UserMenu";

export function Navbar() {
  return (
    <header className="sticky top-0 z-50 h-16 bg-white border-b border-gray-200">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-full">
        <div className="flex justify-between items-center h-full">
          <Link to="/" className="text-xl font-bold text-gray-900 tracking-tight">
            GoDrive
          </Link>
          <nav className="flex items-center gap-8">
            <Link
              to="/cars"
              className="text-gray-600 hover:text-gray-900 font-medium transition-colors duration-200"
            >
              Cars
            </Link>
            <UserMenu />
          </nav>
        </div>
      </div>
    </header>
  );
}
