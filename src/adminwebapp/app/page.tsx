"use client";

import Link from "next/link";
import { Users, Car, FileText, Star, Settings } from "lucide-react";
import { JSX, useEffect, useState } from "react";

export default function AdminDashboard(): JSX.Element {
  const [totalUsers, setTotalUsers] = useState<number>(0);
  const [totalCars, setTotalCars] = useState<number>(0);
  const [totalBookings, setTotalBookings] = useState<number>(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      try {
        const token = localStorage.getItem("token");
        const headers = {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        };

        const usersRes = await fetch(
          "https://godrive-5r3o.onrender.com/api/users",
          { headers }
        );
        const usersData = await usersRes.json();
        setTotalUsers(usersData.data?.data?.length || 0);

        const carsRes = await fetch(
          "https://godrive-5r3o.onrender.com/api/cars",
          { headers }
        );
        const carsData = await carsRes.json();
        setTotalCars(carsData.data?.totalRecords || 0);

        const bookingsRes = await fetch(
          "https://godrive-5r3o.onrender.com/api/bookings",
          { headers }
        );
        const bookingsData = await bookingsRes.json();
        setTotalBookings(bookingsData.data?.totalRecords || 0);

      } catch (error) {
        console.error("Error fetching dashboard ", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <main className="flex-1 p-10">
      <h1 className="text-3xl font-bold mb-8">Admin Dashboard</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">

        <div className="bg-zinc-900 rounded-xl p-6 border border-zinc-800">
          <p className="text-zinc-400 text-sm mb-2">Total Users</p>
          <p className="text-3xl font-bold text-cyan-400">
            {loading ? "..." : totalUsers}
          </p>
        </div>

        <div className="bg-zinc-900 rounded-xl p-6 border border-zinc-800">
          <p className="text-zinc-400 text-sm mb-2">Total Cars</p>
          <p className="text-3xl font-bold text-green-500">
            {loading ? "..." : totalCars}
          </p>
        </div>

        <div className="bg-zinc-900 rounded-xl p-6 border border-zinc-800">
          <p className="text-zinc-400 text-sm mb-2">Total Bookings</p>
          <p className="text-3xl font-bold text-yellow-500">
            {loading ? "..." : totalBookings}
          </p>
        </div>

        <div className="bg-zinc-900 rounded-xl p-6 border border-zinc-800">
          <p className="text-zinc-400 text-sm mb-2">Revenue</p>
          <p className="text-3xl font-bold text-cyan-400">$84,320</p>
        </div>

      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <Link
          href="/users"
          className="bg-zinc-900 rounded-xl p-6 border border-zinc-800 hover:border-zinc-700 transition-colors"
        >
          <div className="flex items-center gap-3 mb-2">
            <Users className="text-zinc-400" size={20} />
            <h3 className="text-lg font-semibold">Users</h3>
          </div>
          <p className="text-zinc-400 text-sm">Manage users</p>
        </Link>

        <Link
          href="/cars"
          className="bg-zinc-900 rounded-xl p-6 border border-zinc-800 hover:border-zinc-700 transition-colors"
        >
          <div className="flex items-center gap-3 mb-2">
            <Car className="text-zinc-400" size={20} />
            <h3 className="text-lg font-semibold">Cars</h3>
          </div>
          <p className="text-zinc-400 text-sm">Manage cars</p>
        </Link>

        <Link
          href="/documents"
          className="bg-zinc-900 rounded-xl p-6 border border-zinc-800 hover:border-zinc-700 transition-colors"
        >
          <div className="flex items-center gap-3 mb-2">
            <FileText className="text-zinc-400" size={20} />
            <h3 className="text-lg font-semibold">Documents</h3>
          </div>
          <p className="text-zinc-400 text-sm">Manage documents</p>
        </Link>

        <Link
          href="/reviews"
          className="bg-zinc-900 rounded-xl p-6 border border-zinc-800 hover:border-zinc-700 transition-colors"
        >
          <div className="flex items-center gap-3 mb-2">
            <Star className="text-zinc-400" size={20} />
            <h3 className="text-lg font-semibold">Reviews</h3>
          </div>
          <p className="text-zinc-400 text-sm">Manage reviews</p>
        </Link>

        <Link
          href="/settings"
          className="bg-zinc-900 rounded-xl p-6 border border-zinc-800 hover:border-zinc-700 transition-colors"
        >
          <div className="flex items-center gap-3 mb-2">
            <Settings className="text-zinc-400" size={20} />
            <h3 className="text-lg font-semibold">Settings</h3>
          </div>
          <p className="text-zinc-400 text-sm">Manage settings</p>
        </Link>
      </div>
    </main>
  );
}