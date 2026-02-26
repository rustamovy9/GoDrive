export function ErrorMessage({ message }: { message: string }) {
  return (
    <div className="text-red-600 bg-red-100 border border-red-300 rounded-lg p-3 my-2">
      {message}
    </div>
  );
}
