interface AvatarProps {
  src?: string | null;
  name?: string;
  size?: "sm" | "md" | "lg";
  className?: string;
}

const sizes = { sm: "w-8 h-8 text-xs", md: "w-10 h-10 text-sm", lg: "w-12 h-12 text-base" };

function initials(name: string): string {
  return name
    .split(" ")
    .map((s) => s[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);
}

export function Avatar({ src, name, size = "md", className = "" }: AvatarProps) {
  if (src) {
    return (
      <img
        src={src}
        alt={name ?? "Avatar"}
        className={`rounded-full object-cover ${sizes[size]} ${className}`}
      />
    );
  }
  return (
    <div
      className={`rounded-full bg-blue-600 text-white flex items-center justify-center font-medium ${sizes[size]} ${className}`}
    >
      {name ? initials(name) : "?"}
    </div>
  );
}
