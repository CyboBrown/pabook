using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        public AnalyticsService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IMapper mapper,
            IMemoryCache cache,
            IHttpContextAccessor contextAccessor)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _cache = cache;
            _contextAccessor = contextAccessor;
        }

        public AnalyticsViewModel GetAnalyticsDashboard()
        {
            try
            {
                return new AnalyticsViewModel
                {
                    TotalBookings = GetTotalBookings(),
                    TotalCancelledBookings = GetCancelledBookings(),
                    TotalUsers = GetTotalUsers(),
                    TotalDeletedUsers = GetDeletedUsers(),
                    MostBookedRoom = GetMostBookedRoom(),
                    PeakTime = GetPeakTime()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAnalyticsDashboard: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new AnalyticsViewModel();
            }
        }

        public int GetTotalBookings()
        {
            // Count all bookings, including cancelled ones
            return _bookingRepository.GetAllBookingsIncludingCancelled().Count();
        }

        public int GetCancelledBookings()
        {
            // Count all bookings that are marked as cancelled but not deleted
            return _bookingRepository.GetAllBookingsIncludingCancelled().Count(b => b.Cancelled && !b.Deleted);
        }

        public int GetTotalUsers()
        {
            return _userRepository.GetUsers().Count(u => !u.Deleted);
        }

        public int GetDeletedUsers()
        {
            return _userRepository.GetUsers().Count(u => u.Deleted);
        }

        public string GetMostBookedRoom()
        {
            var mostBookedRoom = _bookingRepository.GetBookings()
                .Where(b => !b.Deleted && !b.Cancelled)
                .GroupBy(b => b.RoomId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (mostBookedRoom == 0)
                return "No bookings";

            var room = _roomRepository.GetRoom(mostBookedRoom);
            return room?.Name ?? "Unknown Room";
        }

        public string GetPeakTime()
        {
            var peakHour = _bookingRepository.GetBookings()
                .Where(b => !b.Deleted && !b.Cancelled)
                .GroupBy(b => b.StartTime.Hours)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return $"{peakHour:D2}:00 - {(peakHour + 1) % 24:D2}:00";
        }

        public Dictionary<string, MonthlyDataViewModel> GetMonthlyData()
        {
            var currentYear = DateTime.Now.Year;
            var monthlyData = new Dictionary<string, MonthlyDataViewModel>();

            for (int month = 1; month <= 12; month++)
            {
                var startDate = new DateTime(currentYear, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var bookings = _bookingRepository.GetAllBookingsIncludingCancelled()
                    .Where(b => !b.Deleted &&
                                b.Date >= startDate.Date &&
                                b.Date <= endDate.Date)
                    .ToList();

                monthlyData.Add(startDate.ToString("MMM"), new MonthlyDataViewModel
                {
                    BookingSummary = bookings.Count(),
                    RoomUsage = bookings.Where(b => !b.Cancelled).Select(b => b.RoomId).Distinct().Count()
                });
            }

            return monthlyData;
        }
    }
}





