import { NavLink } from "react-router-dom";

interface LinkItem {
  to: string;
  label: string;
}

export function Sidebar({ links, title }: { links: LinkItem[]; title: string }) {
  return (
    <aside className="fixed left-0 top-0 w-64 h-screen bg-white border-r border-gray-200 z-30">
      <div className="flex flex-col h-full pt-20 px-4">
        <h2 className="text-xl font-semibold text-gray-800 mb-6 px-3">
          {title}
        </h2>
        <nav className="space-y-1">
          {links.map(({ to, label }) => (
            <NavLink
              key={to}
              to={to}
              className={({ isActive }) =>
                `block rounded-lg px-3 py-2 text-sm font-medium transition-colors duration-200 ${
                  isActive
                    ? "bg-blue-50 text-blue-700"
                    : "text-gray-600 hover:bg-gray-100"
                }`
              }
            >
              {label}
            </NavLink>
          ))}
        </nav>
      </div>
    </aside>
  );
}
