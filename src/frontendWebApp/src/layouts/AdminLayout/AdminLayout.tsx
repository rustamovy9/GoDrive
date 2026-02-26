import { Link, Outlet } from "react-router-dom";
import { Sidebar } from "../../widgets/Sidebar/Sidebar";

const links = [
  { to: "/admin/dashboard", label: "Dashboard" },
  { to: "/admin/users", label: "Users" },
  { to: "/admin/cars", label: "Cars" },
  { to: "/admin/documents", label: "Documents" },
  { to: "/admin/settings", label: "Settings" },
];

export function AdminLayout() {
  return (
    <div className="min-h-screen bg-gray-50">
      <Sidebar links={links} title="Admin" />
      <div className="ml-64 min-h-screen">
        <header className="sticky top-0 z-40 h-16 bg-white border-b border-gray-200 flex items-center px-8">
          <Link to="/" className="text-lg font-semibold text-gray-900">
            GoDrive
          </Link>
        </header>
        <main className="px-8 py-10">
          <div className="max-w-7xl mx-auto">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}
