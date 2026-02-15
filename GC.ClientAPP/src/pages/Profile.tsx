import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import api from '../api/axios';
import Navbar from '../components/Navbar';
import { useAuth } from '../context/AuthContext';

export default function Profile() {
  const { register, handleSubmit, reset } = useForm();
  const { login, user } = useAuth(); // To update name in navbar
  const [message, setMessage] = useState('');

  useEffect(() => {
    api.get('/profile').then(res => reset(res.data));
  }, [reset]);

  const onSubmit = async (data: any) => {
    try {
      await api.put('/profile', data);
      setMessage('Profile updated successfully!');
      // Update local context info
      const token = localStorage.getItem('token') || '';
      const role = localStorage.getItem('role') || '';
      login(token, role, data.name);
    } catch (err) {
      setMessage('Failed to update profile.');
    }
  };

  return (
    <>
      <Navbar />
      <div className="max-w-2xl mx-auto p-6 mt-10">
        <div className="bg-white p-8 rounded-xl shadow">
          <h2 className="text-2xl font-bold mb-4">My Profile</h2>
          
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Full Name</label>
              <input {...register('name')} className="mt-1 block w-full p-2 border rounded" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Specialty</label>
              <input {...register('specialty')} className="mt-1 block w-full p-2 border rounded" />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Bio</label>
              <textarea {...register('bio')} className="mt-1 block w-full p-2 border rounded" rows={3} />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Google Booking URL</label>
              <input {...register('googleBookingUrl')} className="mt-1 block w-full p-2 border rounded" type="url" />
            </div>

            {message && <div className="text-green-600 text-sm font-bold">{message}</div>}

            <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
              Save Changes
            </button>
          </form>
        </div>
      </div>
    </>
  );
}