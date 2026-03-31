"use client";

import React, { useState, useEffect, useRef } from "react";
import { useRouter, useParams } from "next/navigation";
import Link from "next/link";
import {
    Car,
    Save,
    X,
    Loader2,
    AlertCircle,
    CheckCircle2,
    Tag,
    MapPin,
    Building2,
    Calendar,
    Hash,
    ArrowLeft,
    Image as ImageIcon,
    DollarSign,
    FileText,
    Upload,
    Trash2,
    Plus,
    Eye,
} from "lucide-react";

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
    county: string;
}

interface CarData {
    id: number;
    brand: string;
    model: string;
    year: number;
    registrationNumber: string;
    categoryId: number;
    locationId: number;
    rentalCompanyId: number;
}

interface CarImage {
    id: number;
    url: string;
    isMain: boolean;
}

interface CarDocument {
    id: number;
    documentType: number;
    documentUrl: string;
    uploadedAt: string;
}

interface CarPrice {
    id: number;
    carId: number;
    pricePerDay: number;
    currency: number;
}

const DOCUMENT_TYPES = [
    { id: 1, name: "Technical Passport" },
    { id: 2, name: "Insurance" },
    { id: 3, name: "Ownership Certificate" },
];

const CURRENCIES = [
    { id: 0, name: "USD ($)" },
    { id: 1, name: "EUR (€)" },
    { id: 2, name: "TJS (؋)" },
];

