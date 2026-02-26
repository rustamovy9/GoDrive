import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { carsApi } from "../../services/api/carsApi";
import type { CarCreateInput } from "../../entities/Car/types";
import { Button } from "../../shared/components/Button";
import { Input } from "../../shared/components/Input";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { getApiErrorMessage } from "../../shared/utils/apiError";

const STEPS = [
  { id: 1, title: "Car info" },
  { id: 2, title: "Price per day" },
  { id: 3, title: "Upload images" },
  { id: 4, title: "Upload documents" },
];

export function OwnerCarCreatePage() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [imageFiles, setImageFiles] = useState<File[]>([]);
  const [docFiles, setDocFiles] = useState<File[]>([]);
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { register, handleSubmit, watch, formState: { errors } } = useForm<CarCreateInput>({
    defaultValues: { make: "", model: "", year: new Date().getFullYear(), pricePerDay: 0 },
  });

  const make = watch("make");
  const model = watch("model");
  const year = watch("year");
  const pricePerDay = watch("pricePerDay");

  async function onSubmit(data: CarCreateInput) {
    setError("");
    setIsSubmitting(true);
    try {
      const { data: car } = await carsApi.create(data);
      if (imageFiles.length > 0) {
        const fd = new FormData();
        imageFiles.forEach((f) => fd.append("images", f));
        try {
          await carsApi.uploadImages(car.id, fd);
        } catch {
          // optional
        }
      }
      if (docFiles.length > 0) {
        const fd = new FormData();
        docFiles.forEach((f) => fd.append("documents", f));
        try {
          await carsApi.uploadDocuments(car.id, fd);
        } catch {
          // optional
        }
      }
      navigate(`/owner/cars/${car.id}`);
    } catch (err) {
      setError(getApiErrorMessage(err));
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="max-w-2xl mx-auto space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Add car</h1>

      <div className="flex gap-2">
        {STEPS.map((s) => (
          <button
            key={s.id}
            type="button"
            onClick={() => setStep(s.id)}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors duration-200 ${
              step === s.id ? "bg-blue-600 text-white" : "bg-white text-gray-600 hover:bg-gray-100 border border-gray-200"
            }`}
          >
            {s.id}. {s.title}
          </button>
        ))}
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6 space-y-6">
        {error && <ErrorMessage message={error} />}

        {step === 1 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold text-gray-800">Car info</h2>
            <Input label="Make" {...register("make", { required: "Required" })} error={errors.make?.message} />
            <Input label="Model" {...register("model", { required: "Required" })} error={errors.model?.message} />
            <Input
              label="Year"
              type="number"
              {...register("year", { required: "Required", valueAsNumber: true, min: { value: 1900, message: "Invalid" } })}
              error={errors.year?.message}
            />
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
              <textarea
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                rows={3}
                {...register("description")}
              />
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold text-gray-800">Price per day</h2>
            <Input
              label="Price per day ($)"
              type="number"
              step={0.01}
              {...register("pricePerDay", { required: "Required", valueAsNumber: true, min: { value: 0, message: "Min 0" } })}
              error={errors.pricePerDay?.message}
            />
            <Input label="Image URL (optional)" {...register("imageUrl")} />
          </div>
        )}

        {step === 3 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold text-gray-800">Upload images</h2>
            <input
              type="file"
              accept="image/*"
              multiple
              onChange={(e) => setImageFiles(Array.from(e.target.files ?? []))}
              className="w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-blue-50 file:text-blue-700"
            />
            {imageFiles.length > 0 && <p className="text-gray-500 text-sm">{imageFiles.length} file(s) selected</p>}
          </div>
        )}

        {step === 4 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold text-gray-800">Upload documents</h2>
            <input
              type="file"
              accept=".pdf,.doc,.docx"
              multiple
              onChange={(e) => setDocFiles(Array.from(e.target.files ?? []))}
              className="w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-blue-50 file:text-blue-700"
            />
            {docFiles.length > 0 && <p className="text-gray-500 text-sm">{docFiles.length} file(s) selected</p>}
          </div>
        )}

        <div className="flex justify-between pt-4">
          <Button
            type="button"
            variant="secondary"
            onClick={() => setStep((s) => Math.max(1, s - 1))}
          >
            Back
          </Button>
          {step < 4 ? (
            <Button type="button" onClick={() => setStep((s) => Math.min(4, s + 1))}>
              Next
            </Button>
          ) : (
            <Button type="submit" isLoading={isSubmitting}>
              Submit
            </Button>
          )}
        </div>
      </form>
    </div>
  );
}
