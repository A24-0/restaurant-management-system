using RestaurantManagementSystem;
using System;
using System.Globalization;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

namespace RestaurantManagementSystem
{
    class Program
    {
        // Предопределённые столики, для проверки
        private static Table[] AvailableTables =
        {
            new Table(1, "Зал у окна", 4),
            new Table(2, "У барной стойки", 2),
            new Table(3, "VIP-зона", 6),
            new Table(4, "Терраса", 2)
        };

        private static List<Dish> _dishes = new List<Dish>();
        private static List<Order> _orders = new List<Order>();
        private static int _nextOrderId = 1;

        static void Main(string[] args)
        {
            Table.InitializeTables(AvailableTables);
            Table.InitializeTables(AvailableTables);

            ConsoleTheme.PrintBanner("Restaurant Management System", "Добро пожаловать в цифровой ресторан!");

            //Предустановленные блюда, для проверки
            _dishes = new List<Dish>
            {
                // Напитки
                Dish.CreateDish(1, "Морс клюквенный", "Клюква, сахар, вода", "200 мл", 180m, DishCategory.Напитки, 0, new[] { "освежающее", "вегетарианское" }),
                Dish.CreateDish(2, "Чай чёрный (листовой)", "Чай, вода", "400 мл", 150m, DishCategory.Напитки, 3, new[] { "вегетарианское" }),

                // Салаты
                Dish.CreateDish(3, "Оливье", "Картофель, морковь, яйцо, колбаса, горошек, майонез", "200 г", 350m, DishCategory.Салаты, 10, new[] { "классическое" }),
                Dish.CreateDish(4, "Цезарь с курицей", "Куриная грудка, салат айсберг, сухарики, пармезан, соус", "220 г", 450m, DishCategory.Салаты, 12, new[] { "популярное" }),

                // Холодные закуски
                Dish.CreateDish(5, "Селёдка под шубой", "Сельдь, свёкла, картофель, морковь, лук, майонез", "180 г", 320m, DishCategory.ХолодныеЗакуски, 15, new[] { "традиционное" }),
                Dish.CreateDish(6, "Ассорти из копчёностей", "Сырокопчёная колбаса, буженина, оливки, горчица", "200 г", 550m, DishCategory.ХолодныеЗакуски, 5, new[] { "пикантное" }),

                // Горячие закуски
                Dish.CreateDish(7, "Печёночные котлетки", "Свиная печень, лук, хлеб, яйцо", "180 г", 380m, DishCategory.ГорячиеЗакуски, 25, new[] { "домашнее" }),
                Dish.CreateDish(8, "Гренки с грибами и сыром", "Багет, шампиньоны, лук, сливки, сыр", "160 г", 340m, DishCategory.ГорячиеЗакуски, 20, new[] { "вегетарианское", "острое" }),

                // Супы
                Dish.CreateDish(9, "Борщ", "Свёкла, капуста, картофель, мясо, фасоль, сметана", "300 мл", 320m, DishCategory.Супы, 40, new[] { "традиционное", "наваристое" }),
                Dish.CreateDish(10, "Куриный суп с лапшой", "Куриный бульон, вермишель, морковь, лук, зелень", "300 мл", 280m, DishCategory.Супы, 25, new[] { "лёгкое", "вегетарианское" }),

                // Горячие блюда
                Dish.CreateDish(11, "Бефстроганов", "Говядина, сметана, лук, специи, картофельное пюре", "300 г", 650m, DishCategory.ГорячиеБлюда, 35, new[] { "популярное", "мясное" }),
                Dish.CreateDish(12, "Запечённая семга", "Сёмга, лимон, оливковое масло, овощи гриль", "250 г", 850m, DishCategory.ГорячиеБлюда, 30, new[] { "премиум", "диетическое" }),
                Dish.CreateDish(13, "Пельмени домашние", "Тесто, мясной фарш (свинина/говядина), сметана/масло", "200 г (30 шт)", 420m, DishCategory.ГорячиеБлюда, 15, new[] { "традиционное", "быстро" }),

                // Десерты
                Dish.CreateDish(14, "Медовик", "Мёд, сгущёнка, коржи, сметанный крем", "150 г", 320m, DishCategory.Десерт, 5, new[] { "сладкое", "домашнее" }),
                Dish.CreateDish(15, "Тирамису", "Маскарпоне, кофе, бисквит Савоярди, какао", "180 г", 390m, DishCategory.Десерт, 10, new[] { "итальянское", "популярное" })
            };
            while (true)
            {
                ConsoleTheme.PrintMenuHeader("Главное меню");
                ConsoleTheme.PrintMenuOption("1", "Система бронирования");
                ConsoleTheme.PrintMenuOption("2", "Система управления столиками");
                ConsoleTheme.PrintMenuOption("3", "Система управления заказами");
                ConsoleTheme.PrintMenuOption("4", "Система управления меню");
                ConsoleTheme.PrintMenuOption("0", "Выйти");
                ConsoleTheme.WritePrompt("Ваш выбор: ");

                string input = Console.ReadLine();
                ConsoleTheme.DrawSeparator();

                switch (input)
                {
                    case "1":
                        ConsoleTheme.ClearScreen();
                        BookingSystem();
                        break;

                    case "2":
                        ConsoleTheme.ClearScreen();
                        TablesSystem();
                        break;

                    case "3":
                        ConsoleTheme.ClearScreen();
                        OrdersSystem();
                        break;

                    case "4":
                        ConsoleTheme.ClearScreen();
                        DishSystem();
                        break;
                    case "0":
                        ConsoleTheme.ClearScreen();
                        ConsoleTheme.WriteSuccess("До свидания!");
                        return;
                    default:
                        ConsoleTheme.WriteWarning("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                        break;
                }
            }
        }

        static void BookingSystem()
        {
            ConsoleTheme.PrintMenuHeader("Система бронирования");
            ConsoleTheme.PrintMenuOption("1", "Показать все бронирования");
            ConsoleTheme.PrintMenuOption("2", "Создать новую бронь");
            ConsoleTheme.PrintMenuOption("3", "Изменить бронь");
            ConsoleTheme.PrintMenuOption("4", "Отменить бронь");
            ConsoleTheme.PrintMenuOption("0", "Назад");
            ConsoleTheme.WritePrompt("Ваш выбор: ");

            string input = Console.ReadLine();
            ConsoleTheme.DrawSeparator();
            ConsoleTheme.ClearScreen();
            ConsoleTheme.ClearScreen();

            switch (input)
            {
                case "1":
                    Booking.DisplayAllBookings();
                    break;

                case "2":
                    CreateNewBookingInteractive();
                    break;

                case "3":
                    EditBookingInteractive();
                    break;

                case "4":
                    CancelBookingInteractive();
                    break;
                case "0":
                    return;

                default:
                    ConsoleTheme.WriteWarning("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void TablesSystem()
        {
            ConsoleTheme.PrintMenuHeader("Система столиков");
            ConsoleTheme.PrintMenuOption("1", "Показать расписание столиков");
            ConsoleTheme.PrintMenuOption("2", "Создать столик");
            ConsoleTheme.PrintMenuOption("3", "Изменить столик");
            ConsoleTheme.PrintMenuOption("0", "Назад");
            ConsoleTheme.WritePrompt("Ваш выбор: ");

            string input = Console.ReadLine();
            ConsoleTheme.DrawSeparator();
            ConsoleTheme.ClearScreen();

            switch (input)
            {
                case "1":
                    ShowAllTablesSchedule();
                    break;

                case "2":
                    CreateNewTableInteractive();
                    break;

                case "3":
                    UpdateTableInteractive();
                    break;

                case "0":
                    return;

                default:
                    ConsoleTheme.WriteWarning("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void OrdersSystem()
        {
            ConsoleTheme.PrintMenuHeader("Система заказов");
            ConsoleTheme.PrintMenuOption("1", "Создать заказ");
            ConsoleTheme.PrintMenuOption("2", "Добавить блюдо в заказ");
            ConsoleTheme.PrintMenuOption("3", "Удалить блюдо из заказа");
            ConsoleTheme.PrintMenuOption("4", "Изменить заказ");
            ConsoleTheme.PrintMenuOption("5", "Показать информацию о заказе");
            ConsoleTheme.PrintMenuOption("6", "Удалить заказ");
            ConsoleTheme.PrintMenuOption("7", "Закрыть заказ");
            ConsoleTheme.PrintMenuOption("8", "Вывести чек (только закрытые)");
            ConsoleTheme.PrintMenuOption("0", "Назад");
            ConsoleTheme.WritePrompt("Ваш выбор: ");

            string input = Console.ReadLine();
            ConsoleTheme.DrawSeparator();
            ConsoleTheme.ClearScreen();

            switch (input)
            {
                case "1":
                    CreateOrderInteractive();
                    break;
                case "2":
                    AddDishToOrderInteractive();
                    break;

                case "3":
                    RemoveDishFromOrderInteractive();
                    break;

                case "4":
                    EditOrderInteractive();
                    break;

                case "5":
                    ShowOrderInfoInteractive();
                    break;

                case "6":
                    DeleteOrderInteractive();
                    break;

                case "7":
                    CloseOrderInteractive();
                    break;

                case "8":
                    PrintReceiptInteractive();
                    break;

                case "0":
                    return;

                default:
                    ConsoleTheme.WriteWarning("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void DishSystem()
        {
            ConsoleTheme.PrintMenuHeader("Система меню");
            ConsoleTheme.PrintMenuOption("1", "Создать блюдо");
            ConsoleTheme.PrintMenuOption("2", "Редактировать блюдо");
            ConsoleTheme.PrintMenuOption("3", "Показать меню");
            ConsoleTheme.PrintMenuOption("0", "Назад");
            ConsoleTheme.WritePrompt("Ваш выбор: ");

            string input = Console.ReadLine();
            ConsoleTheme.DrawSeparator();

            switch (input)
            {
                case "1":
                    CreateDishInteractive();
                    break;

                case "2":
                    EditDishInteractive();
                    break;

                case "3":
                    ShowAllDishes();
                    break;

                case "0":
                    return;

                default:
                    ConsoleTheme.WriteWarning("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }
        static void ShowAllTablesSchedule()
        {
            ConsoleTheme.PrintMenuHeader("Расписание столиков");
            foreach (var table in AvailableTables)
            {
                table.ShowTableInfo();
                ConsoleTheme.DrawSeparator();
            }
        }

        static void CreateNewTableInteractive()
        {
            try
            {
                ConsoleTheme.WritePrompt("Введите id столика: ");
                string id = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(id))
                {
                    ConsoleTheme.WriteWarning("Id не может быть пустым.");
                    return;
                }
                if (TryParseInt(id) == false)
                {
                    ConsoleTheme.WriteWarning("Введите числовое значение id.");
                    return;
                }
                int idInt = int.Parse(id);
                ConsoleTheme.WritePrompt("Введите локацию столика: ");
                string location = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(location))
                {
                    ConsoleTheme.WriteWarning("Поле локации не может быть пустым.");
                    return;
                }
                ConsoleTheme.WritePrompt("Введите количество мест (от одного до шести): ");
                string seats = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(seats))
                {
                    ConsoleTheme.WriteWarning("Поле мест не может быть пустым.");
                    return;
                }
                if (TryParseInt(seats) == false)
                {
                    ConsoleTheme.WriteWarning("Введите числовое значение мест.");
                    return;
                }
                int seatsInt = int.Parse(seats);

                Table[] newArray = new Table[AvailableTables.Length + 1];

                Array.Copy(AvailableTables, newArray, AvailableTables.Length);

                newArray[newArray.Length - 1] = new Table(idInt, location, seatsInt);

                AvailableTables = newArray;

                ConsoleTheme.WriteSuccess($"Столик успешно создан! ID столика: {idInt:00}");
            }
            catch (InvalidOperationException ex)
            {
                ConsoleTheme.WriteError($"Ошибка при создании столика: {ex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleTheme.WriteError($"Неожиданная ошибка: {ex.Message}");
            }
        }

        static void UpdateTableInteractive()
        {
            ConsoleTheme.WritePrompt("Введите ID стола, который хотите обновить: ");
            int tableId = SafeParse(Console.ReadLine());
            ConsoleTheme.WritePrompt("Введите новую локацию: ");
            string newLocation = Console.ReadLine();
            ConsoleTheme.WritePrompt("Введите новое количество мест (от 1 до 6): ");
            int newSeats = SafeParse(Console.ReadLine());

            Table table = Array.Find(AvailableTables, t => t.ID == tableId);

            if (table != null)
            {
                bool success = table.UpdateTable(newLocation, newSeats);
                if (success)
                {
                    ConsoleTheme.WriteSuccess($"Стол {tableId} обновлён.");
                }
            }
            else
            {
                ConsoleTheme.WriteWarning($"Стол с ID {tableId} не найден.");
            }
        }

        static void EditBookingInteractive()
        {
            var bookings = Booking.GetAllBookings();

            Booking.DisplayAllBookings();
            ConsoleTheme.WritePrompt("Введите имя, указанное в брони, которую хотите изменить: ");
            string bookingName = Console.ReadLine();

            var bookingToEdit = Booking.GetAllBookings()
                .Where(b => b.ClientName.Equals(bookingName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!bookingToEdit.Any())
            {
                ConsoleTheme.WriteWarning($"Бронирования для клиента '{bookingName}' не найдены.");
            }
            else
            {
                foreach (var booking in bookingToEdit)
                {
                    booking.EditBooking();
                    CreateNewBookingInteractive();
                    ConsoleTheme.WriteSuccess("Бронь обновлена.");
                }
            }
        }

        static void CancelBookingInteractive()
        {
            var bookings = Booking.GetAllBookings();

            Booking.DisplayAllBookings();
            ConsoleTheme.WritePrompt("Введите имя, указанное в брони, которую хотите отменить: ");
            string bookingName = Console.ReadLine();

            var bookingsToRemove = Booking.GetAllBookings()
                .Where(b => b.ClientName.Equals(bookingName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!bookingsToRemove.Any())
            {
                ConsoleTheme.WriteWarning($"Бронирования для клиента '{bookingName}' не найдены.");
            }
            else
            {
                foreach (var booking in bookingsToRemove)
                {
                    booking.CancelBooking();
                }
            }
        }

        static void CreateNewBookingInteractive()
        {
            try
            {
                ConsoleTheme.WritePrompt("Введите имя клиента: ");
                string name = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ConsoleTheme.WriteWarning("Имя не может быть пустым.");
                    return;
                }

                ConsoleTheme.WritePrompt("Введите телефон (например, +79001234567): ");
                string phone = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(phone))
                {
                    ConsoleTheme.WriteWarning("Телефон не может быть пустым.");
                    return;
                }

                ConsoleTheme.WritePrompt("Введите дату и время начала (например, 05.11.2025 19:00): ");
                if (!TryParseDateTime(Console.ReadLine(), out DateTime start))
                {
                    return;
                }

                ConsoleTheme.WritePrompt("Введите длительность бронирования в часах (целое число, например 2): ");
                if (!int.TryParse(Console.ReadLine(), out int hours) || hours <= 0)
                {
                    ConsoleTheme.WriteWarning("Длительность должна быть положительным целым числом.");
                    return;
                }
                // Округляем до ближайшего часа вниз для совместимости с расписанием (9:00-10:00 и т.д.)
                start = start.Date.AddHours(start.Hour); // обрезаем минуты
                DateTime end = start.AddHours(hours);

                // Проверка: столы работают с 9:00 до 19:00 (18:00-19:00 последний слот)
                if (start.Hour < 9 || end.Hour > 19 || start.Hour >= end.Hour)
                {
                    ConsoleTheme.WriteWarning("Ресторан работает с 9:00 до 19:00. Бронирование должно укладываться в это время.");
                    return;
                }

                ConsoleTheme.PrintMenuHeader("Доступные столики");
                foreach (var table in AvailableTables)
                {
                    ConsoleTheme.WriteList(new[] { $"ID {table.ID:00} — {table.Seats} мест — {table.Location}" });
                }

                ConsoleTheme.WritePrompt("Выберите ID столика: ");
                if (!int.TryParse(Console.ReadLine(), out int tableId))
                {
                    ConsoleTheme.WriteWarning("Неверный ID столика.");
                    return;
                }

                var selectedTable = Array.Find(AvailableTables, t => t.ID == tableId);
                if (selectedTable == null)
                {
                    ConsoleTheme.WriteWarning("Столик с таким ID не найден.");
                    return;
                }

                if (!selectedTable.IsAvailable(start, end))
                {
                    ConsoleTheme.WriteWarning($"Столик ID {tableId} уже забронирован на выбранное время.");
                    ConsoleTheme.WriteInfo("Расписание выбранного столика:");
                    selectedTable.ShowTableInfo();
                    return;
                }

                ConsoleTheme.WritePrompt("Введите комментарий (можно оставить пустым): ");
                string comment = Console.ReadLine()?.Trim() ?? "";

                int clientId = (int)(DateTime.Now.Ticks % 100000);

                var booking = Booking.CreateBooking(
                    clientId: clientId,
                    clientName: name,
                    phone: phone,
                    timeStart: start,
                    timeEnd: end,
                    comment: comment,
                    table: selectedTable
                );

                selectedTable.AddBooking(booking);

                ConsoleTheme.WriteSuccess($"Бронирование успешно создано! ID брони: {booking.ClientId}");
                ConsoleTheme.WriteInfo($"{selectedTable.Location}, {start:dd.MM.yyyy HH:mm} – {end:HH:mm}");
            }
            catch (InvalidOperationException ex)
            {
                ConsoleTheme.WriteError($"Ошибка при создании брони: {ex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleTheme.WriteError($"Неожиданная ошибка: {ex.Message}");
            }
        }


        static void CreateDishInteractive()
        {
            try
            {
                ConsoleTheme.WritePrompt("ID блюда: ");
                int id = int.Parse(Console.ReadLine());

                // Проверка на дубликат ID
                if (_dishes.Any(d => d.ID == id))
                {
                    ConsoleTheme.WriteWarning("Блюдо с таким ID уже существует.");
                    return;
                }

                ConsoleTheme.WritePrompt("Название: ");
                string name = Console.ReadLine();

                ConsoleTheme.WritePrompt("Состав: ");
                string composition = Console.ReadLine();

                ConsoleTheme.WritePrompt("Вес: ");
                string weight = Console.ReadLine();

                ConsoleTheme.WritePrompt("Цена: ");
                decimal price = decimal.Parse(Console.ReadLine());

                ConsoleTheme.PrintMenuHeader("Категория блюда");
                var categories = Enum.GetValues<DishCategory>();
                for (int i = 0; i < categories.Length; i++)
                {
                    ConsoleTheme.PrintMenuOption((i + 1).ToString(), categories[i].ToString(), highlight: false);
                }

                ConsoleTheme.WritePrompt("Номер категории: ");
                int catIndex = int.Parse(Console.ReadLine()) - 1;
                DishCategory category = categories[catIndex];

                ConsoleTheme.WritePrompt("Время приготовления (в минутах): ");
                int cookingTime = int.Parse(Console.ReadLine());

                ConsoleTheme.WritePrompt("Типы (через запятую, напр. 'острое,вегетарианское'): ");
                string typesInput = Console.ReadLine();
                string[] types = string.IsNullOrWhiteSpace(typesInput)
                    ? new string[0]
                    : typesInput.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                var dish = Dish.CreateDish(id, name, composition, weight, price, category, cookingTime, types);

                //Dish.AddDish(dish);
                _dishes.Add(dish);
                ConsoleTheme.WriteSuccess($"Блюдо '{name}' добавлено.");
            }
            catch (Exception ex)
            {
                ConsoleTheme.WriteError($"Ошибка ввода: {ex.Message}");
            }
        }

        static void EditDishInteractive()
        {
            try
            {
                ConsoleTheme.WritePrompt("Введите ID блюда для редактирования: ");
                int id = int.Parse(Console.ReadLine());

                var dish = _dishes.FirstOrDefault(d => d.ID == id);
                if (dish == null)
                {
                    ConsoleTheme.WriteWarning($"Блюдо с ID {id} не найдено.");
                    return;
                }

                ConsoleTheme.PrintMenuHeader($"Редактирование — {dish.Name}");
                ConsoleTheme.WriteInfo("Оставьте поле пустым, чтобы не менять значение.");

                ConsoleTheme.WritePrompt($"Название [{dish.Name}]: ");
                string name = Console.ReadLine() ?? dish.Name;
                name = string.IsNullOrWhiteSpace(name) ? dish.Name : name;

                ConsoleTheme.WritePrompt($"Состав [{dish.Compostion}]: ");
                string composition = Console.ReadLine() ?? dish.Compostion;
                composition = string.IsNullOrWhiteSpace(composition) ? dish.Compostion : composition;

                ConsoleTheme.WritePrompt($"Вес [{dish.Weight}]: ");
                string weight = Console.ReadLine() ?? dish.Weight;
                weight = string.IsNullOrWhiteSpace(weight) ? dish.Weight : weight;

                ConsoleTheme.WritePrompt($"Цена [{dish.Price}]: ");
                string priceStr = Console.ReadLine();
                decimal price = string.IsNullOrWhiteSpace(priceStr)
                    ? dish.Price
                    : decimal.Parse(priceStr);

                ConsoleTheme.WriteInfo("Категория (оставьте пустым для сохранения текущей):");
                var categories = Enum.GetValues<DishCategory>();
                for (int i = 0; i < categories.Length; i++)
                {
                    ConsoleTheme.PrintMenuOption((i + 1).ToString(), categories[i].ToString());
                }
                ConsoleTheme.WritePrompt($"Текущая: {dish.Category}. Новый номер: ");
                string catInput = Console.ReadLine();
                DishCategory category = dish.Category;
                if (!string.IsNullOrWhiteSpace(catInput) && int.TryParse(catInput, out int catIndex) && catIndex > 0 && catIndex <= categories.Length)
                {
                    category = categories[catIndex - 1];
                }

                ConsoleTheme.WritePrompt($"Время приготовления [{dish.CookingTime}]: ");
                string timeStr = Console.ReadLine();
                int cookingTime = string.IsNullOrWhiteSpace(timeStr)
                    ? dish.CookingTime
                    : int.Parse(timeStr);

                ConsoleTheme.WritePrompt($"Типы (через запятую) [{string.Join(", ", dish.Types)}]: ");
                string typesInput = Console.ReadLine();
                string[] types = dish.Types;
                if (!string.IsNullOrWhiteSpace(typesInput))
                {
                    types = typesInput.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                }

                dish.EditDish(dish.ID, name, composition, weight, price, category, cookingTime, types);

                ConsoleTheme.WriteSuccess($"Блюдо '{dish.Name}' обновлено.");
            }
            catch (Exception ex)
            {
                ConsoleTheme.WriteError($"Ошибка: {ex.Message}");
            }
        }

        static void ShowAllDishes()
        {
            if (_dishes.Count == 0)
            {
                ConsoleTheme.WriteWarning("Нет добавленных блюд.");
                return;
            }

            ConsoleTheme.PrintMenuHeader("Меню ресторана");
            foreach (var dish in _dishes)
            {
                dish.ShowInfo();
                ConsoleTheme.DrawSeparator();
            }
        }

        static void CreateOrderInteractive()
        {
            ConsoleTheme.PrintMenuHeader("Создание нового заказа");

            ConsoleTheme.WritePrompt("ID столика: ");
            if (!int.TryParse(Console.ReadLine(), out int tableId))
            {
                ConsoleTheme.WriteWarning("Неверный ID столика.");
                return;
            }

            var table = Array.Find(AvailableTables, t => t.ID == tableId);
            if (table == null)
            {
                ConsoleTheme.WriteWarning($"Столик с ID {tableId} не найден.");
                return;
            }

            // Проверка наличия активной брони на столик
            if (!Booking.HasActiveBookingForTable(tableId))
            {
                ConsoleTheme.WriteError("❌ Нельзя создать заказ: на столик нет активной брони!");
                ConsoleTheme.WriteInfo("Сначала создайте бронирование на этот столик в Системе бронирования.");

                var activeBooking = Booking.GetActiveBookingForTable(tableId);
                if (activeBooking == null)
                {
                    ConsoleTheme.WriteInfo($"На столик {tableId} сейчас нет активных бронирований.");
                }
                return;
            }

            // Показываем информацию об активной брони
            var booking = Booking.GetActiveBookingForTable(tableId);
            ConsoleTheme.WriteSuccess($"✓ Найдена активная бронь: {booking.ClientName} ({booking.Phone})");
            ConsoleTheme.WriteInfo($"  Время брони: {booking.TimeStart:HH:mm} - {booking.TimeEnd:HH:mm}");

            ConsoleTheme.WritePrompt("ID официанта: ");
            if (!int.TryParse(Console.ReadLine(), out int waiterId) || waiterId <= 0)
            {
                ConsoleTheme.WriteWarning("Неверный ID официанта.");
                return;
            }

            ConsoleTheme.WritePrompt("Комментарий (можно оставить пустым): ");
            string comment = Console.ReadLine()?.Trim() ?? "";

            var order = Order.CreateOrder(_nextOrderId++, tableId, waiterId, comment);
            _orders.Add(order);

            ConsoleTheme.WriteSuccess($"Заказ #{order.OrderId} успешно создан для столика {tableId}.");
        }

        static void AddDishToOrderInteractive()
        {
            if (_orders.Count == 0)
            {
                ConsoleTheme.WriteWarning("Нет активных заказов.");
                return;
            }

            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                ConsoleTheme.WriteWarning("Нельзя добавлять блюда в закрытый заказ.");
                return;
            }

            ConsoleTheme.WritePrompt("ID блюда: ");
            if (!int.TryParse(Console.ReadLine(), out int dishId))
            {
                ConsoleTheme.WriteWarning("Неверный ID блюда.");
                return;
            }

            var dish = _dishes.FirstOrDefault(d => d.ID == dishId);
            if (dish == null)
            {
                ConsoleTheme.WriteWarning($"Блюдо с ID {dishId} не найдено.");
                return;
            }

            ConsoleTheme.WritePrompt("Количество (по умолчанию 1): ");
            int quantity = 1;
            string qtyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(qtyInput) && !int.TryParse(qtyInput, out quantity))
            {
                ConsoleTheme.WriteWarning("Неверное количество. Используется 1.");
                quantity = 1;
            }
            if (quantity <= 0) quantity = 1;

            order.AddDish(dish, quantity);
            ConsoleTheme.WriteSuccess($"Блюдо '{dish.Name}' x{quantity} добавлено в заказ #{orderId}.");
        }

        static void RemoveDishFromOrderInteractive()
        {
            if (_orders.Count == 0)
            {
                ConsoleTheme.WriteWarning("Нет активных заказов.");
                return;
            }

            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                ConsoleTheme.WriteWarning("Нельзя удалять блюда из закрытого заказа.");
                return;
            }

            ConsoleTheme.WritePrompt("ID блюда: ");
            if (!int.TryParse(Console.ReadLine(), out int dishId))
            {
                ConsoleTheme.WriteWarning("Неверный ID блюда.");
                return;
            }

            int currentQty = order.GetDishQuantity(dishId);
            if (currentQty == 0)
            {
                ConsoleTheme.WriteWarning("Такого блюда нет в заказе.");
                return;
            }

            ConsoleTheme.WritePrompt($"Сколько порций удалить (всего {currentQty}, по умолчанию 1): ");
            int quantity = 1;
            string qtyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(qtyInput) && !int.TryParse(qtyInput, out quantity))
            {
                ConsoleTheme.WriteWarning("Неверное количество. Используется 1.");
                quantity = 1;
            }
            if (quantity <= 0) quantity = 1;

            order.RemoveDish(dishId, quantity);
            ConsoleTheme.WriteSuccess($"Удалено {quantity} порций из заказа #{orderId}.");
        }

        static void EditOrderInteractive()
        {
            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                ConsoleTheme.WriteWarning("Нельзя изменять закрытый заказ.");
                return;
            }

            ConsoleTheme.WriteInfo($"Текущий комментарий: '{order.Comment}'");
            ConsoleTheme.WritePrompt("Новый комментарий (оставьте пустым, чтобы не менять): ");
            string newComment = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newComment))
            {
                order.EditOrder(order.OrderId, order.TableId, order.WaiterId, newComment);
                ConsoleTheme.WriteSuccess("Комментарий обновлён.");
            }
            else
            {
                ConsoleTheme.WriteInfo("Комментарий не изменён.");
            }
        }

        static void ShowOrderInfoInteractive()
        {
            ConsoleTheme.WritePrompt("ID заказа (оставьте пустым для всех): ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                if (_orders.Count == 0)
                {
                    ConsoleTheme.WriteWarning("Нет заказов.");
                    return;
                }
                ConsoleTheme.PrintMenuHeader("Все заказы");
                foreach (var order in _orders)
                {
                    order.ShowInfo();
                    ConsoleTheme.DrawSeparator();
                }
            }
            else if (int.TryParse(input, out int orderId))
            {
                var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                    return;
                }
                ConsoleTheme.PrintMenuHeader("Информация о заказе");
                order.ShowInfo();
            }
            else
            {
                ConsoleTheme.WriteWarning("Неверный ввод.");
            }
        }

        static void DeleteOrderInteractive()
        {
            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                ConsoleTheme.WriteWarning("Закрытые заказы нельзя удалять.");
                return;
            }

            ConsoleTheme.WritePrompt($"Удалить заказ #{orderId}? (y/n): ");
            string confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y" || confirm == "yes")
            {
                _orders.Remove(order);
                ConsoleTheme.WriteSuccess($"Заказ #{orderId} удалён.");
            }
            else
            {
                ConsoleTheme.WriteInfo("Удаление отменено.");
            }
        }

        static void CloseOrderInteractive()
        {
            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                ConsoleTheme.WriteWarning("Заказ уже закрыт.");
                return;
            }

            if (order.OrderItems.Count == 0)
            {
                ConsoleTheme.WriteWarning("Нельзя закрыть пустой заказ.");
                return;
            }

            order.CloseOrder();
            ConsoleTheme.WriteSuccess($"Заказ #{orderId} закрыт в {order.ClosedAt:HH:mm dd.MM.yyyy}. Итог: {order.TotalCost} руб.");
        }

        static void PrintReceiptInteractive()
        {
            ConsoleTheme.WritePrompt("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                ConsoleTheme.WriteWarning("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                ConsoleTheme.WriteWarning($"Заказ с ID {orderId} не найден.");
                return;
            }

            order.PrintReceipt();
        }

        //Проверка на возможность парсинга чисел
        static bool TryParseInt(string input)
        {
            bool output = int.TryParse(input, out int number);
            return output;
        }


        //Метод для безопасного парсинга чисел
        static int SafeParse(string input)
        {
            int output;
            while (true)
            {
                try
                {
                    output = int.Parse(input);
                    break;
                }
                catch (Exception)
                {
                    ConsoleTheme.WriteWarning("Ошибка ввода, пожалуйста, повторите попытку.");
                }
            }
            return output;
        }


        //Метод для безопасного парсинга даты и времени
        static bool TryParseDateTime(string input, out DateTime result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleTheme.WriteWarning("Дата не может быть пустой.");
                return false;
            }

            // Поддерживаем форматы: "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm"
            string[] formats = { "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm", "dd.MM.yyyy H:mm" };

            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                if (result < DateTime.Now)
                {
                    ConsoleTheme.WriteWarning("Нельзя забронировать столик в прошлом.");
                    return false;
                }
                return true;
            }

            ConsoleTheme.WriteWarning("Неверный формат даты. Используйте, например: 05.11.2025 19:00");
            return false;
        }
        static void Debugging()
        {
            ConsoleTheme.WriteInfo("Режим отладки запущен!");
        }
    }
}