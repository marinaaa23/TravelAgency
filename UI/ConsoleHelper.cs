using System;

namespace TravelAgency.UI
{
    public static class ConsoleHelper
    {
        // Цвета палитры 2025 года
        private static ConsoleColor Pink = ConsoleColor.Magenta;
        private static ConsoleColor Brown = ConsoleColor.DarkYellow;
        private static ConsoleColor Cream = ConsoleColor.Yellow;
        private static ConsoleColor Text = ConsoleColor.White;
        private static ConsoleColor Success = ConsoleColor.Green;
        private static ConsoleColor Error = ConsoleColor.Red;

        public static void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = Pink;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                              ║");
            Console.ForegroundColor = Cream;
            Console.WriteLine("║                 🏝️  ТУРИСТИЧЕСКОЕ АГЕНТСТВО                 ║");
            Console.ForegroundColor = Pink;
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintMenuTitle(string title)
        {
            Console.ForegroundColor = Brown;
            Console.WriteLine($"┌─ {title} ──────────────────────────────────────────────");
            Console.ResetColor();
        }

        public static void PrintMenuItem(int number, string text)
        {
            Console.ForegroundColor = Cream;
            Console.Write($" {number}. ");
            Console.ForegroundColor = Text;
            Console.WriteLine(text);
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = Success;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = Error;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = Brown;
            Console.WriteLine($"ℹ {message}");
            Console.ResetColor();
        }

        public static void PrintSeparator()
        {
            Console.ForegroundColor = Pink;
            Console.WriteLine("──────────────────────────────────────────────────────────────");
            Console.ResetColor();
        }

        public static int ReadChoice(int min, int max)
        {
            Console.ForegroundColor = Cream;
            Console.Write("Выберите пункт: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
            {
                return choice;
            }

            PrintError($"Введите число от {min} до {max}");
            return ReadChoice(min, max);
        }

        public static void WaitForContinue()
        {
            Console.ForegroundColor = Brown;
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}