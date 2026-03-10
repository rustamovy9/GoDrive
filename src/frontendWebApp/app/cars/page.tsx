"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { Search, Star } from "lucide-react";

interface Car {
  id: number;
  brand: string;
  model: string;
  year: number;
  images: string[];
  carStatus: number;
  locationId: number;
}

interface Category {
  id: number;
  name: string;
}

interface Location {
  id: number;
  country: string;
  city: string;
}

interface CarWithPrice extends Car {
  pricePerDay: number | null;
}

export default function BrowseCars() {
  const [cars, setCars] = useState<CarWithPrice[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [locations, setLocations] = useState<Location[]>([]);

  const [search, setSearch] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
  const [selectedLocation, setSelectedLocation] = useState<number | null>(null);
  const [selectedStatus, setSelectedStatus] = useState<number | null>(null);

  const [loading, setLoading] = useState(true);

  const token =
    typeof window !== "undefined" ? localStorage.getItem("token") : null;

  const statusConfig: Record<number, { label: string; color: string }> = {
    0: { label: "PendingApproval", color: "bg-gray-500" },
    1: { label: "Available", color: "bg-green-500" },
    2: { label: "Rented", color: "bg-yellow-500" },
    3: { label: "Maintenance", color: "bg-blue-500" },
    4: { label: "Blocked", color: "bg-red-600" },
  };

  const fetchCategories = async () => {
    try {
      const res = await fetch(
        "https://godrive-ruc4.onrender.com/api/categories",
        { headers: { Authorization: `Bearer ${token}` } }
      );
      const data = await res.json();
      if (data.isSuccess) setCategories(data.data.data);
    } catch (err) {
      console.error(err);
    }
  };

  const fetchLocations = async () => {
    try {
      const res = await fetch(
        "https://godrive-ruc4.onrender.com/api/locations",
        { headers: { Authorization: `Bearer ${token}` } }
      );
      const data = await res.json();
      if (data.isSuccess) setLocations(data.data.data);
    } catch (err) {
      console.error(err);
    }
  };

  const fetchCars = async () => {
    try {
      setLoading(true);

      let url = `https://godrive-ruc4.onrender.com/api/cars?pageNumber=1&pageSize=12`;
      if (search) url += `&search=${search}`;
      if (selectedCategory) url += `&categoryId=${selectedCategory}`;
      if (selectedLocation) url += `&locationId=${selectedLocation}`;
      if (selectedStatus !== null) url += `&status=${selectedStatus}`;

      const res = await fetch(url, {
        headers: { Authorization: `Bearer ${token}` },
      });

      const data = await res.json();

      if (data.isSuccess) {
        const carsData: Car[] = data.data.data;

        const carsWithPrices = await Promise.all(
          carsData.map(async (car) => {
            try {
              const priceRes = await fetch(
                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${car.id}`,
                { headers: { Authorization: `Bearer ${token}` } }
              );
              const priceData = await priceRes.json();

              return {
                ...car,
                pricePerDay: priceData?.isSuccess
                  ? priceData.data.pricePerDay
                  : null,
              };
            } catch {
              return { ...car, pricePerDay: null };
            }
          })
        );

        setCars(carsWithPrices);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchLocations();
  }, []);

  useEffect(() => {
    fetchCars();
  }, [search, selectedCategory, selectedLocation, selectedStatus]);

  return (
    <div className="min-h-screen pb-[100px] bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-8">

      <div className="mb-12">
        <h1 className="text-5xl font-bold">Browse Cars</h1>
        <p className="text-gray-400 mt-2">
          Choose your perfect ride from our premium collection
        </p>
      </div>

      {loading ? (
        <p className="text-gray-400">Loading...</p>
      ) : (
        <div className="grid md:grid-cols-3 sm:grid-cols-2 gap-12">
          {cars.map((car) => {
            const status = statusConfig[car.carStatus];
            const location = locations.find(
              (l) => l.id === car.locationId
            );

            return (
              <Link key={car.id} href={`/cars/${car.id}`}>
                <div className="group rounded-3xl overflow-hidden bg-gradient-to-b from-[#1c1c1c] to-[#111] border border-gray-800 hover:border-gray-600 transition-all duration-500 shadow-xl hover:shadow-2xl cursor-pointer">

                  <div className="relative h-[260px] overflow-hidden">
                    <img
                      src={car.images?.[0] || "/default-car.jpg"}
                      className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700"
                    />

                    <span
                      className={`absolute top-4 right-4 px-4 py-1 text-sm font-semibold rounded-full text-white ${status?.color}`}
                    >
                      {status?.label}
                    </span>
                  </div>

                  <div className="p-6 space-y-5">
                    <div className="flex justify-between items-start">
                      <div>
                        <h2 className="text-xl font-semibold">
                          {car.brand} {car.model}
                        </h2>
                        <p className="text-gray-500 text-sm mt-1">
                          {car.year}
                        </p>
                      </div>

                      <div className="flex items-center gap-1 text-yellow-400">
                        <Star size={16} fill="currentColor" />
                      </div>
                    </div>

                    <div className="flex justify-between pt-4 border-t border-gray-800">
                      <span className="text-gray-400 text-sm">
                        {location
                          ? `${location.country} - ${location.city}`
                          : "Unknown Location"}
                      </span>

                      <div>
                        <span className="text-2xl font-bold text-cyan-400">
                          {car.pricePerDay
                            ? `$${car.pricePerDay}`
                            : "No Price"}
                        </span>
                        <span className="text-gray-400 text-sm">/day</span>
                      </div>
                    </div>
                  </div>

                </div>
              </Link>
            );
          })}
        </div>
      )}
    </div>
  );
}