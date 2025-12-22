using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace GlobalConnect.Application.Modules.Booking.Interfaces
{
    public interface IGoogleCalendarService
    {
        Task<Event?> GetLatestEventAsync(string refreshToken, string calendarId);
        Task DeleteEventAsync(string refreshToken, string calendarId, string eventId);
    }
}
