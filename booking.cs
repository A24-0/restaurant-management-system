using RestaurantManagementSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManagementSystem
{
    public class Booking
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Comment { get; set; }
        public Table Table { get; set; }

        private static readonly List<Booking> _bookings = new List<Booking>();

        public static Booking CreateBooking(
            int clientId,
            string clientName,
            string phone,
            DateTime timeStart,
            DateTime timeEnd,
            string comment,
            Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            // Проверка пересечения бронирований для этого стола
            foreach (var b in _bookings)
            {
                if (b.Table == table && timeStart < b.TimeEnd && timeEnd > b.TimeStart)
                {
                    throw new InvalidOperationException(
                        $"Столик {table.ID} ({table.Location}) занят с {b.TimeStart:HH:mm} до {b.TimeEnd:HH:mm}.");
                }
            }
            var booking = new Booking
            {
                ClientId = clientId,
                ClientName = clientName,
                Phone = phone,
                TimeStart = timeStart,
                TimeEnd = timeEnd,
                Comment = comment,
                Table = table
            };
            _bookings.Add(booking);
            return booking;
        }

        public void EditBooking()
        {
            Table?.RemoveBooking(this);
            if (_bookings.Remove(this))
            {
                
            }
            else
            {
                Console.WriteLine($"Бронь '{ClientName}' не найдена в списке активных.");
            }
        }

        /*public void EditBooking(
            string clientName = null,
            string phone = null,
            DateTime? timeStart = null,
            DateTime? timeEnd = null,
            string comment = null,
            Table newTable = null)
        {
            var old = new Booking
            {
                ClientId = ClientId,
                ClientName = ClientName,
                Phone = Phone,
                TimeStart = TimeStart,
                TimeEnd = TimeEnd,
                Comment = Comment,
                Table = Table
            };

            _bookings.Remove(this);
            try
            {
                if (clientName != null) ClientName = clientName;
                if (phone != null) Phone = phone;
                if (timeStart.HasValue) TimeStart = timeStart.Value;
                if (timeEnd.HasValue) TimeEnd = timeEnd.Value;
                if (comment != null) Comment = comment;
                if (newTable != null) Table = newTable;

                // Проверка конфликтов
                foreach (var b in _bookings)
                {
                    if (b.Table == Table && TimeStart < b.TimeEnd && TimeEnd > b.TimeStart)
                    {
                        throw new InvalidOperationException(
                            $"Конфликт: {b.ClientName} уже забронировал столик {Table.ID} в это время.");
                    }
                }

                _bookings.Add(this);
                Console.WriteLine($"Бронь обновлена для {ClientName}");
            }
            catch
            {
                _bookings.Add(old);
                throw;
            }
        }*/

        public void CancelBooking()
        {
            Table?.RemoveBooking(this);
            if (_bookings.Remove(this))
            {
                ConsoleTheme.WriteSuccess($"Бронь клиента '{ClientName}' отменена.");
            }
            else
            {
                ConsoleTheme.WriteWarning($"Бронь '{ClientName}' не найдена в списке активных (возможно, уже отменена).");
            }
        }

        public static void DisplayAllBookings()
        {
            ConsoleTheme.PrintMenuHeader("Активные бронирования");
            if (!_bookings.Any())
            {
                ConsoleTheme.WriteInfo("Нет активных бронирований.");
                return;
            }

            foreach (var booking in _bookings.OrderBy(b => b.TimeStart))
            {
                string tableInfo = booking.Table != null
                    ? $"{booking.Table.ID:00} ({booking.Table.Location})"
                    : "Не назначен";

                var lines = new List<string>
                {
                    $"Клиент: {booking.ClientName}",
                    $"Телефон: {booking.Phone}",
                    $"Стол: {tableInfo}",
                    $"Время: {booking.TimeStart:dd.MM HH:mm} – {booking.TimeEnd:HH:mm}",
                    $"Комментарий: {(string.IsNullOrWhiteSpace(booking.Comment) ? "—" : booking.Comment)}"
                };

                ConsoleTheme.DrawCard($"Бронь #{booking.ClientId}", lines, ConsoleColor.DarkMagenta);
            }
        }
        public static List<Booking> GetAllBookings() => new List<Booking>(_bookings);
    }
}