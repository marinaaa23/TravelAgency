using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.UI
{
    public class MenuService
    {
        private readonly TravelRepository _repository;

        public MenuService()
        {
            _repository = new TravelRepository();
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader();
                ConsoleHelper.PrintMenuTitle("ГЛАВНОЕ МЕНЮ");

                ConsoleHelper.PrintMenuItem(1, "📋 Просмотр всех данных");
                ConsoleHelper.PrintMenuItem(2, "👥 Управление клиентами");
                ConsoleHelper.PrintMenuItem(3, "🏝️ Управление турами");
                ConsoleHelper.PrintMenuItem(4, "📅 Управление бронированиями");
                ConsoleHelper.PrintMenuItem(5, "📊 Отчеты и аналитика");
                ConsoleHelper.PrintMenuItem(0, "🚪 Выход");

                ConsoleHelper.PrintSeparator();

                var choice = ConsoleHelper.ReadChoice(0, 5);

                switch (choice)
                {
                    case 1:
                        ShowAllDataMenu();
                        break;
                    case 2:
                        ShowClientsMenu();
                        break;
                    case 3:
                        ShowToursMenu();
                        break;
                    case 4:
                        ShowBookingsMenu();
                        break;
                    case 5:
                        ShowReportsMenu();
                        break;
                    case 0:
                        ConsoleHelper.PrintSuccess("До свидания! 🛫");
                        return;
                }
            }
        }

        private void ShowAllDataMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ВСЕ ДАННЫЕ");

            ConsoleHelper.PrintInfo("--- КЛИЕНТЫ ---");
            var clients = _repository.GetAllClients();
            foreach (var client in clients)
            {
                Console.WriteLine($"  {client.ClientId}. {client.FullName} 📧 {client.Email} 📞 {client.Phone}");
            }

            ConsoleHelper.PrintInfo("\n--- ТУРЫ ---");
            var tours = _repository.GetAllTours();
            foreach (var tour in tours)
            {
                Console.WriteLine($"  {tour.TourId}. {tour.Title}");
                Console.WriteLine($"     🗺️  {tour.Destination} 💰 {tour.Price:0} руб.");
                Console.WriteLine($"     📅 {tour.StartDate:dd.MM.yyyy} - {tour.EndDate:dd.MM.yyyy}");
                Console.WriteLine($"     🎫 Доступно: {tour.AvailableSpots}/{tour.Capacity}");
            }

            ConsoleHelper.PrintInfo("\n--- БРОНИРОВАНИЯ ---");
            var bookings = _repository.GetAllBookings();
            foreach (var booking in bookings)
            {
                Console.WriteLine($"  {booking.BookingId}. {booking.Client?.FullName} → {booking.Tour?.Title}");
                Console.WriteLine($"     📅 {booking.BookingDate:dd.MM.yyyy} 👥 {booking.NumberOfPersons} чел.");
                Console.WriteLine($"     💰 {booking.TotalAmount:0} руб. 📝 {booking.Notes}");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void ShowReportsMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ОТЧЕТЫ И АНАЛИТИКА");

            ConsoleHelper.PrintMenuItem(1, "📈 Популярные туры за год");
            ConsoleHelper.PrintMenuItem(2, "📅 Бронирования текущего месяца");
            ConsoleHelper.PrintMenuItem(3, "👑 VIP клиенты (>3 бронирований)");
            ConsoleHelper.PrintMenuItem(0, "🔙 Назад");

            var choice = ConsoleHelper.ReadChoice(0, 3);

            switch (choice)
            {
                case 1:
                    ShowPopularTours();
                    break;
                case 2:
                    ShowCurrentMonthBookings();
                    break;
                case 3:
                    ShowVipClients();
                    break;
            }
        }

        private void ShowPopularTours()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ПОПУЛЯРНЫЕ ТУРЫ ЗА ГОД");

            var popularTours = _repository.GetMostPopularToursLastYear();

            if (popularTours.Any())
            {
                int place = 1;
                foreach (var tour in popularTours)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"🏆 {place}. ");
                    Console.ResetColor();
                    Console.WriteLine($"{tour.Title}");
                    Console.WriteLine($"   🗺️  {tour.Destination} 💰 {tour.Price:0} руб.");
                    Console.WriteLine($"   📅 {tour.StartDate:dd.MM.yyyy} - {tour.EndDate:dd.MM.yyyy}");
                    Console.WriteLine();
                    place++;
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Популярные туры не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void ShowCurrentMonthBookings()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("БРОНИРОВАНИЯ ТЕКУЩЕГО МЕСЯЦА");

            var bookings = _repository.GetBookingsInCurrentMonth();

            if (bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"📅 {booking.BookingDate:dd.MM.yyyy} ");
                    Console.ResetColor();
                    Console.WriteLine($"{booking.Client?.FullName}");
                    Console.WriteLine($"   🏝️  {booking.Tour?.Title}");
                    Console.WriteLine($"   👥 {booking.NumberOfPersons} чел. 💰 {booking.TotalAmount:0} руб.");
                    Console.WriteLine($"   📝 {booking.Notes}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Бронирования за текущий месяц не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void ShowVipClients()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("VIP КЛИЕНТЫ (>3 БРОНИРОВАНИЙ)");

            var vipClients = _repository.GetClientsWithMoreThan3Bookings();

            if (vipClients.Any())
            {
                foreach (var client in vipClients)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"👑 ");
                    Console.ResetColor();
                    Console.WriteLine($"{client.FullName}");
                    Console.WriteLine($"   📧 {client.Email} 📞 {client.Phone}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("VIP клиенты не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        // Заглушки для остальных меню
        private void ShowClientsMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ КЛИЕНТАМИ");
            ConsoleHelper.PrintInfo("Функционал в разработке... 🛠️");
            ConsoleHelper.WaitForContinue();
        }

        private void ShowToursMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ ТУРАМИ");
            ConsoleHelper.PrintInfo("Функционал в разработке... 🛠️");
            ConsoleHelper.WaitForContinue();
        }

        private void ShowBookingsMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ БРОНИРОВАНИЯМИ");
            ConsoleHelper.PrintInfo("Функционал в разработке... 🛠️");
            ConsoleHelper.WaitForContinue();
        }
    }
}