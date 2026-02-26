import { Link } from "react-router-dom";

export function AdminDashboardPage() {
  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Admin dashboard</h1>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Link to="/admin/users" className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6 block">
          <h2 className="text-xl font-semibold text-gray-800">Users</h2>
          <p className="text-gray-500 text-sm mt-1">Manage users</p>
        </Link>
        <Link to="/admin/cars" className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6 block">
          <h2 className="text-xl font-semibold text-gray-800">Cars</h2>
          <p className="text-gray-500 text-sm mt-1">Manage all cars</p>
        </Link>
        <Link to="/admin/documents" className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6 block">
          <h2 className="text-xl font-semibold text-gray-800">Documents</h2>
          <p className="text-gray-500 text-sm mt-1">Review documents</p>
        </Link>
      </div>
    </div>
  );
}