export default function EditCarPage() {
    const router = useRouter();
    const params = useParams();
    const carId = params?.id;
    const [brand, setBrand] = useState("");
    const [model, setModel] = useState("");
    const [year, setYear] = useState("");
    const [registrationNumber, setRegistrationNumber] = useState("");
    const [categoryId, setCategoryId] = useState("");
    const [locationId, setLocationId] = useState("");
    const [rentalCompanyId, setRentalCompanyId] = useState("");
    const [pricePerDay, setPricePerDay] = useState("");
    const [currency, setCurrency] = useState("0");
    const [priceId, setPriceId] = useState<number | null>(null);
    const [images, setImages] = useState<CarImage[]>([]);
    const [newImages, setNewImages] = useState<File[]>([]);
    const [imagePreviews, setImagePreviews] = useState<string[]>([]);
    const [uploadingImages, setUploadingImages] = useState(false);
    const [documents, setDocuments] = useState<CarDocument[]>([]);
    const [newDocuments, setNewDocuments] = useState<{ file: File; type: number }[]>([]);
    const [uploadingDocs, setUploadingDocs] = useState(false);
    const [categories, setCategories] = useState<Category[]>([]);
    const [locations, setLocations] = useState<Location[]>([]);
    const [companies, setCompanies] = useState<RentalCompany[]>([]);
    const [loading, setLoading] = useState(false);
    const [fetchingData, setFetchingData] = useState(true);
    const [message, setMessage] = useState<{ text: string; type: "success" | "error" } | null>(null);
    const [activeTab, setActiveTab] = useState<"details" | "price" | "images" | "documents">("details");
    const fileInputRef = useRef<HTMLInputElement>(null);
    const docInputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        const fetchData = async () => {
            const token = localStorage.getItem("token");
            if (!token) {
                setMessage({ text: "Token not found. Please login first.", type: "error" });
                setFetchingData(false);
                return;
            }

            const headers = { "Authorization": `Bearer ${token}` };

            try {
                const [
                    carRes, categoriesRes, locationsRes, companiesRes,
                    imagesRes, docsRes, priceRes
                ] = await Promise.all([
                    fetch(`https://godrive-5r3o.onrender.com/api/cars/${carId}`, { headers }),
                    fetch("https://godrive-5r3o.onrender.com/api/categories", { headers }),
                    fetch("https://godrive-5r3o.onrender.com/api/locations", { headers }),
                    fetch("https://godrive-5r3o.onrender.com/api/rental-companies", { headers }),
                    fetch(`https://godrive-5r3o.onrender.com/api/car-images/car/${carId}`, { headers }),
                    fetch(`https://godrive-5r3o.onrender.com/api/car-documents/car/${carId}`, { headers }),
                    fetch(`https://godrive-5r3o.onrender.com/api/car-prices/by-car/${carId}`, { headers }),
                ]);

                const carData = await carRes.json();
                const categoriesData = await categoriesRes.json();
                const locationsData = await locationsRes.json();
                const companiesData = await companiesRes.json();
                const imagesData = await imagesRes.json();
                const docsData = await docsRes.json();
                const priceData = await priceRes.json();

                if (carData.isSuccess) {
                    const car: CarData = carData.data;
                    setBrand(car.brand || "");
                    setModel(car.model || "");
                    setYear(car.year?.toString() || "");
                    setRegistrationNumber(car.registrationNumber || "");
                    setCategoryId(car.categoryId?.toString() || "");
                    setLocationId(car.locationId?.toString() || "");
                    setRentalCompanyId(car.rentalCompanyId?.toString() || "");
                }

                if (categoriesData.isSuccess) setCategories(categoriesData.data.data);
                if (locationsData.isSuccess) setLocations(locationsData.data.data);
                if (companiesData.isSuccess) setCompanies(companiesData.data.data);

                if (imagesData.isSuccess) setImages(imagesData.data.data || []);

                if (docsData.isSuccess) setDocuments(docsData.data.data || []);

                if (priceData.isSuccess && priceData.data) {
                    const price: CarPrice = priceData.data;
                    setPricePerDay(price.pricePerDay?.toString() || "");
                    setCurrency(price.currency?.toString() || "0");
                    setPriceId(price.id);
                }

            } catch (err) {
                console.error("Failed to fetch data:", err);
                setMessage({ text: "Failed to load car data.", type: "error" });
            } finally {
                setFetchingData(false);
            }
        };

        if (carId) fetchData();
    }, [carId]);

    const handleUpdateCar = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage(null);

        const token = localStorage.getItem("token");
        if (!token) {
            setMessage({ text: "Token not found.", type: "error" });
            setLoading(false);
            return;
        }

        try {
            const res = await fetch(`https://godrive-5r3o.onrender.com/api/cars/${carId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "*/*",
                    "Authorization": `Bearer ${token}`,
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
                setMessage({ text: "✅ Car details updated!", type: "success" });
            } else {
                setMessage({ text: `Error: ${data.error?.message || "Unknown"}`, type: "error" });
            }
        } catch (err) {
            setMessage({ text: "Network error. Try again.", type: "error" });
        } finally {
            setLoading(false);
        }
    };

    const handleSavePrice = async () => {
        const token = localStorage.getItem("token");
        if (!token || !pricePerDay) return;

        setUploadingImages(true);
        setMessage(null);

        try {
            const url = priceId
                ? `https://godrive-5r3o.onrender.com/api/car-prices/${priceId}`
                : "https://godrive-5r3o.onrender.com/api/car-prices";
            
            const method = priceId ? "PUT" : "POST";
            const body = priceId
                ? JSON.stringify({ pricePerDay: Number(pricePerDay) })
                : JSON.stringify({
                    carId: Number(carId),
                    pricePerDay: Number(pricePerDay),
                    currency: Number(currency),
                });

            const res = await fetch(url, {
                method,
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "*/*",
                    "Authorization": `Bearer ${token}`,
                },
                body,
            });

            const data = await res.json();
            if (data.isSuccess) {
                setMessage({ text: "💰 Price updated successfully!", type: "success" });
                if (!priceId && data.data?.id) setPriceId(data.data.id);
            } else {
                setMessage({ text: `Error: ${data.error?.message || "Unknown"}`, type: "error" });
            }
        } catch (err) {
            setMessage({ text: "Network error.", type: "error" });
        } finally {
            setUploadingImages(false);
        }
    };

    const handleImageSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        const files = Array.from(e.target.files || []);
        setNewImages(prev => [...prev, ...files]);
        
        const previews = files.map(file => URL.createObjectURL(file));
        setImagePreviews(prev => [...prev, ...previews]);
    };

    const handleUploadImages = async () => {
        if (newImages.length === 0) return;
        
        const token = localStorage.getItem("token");
        if (!token) return;

        setUploadingImages(true);
        setMessage(null);

        try {
            for (let i = 0; i < newImages.length; i++) {
                const formData = new FormData();
                formData.append("CarId", carId as string);
                formData.append("File", newImages[i]);
                formData.append("IsMain", (i === 0 && images.length === 0).toString());

                await fetch("https://godrive-5r3o.onrender.com/api/car-images", {
                    method: "POST",
                    headers: { "Authorization": `Bearer ${token}` },
                    body: formData,
                });
            }
            setMessage({ text: `✅ ${newImages.length} image(s) uploaded!`, type: "success" });
            setNewImages([]);
            setImagePreviews([]);
            const res = await fetch(`https://godrive-5r3o.onrender.com/api/car-images/car/${carId}`, {
                headers: { "Authorization": `Bearer ${token}` }
            });
            const data = await res.json();
            if (data.isSuccess) setImages(data.data.data || []);
        } catch (err) {
            setMessage({ text: "Failed to upload images.", type: "error" });
        } finally {
            setUploadingImages(false);
        }
    };

    const handleDeleteImage = async (imageId: number) => {
        const token = localStorage.getItem("token");
        if (!token) return;

        try {
            const res = await fetch(`https://godrive-5r3o.onrender.com/api/car-images/${imageId}`, {
                method: "DELETE",
                headers: { "Authorization": `Bearer ${token}` },
            });
            const data = await res.json();
            if (data.isSuccess) {
                setImages(prev => prev.filter(img => img.id !== imageId));
                setMessage({ text: "🗑️ Image deleted.", type: "success" });
            }
        } catch (err) {
            setMessage({ text: "Failed to delete image.", type: "error" });
        }
    };

    const handleDocSelect = (e: React.ChangeEvent<HTMLInputElement>, docType: number) => {
        const file = e.target.files?.[0];
        if (file) {
            setNewDocuments(prev => [...prev, { file, type: docType }]);
        }
    };

    const handleUploadDocs = async () => {
        if (newDocuments.length === 0) return;
        
        const token = localStorage.getItem("token");
        if (!token) return;

        setUploadingDocs(true);
        setMessage(null);

        try {
            for (const { file, type } of newDocuments) {
                const formData = new FormData();
                formData.append("CarId", carId as string);
                formData.append("DocumentType", type.toString());
                formData.append("File", file);

                await fetch("https://godrive-5r3o.onrender.com/api/car-documents", {
                    method: "POST",
                    headers: { "Authorization": `Bearer ${token}` },
                    body: formData,
                });
            }
            setMessage({ text: `✅ ${newDocuments.length} document(s) uploaded!`, type: "success" });
            setNewDocuments([]);
            const res = await fetch(`https://godrive-5r3o.onrender.com/api/car-documents/car/${carId}`, {
                headers: { "Authorization": `Bearer ${token}` }
            });
            const data = await res.json();
            if (data.isSuccess) setDocuments(data.data.data || []);
        } catch (err) {
            setMessage({ text: "Failed to upload documents.", type: "error" });
        } finally {
            setUploadingDocs(false);
        }
    };

    const handleDeleteDoc = async (docId: number) => {
        const token = localStorage.getItem("token");
        if (!token) return;

        try {
            const res = await fetch(`https://godrive-5r3o.onrender.com/api/car-documents/${docId}`, {
                method: "DELETE",
                headers: { "Authorization": `Bearer ${token}` },
            });
            const data = await res.json();
            if (data.isSuccess) {
                setDocuments(prev => prev.filter(doc => doc.id !== docId));
                setMessage({ text: "🗑️ Document deleted.", type: "success" });
            }
        } catch (err) {
            setMessage({ text: "Failed to delete document.", type: "error" });
        }
    };

    if (fetchingData) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-950 via-gray-900 to-black">
                <div className="flex items-center gap-3 text-gray-400">
                    <Loader2 className="h-6 w-6 text-purple-500 animate-spin" />
                    <span>Loading car data...</span>
                </div>
            </div>
        );
    }

    const TabButton = ({ id, label, icon: Icon }: { id: typeof activeTab, label: string, icon: any }) => (
        <button
            type="button"
            onClick={() => setActiveTab(id)}
            className={`flex-1 py-2.5 px-3 rounded-xl text-sm font-medium transition-all duration-300 flex items-center justify-center gap-1.5 ${
                activeTab === id
                    ? "bg-gradient-to-r from-purple-600 to-pink-600 text-white shadow-lg shadow-purple-600/30"
                    : "bg-gray-800/50 text-gray-400 hover:bg-gray-800 hover:text-gray-200"
            }`}
        >
            <Icon className="w-4 h-4" />
            <span className="hidden sm:inline">{label}</span>
        </button>
    );

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-950 via-gray-900 to-black p-4">
            <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-96 h-96 bg-purple-600/20 rounded-full blur-3xl pointer-events-none"></div>

            <div className="w-full max-w-2xl">
                <form className="relative bg-gray-900/80 backdrop-blur-xl border border-gray-800 p-6 md:p-8 rounded-2xl shadow-2xl space-y-6">
                    
                    <div className="text-center space-y-2">
                        <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-br from-purple-600 to-pink-600 rounded-2xl mb-3 shadow-lg shadow-purple-600/30">
                            <Car className="w-8 h-8 text-white" />
                        </div>
                        <h1 className="text-2xl md:text-3xl font-bold bg-gradient-to-r from-purple-400 via-pink-400 to-rose-400 bg-clip-text text-transparent">
                            Edit Car
                        </h1>
                        <p className="text-gray-400 text-sm">Update all car information</p>
                    </div>

                    <div className="flex gap-2 p-1 bg-gray-800/50 rounded-xl">
                        <TabButton id="details" label="Details" icon={Car} />
                        <TabButton id="price" label="Price" icon={DollarSign} />
                        <TabButton id="images" label="Images" icon={ImageIcon} />
                        <TabButton id="documents" label="Documents" icon={FileText} />
                    </div>

                    <div className="space-y-4">
                        
                        {activeTab === "details" && (
                            <div className="space-y-4 animate-fade-in">
                                <div>
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                        <Car className="w-3.5 h-3.5" /> Brand
                                    </label>
                                    <input type="text" placeholder="e.g. BMW" value={brand} onChange={(e) => setBrand(e.target.value)}
                                        className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all" required />
                                </div>
                                <div>
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                        <Tag className="w-3.5 h-3.5" /> Model
                                    </label>
                                    <input type="text" placeholder="e.g. M5 F90" value={model} onChange={(e) => setModel(e.target.value)}
                                        className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all" required />
                                </div>
                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                            <Calendar className="w-3.5 h-3.5" /> Year
                                        </label>
                                        <input type="number" placeholder="2024" value={year} onChange={(e) => setYear(e.target.value)} min="1900" max="2030"
                                            className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all" required />
                                    </div>
                                    <div>
                                        <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                            <Hash className="w-3.5 h-3.5" /> Reg. Number
                                        </label>
                                        <input type="text" placeholder="2011AA01" value={registrationNumber} onChange={(e) => setRegistrationNumber(e.target.value)}
                                            className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all" required />
                                    </div>
                                </div>
                                <div>
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                        <Tag className="w-3.5 h-3.5" /> Category
                                    </label>
                                    <select value={categoryId} onChange={(e) => setCategoryId(e.target.value)}
                                        className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all appearance-none cursor-pointer" required>
                                        <option value="" disabled>Select Category</option>
                                        {categories.map(cat => <option key={cat.id} value={cat.id} className="bg-gray-900">{cat.name}</option>)}
                                    </select>
                                </div>
                                <div>
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                        <MapPin className="w-3.5 h-3.5" /> Location
                                    </label>
                                    <select value={locationId} onChange={(e) => setLocationId(e.target.value)}
                                        className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all appearance-none cursor-pointer" required>
                                        <option value="" disabled>Select Location</option>
                                        {locations.map(loc => <option key={loc.id} value={loc.id} className="bg-gray-900">{loc.city}, {loc.country}</option>)}
                                    </select>
                                </div>
                                <div>
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-1.5 ml-1">
                                        <Building2 className="w-3.5 h-3.5" /> Rental Company
                                    </label>
                                    <select value={rentalCompanyId} onChange={(e) => setRentalCompanyId(e.target.value)}
                                        className="w-full px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all appearance-none cursor-pointer" required>
                                        <option value="" disabled>Select Company</option>
                                        {companies.map(comp => <option key={comp.id} value={comp.id} className="bg-gray-900">{comp.name} — {comp.city}</option>)}
                                    </select>
                                </div>
                                <button type="submit" onClick={handleUpdateCar} disabled={loading}
                                    className="w-full py-3.5 bg-gradient-to-r from-purple-600 via-pink-600 to-rose-600 hover:from-purple-500 hover:via-pink-500 hover:to-rose-500 text-white font-semibold rounded-xl shadow-lg shadow-purple-600/30 hover:shadow-purple-600/50 transition-all duration-300 disabled:opacity-50 flex items-center justify-center gap-2">
                                    {loading ? <><Loader2 className="h-5 w-5 animate-spin" /> Saving...</> : <><Save className="w-5 h-5" /> Save Details</>}
                                </button>
                            </div>
                        )}

                        {activeTab === "price" && (
                            <div className="space-y-4 animate-fade-in">
                                <div className="p-4 bg-gray-800/50 rounded-xl border border-gray-700">
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-2">
                                        <DollarSign className="w-4 h-4" /> Price Per Day
                                    </label>
                                    <div className="flex gap-3">
                                        <input type="number" placeholder="1800" value={pricePerDay} onChange={(e) => setPricePerDay(e.target.value)}
                                            className="flex-1 px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white placeholder-gray-600 focus:outline-none focus:border-purple-500/50 focus:ring-2 focus:ring-purple-500/20 transition-all" />
                                        <select value={currency} onChange={(e) => setCurrency(e.target.value)}
                                            className="px-4 py-3 bg-gray-950/50 border border-gray-800 rounded-xl text-white focus:outline-none focus:border-purple-500/50 transition-all">
                                            {CURRENCIES.map(curr => <option key={curr.id} value={curr.id}>{curr.name}</option>)}
                                        </select>
                                    </div>
                                </div>
                                <button type="button" onClick={handleSavePrice} disabled={uploadingImages || !pricePerDay}
                                    className="w-full py-3.5 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-500 hover:to-emerald-500 text-white font-semibold rounded-xl shadow-lg shadow-green-600/30 transition-all duration-300 disabled:opacity-50 flex items-center justify-center gap-2">
                                    {uploadingImages ? <><Loader2 className="h-5 w-5 animate-spin" /> Saving...</> : <><Save className="w-5 h-5" /> Save Price</>}
                                </button>
                            </div>
                        )}

                        {activeTab === "images" && (
                            <div className="space-y-4 animate-fade-in">
                                {images.length > 0 && (
                                    <div>
                                        <label className="block text-gray-400 text-xs font-medium mb-2">Current Images</label>
                                        <div className="grid grid-cols-3 gap-2">
                                            {images.map(img => (
                                                <div key={img.id} className="relative group aspect-square rounded-lg overflow-hidden border border-gray-700">
                                                    <img src={img.url} alt="car" className="w-full h-full object-cover" />
                                                    {img.isMain && <span className="absolute top-1 left-1 px-1.5 py-0.5 bg-purple-600 text-white text-xs rounded">Main</span>}
                                                    <button type="button" onClick={() => handleDeleteImage(img.id)}
                                                        className="absolute top-1 right-1 p-1 bg-red-500/80 hover:bg-red-500 rounded-full opacity-0 group-hover:opacity-100 transition-opacity">
                                                        <Trash2 className="w-3 h-3 text-white" />
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                )}
                                <div className="p-4 bg-gray-800/50 rounded-xl border border-gray-700">
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-2">
                                        <Upload className="w-4 h-4" /> Add New Images
                                    </label>
                                    <input ref={fileInputRef} type="file" accept="image/*" multiple onChange={handleImageSelect} className="hidden" />
                                    <button type="button" onClick={() => fileInputRef.current?.click()}
                                        className="w-full py-3 border-2 border-dashed border-gray-600 hover:border-purple-500 rounded-xl text-gray-400 hover:text-purple-400 transition-all flex items-center justify-center gap-2">
                                        <Plus className="w-4 h-4" /> Select Images
                                    </button>
                                    {imagePreviews.length > 0 && (
                                        <div className="grid grid-cols-4 gap-2 mt-3">
                                            {imagePreviews.map((preview, i) => (
                                                <div key={i} className="aspect-square rounded-lg overflow-hidden border border-gray-700">
                                                    <img src={preview} alt="preview" className="w-full h-full object-cover" />
                                                </div>
                                            ))}
                                        </div>
                                    )}
                                    {newImages.length > 0 && (
                                        <button type="button" onClick={handleUploadImages} disabled={uploadingImages}
                                            className="w-full mt-3 py-2.5 bg-purple-600 hover:bg-purple-500 text-white font-medium rounded-lg transition-all flex items-center justify-center gap-2">
                                            {uploadingImages ? <><Loader2 className="h-4 w-4 animate-spin" /> Uploading...</> : <><Upload className="w-4 h-4" /> Upload {newImages.length} Image(s)</>}
                                        </button>
                                    )}
                                </div>
                            </div>
                        )}

                        {activeTab === "documents" && (
                            <div className="space-y-4 animate-fade-in">
                                {documents.length > 0 && (
                                    <div>
                                        <label className="block text-gray-400 text-xs font-medium mb-2">Uploaded Documents</label>
                                        <div className="space-y-2">
                                            {documents.map(doc => (
                                                <div key={doc.id} className="flex items-center justify-between p-3 bg-gray-800/50 rounded-lg border border-gray-700">
                                                    <div className="flex items-center gap-3">
                                                        <FileText className="w-5 h-5 text-purple-400" />
                                                        <div>
                                                            <p className="text-white text-sm font-medium">
                                                                {DOCUMENT_TYPES.find(d => d.id === doc.documentType)?.name || "Unknown"}
                                                            </p>
                                                            <p className="text-gray-500 text-xs">{new Date(doc.uploadedAt).toLocaleDateString()}</p>
                                                        </div>
                                                    </div>
                                                    <div className="flex gap-1">
                                                        <a href={doc.documentUrl} target="_blank" rel="noopener noreferrer"
                                                            className="p-2 text-gray-400 hover:text-purple-400 transition-colors">
                                                            <Eye className="w-4 h-4" />
                                                        </a>
                                                        <button type="button" onClick={() => handleDeleteDoc(doc.id)}
                                                            className="p-2 text-gray-400 hover:text-red-400 transition-colors">
                                                            <Trash2 className="w-4 h-4" />
                                                        </button>
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                )}
                                <div className="p-4 bg-gray-800/50 rounded-xl border border-gray-700">
                                    <label className="flex items-center gap-2 text-gray-400 text-xs font-medium mb-3">
                                        <Upload className="w-4 h-4" /> Add New Document
                                    </label>
                                    <input ref={docInputRef} type="file" accept="image/*,.pdf" onChange={(e) => handleDocSelect(e, 1)} className="hidden" />
                                    <div className="space-y-2">
                                        {DOCUMENT_TYPES.map(docType => (
                                            <div key={docType.id} className="flex items-center gap-2">
                                                <button type="button" onClick={() => { docInputRef.current?.click(); }}
                                                    className="flex-1 py-2.5 px-3 bg-gray-950/50 border border-gray-800 hover:border-purple-500 rounded-lg text-gray-300 text-sm transition-all text-left">
                                                    {docType.name}
                                                </button>
                                                <span className="text-gray-500 text-xs w-20 text-right">
                                                    {documents.some(d => d.documentType === docType.id) ? "✓ Uploaded" : ""}
                                                </span>
                                            </div>
                                        ))}
                                    </div>
                                    {newDocuments.length > 0 && (
                                        <button type="button" onClick={handleUploadDocs} disabled={uploadingDocs}
                                            className="w-full mt-3 py-2.5 bg-purple-600 hover:bg-purple-500 text-white font-medium rounded-lg transition-all flex items-center justify-center gap-2">
                                            {uploadingDocs ? <><Loader2 className="h-4 w-4 animate-spin" /> Uploading...</> : <><Upload className="w-4 h-4" /> Upload {newDocuments.length} Document(s)</>}
                                        </button>
                                    )}
                                </div>
                            </div>
                        )}
                    </div>

                    {message && (
                        <div className={`flex items-center gap-2 p-3 rounded-lg text-sm text-center animate-fade-in ${
                            message.type === "success" ? "bg-green-500/10 border border-green-500/30 text-green-400" : "bg-red-500/10 border border-red-500/30 text-red-400"
                        }`}>
                            {message.type === "success" ? <CheckCircle2 className="w-4 h-4 flex-shrink-0" /> : <AlertCircle className="w-4 h-4 flex-shrink-0" />}
                            {message.text}
                        </div>
                    )}

                    <Link href="/cars">
                        <button type="button"
                            className="w-full py-3 bg-gray-800/50 hover:bg-gray-800 border border-gray-700 text-gray-300 font-medium rounded-xl transition-all flex items-center justify-center gap-2">
                            <ArrowLeft className="w-4 h-4" /> Cancel
                        </button>
                    </Link>
                </form>

                <div className="mt-6 text-center">
                    <div className="inline-flex items-center gap-2 px-4 py-2 bg-gray-900/50 rounded-full border border-gray-800">
                        <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
                        <span className="text-gray-400 text-xs">Editor Ready</span>
                    </div>
                </div>
            </div>
        </div>
    );
}