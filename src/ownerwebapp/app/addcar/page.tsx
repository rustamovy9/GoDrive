"use client";

import React, { useState, useEffect } from "react";

interface Category {
    id: number;
    name: string;
}

interface Location {
    id: number;
    country: string;
    city: string;
}

interface RentalCompany {
    id: number;
    name: string;
    city: string;
}

const AddCarPage: React.FC = () => {
    const [brand, setBrand] = useState<string>("");
    const [model, setModel] = useState<string>("");
    const [year, setYear] = useState<string>("");
    const [registrationNumber, setRegistrationNumber] = useState<string>("");
    const [categoryId, setCategoryId] = useState<string>("");
    const [locationId, setLocationId] = useState<string>("");
    const [rentalCompanyId, setRentalCompanyId] = useState<string>("");

    const [categories, setCategories] = useState<Category[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    const [companies, setCompanies] = useState<RentalCompany[]>([]);

    const [loading, setLoading] = useState<boolean>(false);
    const [fetchingData, setFetchingData] = useState<boolean>(true);
    const [message, setMessage] = useState<string>("");

    // Fetch dropdown data
    useEffect(() => {
        const fetchData = async () => {
            const token = localStorage.getItem("token");
            if (!token) {
                setMessage("Token not found. Please login first.");
                setFetchingData(false);
                return;
            }

            const headers = {
                "Accept": "*/*",
                "Authorization": `Bearer ${token}`,
            };

            try {
                const [categoriesRes, locationsRes, companiesRes] = await Promise.all([
                    fetch("https://godrive-5r3o.onrender.com/api/categories", { headers }),
                    fetch("https://godrive-5r3o.onrender.com/api/locations", { headers }),
                    fetch("https://godrive-5r3o.onrender.com/api/rental-companies", { headers }),
                ]);

                const [categoriesData, locationsData, companiesData] = await Promise.all([
                    categoriesRes.json(),
                    locationsRes.json(),
                    companiesRes.json(),
                ]);

                if (categoriesData.isSuccess) setCategories(categoriesData.data.data);
                if (locationsData.isSuccess) setLocations(locationsData.data.data);
                if (companiesData.isSuccess) setCompanies(companiesData.data.data);

            } catch (err) {
                console.error("Failed to fetch dropdown data:", err);
                setMessage("Failed to load categories, locations, or companies.");
            } finally {
                setFetchingData(false);
            }
        };

        fetchData();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage("");

        const token = localStorage.getItem("token");
        if (!token) {
            setMessage("Token not found. Please login first.");
            setLoading(false);
            return;
        }

        try {
            const res = await fetch("https://godrive-5r3o.onrender.com/api/cars", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    brand,
                    model,
                    year: year === "" ? null : Number(year),
                    registrationNumber,
                    categoryId: categoryId === "" ? null : Number(categoryId),
                    locationId: locationId === "" ? null : Number(locationId),
                    rentalCompanyId: rentalCompanyId === "" ? null : Number(rentalCompanyId),
                }),
            });

            const data = await res.json();

            if (data.isSuccess) {
                setMessage("Car added successfully!");
                setBrand("");
                setModel("");
                setYear("");
                setRegistrationNumber("");
                setCategoryId("");
                setLocationId("");
                setRentalCompanyId("");
            } else {
                setMessage(`Error: ${data.error.message || "Unknown error"}`);
            }
        } catch (err) {
            setMessage("Network error. Try again.");
        }

        setLoading(false);
    };

    if (fetchingData) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-950 via-gray-900 to-black">
                <div className="flex items-center gap-3 text-gray-400">
                    <svg className="animate-spin h-6 w-6 text-blue-500" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                    </svg>
                    <span>Loading data...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-950 via-gray-900 to-black p-4">
            <div className="w-full max-w-lg">
                <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-96 h-96 bg-blue-600/20 rounded-full blur-3xl pointer-events-none"></div>

                <form
                    onSubmit={handleSubmit}
                    className="relative bg-gray-900/80 backdrop-blur-xl border border-gray-800 p-8 rounded-2xl shadow-2xl space-y-5"
                >
                    <div className="text-center space-y-2">
                        <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-br from-blue-600 to-purple-600 rounded-2xl mb-3 shadow-lg shadow-blue-600/30">
                            <svg className="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
                            </svg>
                        </div>
                        <h1 className="text-3xl font-bold bg-gradient-to-r from-blue-400 via-purple-400 to-pink-400 bg-clip-text text-transparent">
                            Add New Car
                        </h1>
                        <p className="text-gray-400 text-sm">Fill in the details below</p>
                    </div>

                    <div className="space-y-4">
                        <div className="group">
                            <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Brand</label>
                            <input
                                type="text"
                                placeholder="e.g. Toyota"
                                value={brand}
                                onChange={(e) => setBrand(e.target.value)}
                                className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
                                required
                            />
                        </div>

                        <div className="group">
                            <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Model</label>
                            <input
                                type="text"
                                placeholder="e.g. Camry"
                                value={model}
                                onChange={(e) => setModel(e.target.value)}
                                className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
                                required
                            />
                        </div>

                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Year</label>
                                <input
                                    type="number"
                                    placeholder="2024"
                                    value={year}
                                    onChange={(e) => setYear(e.target.value)}
                                    className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
                                    required
                                />
                            </div>
                            <div>
                                <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Reg. Number</label>
                                <input
                                    type="text"
                                    placeholder="2011AA01"
                                    value={registrationNumber}
                                    onChange={(e) => setRegistrationNumber(e.target.value)}
                                    className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
                                    required
                                />
                            </div>
                        </div>

                        <div>
                            <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Category</label>
                            <select
                                value={categoryId}
                                onChange={(e) => setCategoryId(e.target.value)}
                                className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300 appearance-none cursor-pointer"
                                required
                            >
                                <option value="" disabled>Select Category</option>
                                {categories.map((cat) => (
                                    <option key={cat.id} value={cat.id} className="bg-gray-900">
                                        {cat.name}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div>
                            <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Location</label>
                            <select
                                value={locationId}
                                onChange={(e) => setLocationId(e.target.value)}
                                className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300 appearance-none cursor-pointer"
                                required
                            >
                                <option value="" disabled>Select Location</option>
                                {locations.map((loc) => (
                                    <option key={loc.id} value={loc.id} className="bg-gray-900">
                                        {loc.city}, {loc.country}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div>
                            <label className="block text-gray-400 text-xs font-medium mb-1.5 ml-1">Rental Company</label>
                            <select
                                value={rentalCompanyId}
                                onChange={(e) => setRentalCompanyId(e.target.value)}
                                className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-blue-500/50 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300 appearance-none cursor-pointer"
                                required
                            >
                                <option value="" disabled>Select Company</option>
                                {companies.map((comp) => (
                                    <option key={comp.id} value={comp.id} className="bg-gray-900">
                                        {comp.name} — {comp.city}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full py-3.5 bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 hover:from-blue-500 hover:via-purple-500 hover:to-pink-500 text-white font-semibold rounded-xl shadow-lg shadow-blue-600/30 hover:shadow-blue-600/50 transform hover:scale-[1.02] transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                    >
                        {loading ? (
                            <span className="flex items-center justify-center gap-2">
                                <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                                </svg>
                                Adding Car...
                            </span>
                        ) : (
                            <span className="flex items-center justify-center gap-2">
                                Add Car
                                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
                                </svg>
                            </span>
                        )}
                    </button>

                    {message && (
                        <div className={`p-3 rounded-lg text-sm text-center animate-fade-in ${message.includes("success")
                                ? "bg-green-500/10 border border-green-500/30 text-green-400"
                                : "bg-red-500/10 border border-red-500/30 text-red-400"
                            }`}>
                            {message}
                        </div>
                    )}
                </form>

                <div className="mt-6 text-center">
                    <div className="inline-flex items-center gap-2 px-4 py-2 bg-gray-900/50 rounded-full border border-gray-800">
                        <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
                        <span className="text-gray-400 text-xs">System Ready</span>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AddCarPage;