"use client";

import { useState, useEffect } from "react";
import { X, Plus, MapPin, Tag, Loader2, AlertCircle, Trash2, Info } from "lucide-react";
import { message } from "antd";

type Location = {
    id: number;
    country: string;
    city: string;
    latitude: number;
    longitude: number;
};

type Category = {
    id: number;
    name: string;
};

type ApiError = {
    code: number | null;
    message: string | null;
    errorType: number;
};

type ApiResponse<T> = {
    isSuccess: boolean;
    error: ApiError;
    data: T | null;
};

type PaginatedData<T> = {
    totalPages: number;
    totalRecords: number;
    data: T[];
    search: string | null;
    pageSize: number;
    pageNumber: number;
};

type DeleteModalProps = {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
    title: string;
    message: string;
    itemType: "location" | "category";
    loading?: boolean;
};

const DeleteConfirmModal: React.FC<DeleteModalProps> = ({
    isOpen,
    onClose,
    onConfirm,
    title,
    message,
    itemType,
    loading = false
}) => {
    if (!isOpen) return null;

    const colors = itemType === "location"
        ? { bg: "bg-cyan-500", hover: "hover:bg-cyan-600", text: "text-cyan-400" }
        : { bg: "bg-green-500", hover: "hover:bg-green-600", text: "text-green-400" };

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center">
            <div className="absolute inset-0 bg-black/80 backdrop-blur-sm" onClick={onClose} />
            <div className="relative bg-zinc-900 border border-zinc-800 rounded-2xl shadow-2xl w-full max-w-sm mx-4 p-6">
                <div className="flex items-center gap-3 mb-4">
                    <div className={`p-2 rounded-lg ${colors.bg}/10`}>
                        <Trash2 className={colors.text} size={20} />
                    </div>
                    <h3 className="text-white font-semibold text-lg">{title}</h3>
                </div>
                <p className="text-zinc-400 text-sm mb-6">{message}</p>
                <div className="flex gap-3">
                    <button
                        onClick={onClose}
                        disabled={loading}
                        className="flex-1 bg-zinc-800 hover:bg-zinc-700 text-white font-medium py-2.5 rounded-lg transition-colors disabled:opacity-50"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={onConfirm}
                        disabled={loading}
                        className={`flex-1 ${colors.bg} ${colors.hover} text-black font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2 disabled:opacity-50`}
                    >
                        {loading ? <><Loader2 className="animate-spin" size={18} /> Deleting...</> : "Delete"}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default function Settings() {
    const [locations, setLocations] = useState<Location[]>([]);
    const [locationsLoading, setLocationsLoading] = useState(true);

    const [categories, setCategories] = useState<Category[]>([]);
    const [categoriesLoading, setCategoriesLoading] = useState(true);

    const [error, setError] = useState<string | null>(null);
    const [saving, setSaving] = useState(false);

    const [showLocationModal, setShowLocationModal] = useState(false);
    const [locationForm, setLocationForm] = useState({ country: "", city: "", latitude: "", longitude: "" });

    const [showCategoryModal, setShowCategoryModal] = useState(false);
    const [categoryForm, setCategoryForm] = useState({ name: "" });

    const [deleteModal, setDeleteModal] = useState<{
        isOpen: boolean;
        type: "location" | "category" | null;
        id: number | null;
        name: string;
        loading: boolean;
    }>({ isOpen: false, type: null, id: null, name: "", loading: false });

    const API_BASE = "https://godrive-5r3o.onrender.com/api";

    useEffect(() => {
        fetchLocations();
        fetchCategories();
    }, []);

    const fetchLocations = async () => {
        try {
            setLocationsLoading(true);
            const token = localStorage.getItem("token");
            const res = await fetch(`${API_BASE}/locations`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            });
            const result: ApiResponse<PaginatedData<Location>> = await res.json();
            if (result.isSuccess && result.data) {
                setLocations(result.data.data);
            } else {
                console.warn("Locations fetch failed:", result.error);
            }
        } catch (err) {
            console.error("Fetch locations error:", err);
        } finally {
            setLocationsLoading(false);
        }
    };

    const fetchCategories = async () => {
        try {
            setCategoriesLoading(true);
            const res = await fetch(`${API_BASE}/categories`, {
                headers: { "Content-Type": "application/json" },
            });
            const result: ApiResponse<PaginatedData<Category>> = await res.json();
            if (result.isSuccess && result.data) {
                setCategories(result.data.data);
            } else {
                console.warn("Categories fetch failed:", result.error);
            }
        } catch (err) {
            console.error("Fetch categories error:", err);
        } finally {
            setCategoriesLoading(false);
        }
    };

    const handleAddLocation = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!locationForm.country.trim() || !locationForm.city.trim()) {
            message.error("Country and City are required!");
            return;
        }
        try {
            setSaving(true);
            const token = localStorage.getItem("token");
            const res = await fetch(`${API_BASE}/locations`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    country: locationForm.country.trim(),
                    city: locationForm.city.trim(),
                    latitude: parseFloat(locationForm.latitude) || 0,
                    longitude: parseFloat(locationForm.longitude) || 0,
                }),
            });
            const result: ApiResponse<Location> = await res.json();
            if (result.isSuccess) {
                message.success("Location added! 🎉");
                await fetchLocations();
                setShowLocationModal(false);
                setLocationForm({ country: "", city: "", latitude: "", longitude: "" });
            } else {
                const errorMsg = result.error.message || `Error ${result.error.code || 500}`;
                message.error(`Failed: ${errorMsg}`);
                console.error("Add location failed:", result.error);
            }
        } catch (err: any) {
            message.error(`Connection error: ${err.message}`);
            console.error(err);
        } finally {
            setSaving(false);
        }
    };

    const handleAddCategory = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!categoryForm.name.trim()) {
            message.error("Category name is required!");
            return;
        }
        try {
            setSaving(true);
            const token = localStorage.getItem("token");

            const res = await fetch(`${API_BASE}/categories`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`,
                    "accept": "*/*",
                },
                body: JSON.stringify({ name: categoryForm.name.trim() }),
            });

            const responseText = await res.text();
            console.log("Raw response:", responseText);

            let result: ApiResponse<null>;
            try {
                result = JSON.parse(responseText);
            } catch {
                throw new Error(`Invalid JSON response: ${responseText.slice(0, 100)}`);
            }

            if (result.isSuccess) {
                message.success("Category added! 🎉");
                await fetchCategories();
                setShowCategoryModal(false);
                setCategoryForm({ name: "" });
            } else {
                const errorMsg = result.error.message || `Server error ${result.error.code || 500}`;
                message.error(`Failed: ${errorMsg}`);
                console.error("Add category failed:", result.error);

                if (result.error.code === 500) {
                    message.warning("Server error - please try again later or contact support");
                }
            }
        } catch (err: any) {
            message.error(`Error: ${err.message || "Connection failed"}`);
            console.error("Add category error:", err);
        } finally {
            setSaving(false);
        }
    };

    const openDeleteModal = (type: "location" | "category", id: number, name: string) => {
        setDeleteModal({ isOpen: true, type, id, name, loading: false });
    };

    const closeDeleteModal = () => {
        setDeleteModal(prev => ({ ...prev, isOpen: false, loading: false }));
    };

    const confirmDelete = async () => {
        if (!deleteModal.type || !deleteModal.id) return;

        try {
            setDeleteModal(prev => ({ ...prev, loading: true }));
            const token = localStorage.getItem("token");
            const endpoint = deleteModal.type === "location"
                ? `${API_BASE}/locations/${deleteModal.id}`
                : `${API_BASE}/categories/${deleteModal.id}`;

            const res = await fetch(endpoint, {
                method: "DELETE",
                headers: { Authorization: `Bearer ${token}` },
            });

            const result: ApiResponse<null> = await res.json();

            if (result.isSuccess) {
                message.success(`${deleteModal.type === "location" ? "Location" : "Category"} deleted! 🗑️`);

                if (deleteModal.type === "location") {
                    setLocations(prev => prev.filter(l => l.id !== deleteModal.id));
                } else {
                    setCategories(prev => prev.filter(c => c.id !== deleteModal.id));
                }
                closeDeleteModal();
            } else {
                message.error(result.error.message || "Failed to delete");
                console.error("Delete failed:", result.error);
            }
        } catch (err: any) {
            message.error(`Error: ${err.message || "Connection failed"}`);
            console.error("Delete error:", err);
        } finally {
            setDeleteModal(prev => ({ ...prev, loading: false }));
        }
    };

    return (
        <main className="flex-1 p-10 bg-black min-h-screen">
            <h1 className="text-3xl font-bold mb-8 text-white">Settings</h1>

            {error && (
                <div className="mb-6 p-4 bg-red-500/10 border border-red-500/50 rounded-xl flex items-center gap-3">
                    <AlertCircle className="text-red-400" size={20} />
                    <span className="text-red-400 text-sm">{error}</span>
                    <button onClick={() => setError(null)} className="ml-auto hover:text-red-300">
                        <X size={16} />
                    </button>
                </div>
            )}

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">

                <div className="bg-zinc-900/60 backdrop-blur-xl rounded-2xl border border-zinc-800 p-6">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="text-xl font-semibold text-white flex items-center gap-2">
                            <MapPin className="text-cyan-400" size={20} />
                            Locations ({locations.length})
                        </h2>
                        <button
                            onClick={() => setShowLocationModal(true)}
                            className="flex items-center gap-2 bg-cyan-500 hover:bg-cyan-600 text-black px-4 py-2 rounded-lg font-medium transition-colors"
                        >
                            <Plus size={18} /> Add
                        </button>
                    </div>

                    {locationsLoading ? (
                        <div className="flex justify-center py-8"><Loader2 className="animate-spin text-cyan-400" size={24} /></div>
                    ) : locations.length === 0 ? (
                        <p className="text-zinc-500 text-center py-8">No locations found</p>
                    ) : (
                        <div className="space-y-3 max-h-80 overflow-y-auto pr-2">
                            {locations.map((loc) => (
                                <div key={loc.id} className="flex items-center justify-between px-4 py-3 bg-zinc-800/50 hover:bg-zinc-800 rounded-xl transition-colors">
                                    <div>
                                        <p className="text-white font-medium">{loc.city}, {loc.country}</p>
                                        <p className="text-zinc-500 text-xs">{loc.latitude}, {loc.longitude}</p>
                                    </div>
                                    <button
                                        onClick={() => openDeleteModal("location", loc.id, `${loc.city}, ${loc.country}`)}
                                        className="p-2 text-zinc-400 hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-colors"
                                    >
                                        <Trash2 size={18} />
                                    </button>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                <div className="bg-zinc-900/60 backdrop-blur-xl rounded-2xl border border-zinc-800 p-6">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="text-xl font-semibold text-white flex items-center gap-2">
                            <Tag className="text-green-400" size={20} />
                            Categories ({categories.length})
                        </h2>
                        <button
                            onClick={() => setShowCategoryModal(true)}
                            className="flex items-center gap-2 bg-green-500 hover:bg-green-600 text-black px-4 py-2 rounded-lg font-medium transition-colors"
                        >
                            <Plus size={18} /> Add
                        </button>
                    </div>

                    {categoriesLoading ? (
                        <div className="flex justify-center py-8"><Loader2 className="animate-spin text-green-400" size={24} /></div>
                    ) : categories.length === 0 ? (
                        <p className="text-zinc-500 text-center py-8">No categories found</p>
                    ) : (
                        <div className="space-y-3 max-h-80 overflow-y-auto pr-2">
                            {categories.map((cat) => (
                                <div key={cat.id} className="flex items-center justify-between px-4 py-3 bg-zinc-800/50 hover:bg-zinc-800 rounded-xl transition-colors">
                                    <span className="text-white font-medium">{cat.name}</span>
                                    <button
                                        onClick={() => openDeleteModal("category", cat.id, cat.name)}
                                        className="p-2 text-zinc-400 hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-colors"
                                    >
                                        <Trash2 size={18} />
                                    </button>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>

            {showLocationModal && (
                <div className="fixed inset-0 z-50 flex items-center justify-center">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-sm" onClick={() => setShowLocationModal(false)} />
                    <div className="relative bg-zinc-900 border border-zinc-800 rounded-2xl shadow-2xl w-full max-w-md mx-4">
                        <div className="flex items-center justify-between px-6 py-4 border-b border-zinc-800">
                            <div className="flex items-center gap-2">
                                <MapPin className="text-cyan-400" size={20} />
                                <h3 className="text-white font-semibold">Add Location</h3>
                            </div>
                            <button onClick={() => setShowLocationModal(false)} className="text-zinc-400 hover:text-white p-1 hover:bg-zinc-800 rounded-lg">
                                <X size={20} />
                            </button>
                        </div>
                        <form onSubmit={handleAddLocation} className="p-6 space-y-4">
                            <div>
                                <label className="block text-zinc-300 text-sm mb-2">Country *</label>
                                <input type="text" value={locationForm.country} onChange={(e) => setLocationForm({ ...locationForm, country: e.target.value })} className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-2.5 text-white focus:border-cyan-500 focus:outline-none" placeholder="Tajikistan" required />
                            </div>
                            <div>
                                <label className="block text-zinc-300 text-sm mb-2">City *</label>
                                <input type="text" value={locationForm.city} onChange={(e) => setLocationForm({ ...locationForm, city: e.target.value })} className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-2.5 text-white focus:border-cyan-500 focus:outline-none" placeholder="Dushanbe" required />
                            </div>
                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label className="block text-zinc-400 text-sm mb-2">Latitude</label>
                                    <input type="number" step="0.0001" value={locationForm.latitude} onChange={(e) => setLocationForm({ ...locationForm, latitude: e.target.value })} className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-2.5 text-white focus:border-cyan-500 focus:outline-none" placeholder="0.0000" />
                                </div>
                                <div>
                                    <label className="block text-zinc-400 text-sm mb-2">Longitude</label>
                                    <input type="number" step="0.0001" value={locationForm.longitude} onChange={(e) => setLocationForm({ ...locationForm, longitude: e.target.value })} className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-2.5 text-white focus:border-cyan-500 focus:outline-none" placeholder="0.0000" />
                                </div>
                            </div>
                            <div className="flex gap-3 pt-2">
                                <button type="button" onClick={() => setShowLocationModal(false)} className="flex-1 bg-zinc-800 hover:bg-zinc-700 text-white py-2.5 rounded-lg transition-colors">Cancel</button>
                                <button type="submit" disabled={saving} className="flex-1 bg-cyan-500 hover:bg-cyan-600 text-black font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2">
                                    {saving ? <><Loader2 className="animate-spin" size={18} /> Adding...</> : "Add Location"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {showCategoryModal && (
                <div className="fixed inset-0 z-50 flex items-center justify-center">
                    <div className="absolute inset-0 bg-black/80 backdrop-blur-sm" onClick={() => setShowCategoryModal(false)} />
                    <div className="relative bg-zinc-900 border border-zinc-800 rounded-2xl shadow-2xl w-full max-w-md mx-4">
                        <div className="flex items-center justify-between px-6 py-4 border-b border-zinc-800">
                            <div className="flex items-center gap-2">
                                <Tag className="text-green-400" size={20} />
                                <h3 className="text-white font-semibold">Add Category</h3>
                            </div>
                            <button onClick={() => setShowCategoryModal(false)} className="text-zinc-400 hover:text-white p-1 hover:bg-zinc-800 rounded-lg">
                                <X size={20} />
                            </button>
                        </div>
                        <form onSubmit={handleAddCategory} className="p-6">
                            <div className="mb-6">
                                <label className="block text-zinc-300 text-sm mb-2">Category Name *</label>
                                <input
                                    type="text"
                                    value={categoryForm.name}
                                    onChange={(e) => setCategoryForm({ name: e.target.value })}
                                    className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-3 text-white placeholder-zinc-500 focus:border-green-500 focus:outline-none focus:ring-1 focus:ring-green-500 transition-all"
                                    placeholder="e.g., Ferrari, Audi, Tesla..."
                                    required
                                />
                            </div>
                            <div className="flex gap-3">
                                <button type="button" onClick={() => setShowCategoryModal(false)} className="flex-1 bg-zinc-800 hover:bg-zinc-700 text-white font-medium py-2.5 rounded-lg transition-colors">Cancel</button>
                                <button type="submit" disabled={saving} className="flex-1 bg-green-500 hover:bg-green-600 text-black font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2">
                                    {saving ? <><Loader2 className="animate-spin" size={18} /> Adding...</> : "Add Category"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            <DeleteConfirmModal
                isOpen={deleteModal.isOpen}
                onClose={closeDeleteModal}
                onConfirm={confirmDelete}
                title={`Delete ${deleteModal.type === "location" ? "Location" : "Category"}`}
                message={`Are you sure you want to delete "${deleteModal.name}"? This action cannot be undone.`}
                itemType={deleteModal.type || "location"}
                loading={deleteModal.loading}
            />
        </main>
    );
}