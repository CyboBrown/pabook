using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetBookings();
        bool BookingExists(int id);
        Booking GetBooking(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void CancelBooking(int id);
        void DeleteBooking(int id);

    }
}
