using TravelAgency.Security;
using TravelAgency.UI;

// Проверка аутентификации
if (!AuthService.Authenticate())
{
    return; // Выход если аутентификация не пройдена
}

// Запускаем красивое меню
try
{
    var menuService = new MenuService();
    menuService.ShowMainMenu();
}
catch (Exception ex)
{
    Console.WriteLine($"Произошла ошибка: {ex.Message}");
    Console.WriteLine("Нажмите любую клавишу для выхода...");
    Console.ReadKey();
}