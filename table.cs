using RestaurantManagementSystem;
using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem
{
    public class Table
    {
        public int ID { get; set; }
        public string Location { get; set; }
        public int Seats { get; set; }
        public Dictionary<string, Booking> Schedule { get; set; }

        // Конструктор
        public Table(int id, string location, int seats)
        {
            ID = id;
            Location = location;
            Seats = seats;
            Schedule = new Dictionary<string, Booking>();
            InitializeSchedule();
        }

        public static List<Table> Tables { get; private set; } = new List<Table>();

        // Инициализация столов
        public static void InitializeTables(Table[] tables)
        {
            Tables.Clear();
            Tables.AddRange(tables);
        }

        // Инициализация расписания (9:00-18:00+)
        private void InitializeSchedule()
        {
            for (int hour = 9; hour <= 18; hour++)
            {
                string timeSlot = $"{hour:00}:00-{hour + 1:00}:00";
                Schedule[timeSlot] = null;
            }
        }

        // Создать стол
        public static Table CreateTable(int id, string location, int seats)
        {
            return new Table(id, location, seats);
        }

        // Изменить данные стола (только если нет активных броней)
        public bool UpdateTable(string newLocation, int newSeats)
        {
            if (HasActiveBookings())
            {
                Console.WriteLine("Нельзя изменить стол - есть активные брони!");
                return false;
            }

            Location = newLocation;
            Seats = newSeats;
            return true;
        }

        // Показать информацию о столе с расписанием
        public void ShowTableInfo()
        {
            Console.WriteLine($"ID: {ID:00}");
            Console.WriteLine($"Расположение: {Location}");
            Console.WriteLine($"Количество мест: {Seats}");
            Console.WriteLine("Расписание:");

            foreach (var timeSlot in Schedule)
            {
                if (timeSlot.Value == null)
                {
                    Console.WriteLine($"{timeSlot.Key} ---------------");
                }
                else
                {
                    Console.WriteLine($"{timeSlot.Key} ID {timeSlot.Value.ClientId}, {timeSlot.Value.ClientName}, {timeSlot.Value.Phone}");
                }
            }
        }

        // Проверить, есть ли активные брони
        public bool HasActiveBookings()
        {
            foreach (var booking in Schedule.Values)
            {
                if (booking != null && booking.TimeEnd > DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        // Добавить бронь в расписание
        public void AddBooking(Booking booking)
        {
            DateTime start = booking.TimeStart;
            DateTime end = booking.TimeEnd;

            for (int hour = start.Hour; hour < end.Hour; hour++)
            {
                string timeSlot = $"{hour:00}:00-{hour + 1:00}:00";
                if (Schedule.ContainsKey(timeSlot))
                {
                    Schedule[timeSlot] = booking;
                }
            }
        }

        // Удалить бронь из расписания
        public void RemoveBooking(Booking booking)
        {
            DateTime start = booking.TimeStart;
            DateTime end = booking.TimeEnd;

            for (int hour = start.Hour; hour < end.Hour; hour++)
            {
                string timeSlot = $"{hour:00}:00-{hour + 1:00}:00";
                if (Schedule.ContainsKey(timeSlot) && Schedule[timeSlot] == booking)
                {
                    Schedule[timeSlot] = null;
                }
            }
        }
        // Проверить доступность стола в указанное время
        public bool IsAvailable(DateTime startTime, DateTime endTime)
        {
            for (int hour = startTime.Hour; hour < endTime.Hour; hour++)
            {
                string timeSlot = $"{hour:00}:00-{hour + 1:00}:00";
                if (Schedule.ContainsKey(timeSlot) && Schedule[timeSlot] != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}