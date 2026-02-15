import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Search, MapPin } from 'lucide-react';
import api from '../api/axios';

interface ProviderSummary {
  id: number;
  name: string;
  specialty: string;
  photoUrl: string;
  nationality?: string;
}

export default function Home() {
  const [providers, setProviders] = useState<ProviderSummary[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchProviders();
  }, []);

  const fetchProviders = async (search = '') => {
    setLoading(true);
    try {
      // Calls the public endpoint we just created
      const res = await api.get(`/public/providers?search=${search}`);
      setProviders(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    fetchProviders(searchTerm);
  };

  return (
    <div className="flex flex-col min-h-screen">
      
      {/* Hero Section */}
      <section className="bg-gradient-to-r from-blue-600 to-blue-800 text-white py-20 px-4 text-center">
        <h1 className="text-4xl md:text-5xl font-bold mb-4">Find Your Shor (Consultation)</h1>
        <p className="text-blue-100 text-lg mb-8 max-w-2xl mx-auto">
          Connect with top-rated doctors and specialists instantly. No registration required.
        </p>

        {/* Search Bar */}
        <form onSubmit={handleSearch} className="max-w-xl mx-auto relative flex shadow-2xl rounded-full">
          <input
            type="text"
            placeholder="Search by doctor name or specialty..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-6 pr-14 py-4 rounded-full text-gray-800 focus:outline-none focus:ring-4 focus:ring-blue-300 transition"
          />
          <button 
            type="submit"
            className="absolute right-2 top-2 bg-blue-600 hover:bg-blue-700 text-white p-2.5 rounded-full transition"
          >
            <Search size={24} />
          </button>
        </form>
      </section>

      {/* Providers Grid */}
      <main className="max-w-7xl mx-auto py-12 px-4 w-full">
        <div className="flex justify-between items-center mb-8">
          <h2 className="text-2xl font-bold text-gray-800">Available Specialists</h2>
          <Link to="/register" className="text-blue-600 hover:underline font-medium text-sm">
            Are you a doctor? Join us
          </Link>
        </div>

        {loading ? (
          <div className="text-center py-20 text-gray-500">Loading specialists...</div>
        ) : providers.length === 0 ? (
          <div className="text-center py-20 bg-white rounded-xl shadow-sm border border-dashed border-gray-300">
            <p className="text-gray-500 text-lg">No providers found matching "{searchTerm}".</p>
            <button onClick={() => {setSearchTerm(''); fetchProviders('')}} className="mt-4 text-blue-600 font-bold hover:underline">
              Clear Search
            </button>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            {providers.map((provider) => (
              <Link 
                key={provider.id} 
                to={`/view/${provider.id}`}
                className="group bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden hover:shadow-xl hover:-translate-y-1 transition-all duration-300"
              >
                <div className="h-56 bg-gray-100 relative overflow-hidden">
                   <img 
                    src={provider.photoUrl || '/assets/default-avatar.png'} 
                    alt={provider.name}
                    onError={(e) => (e.currentTarget.src = '/assets/default-avatar.png')}
                    className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                  />
                  <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent opacity-0 group-hover:opacity-100 transition-opacity flex items-end p-4">
                    <span className="text-white font-medium">View Profile &rarr;</span>
                  </div>
                </div>
                
                <div className="p-5">
                  <h3 className="font-bold text-lg text-gray-900 mb-1 truncate">{provider.name}</h3>
                  <p className="text-blue-600 font-medium text-sm mb-3">{provider.specialty}</p>
                  
                  {provider.nationality && (
                    <div className="flex items-center gap-1 text-gray-500 text-xs bg-gray-50 px-2 py-1 rounded w-fit">
                      <MapPin size={12} />
                      {provider.nationality}
                    </div>
                  )}
                </div>
              </Link>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}