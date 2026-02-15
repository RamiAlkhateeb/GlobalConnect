import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from '../api/axios';

export default function PublicProvider() {
  const { id } = useParams();
  const [provider, setProvider] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/public/provider/${id}`)
      .then(res => setProvider(res.data))
      .catch(() => setProvider(null))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div className="text-center mt-10">Loading...</div>;
  if (!provider) return <div className="text-center mt-10 text-red-500">Provider not found</div>;

  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
      <div className="bg-white p-6 rounded-2xl shadow-xl max-w-sm w-full text-center">
        <div className="w-32 h-32 mx-auto rounded-full overflow-hidden border-4 border-white shadow-lg -mt-16 bg-gray-200">
           <img 
             src={provider.photoUrl || '/assets/default-avatar.png'} 
             alt={provider.name}
             onError={(e) => (e.currentTarget.src = '/assets/default-avatar.png')}
             className="w-full h-full object-cover"
           />
        </div>
        
        <h1 className="text-2xl font-bold mt-4 text-gray-800">{provider.name}</h1>
        <p className="text-blue-600 font-medium">{provider.specialty}</p>
        
        {provider.nationality && (
          <span className="inline-block bg-gray-100 rounded-full px-3 py-1 text-xs font-semibold text-gray-600 mt-2">
            📍 {provider.nationality}
          </span>
        )}

        <p className="mt-4 text-gray-600 text-sm leading-relaxed">
          {provider.bio || 'No biography available.'}
        </p>

        <a 
          href={provider.googleBookingUrl} 
          target="_blank"
          rel="noreferrer"
          className="block mt-6 w-full bg-green-500 text-white font-bold py-3 rounded-full hover:bg-green-600 transition transform hover:scale-105"
        >
          📅 Book Appointment
        </a>
      </div>
    </div>
  );
}