using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Data
{
    public class TravelRepository
    {
        private readonly TravelDbContext _context;

        public TravelRepository()
        {
            _context = new TravelDbContext();
        }

        // 1. Найти клиентов, забронировавших более 3 путевок
        public List<Client> GetClientsWithMoreThan3Bookings()
        {
            return _context.Clients
                .Where(c => _context.Bookings
                    .Count(b => b.ClientId == c.ClientId && b.Status == "Confirmed") > 3)
                .ToList();
        }

        // 2. Определить наиболее популярные путевки за последний год
        public List<Tour> GetMostPopularToursLastYear(int topCount = 5)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);

            return _context.Tours
                .Where(t => t.StartDate >= oneYearAgo)
                .Select(t => new
                {
                    Tour = t,
                    BookingCount = _context.Bookings
                        .Count(b => b.TourId == t.TourId &&
                                   b.BookingDate >= oneYearAgo &&
                                   b.Status == "Confirmed")
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(topCount)
                .Select(x => x.Tour)
                .ToList();
        }

        // 3. Найти бронирования, сделанные в текущем месяце
        public List<Booking> GetBookingsInCurrentMonth()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.Tour)
                .ThenInclude(t => t.Operator)
                .Where(b => b.BookingDate >= firstDayOfMonth &&
                           b.BookingDate <= lastDayOfMonth)
                .OrderByDescending(b => b.BookingDate)
                .ToList();
        }

        // Дополнительные методы для работы с данными
        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public List<Tour> GetAllTours()
        {
            return _context.Tours.Include(t => t.Operator).ToList();
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.Tour)
                .ThenInclude(t => t.Operator)
                .ToList();
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }
    }
}