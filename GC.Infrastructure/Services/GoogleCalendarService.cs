using GlobalConnect.Application.Modules.Booking.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Infrastructure.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly IConfiguration _config;

        public GoogleCalendarService(IConfiguration config) => _config = config;

        private CalendarService GetService(string refreshToken)
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _config["Google:ClientId"],
                    ClientSecret = _config["Google:ClientSecret"]
                }
            });

            var token = new TokenResponse { RefreshToken = refreshToken };
            var userCredential = new UserCredential(flow, "user", token);

            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = "GlobalConnect"
            });
        }

        public async Task<Event?> GetLatestEventAsync(string refreshToken, string calendarId)
        {
            var service = GetService(refreshToken);
            var request = service.Events.List(calendarId);
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            request.MaxResults = 1; // Get the most recently modified/added
            var events = await request.ExecuteAsync();
            return events.Items.FirstOrDefault();
        }

        public async Task DeleteEventAsync(string refreshToken, string calendarId, string eventId)
        {
            var service = GetService(refreshToken);
            await service.Events.Delete(calendarId, eventId).ExecuteAsync();
        }
    }
}
