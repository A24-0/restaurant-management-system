using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManagementSystem
{
    public static class ConsoleTheme
    {
        private const int MinWidth = 48;
        private const int MaxWidth = 90;

        public static void PrintBanner(string title, string subtitle = null)
        {
            var lines = new List<string> { title };
            if (!string.IsNullOrWhiteSpace(subtitle))
            {
                lines.Add(subtitle);
            }

            int width = Clamp(lines.Max(line => line.Length) + 10, MinWidth, MaxWidth);
            string border = new string('=', width);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"+{border}+");
            foreach (var line in lines)
            {
                Console.WriteLine($"| {Center(line, width - 2)} |");
            }
            Console.WriteLine($"+{border}+");
            Reset();
        }

        public static void PrintMenuHeader(string title)
        {
            int width = Clamp(title.Length + 8, MinWidth, MaxWidth);
            string border = new string('=', width);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine();
            Console.WriteLine($"+{border}+");
            Console.WriteLine($"| {Center(title.ToUpperInvariant(), width - 2)} |");
            Console.WriteLine($"+{border}+");
            Reset();
        }

        public static void PrintMenuOption(string key, string description, bool highlight = false)
        {
            if (highlight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"  [{key}] {description}");
            Reset();
        }

        public static void DrawSeparator(int width = MinWidth)
        {
            width = Clamp(width, MinWidth, MaxWidth);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', width));
            Reset();
        }

        public static void DrawCard(string title, IEnumerable<string> lines, ConsoleColor borderColor = ConsoleColor.DarkGray, ConsoleColor titleColor = ConsoleColor.White)
        {
            var content = (lines ?? Enumerable.Empty<string>()).Select(line => line ?? string.Empty).ToList();
            int contentWidth = content.Any() ? content.Max(line => line.Length) : 0;
            int width = Clamp(Math.Max(title.Length + 8, contentWidth + 6), MinWidth, MaxWidth);
            string border = new string('=', width);

            Console.ForegroundColor = borderColor;
            Console.WriteLine($"+{border}+");
            Reset();

            Console.ForegroundColor = titleColor;
            Console.WriteLine($"| {Center(title.ToUpperInvariant(), width - 2)} |");
            Reset();

            Console.ForegroundColor = borderColor;
            Console.WriteLine($"| {new string('-', width - 2)} |");
            Reset();

            foreach (var line in content)
            {
                Console.WriteLine($"| {PadRight(line, width - 2)} |");
            }

            Console.ForegroundColor = borderColor;
            Console.WriteLine($"+{border}+");
            Reset();
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[i] {message}");
            Reset();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[ok] {message}");
            Reset();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[!] {message}");
            Reset();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[x] {message}");
            Reset();
        }

        public static void WritePrompt(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(message);
            Reset();
        }

        public static void WriteKeyValue(string key, string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{key}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value);
            Reset();
        }

        public static void WriteScheduleEntry(string timeSlot, string content)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{timeSlot} ");
            Console.ForegroundColor = string.IsNullOrWhiteSpace(content) ? ConsoleColor.DarkGray : ConsoleColor.White;
            Console.WriteLine(content);
            Reset();
        }

        public static void WriteList(IEnumerable<string> items, string bullet = "•", int indent = 2)
        {
            string prefix = new string(' ', Math.Max(0, indent));
            foreach (var item in items)
            {
                Console.WriteLine($"{prefix}{bullet} {item}");
            }
        }

        public static void ClearScreen()
        {
            Console.Clear();
            Reset();
        }

        private static string Center(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) return new string(' ', width);
            text = text.Length > width ? text.Substring(0, width) : text;
            int padding = (width - text.Length) / 2;
            return new string(' ', padding) + text + new string(' ', width - padding - text.Length);
        }

        private static string PadRight(string text, int targetWidth)
        {
            text ??= string.Empty;
            if (text.Length >= targetWidth)
            {
                return text.Substring(0, targetWidth);
            }
            return text + new string(' ', targetWidth - text.Length);
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private static void Reset()
        {
            Console.ResetColor();
        }
    }
}

