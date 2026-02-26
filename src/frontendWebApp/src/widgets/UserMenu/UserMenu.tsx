import { useState, useRef, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../features/auth/useAuth";
import { Avatar } from "../../shared/components/Avatar";

export function UserMenu() {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);
  const navigate = useNavigate();

  useEffect(() => {
    const fn = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener("mousedown", fn);
    return () => document.removeEventListener("mousedown", fn);
  }, []);

  async function onLogout() {
    await logout();
    setOpen(false);
    navigate("/");
  }

  if (!user) {
    return (
      <div className="flex items-center gap-3">
        <Link
          to="/login"
          className="border border-gray-300 rounded-lg px-4 py-2 font-medium text-gray-700 hover:bg-gray-100 transition-colors duration-200"
        >
          Log in
        </Link>
        <Link
          to="/register"
          className="bg-blue-600 hover:bg-blue-700 text-white rounded-lg px-4 py-2 font-medium transition-colors duration-200"
        >
          Sign up
        </Link>
      </div>
    );
  }

  return (
    <div className="relative" ref={ref}>
      <button
        type="button"
        onClick={() => setOpen(!open)}
        className="flex items-center gap-2 rounded-full focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors duration-200"
      >
        <Avatar src={user.avatarUrl} name={user.name} size="md" />
        <span className="text-gray-600 font-medium hidden sm:inline">{user.name}</span>
      </button>
      {open && (
        <div className="absolute right-0 mt-2 w-52 bg-white rounded-xl shadow-lg border border-gray-200 py-1 z-50">
          <Link
            to="/profile"
            className="block px-4 py-2.5 text-gray-600 hover:bg-gray-100 rounded-lg mx-1 transition-colors duration-200"
            onClick={() => setOpen(false)}
          >
            Profile
          </Link>
          <Link
            to="/my-bookings"
            className="block px-4 py-2.5 text-gray-600 hover:bg-gray-100 rounded-lg mx-1 transition-colors duration-200"
            onClick={() => setOpen(false)}
          >
            My bookings
          </Link>
          <Link
            to="/notifications"
            className="block px-4 py-2.5 text-gray-600 hover:bg-gray-100 rounded-lg mx-1 transition-colors duration-200"
            onClick={() => setOpen(false)}
          >
            Notifications
          </Link>
          {user.role === "Owner" && (
            <Link
              to="/owner/dashboard"
              className="block px-4 py-2.5 text-gray-600 hover:bg-gray-100 rounded-lg mx-1 transition-colors duration-200"
              onClick={() => setOpen(false)}
            >
              Owner dashboard
            </Link>
          )}
          {user.role === "Admin" && (
            <Link
              to="/admin/dashboard"
              className="block px-4 py-2.5 text-gray-600 hover:bg-gray-100 rounded-lg mx-1 transition-colors duration-200"
              onClick={() => setOpen(false)}
            >
              Admin
            </Link>
          )}
          <button
            type="button"
            onClick={onLogout}
            className="w-full text-left px-4 py-2.5 text-red-600 hover:bg-red-50 rounded-lg mx-1 transition-colors duration-200"
          >
            Log out
          </button>
        </div>
      )}
    </div>
  );
}
