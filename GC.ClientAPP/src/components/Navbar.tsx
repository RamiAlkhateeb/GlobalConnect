import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { LogIn, LogOut, User } from 'lucide-react'; // Ensure lucide-react is installed

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <nav className="bg-white shadow-sm border-b sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16 items-center">
          
          {/* Logo */}
          <Link to="/" className="text-2xl font-bold text-blue-600 tracking-tight flex items-center gap-2">
            Shor <span className="text-gray-400 text-base font-normal">| Consultations</span>
          </Link>

          {/* Actions */}
          <div className="flex items-center gap-4">
            {user ? (
              // Logged In View
              <>
                <span className="hidden md:block text-sm text-gray-600">
                  Welcome, <strong>{user.name}</strong>
                </span>
                
                <Link 
                  to={user.role === 'Admin' ? '/admin' : '/profile'}
                  className="p-2 text-gray-500 hover:text-blue-600 transition rounded-full hover:bg-blue-50"
                  title="My Profile"
                >
                  <User size={20} />
                </Link>

                <button 
                  onClick={handleLogout}
                  className="flex items-center gap-2 text-red-500 hover:text-red-700 text-sm font-medium px-3 py-2 rounded-md hover:bg-red-50 transition"
                >
                  <LogOut size={18} />
                  <span className="hidden sm:inline">Logout</span>
                </button>
              </>
            ) : (
              // Guest View
              <div className="flex items-center gap-4">
                 <Link 
                  to="/login" 
                  className="text-gray-600 hover:text-blue-600 font-medium text-sm flex items-center gap-1"
                >
                  <LogIn size={16} />
                  Provider Login
                </Link>
                <Link 
                  to="/register" 
                  className="bg-blue-600 text-white px-5 py-2 rounded-full hover:bg-blue-700 text-sm font-bold shadow-md hover:shadow-lg transition transform hover:-translate-y-0.5"
                >
                  Join as Doctor
                </Link>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}