import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useNavigate } from 'react-router-dom';
import api from '../api/axios';

export default function Register() {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = async (data: any) => {
    setIsLoading(true);
    setError('');
    
    try {
      await api.post('/auth/register', data);
      setSuccess(true);
    } catch (err: any) {
      // Handle generic or specific errors from backend
      setError(err.response?.data?.message || 'Registration failed. Try a different number.');
    } finally {
      setIsLoading(false);
    }
  };

  if (success) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-100 p-4">
        <div className="bg-white p-8 rounded-xl shadow-lg w-full max-w-md text-center">
          <div className="w-16 h-16 bg-green-100 text-green-500 rounded-full flex items-center justify-center mx-auto mb-4 text-3xl">
            ✓
          </div>
          <h2 className="text-2xl font-bold text-gray-800 mb-2">Registration Successful!</h2>
          <p className="text-gray-600 mb-6">
            Your account has been created. Please wait for an administrator to activate your account before logging in.
          </p>
          <Link 
            to="/login" 
            className="inline-block bg-blue-600 text-white px-6 py-2 rounded-md hover:bg-blue-700 transition"
          >
            Back to Login
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-4">
      <div className="bg-white p-8 rounded-xl shadow-lg w-full max-w-md">
        <h2 className="text-2xl font-bold mb-2 text-center text-slate-800">Join Shor</h2>
        <p className="text-center text-gray-500 mb-6">Create your provider account</p>
        
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          
          {/* Full Name */}
          <div>
            <label className="block text-sm font-medium text-gray-700">Full Name</label>
            <input 
              {...register('name', { required: 'Name is required' })} 
              className={`mt-1 block w-full p-2 border rounded-md ${errors.name ? 'border-red-500' : 'border-gray-300'}`}
              placeholder="Dr. Name"
            />
            {errors.name && <span className="text-red-500 text-xs">{errors.name.message as string}</span>}
          </div>

          {/* Mobile Number */}
          <div>
            <label className="block text-sm font-medium text-gray-700">Mobile Number</label>
            <input 
              {...register('mobileNumber', { 
                required: 'Mobile Number is required',
                pattern: { value: /^01[0125][0-9]{8}$/, message: 'Invalid Egyptian mobile number' } 
              })} 
              className={`mt-1 block w-full p-2 border rounded-md ${errors.mobileNumber ? 'border-red-500' : 'border-gray-300'}`}
              placeholder="01xxxxxxxxx"
            />
            {errors.mobileNumber && <span className="text-red-500 text-xs">{errors.mobileNumber.message as string}</span>}
          </div>

          {/* Password */}
          <div>
            <label className="block text-sm font-medium text-gray-700">Password</label>
            <input 
              type="password" 
              {...register('password', { required: 'Password is required', minLength: { value: 6, message: 'Min 6 chars' } })} 
              className={`mt-1 block w-full p-2 border rounded-md ${errors.password ? 'border-red-500' : 'border-gray-300'}`}
            />
            {errors.password && <span className="text-red-500 text-xs">{errors.password.message as string}</span>}
          </div>

          {/* Error Message */}
          {error && <div className="text-red-500 text-sm bg-red-50 p-2 rounded text-center">{error}</div>}

          {/* Submit Button */}
          <button 
            type="submit" 
            disabled={isLoading}
            className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 disabled:bg-blue-300 transition"
          >
            {isLoading ? 'Creating Account...' : 'Sign Up'}
          </button>
        </form>

        <div className="mt-4 text-center text-sm text-gray-600">
          Already have an account? <Link to="/login" className="text-blue-600 hover:underline">Login here</Link>
        </div>
      </div>
    </div>
  );
}