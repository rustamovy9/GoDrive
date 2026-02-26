import { useForm } from "react-hook-form";
import { Button } from "../../shared/components/Button";
import { Input } from "../../shared/components/Input";
import type { BookingCreateInput } from "../../entities/Booking/types";

interface BookingFormProps {
  carId: string;
  onSubmit: (data: BookingCreateInput) => void;
  isLoading?: boolean;
  minDate?: string;
}

export function BookingForm({ carId, onSubmit, isLoading, minDate }: BookingFormProps) {
  const { register, handleSubmit, watch, formState: { errors } } = useForm<BookingCreateInput>({
    defaultValues: { carId, startDate: "", endDate: "" },
  });
  const startDate = watch("startDate");

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <input type="hidden" {...register("carId")} />
      <Input
        label="Start date"
        type="date"
        {...register("startDate", { required: "Required" })}
        error={errors.startDate?.message}
        min={minDate}
      />
      <Input
        label="End date"
        type="date"
        {...register("endDate", {
          required: "Required",
          validate: (v) => !startDate || v >= startDate || "End must be after start",
        })}
        error={errors.endDate?.message}
        min={startDate || minDate}
      />
      <Button type="submit" isLoading={isLoading} fullWidth>
        Book
      </Button>
    </form>
  );
}
