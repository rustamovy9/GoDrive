import type { ReactNode } from "react";

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  children: ReactNode;
}

export function Modal({ isOpen, onClose, title, children }: ModalProps) {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div className="absolute inset-0 bg-black/50" onClick={onClose} aria-hidden />
      <div className="relative bg-white rounded-xl shadow-xl max-w-md w-full max-h-[90vh] overflow-auto" role="dialog" aria-modal="true">
        {title && (
          <div className="flex items-center justify-between p-4 border-b">
            <h2 className="text-lg font-semibold text-gray-900">{title}</h2>
            <button type="button" onClick={onClose} className="p-1 rounded hover:bg-gray-100 text-gray-500" aria-label="Close">
              ×
            </button>
          </div>
        )}
        <div className="p-4">{children}</div>
      </div>
    </div>
  );
}
