export interface Car {
  id: string;
  make: string;
  model: string;
  year: number;
  pricePerDay: number;
  imageUrl?: string;
  images?: string[];
  description?: string;
  ownerId?: string;
  isAvailable?: boolean;
  city?: string;
  category?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface CarCreateInput {
  make: string;
  model: string;
  year: number;
  pricePerDay: number;
  imageUrl?: string;
  description?: string;
}

export type CarUpdateInput = Partial<CarCreateInput>;
