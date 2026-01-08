using GlobalConnect.Application.Modules.Booking.DTOs;

namespace GlobalConnect.Application.Modules.Booking.Interfaces
{
    public interface IBookingService
    {

        Task<List<BookingDto>> GetMyAppointments(int userId);
        Task<int> SyncWithGoogle(int userId, SyncRequest request);
    }
}
