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

        public void AddBooking(Booking booking)
        {
            Console.WriteLine(" > BookingRepo: AddBooking");
            // Ayaw i set manually and ID kay mag error
            this.GetDbSet<Booking>().Add(booking);
            UnitOfWork.SaveChanges();
        }

        public bool BookingExists(int id)
        {
            Console.WriteLine(" > BookingRepo: BookingExists");
            return this.GetDbSet<Booking>().Any(x => x.Id == id);
        }

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

        public Booking GetBooking(int id)
        {
            Console.WriteLine(" > BookingRepo: GetBooking");
            return this.GetDbSet<Booking>().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Booking> GetBookings()
        {
            Console.WriteLine(" > BookingRepo: GetBookings");
            return this.GetDbSet<Booking>().Where(b => !b.Cancelled); 
        }

        public void UpdateBooking(Booking booking)
        {
            Console.WriteLine(" > BookingRepo: UpdateBooking");
            this.GetDbSet<Booking>().Update(booking);
            UnitOfWork.SaveChanges();
        }
    }
}
