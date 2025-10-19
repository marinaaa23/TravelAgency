using System;

namespace TravelAgency.Security
{
    public static class AuthService
    {
        private static string CurrentPassword = "admin123";

        public static bool Authenticate()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     СИСТЕМА БЕЗОПАСНОСТИ                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            int attempts = 3;

            while (attempts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Введите пароль: ");
                Console.ResetColor();

                // Простой ввод без скрытия
                string password = Console.ReadLine();

                if (password == CurrentPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Аутентификация успешна! Добро пожаловать в систему.");
                    Console.ResetColor();
                    Console.WriteLine();
                    System.Threading.Thread.Sleep(1500);
                    return true;
                }
                else
                {
                    attempts--;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Неверный пароль! Осталось попыток: {attempts}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("🚫 Доступ запрещен! Превышено количество попыток.");
            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            return false;
        }

        public static void ChangePassword(string newPassword)
        {
            CurrentPassword = newPassword;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Пароль успешно изменен!");
            Console.ResetColor();
        }

        public static void ShowDefaultPassword()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💡 Текущий пароль: {CurrentPassword}");
            Console.ResetColor();
        }
    }
}