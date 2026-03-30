"use client";

import { useEffect, useState } from "react";
import { Building2, Pencil, Trash2 } from "lucide-react";

type Company = {
  id: number;
  name: string;
  contactInfo: string;
  city: string;
  county: string;
  locationId: number;
  createdAt: string;
};

type Location = {
  id: number;
  city: string;
  country: string;
};

export default function CompanyPage() {
  const [company, setCompany] = useState<Company | null>(null);
  const [locations, setLocations] = useState<Location[]>([]);
  const [loading, setLoading] = useState(true);
  const [showEdit, setShowEdit] = useState(false);

  const [form, setForm] = useState({
    name: "",
    contactInfo: "",
    locationId: 1,
  });

  const token =
    typeof window !== "undefined"
      ? localStorage.getItem("token")
      : null;

  const fetchCompany = async () => {
    const res = await fetch(
      "https://godrive-5r3o.onrender.com/api/rental-companies",
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

    const data = await res.json();

    if (data?.data?.data?.length > 0) {
      const c = data.data.data[0];
      setCompany(c);

      setForm({
        name: c.name,
        contactInfo: c.contactInfo,
        locationId: c.locationId,
      });
    } else {
      setCompany(null);
    }
  };

  const fetchLocations = async () => {
    const res = await fetch(
      "https://godrive-5r3o.onrender.com/api/locations",
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

    const data = await res.json();
    setLocations(data?.data?.data || []);
  };

  useEffect(() => {
    Promise.all([fetchCompany(), fetchLocations()]).finally(() =>
      setLoading(false)
    );
  }, []);

  const handleUpdate = async () => {
    if (!company) return;

    await fetch(
      `https://godrive-5r3o.onrender.com/api/rental-companies/${company.id}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(form),
      }
    );

    setShowEdit(false);
    fetchCompany();
  };

  const handleDelete = async () => {
    if (!company) return;

    if (!confirm("Delete company?")) return;

    await fetch(
      `https://godrive-5r3o.onrender.com/api/rental-companies/${company.id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    setCompany(null);
  };

  if (loading) {
    return (
      <main className="p-8">
        <div className="h-8 w-48 bg-zinc-800 animate-pulse mb-6 rounded" />
        <div className="h-48 bg-zinc-900 animate-pulse rounded-2xl" />
      </main>
    );
  }

  return (
    <main className="p-8 text-white">
      <h1 className="text-3xl font-bold mb-6">Rental Company</h1>

      {!company ? (
        <div className="bg-gradient-to-br from-zinc-900 to-zinc-800 p-10 rounded-2xl text-center border border-zinc-700">
          <Building2 size={60} className="mx-auto mb-4 text-zinc-500" />
          <h2 className="text-xl font-semibold">No Company Yet</h2>
        </div>
      ) : (
        <div className="bg-gradient-to-br from-zinc-900 to-zinc-800 border border-zinc-700 rounded-2xl p-8 shadow-xl max-w-xl">

          <div className="flex items-center gap-4 mb-6">
            <div className="p-4 bg-cyan-500/10 rounded-xl">
              <Building2 className="text-cyan-400" />
            </div>

            <div>
              <h2 className="text-xl font-bold">{company.name}</h2>
              <p className="text-zinc-400 text-sm">
                {company.city}, {company.county}
              </p>
            </div>
          </div>

          <p className="text-zinc-300 mb-2">
            📞 {company.contactInfo}
          </p>

          <div className="flex gap-4 mt-6">
            <button
              onClick={() => setShowEdit(true)}
              className="flex items-center gap-2 bg-yellow-500 hover:bg-yellow-400 transition px-5 py-2 rounded-xl shadow"
            >
              <Pencil size={16} /> Edit
            </button>

            <button
              onClick={handleDelete}
              className="flex items-center gap-2 bg-red-500 hover:bg-red-400 transition px-5 py-2 rounded-xl shadow"
            >
              <Trash2 size={16} /> Delete
            </button>
          </div>
        </div>
      )}

      {showEdit && (
        <div className="fixed inset-0 bg-black/70 flex items-center justify-center">
          <div className="bg-gradient-to-br from-zinc-900 to-zinc-800 p-6 rounded-2xl w-full max-w-md border border-zinc-700 shadow-xl">

            <h2 className="text-lg font-bold mb-4">
              Edit Company
            </h2>

            <input
              value={form.name}
              onChange={(e) =>
                setForm({ ...form, name: e.target.value })
              }
              className="w-full mb-3 p-3 rounded-lg bg-zinc-800 border border-zinc-700"
              placeholder="Company Name"
            />

            <input
              value={form.contactInfo}
              onChange={(e) =>
                setForm({
                  ...form,
                  contactInfo: e.target.value,
                })
              }
              className="w-full mb-3 p-3 rounded-lg bg-zinc-800 border border-zinc-700"
              placeholder="Contact"
            />

            <select
              value={form.locationId}
              onChange={(e) =>
                setForm({
                  ...form,
                  locationId: Number(e.target.value),
                })
              }
              className="w-full mb-4 p-3 rounded-lg bg-zinc-800 border border-zinc-700"
            >
              {locations.map((loc) => (
                <option key={loc.id} value={loc.id}>
                  {loc.city}, {loc.country}
                </option>
              ))}
            </select>

            <div className="flex justify-end gap-3">
              <button
                onClick={() => setShowEdit(false)}
                className="px-4 py-2 bg-zinc-700 rounded-lg"
              >
                Cancel
              </button>

              <button
                onClick={handleUpdate}
                className="px-4 py-2 bg-cyan-500 hover:bg-cyan-400 rounded-lg"
              >
                Save
              </button>
            </div>
          </div>
        </div>
      )}
    </main>
  );
}