using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.UI;

// Запускаем красивое меню
var menuService = new MenuService();
menuService.ShowMainMenu();

// Создаем репозиторий
var repository = new TravelRepository();

Console.WriteLine("=== СИСТЕМА УЧЕТА ТУРИСТИЧЕСКИХ ПУТЕВОК ===");

// Показываем все данные
Console.WriteLine("\n--- Все клиенты ---");
var clients = repository.GetAllClients();
foreach (var client in clients)
{
    Console.WriteLine($"{client.ClientId}: {client.FullName} - {client.Email}");
}

Console.WriteLine("\n--- Все туры ---");
var tours = repository.GetAllTours();
foreach (var tour in tours)
{
    Console.WriteLine($"{tour.TourId}: {tour.Title} - {tour.Destination} - {tour.Price} руб.");
}

Console.WriteLine("\n--- Все бронирования ---");
var bookings = repository.GetAllBookings();
foreach (var booking in bookings)
{
    Console.WriteLine($"{booking.BookingId}: {booking.Client?.FullName} - {booking.Tour?.Title} - {booking.TotalAmount} руб.");
}

// Тестируем функции из задания
Console.WriteLine("\n--- Бронирования текущего месяца ---");
var currentMonthBookings = repository.GetBookingsInCurrentMonth();
foreach (var booking in currentMonthBookings)
{
    Console.WriteLine($"{booking.BookingDate:dd.MM.yyyy}: {booking.Client?.FullName} - {booking.Tour?.Title}");
}

Console.WriteLine("\n--- Самые популярные туры за год ---");
var popularTours = repository.GetMostPopularToursLastYear();
foreach (var tour in popularTours)
{
    Console.WriteLine($"{tour.Title} - {tour.Destination}");
}

Console.WriteLine("\n--- Клиенты с более чем 3 бронированиями ---");
var activeClients = repository.GetClientsWithMoreThan3Bookings();
foreach (var client in activeClients)
{
    Console.WriteLine($"{client.FullName} - {client.Email}");
}

Console.WriteLine("\nНажмите любую клавишу для выхода...");
Console.ReadKey();