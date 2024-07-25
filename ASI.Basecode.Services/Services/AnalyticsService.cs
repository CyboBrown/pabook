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

        /// <summary>
        /// Retrieves a comprehensive analytics dashboard containing various metrics for summary cards.
        /// </summary>
        /// <returns>An AnalyticsViewModel containing total bookings, cancelled bookings, total users, 
        /// deleted users, most booked room, and peak time. Returns an empty AnalyticsViewModel if an exception occurs.</returns>
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
                return new AnalyticsViewModel();
            }
        }

        /// <summary>
        /// Retrieves the total number of bookings, including cancelled ones.
        /// </summary>
        /// <returns>An integer representing the total number of bookings.</returns>
        public int GetTotalBookings()
        {
            return _bookingRepository.GetAllBookingsIncludingCancelled().Count();
        }

        /// <summary>
        /// Retrieves the number of cancelled bookings that have not been deleted.
        /// </summary>
        /// <returns>An integer representing the number of cancelled bookings.</returns>
        public int GetCancelledBookings()
        {
            // Count all bookings that are marked as cancelled but not deleted
            return _bookingRepository.GetAllBookingsIncludingCancelled().Count(b => b.Cancelled && !b.Deleted);
        }

        /// <summary>
        /// Retrieves the total number of active (non-deleted) users.
        /// </summary>
        /// <returns>An integer representing the total number of active users.</returns>
        public int GetTotalUsers()
        {
            return _userRepository.GetUsers().Count(u => !u.Deleted);
        }

        /// <summary>
        /// Retrieves the number of deleted users.
        /// </summary>
        /// <returns>An integer representing the number of deleted users.</returns>
        public int GetDeletedUsers()
        {
            return _userRepository.GetUsers().Count(u => u.Deleted);
        }

        /// <summary>
        /// Retrieves the name of the most booked room.
        /// </summary>
        /// <returns>A string containing the name of the most booked room, "No bookings" if there are no bookings, 
        /// or "Unknown Room" if the room cannot be found.</returns>
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

        // <summary>
        /// Retrieves the peak booking time.
        /// </summary>
        /// <returns>A string representing the peak booking hour in the format "HH:00 - HH:00".</returns>
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

        /// <summary>
        /// Retrieves monthly data for the current year.
        /// </summary>
        /// <returns>A Dictionary with month abbreviations as keys and MonthlyDataViewModel objects as values, 
        /// containing booking summaries and room usage for each month.</returns>
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

        /// <summary>
        /// Retrieves monthly data for a specific year and month.
        /// </summary>
        /// <param name="year">The year for which to retrieve data.</param>
        /// <param name="month">The month for which to retrieve data.</param>
        /// <returns>A Dictionary with a single entry, where the key is the month abbreviation and the value is a MonthlyDataViewModel 
        /// containing booking summary and room usage for the specified month.</returns>
        public Dictionary<string, MonthlyDataViewModel> GetMonthlyData(int year, int month)
        {
            var monthlyData = new Dictionary<string, MonthlyDataViewModel>();

            var startDate = new DateTime(year, month, 1);
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

            return monthlyData;
        }

        /// <summary>
        /// Retrieves yearly data for a specific year.
        /// </summary>
        /// <param name="year">The year for which to retrieve data.</param>
        /// <returns>A Dictionary with month abbreviations as keys and MonthlyDataViewModel objects as values, 
        /// containing booking summaries and room usage for each month of the specified year.</returns>
        public Dictionary<string, MonthlyDataViewModel> GetYearlyData(int year)
        {
            var yearlyData = new Dictionary<string, MonthlyDataViewModel>();

            for (int month = 1; month <= 12; month++)
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var bookings = _bookingRepository.GetAllBookingsIncludingCancelled()
                    .Where(b => !b.Deleted &&
                                b.Date >= startDate.Date &&
                                b.Date <= endDate.Date)
                    .ToList();

                yearlyData.Add(startDate.ToString("MMM"), new MonthlyDataViewModel
                {
                    BookingSummary = bookings.Count(),
                    RoomUsage = bookings.Where(b => !b.Cancelled).Select(b => b.RoomId).Distinct().Count()
                });
            }
            return yearlyData;
        }
    }
}





