using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Adds the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        public void AddBooking(Booking booking)
        {
            Console.WriteLine(" > BookingRepo: AddBooking");
            // Ayaw i set manually and ID kay mag error
            this.GetDbSet<Booking>().Add(booking);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Checks if booking exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool BookingExists(int id)
        {
            Console.WriteLine(" > BookingRepo: BookingExists");
            return this.GetDbSet<Booking>().Any(x => x.Id == id);
        }

        /// <summary>
        /// Cancels the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void CancelBooking(int id)
        {
            Console.WriteLine(" > BookingRepo: CancelBooking");
            var bookingToCancel = this.GetDbSet<Booking>().FirstOrDefault(x => x.Cancelled != true && x.Id == id);
            if (bookingToCancel != null)
            {
                bookingToCancel.Cancelled = true;
                bookingToCancel.UpdatedDate = DateTime.Now;
                bookingToCancel.UpdatedBy = "[Cancelled]";
            }
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Deletes the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteBooking(int id)
        {
            Console.WriteLine(" > BookingRepo: DeleteBooking");
            var bookingToDelete = this.GetDbSet<Booking>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (bookingToDelete != null)
            {
                bookingToDelete.Deleted = true;
                bookingToDelete.UpdatedDate = DateTime.Now;
                bookingToDelete.UpdatedBy = "[Deleted]";
            }
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Gets the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The booking that match the id.</returns>
        public Booking GetBooking(int id)
        {
            Console.WriteLine(" > BookingRepo: GetBooking");
            return this.GetDbSet<Booking>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets all bookings.
        /// </summary>
        /// <returns>All bookings.</returns>
        public IQueryable<Booking> GetBookings()
        {
            Console.WriteLine(" > BookingRepo: GetBookings");
            return this.GetDbSet<Booking>().Where(b => !b.Cancelled); 
        }

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        public void UpdateBooking(Booking booking)
        {
            Console.WriteLine(" > BookingRepo: UpdateBooking");
            this.GetDbSet<Booking>().Update(booking);
            UnitOfWork.SaveChanges();
        }
    }
}
