import { useForm } from "react-hook-form";
import { Button } from "../../shared/components/Button";
import { Input } from "../../shared/components/Input";
import type { CarCreateInput, CarUpdateInput } from "../../entities/Car/types";

type FormValues = CarCreateInput;

interface CarFormProps {
  defaultValues?: Partial<FormValues>;
  onSubmit: (data: CarCreateInput | CarUpdateInput) => void;
  isLoading?: boolean;
  submitLabel?: string;
}

export function CarForm({
  defaultValues,
  onSubmit,
  isLoading,
  submitLabel = "Save",
}: CarFormProps) {
  const { register, handleSubmit, formState: { errors } } = useForm<FormValues>({
    defaultValues: defaultValues ?? {},
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <Input label="Make" {...register("make", { required: "Required" })} error={errors.make?.message} />
      <Input label="Model" {...register("model", { required: "Required" })} error={errors.model?.message} />
      <Input
        label="Year"
        type="number"
        {...register("year", { required: "Required", valueAsNumber: true, min: { value: 1900, message: "Invalid" } })}
        error={errors.year?.message}
      />
      <Input
        label="Price per day"
        type="number"
        step={0.01}
        {...register("pricePerDay", { required: "Required", valueAsNumber: true, min: { value: 0, message: "Min 0" } })}
        error={errors.pricePerDay?.message}
      />
      <Input label="Image URL" {...register("imageUrl")} />
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
        <textarea
          className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          rows={3}
          {...register("description")}
        />
      </div>
      <Button type="submit" isLoading={isLoading} fullWidth>
        {submitLabel}
      </Button>
    </form>
  );
}
