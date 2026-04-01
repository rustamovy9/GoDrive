"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { Search, Star, Filter } from "lucide-react";

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

  const resetFilters = () => {
    setSearch("");
    setSelectedCategory(null);
    setSelectedLocation(null);
    setSelectedStatus(null);
  };

  return (
    <div className="min-h-screen pb-[100px] bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-8">
      <div className="mb-12">
        <h1 className="text-5xl font-bold">Browse Cars</h1>
        <p className="text-gray-400 mt-2">
          Find the perfect car for your next adventure
        </p>
      </div>

      <div className="mb-8 p-6 bg-gradient-to-r from-gray-900 to-gray-800 rounded-2xl border border-gray-700">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="md:col-span-2 relative">
            <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 text-gray-400" size={20} />
            <input
              type="text"
              placeholder="Search by name..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-12 pr-4 py-3 bg-black/50 border border-gray-700 rounded-xl text-white placeholder-gray-500 focus:outline-none focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500/20 transition-all"
            />
          </div>

          <div className="relative">
            <select
              value={selectedCategory || ""}
              onChange={(e) => setSelectedCategory(e.target.value ? Number(e.target.value) : null)}
              className="w-full px-4 py-3 bg-black/50 border border-gray-700 rounded-xl text-white appearance-none cursor-pointer focus:outline-none focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500/20 transition-all"
            >
              <option value="">All Categories</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
            <div className="absolute right-4 top-1/2 transform -translate-y-1/2 pointer-events-none">
              <Filter size={16} className="text-gray-400" />
            </div>
          </div>

          <div className="relative">
            <select
              value={selectedLocation || ""}
              onChange={(e) => setSelectedLocation(e.target.value ? Number(e.target.value) : null)}
              className="w-full px-4 py-3 bg-black/50 border border-gray-700 rounded-xl text-white appearance-none cursor-pointer focus:outline-none focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500/20 transition-all"
            >
              <option value="">All Cities</option>
              {locations.map((loc) => (
                <option key={loc.id} value={loc.id}>
                  {loc.city}, {loc.country}
                </option>
              ))}
            </select>
            <div className="absolute right-4 top-1/2 transform -translate-y-1/2 pointer-events-none">
              <Filter size={16} className="text-gray-400" />
            </div>
          </div>
        </div>

        {(search || selectedCategory || selectedLocation || selectedStatus !== null) && (
          <div className="mt-4 flex items-center justify-between">
            <div className="flex gap-2 flex-wrap">
              {search && (
                <span className="px-3 py-1 bg-cyan-500/20 text-cyan-400 rounded-full text-sm border border-cyan-500/30">
                  Search: {search}
                </span>
              )}
              {selectedCategory && (
                <span className="px-3 py-1 bg-purple-500/20 text-purple-400 rounded-full text-sm border border-purple-500/30">
                  Category: {categories.find(c => c.id === selectedCategory)?.name}
                </span>
              )}
              {selectedLocation && (
                <span className="px-3 py-1 bg-green-500/20 text-green-400 rounded-full text-sm border border-green-500/30">
                  Location: {locations.find(l => l.id === selectedLocation)?.city}
                </span>
              )}
            </div>
            <button
              onClick={resetFilters}
              className="px-4 py-2 text-sm text-gray-400 hover:text-white border border-gray-600 hover:border-gray-400 rounded-lg transition-all"
            >
              Reset Filters
            </button>
          </div>
        )}
      </div>

      {/* Results Count */}
      <div className="mb-6">
        <p className="text-gray-400">
          {loading ? "Loading..." : `${cars.length} cars found`}
        </p>
      </div>

      {loading ? (
        <div className="flex justify-center items-center py-20">
          <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-cyan-500"></div>
        </div>
      ) : cars.length === 0 ? (
        <div className="text-center py-20">
          <p className="text-gray-400 text-xl">No cars found</p>
          <p className="text-gray-500 mt-2">Try adjusting your search or filters</p>
        </div>
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
                      alt={`${car.brand} ${car.model}`}
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