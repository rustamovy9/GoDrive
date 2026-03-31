"use client";

import { useEffect, useState } from "react";
import { Car, DollarSign, Calendar, Plus } from "lucide-react";
import Link from "next/link";

export default function OwnerDashboard() {
  const [activeCars, setActiveCars] = useState(0);
  const [pendingBookings, setPendingBookings] = useState(0);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return;

    const fetchCars = async () => {
      try {
        const res = await fetch(
          "https://godrive-5r3o.onrender.com/api/cars",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        const data = await res.json();

        if (data.isSuccess) {
          const cars = data.data.data;
          const active = cars.filter((car: any) => car.carStatus === 1);
          setActiveCars(active.length);
        }
      } catch (err) {
        console.error(err);
      }
    };

    const fetchBookings = async () => {
      try {
        const res = await fetch(
          "https://godrive-5r3o.onrender.com/api/bookings",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        const data = await res.json();

        if (data.isSuccess) {
          const bookings = data.data.data;

          const pending = bookings.filter(
            (b: any) => b.bookingStatus === 0
          );

          setPendingBookings(pending.length);
        }
      } catch (err) {
        console.error(err);
      }
    };

    fetchCars();
    fetchBookings();
  }, []);

  return (
    <main className="flex-1 p-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">
          Owner Dashboard
        </h1>
        <p className="text-zinc-400">
          Welcome back! Here's what's happening with your cars.
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div className="bg-zinc-900/80 backdrop-blur-xl rounded-xl p-6 border border-zinc-800">
          <div className="flex items-center gap-3 mb-3">
            <DollarSign className="text-green-500" size={24} />
            <p className="text-zinc-400 text-sm">Total Income</p>
          </div>
          <p className="text-3xl font-bold text-green-500">12200</p>
        </div>

        <div className="bg-zinc-900/80 backdrop-blur-xl rounded-xl p-6 border border-zinc-800">
          <div className="flex items-center gap-3 mb-3">
            <Car className="text-cyan-400" size={24} />
            <p className="text-zinc-400 text-sm">Active Cars</p>
          </div>
          <p className="text-3xl font-bold text-cyan-400">
            {activeCars}
          </p>
        </div>

        <div className="bg-zinc-900/80 backdrop-blur-xl rounded-xl p-6 border border-zinc-800">
          <div className="flex items-center gap-3 mb-3">
            <Calendar className="text-yellow-500" size={24} />
            <p className="text-zinc-400 text-sm">Pending Bookings</p>
          </div>
          <p className="text-3xl font-bold text-yellow-500">
            {pendingBookings}
          </p>
        </div>
      </div>

      <div className="flex gap-4">
        <Link href="/addcar">
          <button className="flex items-center gap-2 bg-cyan-500 hover:bg-cyan-600 text-black font-semibold px-6 py-3 rounded-lg transition-colors">
            <Plus size={20} />
            Add New Car
          </button>
        </Link>
        <Link href="/bookings">
          <button className="flex items-center gap-2 bg-zinc-800 hover:bg-zinc-700 text-white font-semibold px-6 py-3 rounded-lg border border-zinc-700 transition-colors">
            View Bookings
          </button>
        </Link>
      </div>
    </main>
  );
}