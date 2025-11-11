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

        // Связь с заказом
        public Order LinkedOrder { get; set; }

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

            // Проверка времени работы ресторана (9:00-23:00)
            if (timeStart.Hour < 9 || timeEnd.Hour > 23)
            {
                throw new InvalidOperationException(
                    $"Ресторан работает с 09:00 до 23:00. Выберите время в этом диапазоне.");
            }

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
                Table = table,
                LinkedOrder = null
            };
            _bookings.Add(booking);

            // Добавляем бронь в расписание стола
            table.AddBooking(booking);

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

        public void CancelBooking()
        {
            // Проверяем, есть ли связанный заказ
            if (LinkedOrder != null && !LinkedOrder.IsClosed)
            {
                ConsoleTheme.WriteWarning($"Невозможно отменить бронь - к ней привязан активный заказ #{LinkedOrder.OrderId}. Сначала закройте заказ.");
                return;
            }

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

                string orderInfo = booking.LinkedOrder != null
                    ? $"Заказ #{booking.LinkedOrder.OrderId} ({(booking.LinkedOrder.IsClosed ? "Закрыт" : "Активен")})"
                    : "Заказ не создан";

                var lines = new List<string>
                {
                    $"Клиент: {booking.ClientName}",
                    $"Телефон: {booking.Phone}",
                    $"Стол: {tableInfo}",
                    $"Время: {booking.TimeStart:dd.MM HH:mm} – {booking.TimeEnd:HH:mm}",
                    $"Статус: {orderInfo}",
                    $"Комментарий: {(string.IsNullOrWhiteSpace(booking.Comment) ? "—" : booking.Comment)}"
                };

                ConsoleTheme.DrawCard($"Бронь #{booking.ClientId}", lines, ConsoleColor.DarkMagenta);
            }
        }

        public static List<Booking> GetAllBookings() => new List<Booking>(_bookings);

        // Проверка наличия брони на столик (активной сейчас ИЛИ будущей)
        public static bool HasActiveOrUpcomingBookingForTable(int tableId)
        {
            DateTime now = DateTime.Now;
            foreach (var booking in _bookings)
            {
                // Бронь либо уже активна, либо начнётся в будущем (но не закончилась)
                if (booking.Table.ID == tableId && booking.TimeEnd > now)
                {
                    return true;
                }
            }
            return false;
        }

        // Получить бронь для столика (активную или ближайшую будущую)
        public static Booking GetActiveOrUpcomingBookingForTable(int tableId)
        {
            DateTime now = DateTime.Now;

            // Сначала ищем активную бронь (сейчас)
            foreach (var booking in _bookings)
            {
                if (booking.Table.ID == tableId &&
                    booking.TimeStart <= now &&
                    booking.TimeEnd > now)
                {
                    return booking;
                }
            }

            // Если активной нет, ищем ближайшую будущую
            Booking nearestBooking = null;
            foreach (var booking in _bookings)
            {
                if (booking.Table.ID == tableId &&
                    booking.TimeStart > now &&
                    (nearestBooking == null || booking.TimeStart < nearestBooking.TimeStart))
                {
                    nearestBooking = booking;
                }
            }

            return nearestBooking;
        }

        // Проверка наличия активной брони на столике ТОЛЬКО в текущее время
        public static bool HasActiveBookingForTable(int tableId)
        {
            DateTime now = DateTime.Now;
            foreach (var booking in _bookings)
            {
                if (booking.Table.ID == tableId &&
                    booking.TimeStart <= now &&
                    booking.TimeEnd > now)
                {
                    return true;
                }
            }
            return false;
        }

        // Получить активную бронь для столика (только если активна СЕЙЧАС)
        public static Booking GetActiveBookingForTable(int tableId)
        {
            DateTime now = DateTime.Now;
            foreach (var booking in _bookings)
            {
                if (booking.Table.ID == tableId &&
                    booking.TimeStart <= now &&
                    booking.TimeEnd > now)
                {
                    return booking;
                }
            }
            return null;
        }

        // Связать заказ с бронью
        public void LinkOrder(Order order)
        {
            if (LinkedOrder != null && !LinkedOrder.IsClosed)
            {
                throw new InvalidOperationException($"К этой брони уже привязан активный заказ #{LinkedOrder.OrderId}");
            }
            LinkedOrder = order;
        }
    }
}