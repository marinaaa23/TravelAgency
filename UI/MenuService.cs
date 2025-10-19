using System;
using System.Linq;
using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.Security;

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
                ConsoleHelper.PrintMenuItem(6, "🔐 Управление безопасностью");
                ConsoleHelper.PrintMenuItem(0, "🚪 Выход");

                ConsoleHelper.PrintSeparator();

                var choice = ConsoleHelper.ReadChoice(0, 6);

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
                    case 6:
                        ShowSecurityMenu();
                        break;
                    case 0:
                        ConsoleHelper.PrintSuccess("До свидания! 🛫");
                        return;
                }
            }
        }

        private void ShowSecurityMenu()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ БЕЗОПАСНОСТЬЮ");

            ConsoleHelper.PrintMenuItem(1, "🔑 Сменить пароль");
            ConsoleHelper.PrintMenuItem(2, "ℹ️ Показать текущий пароль");
            ConsoleHelper.PrintMenuItem(0, "🔙 Назад");

            ConsoleHelper.PrintSeparator();

            var choice = ConsoleHelper.ReadChoice(0, 2);

            switch (choice)
            {
                case 1:
                    ChangePassword();
                    break;
                case 2:
                    AuthService.ShowDefaultPassword();
                    ConsoleHelper.WaitForContinue();
                    break;
            }
        }

        private void ChangePassword()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("СМЕНА ПАРОЛЯ");

            Console.Write("Введите новый пароль: ");
            string newPassword = Console.ReadLine();

            Console.Write("Повторите новый пароль: ");
            string confirmPassword = Console.ReadLine();

            if (newPassword == confirmPassword)
            {
                AuthService.ChangePassword(newPassword);
                ConsoleHelper.PrintSuccess("Пароль успешно изменен!");
            }
            else
            {
                ConsoleHelper.PrintError("Пароли не совпадают!");
            }

            ConsoleHelper.WaitForContinue();
        }

        // Остальные методы остаются без изменений
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

        private void ShowClientsMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader();
                ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ КЛИЕНТАМИ");

                ConsoleHelper.PrintMenuItem(1, "👥 Список всех клиентов");
                ConsoleHelper.PrintMenuItem(2, "➕ Добавить нового клиента");
                ConsoleHelper.PrintMenuItem(0, "🔙 Назад");

                ConsoleHelper.PrintSeparator();

                var choice = ConsoleHelper.ReadChoice(0, 2);

                switch (choice)
                {
                    case 1:
                        ShowAllClients();
                        break;
                    case 2:
                        AddNewClient();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ShowToursMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader();
                ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ ТУРАМИ");

                ConsoleHelper.PrintMenuItem(1, "🏝️ Доступные туры");
                ConsoleHelper.PrintMenuItem(2, "📊 Статистика по турам");
                ConsoleHelper.PrintMenuItem(0, "🔙 Назад");

                ConsoleHelper.PrintSeparator();

                var choice = ConsoleHelper.ReadChoice(0, 2);

                switch (choice)
                {
                    case 1:
                        ShowAvailableTours();
                        break;
                    case 2:
                        ShowTourStatistics();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ShowBookingsMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader();
                ConsoleHelper.PrintMenuTitle("УПРАВЛЕНИЕ БРОНИРОВАНИЯМИ");

                ConsoleHelper.PrintMenuItem(1, "📋 Все бронирования");
                ConsoleHelper.PrintMenuItem(2, "➕ Создать бронирование");
                ConsoleHelper.PrintMenuItem(0, "🔙 Назад");

                ConsoleHelper.PrintSeparator();

                var choice = ConsoleHelper.ReadChoice(0, 2);

                switch (choice)
                {
                    case 1:
                        ShowAllBookings();
                        break;
                    case 2:
                        CreateNewBooking();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ShowAllClients()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("СПИСОК ВСЕХ КЛИЕНТОВ");

            var clients = _repository.GetAllClients();

            if (clients.Any())
            {
                foreach (var client in clients)
                {
                    Console.WriteLine($"  {client.ClientId}. {client.FullName}");
                    Console.WriteLine($"     📧 {client.Email} 📞 {client.Phone}");
                    Console.WriteLine($"     🆔 Паспорт: {client.PassportNumber}");
                    if (client.DateOfBirth.HasValue)
                        Console.WriteLine($"     🎂 Дата рождения: {client.DateOfBirth:dd.MM.yyyy}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Клиенты не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void AddNewClient()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ДОБАВЛЕНИЕ НОВОГО КЛИЕНТА");

            Console.Write("Имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            Console.Write("Номер паспорта: ");
            string passport = Console.ReadLine();

            Console.Write("Дата рождения (гггг-мм-дд): ");
            string dateInput = Console.ReadLine();

            var newClient = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                PassportNumber = passport
            };

            if (DateTime.TryParse(dateInput, out DateTime birthDate))
            {
                newClient.DateOfBirth = birthDate;
            }

            _repository.AddClient(newClient);
            ConsoleHelper.PrintSuccess($"Клиент {firstName} {lastName} успешно добавлен!");

            ConsoleHelper.WaitForContinue();
        }

        private void ShowAvailableTours()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ДОСТУПНЫЕ ТУРЫ");

            var tours = _repository.GetAvailableTours();

            if (tours.Any())
            {
                foreach (var tour in tours)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"🏝️  {tour.TourId}. {tour.Title}");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"   🗺️  {tour.Destination}");
                    Console.WriteLine($"   💰 {tour.Price:0} руб.");
                    Console.WriteLine($"   📅 {tour.StartDate:dd.MM.yyyy} - {tour.EndDate:dd.MM.yyyy}");
                    Console.WriteLine($"   🎫 Свободно мест: {tour.AvailableSpots}/{tour.Capacity}");
                    Console.WriteLine($"   🏢 Туроператор: {tour.Operator?.Name}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Доступные туры не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void ShowTourStatistics()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("СТАТИСТИКА ПО ТУРАМ");

            var statistics = _repository.GetTourStatistics();

            if (statistics.Any())
            {
                foreach (var stat in statistics)
                {
                    Console.WriteLine($"📊 {stat.Tour.Title}");
                    Console.WriteLine($"   👥 Клиентов забронировало: {stat.TotalClients}");
                    Console.WriteLine($"   📅 Всего бронирований: {stat.TotalBookings}");
                    Console.WriteLine($"   🎫 Свободно мест: {stat.Tour.AvailableSpots}/{stat.Tour.Capacity}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Статистика не доступна");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void ShowAllBookings()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("ВСЕ БРОНИРОВАНИЯ");

            var bookings = _repository.GetAllBookings();

            if (bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"📅 {booking.BookingDate:dd.MM.yyyy}");
                    Console.WriteLine($"   👤 {booking.Client?.FullName}");
                    Console.WriteLine($"   🏝️  {booking.Tour?.Title}");
                    Console.WriteLine($"   👥 {booking.NumberOfPersons} чел. 💰 {booking.TotalAmount:0} руб.");
                    Console.WriteLine($"   📝 {booking.Notes}");
                    Console.WriteLine($"   🏷️  Статус: {booking.Status}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintInfo("Бронирования не найдены");
            }

            ConsoleHelper.WaitForContinue();
        }

        private void CreateNewBooking()
        {
            ConsoleHelper.PrintHeader();
            ConsoleHelper.PrintMenuTitle("СОЗДАНИЕ БРОНИРОВАНИЯ");

            // Показываем доступные туры
            ConsoleHelper.PrintInfo("ДОСТУПНЫЕ ТУРЫ:");
            var availableTours = _repository.GetAvailableTours();
            foreach (var tour in availableTours)
            {
                Console.WriteLine($"  {tour.TourId}. {tour.Title} - {tour.AvailableSpots} свободно");
            }

            // Показываем клиентов
            ConsoleHelper.PrintInfo("\nСУЩЕСТВУЮЩИЕ КЛИЕНТЫ:");
            var clients = _repository.GetAllClients();
            foreach (var client in clients)
            {
                Console.WriteLine($"  {client.ClientId}. {client.FullName}");
            }

            Console.Write("\nID клиента: ");
            if (int.TryParse(Console.ReadLine(), out int clientId))
            {
                Console.Write("ID тура: ");
                if (int.TryParse(Console.ReadLine(), out int tourId))
                {
                    Console.Write("Количество человек: ");
                    if (int.TryParse(Console.ReadLine(), out int persons))
                    {
                        var client = _repository.GetClientById(clientId);
                        var tour = _repository.GetTourById(tourId);

                        if (client != null && tour != null)
                        {
                            var booking = new Booking
                            {
                                ClientId = clientId,
                                TourId = tourId,
                                NumberOfPersons = persons,
                                TotalAmount = tour.Price * persons,
                                Status = "Confirmed",
                                Notes = "Бронирование через систему"
                            };

                            _repository.AddBooking(booking);
                        }
                        else
                        {
                            ConsoleHelper.PrintError("Клиент или тур не найден!");
                        }
                    }
                    else
                    {
                        ConsoleHelper.PrintError("Неверное количество человек!");
                    }
                }
                else
                {
                    ConsoleHelper.PrintError("Неверный ID тура!");
                }
            }
            else
            {
                ConsoleHelper.PrintError("Неверный ID клиента!");
            }

            ConsoleHelper.WaitForContinue();
        }
    }
}