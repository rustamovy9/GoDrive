"use client";

import { useEffect, useState } from "react";
import { Building2, Pencil, Trash2 } from "lucide-react";
import { useTranslations } from "next-intl";

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
  const t = useTranslations("Company");

  const [company, setCompany] = useState<Company | null>(null);
  const [locations, setLocations] = useState<Location[]>([]);
  const [loading, setLoading] = useState(true);

  const [openForm, setOpenForm] = useState(false);
  const [openDelete, setOpenDelete] = useState(false);

  const [isEdit, setIsEdit] = useState(false);

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
    const c = data?.data?.data?.[0];

    if (c) {
      setCompany(c);
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
    if (!token) return;

    Promise.all([fetchCompany(), fetchLocations()]).finally(() =>
      setLoading(false)
    );
  }, []);

  const openCreate = () => {
    setIsEdit(false);
    setForm({ name: "", contactInfo: "", locationId: 1 });
    setOpenForm(true);
  };

  const openEdit = () => {
    if (!company) return;

    setIsEdit(true);
    setForm({
      name: company.name,
      contactInfo: company.contactInfo,
      locationId: company.locationId,
    });

    setOpenForm(true);
  };

  const handleCreate = async () => {
    await fetch(
      "https://godrive-5r3o.onrender.com/api/rental-companies",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(form),
      }
    );

    setOpenForm(false);
    fetchCompany();
  };

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

    setOpenForm(false);
    fetchCompany();
  };

  const handleDelete = async () => {
    if (!company) return;

    await fetch(
      `https://godrive-5r3o.onrender.com/api/rental-companies/${company.id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    setOpenDelete(false);
    setCompany(null);
  };

  if (loading) {
    return (
      <main className="p-8">
        <div className="h-8 w-48 bg-zinc-800 animate-pulse rounded mb-4" />
        <div className="h-40 bg-zinc-900 animate-pulse rounded-2xl" />
      </main>
    );
  }

  return (
    <main className="p-8 text-white">

      <h1 className="text-3xl font-bold mb-6">
        {t("title")}
      </h1>

      {!company ? (
        <div className="p-10 bg-zinc-900 rounded-2xl text-center border border-zinc-700">
          <Building2 className="mx-auto mb-3 text-zinc-500" size={50} />

          <p className="mb-4">{t("noCompany")}</p>

          <button
            onClick={openCreate}
            className="bg-cyan-500 px-5 py-2 rounded-xl"
          >
            ➕ {t("create")}
          </button>
        </div>
      ) : (
        <div className="p-8 bg-zinc-900 rounded-2xl border border-zinc-700 max-w-xl">

          <h2 className="text-xl font-bold">{company.name}</h2>
          <p className="text-zinc-400">
            {company.city}, {company.county}
          </p>

          <p className="mt-2">📞 {company.contactInfo}</p>

          <div className="flex gap-3 mt-6">

            <button
              onClick={openEdit}
              className="bg-yellow-500 px-4 py-2 rounded-lg flex items-center gap-2"
            >
              <Pencil size={16} />
              {t("edit")}
            </button>

            <button
              onClick={() => setOpenDelete(true)}
              className="bg-red-500 px-4 py-2 rounded-lg flex items-center gap-2"
            >
              <Trash2 size={16} />
              {t("delete")}
            </button>

          </div>
        </div>
      )}

      {openForm && (
        <div className="fixed inset-0 bg-black/70 flex items-center justify-center z-50">

          <div className="bg-zinc-900 p-6 rounded-2xl w-full max-w-md border border-zinc-700">

            <h2 className="text-lg font-bold mb-4">
              {isEdit ? t("editCompany") : t("createCompany")}
            </h2>

            <input
              className="w-full p-3 mb-3 bg-zinc-800 rounded-lg"
              placeholder={t("name")}
              value={form.name}
              onChange={(e) =>
                setForm({ ...form, name: e.target.value })
              }
            />

            <input
              className="w-full p-3 mb-3 bg-zinc-800 rounded-lg"
              placeholder={t("contact")}
              value={form.contactInfo}
              onChange={(e) =>
                setForm({ ...form, contactInfo: e.target.value })
              }
            />

            <select
              className="w-full p-3 mb-4 bg-zinc-800 rounded-lg"
              value={form.locationId}
              onChange={(e) =>
                setForm({
                  ...form,
                  locationId: Number(e.target.value),
                })
              }
            >
              {locations.map((l) => (
                <option key={l.id} value={l.id}>
                  {l.city}, {l.country}
                </option>
              ))}
            </select>

            <div className="flex justify-end gap-3">

              <button
                onClick={() => setOpenForm(false)}
                className="px-4 py-2 bg-zinc-700 rounded-lg"
              >
                {t("cancel")}
              </button>

              <button
                onClick={isEdit ? handleUpdate : handleCreate}
                className="px-4 py-2 bg-cyan-500 rounded-lg"
              >
                {t("save")}
              </button>

            </div>

          </div>
        </div>
      )}

      {openDelete && (
        <div className="fixed inset-0 bg-black/70 flex items-center justify-center z-50">

          <div className="bg-zinc-900 p-6 rounded-2xl border border-zinc-700 w-full max-w-sm">

            <h2 className="text-lg font-bold mb-4">
              {t("confirmDelete")}
            </h2>

            <p className="text-zinc-400 mb-6">
              {t("deleteConfirmText")}
            </p>

            <div className="flex justify-end gap-3">

              <button
                onClick={() => setOpenDelete(false)}
                className="px-4 py-2 bg-zinc-700 rounded-lg"
              >
                {t("cancel")}
              </button>

              <button
                onClick={handleDelete}
                className="px-4 py-2 bg-red-500 rounded-lg"
              >
                Delete
              </button>

            </div>

          </div>

        </div>
      )}

    </main>
  );
}