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

        // === СУЩЕСТВУЮЩИЕ МЕТОДЫ ===

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

        // === НЕДОСТАЮЩИЕ МЕТОДЫ ===

        // 4. Получить всех клиентов
        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        // 5. Получить все туры
        public List<Tour> GetAllTours()
        {
            return _context.Tours
                .Include(t => t.Operator)
                .ToList();
        }

        // 6. Получить все бронирования
        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.Tour)
                .ThenInclude(t => t.Operator)
                .ToList();
        }

        // 7. Получить все доступные туры (свободные места > 0)
        public List<Tour> GetAvailableTours()
        {
            return _context.Tours
                .Include(t => t.Operator)
                .Where(t => t.IsActive && t.AvailableSpots > 0)
                .ToList();
        }

        // 8. Добавить нового клиента
        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        // 9. Создать бронирование
        public void AddBooking(Booking booking)
        {
            // Проверяем доступность мест
            var tour = _context.Tours.Find(booking.TourId);
            if (tour != null && tour.AvailableSpots >= booking.NumberOfPersons)
            {
                _context.Bookings.Add(booking);

                // Обновляем доступные места
                tour.AvailableSpots -= booking.NumberOfPersons;
                _context.SaveChanges();

                Console.WriteLine($"✅ Бронирование создано! Осталось мест: {tour.AvailableSpots}");
            }
            else
            {
                Console.WriteLine("❌ Недостаточно свободных мест для бронирования!");
            }
        }

        // 10. Получить статистику по турам (сколько клиентов забронировали)
        public List<TourStatistics> GetTourStatistics()
        {
            return _context.Tours
                .Include(t => t.Operator)
                .Select(t => new TourStatistics
                {
                    Tour = t,
                    TotalBookings = _context.Bookings.Count(b => b.TourId == t.TourId),
                    TotalClients = _context.Bookings
                        .Where(b => b.TourId == t.TourId)
                        .Select(b => b.ClientId)
                        .Distinct()
                        .Count()
                })
                .ToList();
        }

        // 11. Получить клиента по ID
        public Client? GetClientById(int clientId)
        {
            return _context.Clients.Find(clientId);
        }

        // 12. Получить тур по ID
        public Tour? GetTourById(int tourId)
        {
            return _context.Tours
                .Include(t => t.Operator)
                .FirstOrDefault(t => t.TourId == tourId);
        }
    }

    // Класс для статистики по турам
    public class TourStatistics
    {
        public Tour Tour { get; set; } = null!;
        public int TotalBookings { get; set; }
        public int TotalClients { get; set; }
    }
}