import { useEffect, useState } from 'react';
import api from '../api/axios';
import Navbar from '../components/Navbar';

interface Provider {
  id: number; // or providerId
  name: string;
  mobileNumber: string;
  specialty: string;
  isActive: boolean;
}

export default function AdminDashboard() {
  const [providers, setProviders] = useState<Provider[]>([]);
  const [loadingId, setLoadingId] = useState<number | null>(null);

  useEffect(() => {
    loadProviders();
  }, []);

  const loadProviders = async () => {
    try {
      const res = await api.get('/admin/providers');
      setProviders(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  const toggleStatus = async (user: Provider) => {
    setLoadingId(user.id);
    try {
      await api.post(`/admin/toggle-status/${user.id}`);
      // Optimistic update
      setProviders(prev => prev.map(p => 
        p.id === user.id ? { ...p, isActive: !p.isActive } : p
      ));
    } catch (err) {
      alert("Failed to update status");
    } finally {
      setLoadingId(null);
    }
  };

  return (
    <>
      <Navbar />
      <div className="max-w-5xl mx-auto p-6">
        <h1 className="text-2xl font-bold mb-6">Provider Management</h1>
        
        <div className="bg-white rounded-lg shadow overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Name</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Mobile</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Specialty</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Action</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {providers.map((p) => (
                <tr key={p.id}>
                  <td className="px-6 py-4 whitespace-nowrap font-medium">{p.name}</td>
                  <td className="px-6 py-4 whitespace-nowrap">{p.mobileNumber}</td>
                  <td className="px-6 py-4 whitespace-nowrap">{p.specialty || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      p.isActive ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'
                    }`}>
                      {p.isActive ? 'Active' : 'Pending'}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <button
                      onClick={() => toggleStatus(p)}
                      disabled={loadingId === p.id}
                      className={`text-sm px-3 py-1 rounded text-white ${
                        p.isActive 
                        ? 'bg-red-500 hover:bg-red-600' 
                        : 'bg-green-500 hover:bg-green-600'
                      } disabled:opacity-50`}
                    >
                      {p.isActive ? 'Deactivate' : 'Activate'}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {providers.length === 0 && <div className="p-4 text-center text-gray-500">No providers found.</div>}
        </div>
      </div>
    </>
  );
}