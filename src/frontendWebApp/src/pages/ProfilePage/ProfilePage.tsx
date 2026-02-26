import { useRef, useState } from "react";
import { Link } from "react-router-dom";
import { useAuthStore } from "../../features/auth/useAuth";
import { Avatar } from "../../shared/components/Avatar";
import { usersApi } from "../../services/api/usersApi";
import { getApiErrorMessage } from "../../shared/utils/apiError";

export function ProfilePage() {
  const user = useAuthStore((s) => s.user);
  const setUser = useAuthStore((s) => s.setUser);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [uploadError, setUploadError] = useState("");
  const [uploading, setUploading] = useState(false);

  if (!user) return null;

  async function handleAvatarChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file || !user) return;
    setUploadError("");
    setUploading(true);
    try {
      const formData = new FormData();
      formData.append("avatar", file);
      const { data } = await usersApi.uploadAvatar(user.id, formData);
      setUser({ ...user, avatarUrl: data.avatarUrl ?? user.avatarUrl });
    } catch (err) {
      setUploadError(getApiErrorMessage(err));
    } finally {
      setUploading(false);
      e.target.value = "";
    }
  }

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Profile</h1>
      <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
        <div className="flex flex-col sm:flex-row items-start sm:items-center gap-6">
          <div className="relative">
            <Avatar src={user.avatarUrl} name={user.name} size="lg" />
            <input
              ref={fileInputRef}
              type="file"
              accept="image/*"
              className="hidden"
              onChange={handleAvatarChange}
              disabled={uploading}
            />
            <button
              type="button"
              onClick={() => fileInputRef.current?.click()}
              disabled={uploading}
              className="absolute bottom-0 right-0 bg-blue-600 text-white rounded-full p-1.5 shadow hover:bg-blue-700 transition-colors duration-200 disabled:opacity-50"
            >
              {uploading ? (
                <span className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin block" />
              ) : (
                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 9a2 2 0 012-2h.93a2 2 0 001.664-.89l.812-1.22A2 2 0 0110.07 4h3.86a2 2 0 011.664.89l.812 1.22A2 2 0 0018.07 7H19a2 2 0 012 2v9a2 2 0 01-2 2H5a2 2 0 01-2-2V9z" />
                </svg>
              )}
            </button>
          </div>
          <div className="space-y-1">
            <p className="text-xl font-semibold text-gray-800">{user.name}</p>
            <p className="text-gray-600">{user.email}</p>
            <p className="text-gray-400 text-sm">Role: {user.role}</p>
            {uploadError && (
              <p className="text-red-600 text-sm mt-2">{uploadError}</p>
            )}
          </div>
        </div>
        <div className="mt-8 pt-6 border-t border-gray-100 flex flex-wrap gap-4">
          <Link
            to="/my-bookings"
            className="text-blue-600 hover:text-blue-700 font-medium"
          >
            My bookings
          </Link>
          <Link
            to="/notifications"
            className="text-blue-600 hover:text-blue-700 font-medium"
          >
            Notifications
          </Link>
          <Link
            to="/reviews"
            className="text-blue-600 hover:text-blue-700 font-medium"
          >
            Reviews
          </Link>
        </div>
      </div>
    </div>
  );
}
