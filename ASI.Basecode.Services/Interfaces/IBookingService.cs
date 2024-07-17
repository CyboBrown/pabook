using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<BookingViewModel> GetAll(int? id = null, string title = null, string room = null);
        IEnumerable<BookingViewModel> GetAllBookings();
        IEnumerable<BookingViewModel> GetUserBookings();
        /*void Add(BookingViewModel model);*/
        void Delete(int id);
        void AddBooking(BookingViewModel booking);
        BookingViewModel GetBookingById(int id);
        void UpdateBooking(BookingViewModel booking);
        void CancelBooking(int id);
        bool CheckBookingAvailability(BookingViewModel booking);
    }
}
