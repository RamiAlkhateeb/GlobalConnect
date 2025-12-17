using GlobalConnect.Application.Modules.Booking.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Application.Modules.Booking.Interfaces
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(int seekerId, CreateBookingDto request);
        Task CancelBookingAsync(int userId, int bookingId);
    }
}
