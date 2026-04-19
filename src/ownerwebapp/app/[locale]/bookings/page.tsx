"use client";

import { MapPin } from "lucide-react";
import { useEffect, useState } from "react";
import { useTranslations } from "next-intl";

interface Booking {
  id: number;
  carId: number;
  startDateTime: string;
  endDateTime: string;
  totalPrice: number;
  bookingStatus: number;
  pickupCity: string;
  dropOffCity: string;
}

interface Car {
  id: number;
  brand: string;
  model: string;
  year: number;
  location: string;
  images: string[];
}

export default function BookingPage() {
  const t = useTranslations("BookingPage");

  const [bookings, setBookings] = useState<Booking[]>([]);
  const [cars, setCars] = useState<{ [key: number]: Car }>({});
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState<number | null>(null);

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) return;

        const res = await fetch(
          "https://godrive-5r3o.onrender.com/api/bookings",
          { headers: { Authorization: `Bearer ${token}` } }
        );

        const data = await res.json();

        if (data.isSuccess) {
          const bookingsData = data.data.data;
          setBookings(bookingsData);

          const carMap: any = {};
          await Promise.all(
            bookingsData.map(async (b: Booking) => {
              const carRes = await fetch(
                `https://godrive-5r3o.onrender.com/api/cars/${b.carId}`,
                { headers: { Authorization: `Bearer ${token}` } }
              );
              const carData = await carRes.json();
              if (carData.isSuccess) carMap[b.carId] = carData.data;
            })
          );
          setCars(carMap);
        }
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchAll();
  }, []);

  const getStatus = (status: number) => {
    switch (status) {
      case 0: return t("status.pending");
      case 1: return t("status.confirmed");
      case 2: return t("status.cancelled");
      case 3: return t("status.completed");
      case 4: return t("status.rejected");
      default: return t("status.unknown");
    }
  };

  const getStatusColor = (status: number) => {
    switch (status) {
      case 0: return "bg-yellow-500/20 text-yellow-400";
      case 1: return "bg-green-500/20 text-green-400";
      case 2: return "bg-red-500/20 text-red-400";
      case 3: return "bg-blue-500/20 text-blue-400";
      case 4: return "bg-pink-500/20 text-pink-400";
      default: return "bg-gray-500/20 text-gray-400";
    }
  };

  const updateBookingStatus = async (id: number, status: number) => {
    try {
      const token = localStorage.getItem("token");
      if (!token) return;
      setActionLoading(id);

      await fetch(
        `https://godrive-5r3o.onrender.com/api/bookings/${id}/status`,
        {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({ status, reason: "Updated by owner" }),
        }
      );

      setBookings((prev) =>
        prev.map((b) => (b.id === id ? { ...b, bookingStatus: status } : b))
      );
    } catch (err) {
      console.error(err);
    } finally {
      setActionLoading(null);
    }
  };

  if (loading) {
    return (
      <div className="p-4 space-y-4">
        {[1, 2, 3].map((i) => (
          <div key={i} className="animate-pulse bg-zinc-900 h-40 rounded-xl" />
        ))}
      </div>
    );
  }

  return (
    <div className="p-4 md:p-6 text-white">
      <h1 className="text-2xl md:text-3xl font-bold mb-6">
        {t("title")}
      </h1>

      <div className="space-y-4">
        {bookings.map((b) => {
          const car = cars[b.carId];

          return (
            <div key={b.id} className="flex flex-col sm:flex-row gap-4 bg-zinc-900/80 border border-zinc-800 rounded-2xl overflow-hidden">

              <div className="w-full sm:w-60 h-40 bg-zinc-800">
                {car?.images?.length > 0 ? (
                  <img src={car.images[0]} className="w-full h-full object-cover" />
                ) : (
                  <div className="flex items-center justify-center h-full text-zinc-500">
                    {t("noImage")}
                  </div>
                )}
              </div>

              <div className="flex-1 p-4 sm:p-5">
                <div className="flex justify-between mb-2 items-center">
                  <h2 className="text-lg font-semibold">
                    {t("booking")} #{b.id}
                  </h2>

                  <span className={`px-3 py-1 text-sm rounded-full ${getStatusColor(b.bookingStatus)}`}>
                    {getStatus(b.bookingStatus)}
                  </span>
                </div>

                {car && (
                  <p className="text-cyan-400">
                    {car.brand} {car.model} ({car.year})
                  </p>
                )}

                <div className="flex items-center gap-2 my-2">
                  <MapPin size={14} className="text-cyan-400" />
                  <span>{car?.location || t("unknownLocation")}</span>
                </div>

                <p className="text-zinc-400">
                  {t("route")}: {b.pickupCity} → {b.dropOffCity}
                </p>

                <p className="text-zinc-400">
                  {new Date(b.startDateTime).toLocaleDateString()} -{" "}
                  {new Date(b.endDateTime).toLocaleDateString()}
                </p>

                <p className="text-green-400 font-bold mt-3 text-lg">
                  {b.totalPrice} TJS
                </p>

                {b.bookingStatus === 0 && (
                  <div className="flex gap-3 mt-4">
                    <button
                      disabled={actionLoading === b.id}
                      onClick={() => updateBookingStatus(b.id, 1)}
                      className="px-4 py-2 bg-green-500 text-black rounded-lg disabled:opacity-50"
                    >
                      {actionLoading === b.id ? t("loading") : t("accept")}
                    </button>

                    <button
                      disabled={actionLoading === b.id}
                      onClick={() => updateBookingStatus(b.id, 4)}
                      className="px-4 py-2 bg-red-500 text-white rounded-lg disabled:opacity-50"
                    >
                      {t("reject")}
                    </button>
                  </div>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}