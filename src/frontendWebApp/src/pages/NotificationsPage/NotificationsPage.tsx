import { useNotifications, useMarkNotificationRead } from "../../features/notifications/useNotifications";
import { formatDateTime } from "../../shared/utils/formatDate";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";

export function NotificationsPage() {
  const { data: notifications, isLoading, error } = useNotifications();
  const markRead = useMarkNotificationRead();

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Notifications</h1>
      <div className="space-y-3">
        {(notifications ?? []).map((n) => (
          <div key={n.id} className={`bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-5 ${n.read ? "border border-gray-100" : "border border-blue-200 bg-blue-50/30"}`}>
            <div className="flex justify-between items-start">
              <div>
                <h3 className="font-medium text-gray-900">{n.title}</h3>
                <p className="text-gray-600 text-sm mt-1">{n.message}</p>
                <p className="text-xs text-gray-400 mt-2">{formatDateTime(n.createdAt)}</p>
              </div>
              {!n.read && (
                <button type="button" onClick={() => markRead.mutate(n.id)} className="text-sm text-blue-600 hover:underline">Mark read</button>
              )}
            </div>
          </div>
        ))}
      </div>
      {(notifications?.length ?? 0) === 0 && <p className="text-gray-500 py-8">No notifications.</p>}
    </div>
  );
}
