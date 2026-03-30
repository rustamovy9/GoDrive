"use client";

import { useEffect, useState, JSX } from "react";
import { Trash2, Ban, CheckCircle, AlertTriangle, X, Wrench } from "lucide-react";

interface Car {
    id: number;
    brand: string;
    model: string;
    year: number;
    images: string[];
    carStatus: number;
    locationId: number;
}

interface Location {
    id: number;
    country: string;
    city: string;
}

function Modal({
    isOpen,
    title,
    message,
    type = "info",
    onConfirm,
    onCancel
}: {
    isOpen: boolean;
    title: string;
    message: string;
    type?: "info" | "success" | "warning" | "danger";
    onConfirm?: () => void;
    onCancel?: () => void;
}) {
    if (!isOpen) return null;

    const styles = {
        info: { border: "border-cyan-500", icon: <AlertTriangle className="text-cyan-400" />, btn: "bg-cyan-500 hover:bg-cyan-400" },
        success: { border: "border-green-500", icon: <CheckCircle className="text-green-400" />, btn: "bg-green-600 hover:bg-green-500" },
        warning: { border: "border-yellow-500", icon: <AlertTriangle className="text-yellow-400" />, btn: "bg-yellow-600 hover:bg-yellow-500" },
        danger: { border: "border-red-500", icon: <Ban className="text-red-400" />, btn: "bg-red-600 hover:bg-red-500" },
    };

    const currentStyle = styles[type];

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm p-4 animate-in fade-in duration-200">
            <div className="bg-[#1a1a1a] border border-gray-700 rounded-2xl max-w-md w-full overflow-hidden shadow-2xl transform animate-in zoom-in-95 duration-200">
                <div className={`flex items-center gap-3 px-6 py-4 border-b border-gray-700 ${currentStyle.border}`}>
                    {currentStyle.icon}
                    <h3 className="font-bold text-lg text-white">{title}</h3>
                    <button onClick={onCancel} className="ml-auto text-gray-400 hover:text-white transition">
                        <X size={22} />
                    </button>
                </div>
                <p className="px-6 py-5 text-gray-300 text-sm leading-relaxed">{message}</p>
                <div className="flex gap-3 px-6 py-4 bg-[#111] border-t border-gray-700">
                    {onConfirm && (
                        <button
                            onClick={() => { onConfirm(); onCancel?.(); }}
                            className={`flex-1 py-3 rounded-xl font-semibold text-white transition-all transform hover:scale-[1.02] ${currentStyle.btn}`}
                        >
                            Confirm
                        </button>
                    )}
                    <button
                        onClick={onCancel}
                        className="flex-1 py-3 rounded-xl font-semibold bg-gray-700 hover:bg-gray-600 text-white transition-all"
                    >
                        {onConfirm ? "Cancel" : "Close"}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default function CarsManagement() {
    const [cars, setCars] = useState<Car[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState<number | null>(null);

    const [modal, setModal] = useState<{
        open: boolean;
        title: string;
        message: string;
        type: "info" | "success" | "warning" | "danger";
        onConfirm?: () => void;
    }>({ open: false, title: "", message: "", type: "info" });

    const API_BASE = "https://godrive-5r3o.onrender.com";
    const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;

    const statusConfig: Record<number, { label: string; color: string; bg: string; icon: JSX.Element }> = {
        0: { label: "Pending", color: "text-gray-300", bg: "bg-gray-600", icon: <AlertTriangle size={16} /> },
        1: { label: "Available", color: "text-green-400", bg: "bg-green-600", icon: <CheckCircle size={16} /> },
        2: { label: "Rented", color: "text-yellow-400", bg: "bg-yellow-600", icon: <AlertTriangle size={16} /> },
        3: { label: "Maintenance", color: "text-blue-400", bg: "bg-blue-600", icon: <Wrench size={16} /> },
        4: { label: "Blocked", color: "text-red-400", bg: "bg-red-600", icon: <Ban size={16} /> },
    };

    useEffect(() => {
        fetchLocations();
        fetchCars();
    }, []);

    const fetchLocations = async () => {
        try {
            const res = await fetch(`${API_BASE}/api/locations`, { headers: { Authorization: `Bearer ${token}` } });
            const data = await res.json();
            if (data.isSuccess) setLocations(data.data.data);
        } catch (err) { console.error(err); }
    };

    const fetchCars = async () => {
        try {
            const res = await fetch(`${API_BASE}/api/cars?pageNumber=1&pageSize=50`, { headers: { Authorization: `Bearer ${token}` } });
            const data = await res.json();
            if (data.isSuccess) setCars(data.data.data);
        } catch (err) { console.error(err); }
        finally { setLoading(false); }
    };

    const showModal = (title: string, message: string, type: "info" | "success" | "warning" | "danger", onConfirm?: () => void) => {
        setModal({ open: true, title, message, type, onConfirm });
    };

    const handleStatusChange = async (carId: number, newStatus: number) => {
        const car = cars.find(c => c.id === carId);
        if (!car) return;

        const statusLabels: Record<number, string> = {
            1: "Available",
            3: "Maintenance",
            4: "Blocked"
        };

        const confirmMessages: Record<number, { title: string; message: string; type: "warning" | "info" }> = {
            1: {
                title: "Set as Available",
                message: `Are you sure you want to set ${car.brand} ${car.model} as Available? This will make it visible for booking.`,
                type: "info" as const
            },
            4: {
                title: "Block Car",
                message: `Are you sure you want to BLOCK ${car.brand} ${car.model}? This will prevent users from booking this car.`,
                type: "warning" as const
            },
            3: {
                title: "Set Maintenance",
                message: `Are you sure you want to set ${car.brand} ${car.model} to Maintenance mode? The car will be unavailable for booking.`,
                type: "warning" as const
            }
        };

        const confirm = confirmMessages[newStatus];
        if (confirm) {
            showModal(confirm.title, confirm.message, confirm.type, async () => {
                setActionLoading(carId);
                try {
                    const res = await fetch(`${API_BASE}/api/cars/${carId}/status`, {
                        method: "PATCH",
                        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                        body: JSON.stringify({ status: newStatus }),
                    });
                    const data = await res.json();
                    if (data.isSuccess) {
                        setCars(cars.map(c => c.id === carId ? { ...c, carStatus: newStatus } : c));
                        showModal("Success", `Status updated to: ${statusLabels[newStatus]}`, "success");
                    } else {
                        showModal("Error", data.error?.message || "Update failed", "danger");
                    }
                } catch {
                    showModal("Error", "Network error occurred", "danger");
                }
                finally { setActionLoading(null); }
            });
        }
    };

    const handleRemove = (carId: number) => {
        const car = cars.find(c => c.id === carId);
        showModal(
            "Delete Car",
            `Are you sure you want to permanently DELETE ${car?.brand} ${car?.model}? This action cannot be undone.`,
            "danger",
            async () => {
                setActionLoading(carId);
                try {
                    const res = await fetch(`${API_BASE}/api/cars/${carId}`, {
                        method: "DELETE",
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    const data = await res.json();
                    if (data.isSuccess) {
                        setCars(cars.filter(c => c.id !== carId));
                        showModal("Success", "Car removed successfully!", "success");
                    } else {
                        showModal("Error", data.error?.message || "Delete failed", "danger");
                    }
                } catch {
                    showModal("Error", "Network error occurred", "danger");
                }
                finally { setActionLoading(null); }
            }
        );
    };

    const getLocation = (id: number) => {
        const loc = locations.find(l => l.id === id);
        return loc ? `${loc.city}, ${loc.country}` : "Unknown";
    };

    const actionButtons = [
        { status: 1, label: "Available", color: "bg-green-600 hover:bg-green-500", icon: CheckCircle },
        { status: 4, label: "Block", color: "bg-red-600 hover:bg-red-500", icon: Ban },
        { status: 3, label: "Maint.", color: "bg-blue-600 hover:bg-blue-500", icon: Wrench },
    ];

    return (
        <div className="min-h-screen bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-4 md:px-8 pb-20">
            <div className="max-w-6xl mx-auto">
                <div className="flex items-center justify-between mb-10">
                    <div>
                        <h1 className="text-4xl font-bold bg-gradient-to-r from-cyan-400 to-blue-500 bg-clip-text text-transparent">
                            Cars Management
                        </h1>
                        <p className="text-gray-400 mt-2">Manage your fleet and car availability</p>
                    </div>
                    <div className="bg-[#1a1a1a] px-6 py-3 rounded-2xl border border-gray-800">
                        <span className="text-2xl font-bold text-cyan-400">{cars.length}</span>
                        <span className="text-gray-400 ml-2">cars</span>
                    </div>
                </div>

                {loading ? (
                    <div className="flex justify-center py-20">
                        <div className="animate-spin rounded-full h-16 w-16 border-4 border-cyan-500 border-t-transparent" />
                    </div>
                ) : !cars.length ? (
                    <div className="text-center py-20 bg-[#1a1a1a] rounded-3xl border border-gray-800">
                        <AlertTriangle size={64} className="mx-auto mb-4 text-gray-600" />
                        <p className="text-2xl font-semibold text-gray-400">No cars found</p>
                        <p className="text-gray-500 mt-2">Add some cars to get started</p>
                    </div>
                ) : (
                    <div className="space-y-5">
                        {cars.map(car => {
                            const status = statusConfig[car.carStatus];
                            return (
                                <div key={car.id} className="bg-[#1a1a1a] rounded-3xl p-6 border border-gray-800 hover:border-gray-700 transition-all duration-300 hover:shadow-2xl hover:shadow-black/50">
                                    <div className="flex flex-col lg:flex-row lg:items-center gap-6">
                                        <div className="w-full lg:w-64 h-40 rounded-2xl overflow-hidden flex-shrink-0 ring-2 ring-gray-800">
                                            <img
                                                src={car.images?.[0] || "/default-car.jpg"}
                                                alt={car.brand}
                                                className="w-full h-full object-cover hover:scale-110 transition-transform duration-500"
                                            />
                                        </div>

                                        <div className="flex-1 min-w-0">
                                            <div className="flex flex-wrap items-start justify-between gap-4 mb-3">
                                                <div className="flex-1">
                                                    <h3 className="text-2xl font-bold text-white mb-1">
                                                        {car.brand} {car.model}
                                                    </h3>
                                                    <p className="text-gray-400 text-sm">
                                                        {car.year} • {getLocation(car.locationId)}
                                                    </p>
                                                </div>
                                                <div className={`inline-flex items-center gap-2 px-4 py-2 rounded-full text-sm font-semibold text-white ${status.bg} shadow-lg`}>
                                                    {status.icon}
                                                    {status.label}
                                                </div>
                                            </div>

                                            <div className="flex flex-wrap gap-3 mt-4">
                                                {actionButtons.map(btn => {
                                                    const Icon = btn.icon;
                                                    const isActive = car.carStatus === btn.status;
                                                    return (
                                                        <button
                                                            key={btn.status}
                                                            onClick={() => handleStatusChange(car.id, btn.status)}
                                                            disabled={actionLoading === car.id || isActive}
                                                            className={`flex items-center gap-2 px-5 py-2.5 rounded-xl font-semibold text-sm transition-all transform hover:scale-105 disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:scale-100 ${btn.color} ${isActive ? 'ring-2 ring-white/30' : ''}`}
                                                        >
                                                            <Icon size={16} />
                                                            {btn.label}
                                                        </button>
                                                    );
                                                })}
                                            </div>
                                        </div>

                                        <div className="flex lg:flex-col justify-end gap-3">
                                            <button
                                                onClick={() => handleRemove(car.id)}
                                                disabled={actionLoading === car.id}
                                                className="flex items-center gap-2 px-6 py-3 rounded-xl font-semibold bg-red-600/10 hover:bg-red-600/20 text-red-400 hover:text-red-300 border border-red-600/30 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                                            >
                                                <Trash2 size={18} />
                                                <span>Remove</span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            );
                        })}
                    </div>
                )}
            </div>

            <Modal
                isOpen={modal.open}
                title={modal.title}
                message={modal.message}
                type={modal.type}
                onConfirm={modal.onConfirm}
                onCancel={() => setModal(m => ({ ...m, open: false }))}
            />
        </div>
    );
}