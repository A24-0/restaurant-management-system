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

        static bool ifAdminLogged = false;

        static void Main(string[] args)
        {
            Table.InitializeTables(AvailableTables);
            Table.InitializeTables(AvailableTables);

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
            Console.WriteLine("Добро пожаловать в систему управления рестораном!");

            while (true)
            {

                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Система бронирования");
                Console.WriteLine("2. Система управления столиками");
                Console.WriteLine("3. Система управления заказами");
                Console.WriteLine("4. Система управления меню");
                Console.WriteLine("0. Выйти");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BookingSystem();
                        break;

                    case "2":
                        TablesSystem();
                        break;

                    case "3":
                        OrdersSystem();
                        break;

                    case "4":
                        DishSystem();
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    case "A":
                        AdminLogin();
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                        break;
                }
            }
        }

        static void BookingSystem()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Показать все бронирования");
            Console.WriteLine("2. Создать новую бронь");
            Console.WriteLine("3. Изменить бронь");
            Console.WriteLine("4. Отменить бронь");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

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
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void TablesSystem()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Показать расписание столиков");
            Console.WriteLine("2. Создать столик");
            Console.WriteLine("3. Изменить столик");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

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
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void OrdersSystem()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Создать заказ");
            Console.WriteLine("2. Добавить блюдо в заказ");
            Console.WriteLine("3. Удалить блюдо из заказа");
            Console.WriteLine("4. Изменить заказ");
            Console.WriteLine("5. Показать информацию о заказе");
            Console.WriteLine("6. Удалить заказ");
            Console.WriteLine("7. Закрыть заказ");
            Console.WriteLine("8. Вывести чек (только для закрытых заказов)");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

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
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }

        static void DishSystem()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Создать блюдо");
            Console.WriteLine("2. Редактировать блюдо");
            Console.WriteLine("3. Показать меню");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

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
                    Console.WriteLine("Неверный ввод. Пожалуйста, выберите корректный вариант.");
                    break;
            }
        }
        static void ShowAllTablesSchedule()
        {
            Console.WriteLine("\n=== Расписание всех столиков ===");
            foreach (var table in AvailableTables)
            {
                Console.WriteLine(new string('=', 32));
                table.ShowTableInfo();
                Console.WriteLine();
            }
        }

        static void CreateNewTableInteractive()
        {
            try
            {
                Console.WriteLine("Введите id столика: ");
                string id = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine("Id не может быть пустым.");
                    return;
                }
                if (TryParseInt(id) == false)
                {
                    Console.WriteLine("Введите числовое значение id");
                    return;
                }
                int idInt = int.Parse(id);
                Console.WriteLine("Введите локацию столика: ");
                string location = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(location))
                {
                    Console.WriteLine("Поле локации не может быть пустым.");
                    return;
                }
                Console.WriteLine("Введите количество мест(от одного до шести): ");
                string seats = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(seats))
                {
                    Console.WriteLine("Поле локации не может быть пустым.");
                    return;
                }
                if (TryParseInt(seats) == false)
                {
                    Console.WriteLine("Введите числовое значение мест");
                    return;
                }
                int seatsInt = int.Parse(seats);

                Table[] newArray = new Table[AvailableTables.Length + 1];

                Array.Copy(AvailableTables, newArray, AvailableTables.Length);

                newArray[newArray.Length - 1] = new Table(idInt, location, seatsInt);

                AvailableTables = newArray;

                Console.WriteLine($"Столик успешно создан! ID столика: 0{idInt}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка при создании столика: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }

        static void UpdateTableInteractive()
        {
            Console.WriteLine("Введите ID стола, который хотите обновить");
            int tableId = SafeParse(Console.ReadLine());
            Console.WriteLine("Введите новую локацию");
            string newLocation = Console.ReadLine();
            Console.WriteLine("Введите новое количество мест (от 1 до 6)");
            int newSeats = SafeParse(Console.ReadLine());

            Table table = Array.Find(AvailableTables, t => t.ID == tableId);
            
            if (table != null)
            {
                bool success = table.UpdateTable(newLocation, newSeats);
                if (success)
                {
                    Console.WriteLine($"Стол {tableId} обновлён.");
                }
            }
            else
            {
                Console.WriteLine($"Стол с ID {tableId} не найден.");
            }
        }

        static void EditBookingInteractive()
        {
            var bookings = Booking.GetAllBookings();

            Booking.DisplayAllBookings();
            Console.WriteLine("Введите имя, указанное в брони, которую хотите изменить");
            Console.WriteLine("Ваш выбор: ");
            string bookingName = Console.ReadLine();

            var bookingToEdit = Booking.GetAllBookings()
                .Where(b => b.ClientName.Equals(bookingName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!bookingToEdit.Any())
            {
                Console.WriteLine($"Бронирования для клиента '{bookingName}' не найдены.");
            }
            else
            {
                foreach (var booking in bookingToEdit)
                {
                    booking.EditBooking();
                    CreateNewBookingInteractive();
                }
            }
        }

        static void CancelBookingInteractive()
        {
            var bookings = Booking.GetAllBookings();

            Booking.DisplayAllBookings();
            Console.WriteLine("Введите имя, указанное в брони, которую хотите отменить");
            Console.WriteLine("Ваш выбор: ");
            string bookingName = Console.ReadLine();

            var bookingsToRemove = Booking.GetAllBookings()
                .Where(b => b.ClientName.Equals(bookingName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!bookingsToRemove.Any())
            {
                Console.WriteLine($"Бронирования для клиента '{bookingName}' не найдены.");
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
                Console.Write("Введите имя клиента: ");
                string name = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Имя не может быть пустым.");
                    return;
                }

                Console.Write("Введите телефон (например, +79001234567): ");
                string phone = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(phone))
                {
                    Console.WriteLine("Телефон не может быть пустым.");
                    return;
                }

                Console.Write("Введите дату и время начала (например, 05.11.2025 19:00): ");
                if (!TryParseDateTime(Console.ReadLine(), out DateTime start))
                {
                    return;
                }

                Console.Write("Введите длительность бронирования в часах (целое число, например 2): ");
                if (!int.TryParse(Console.ReadLine(), out int hours) || hours <= 0)
                {
                    Console.WriteLine("Длительность должна быть положительным целым числом.");
                    return;
                }
                // Округляем до ближайшего часа вниз для совместимости с расписанием (9:00-10:00 и т.д.)
                start = start.Date.AddHours(start.Hour); // обрезаем минуты
                DateTime end = start.AddHours(hours);

                // Проверка: столы работают с 9:00 до 19:00 (18:00-19:00 последний слот)
                if (start.Hour < 9 || end.Hour > 19 || start.Hour >= end.Hour)
                {
                    Console.WriteLine("Ресторан работает с 9:00 до 19:00. Бронирование должно укладываться в это время.");
                    return;
                }

                Console.WriteLine("\nДоступные столики:");
                foreach (var table in AvailableTables)
                {
                    Console.WriteLine($"  ID {table.ID:00} — {table.Seats} мест — {table.Location}");
                }

                Console.Write("Выберите ID столика: ");
                if (!int.TryParse(Console.ReadLine(), out int tableId))
                {
                    Console.WriteLine("Неверный ID столика.");
                    return;
                }

                var selectedTable = Array.Find(AvailableTables, t => t.ID == tableId);
                if (selectedTable == null)
                {
                    Console.WriteLine("Столик с таким ID не найден.");
                    return;
                }

                if (!selectedTable.IsAvailable(start, end))
                {
                    Console.WriteLine($"Столик ID {tableId} уже забронирован на выбранное время.");
                    Console.WriteLine("Показываю расписание этого столика:");
                    selectedTable.ShowTableInfo();
                    return;
                }

                Console.Write("Введите комментарий (можно оставить пустым): ");
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

                Console.WriteLine($"Бронирование успешно создано! ID брони: {booking.ClientId}");
                Console.WriteLine($"{selectedTable.Location}, {start:dd.MM.yyyy HH:mm} – {end:HH:mm}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка при создании брони: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }


        static void CreateDishInteractive()
        {
            try
            {
                Console.Write("ID блюда: ");
                int id = int.Parse(Console.ReadLine());

                // Проверка на дубликат ID
                if (_dishes.Any(d => d.ID == id))
                {
                    Console.WriteLine("Блюдо с таким ID уже существует.");
                    return;
                }

                Console.Write("Название: ");
                string name = Console.ReadLine();

                Console.Write("Состав: ");
                string composition = Console.ReadLine();

                Console.Write("Вес: ");
                string weight = Console.ReadLine();

                Console.Write("Цена: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Выберите категорию:");
                var categories = Enum.GetValues<DishCategory>();
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                Console.Write("Номер категории: ");
                int catIndex = int.Parse(Console.ReadLine()) - 1;
                DishCategory category = categories[catIndex];

                Console.Write("Время приготовления (в минутах): ");
                int cookingTime = int.Parse(Console.ReadLine());

                Console.Write("Типы (через запятую, напр. 'острое,вегетарианское'): ");
                string typesInput = Console.ReadLine();
                string[] types = string.IsNullOrWhiteSpace(typesInput)
                    ? new string[0]
                    : typesInput.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                var dish = Dish.CreateDish(id, name, composition, weight, price, category, cookingTime, types);

                //Dish.AddDish(dish);
                _dishes.Add(dish);
                Console.WriteLine($"Блюдо '{name}' добавлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }

        static void EditDishInteractive()
        {
            try
            {
                Console.Write("Введите ID блюда для редактирования: ");
                int id = int.Parse(Console.ReadLine());

                var dish = _dishes.FirstOrDefault(d => d.ID == id);
                if (dish == null)
                {
                    Console.WriteLine($"Блюдо с ID {id} не найдено.");
                    return;
                }

                Console.WriteLine($"Редактирование: {dish.Name}");
                Console.WriteLine("Оставьте поле пустым, чтобы не менять.");

                Console.Write($"Название [{dish.Name}]: ");
                string name = Console.ReadLine() ?? dish.Name;
                name = string.IsNullOrWhiteSpace(name) ? dish.Name : name;

                Console.Write($"Состав [{dish.Compostion}]: ");
                string composition = Console.ReadLine() ?? dish.Compostion;
                composition = string.IsNullOrWhiteSpace(composition) ? dish.Compostion : composition;

                Console.Write($"Вес [{dish.Weight}]: ");
                string weight = Console.ReadLine() ?? dish.Weight;
                weight = string.IsNullOrWhiteSpace(weight) ? dish.Weight : weight;

                Console.Write($"Цена [{dish.Price}]: ");
                string priceStr = Console.ReadLine();
                decimal price = string.IsNullOrWhiteSpace(priceStr)
                    ? dish.Price
                    : decimal.Parse(priceStr);

                Console.WriteLine("Категория (оставьте пустым для сохранения текущей):");
                var categories = Enum.GetValues<DishCategory>();
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }
                Console.Write($"Текущая: {dish.Category}. Новый номер: ");
                string catInput = Console.ReadLine();
                DishCategory category = dish.Category;
                if (!string.IsNullOrWhiteSpace(catInput) && int.TryParse(catInput, out int catIndex) && catIndex > 0 && catIndex <= categories.Length)
                {
                    category = categories[catIndex - 1];
                }

                Console.Write($"Время приготовления [{dish.CookingTime}]: ");
                string timeStr = Console.ReadLine();
                int cookingTime = string.IsNullOrWhiteSpace(timeStr)
                    ? dish.CookingTime
                    : int.Parse(timeStr);

                Console.Write($"Типы (через запятую) [{string.Join(", ", dish.Types)}]: ");
                string typesInput = Console.ReadLine();
                string[] types = dish.Types;
                if (!string.IsNullOrWhiteSpace(typesInput))
                {
                    types = typesInput.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                }

                dish.EditDish(dish.ID, name, composition, weight, price, category, cookingTime, types);

                Console.WriteLine($"Блюдо '{dish.Name}' обновлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowAllDishes()
        {
            if (_dishes.Count == 0)
            {
                Console.WriteLine("Нет добавленных блюд.");
                return;
            }

            Console.WriteLine("\nСписок блюд:");
            foreach (var dish in _dishes)
            {
                dish.ShowInfo();
            }
        }

        static void CreateOrderInteractive()
        {
            Console.WriteLine("\n=== Создание нового заказа ===");

            Console.Write("ID столика: ");
            if (!int.TryParse(Console.ReadLine(), out int tableId))
            {
                Console.WriteLine("Неверный ID столика.");
                return;
            }

            var table = Array.Find(AvailableTables, t => t.ID == tableId);
            if (table == null)
            {
                Console.WriteLine($"Столик с ID {tableId} не найден.");
                return;
            }

            Console.Write("ID официанта: ");
            if (!int.TryParse(Console.ReadLine(), out int waiterId) || waiterId <= 0)
            {
                Console.WriteLine("Неверный ID официанта.");
                return;
            }

            Console.Write("Комментарий (можно оставить пустым): ");
            string comment = Console.ReadLine()?.Trim() ?? "";

            var order = Order.CreateOrder(_nextOrderId++, tableId, waiterId, comment);
            _orders.Add(order);

            Console.WriteLine($"Заказ #{order.OrderId} успешно создан для столика {tableId}.");
        }

        static void AddDishToOrderInteractive()
        {
            if (_orders.Count == 0)
            {
                Console.WriteLine("Нет активных заказов.");
                return;
            }

            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                Console.WriteLine("Нельзя добавлять блюда в закрытый заказ.");
                return;
            }

            Console.Write("ID блюда: ");
            if (!int.TryParse(Console.ReadLine(), out int dishId))
            {
                Console.WriteLine("Неверный ID блюда.");
                return;
            }

            var dish = _dishes.FirstOrDefault(d => d.ID == dishId);
            if (dish == null)
            {
                Console.WriteLine($"Блюдо с ID {dishId} не найдено.");
                return;
            }

            Console.Write("Количество (по умолчанию 1): ");
            int quantity = 1;
            string qtyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(qtyInput) && !int.TryParse(qtyInput, out quantity))
            {
                Console.WriteLine("Неверное количество. Используется 1.");
                quantity = 1;
            }
            if (quantity <= 0) quantity = 1;

            order.AddDish(dish, quantity);
            Console.WriteLine($"Блюдо '{dish.Name}' x{quantity} добавлено в заказ #{orderId}.");
        }

        static void RemoveDishFromOrderInteractive()
        {
            if (_orders.Count == 0)
            {
                Console.WriteLine("Нет активных заказов.");
                return;
            }

            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                Console.WriteLine("Нельзя удалять блюда из закрытого заказа.");
                return;
            }

            Console.Write("ID блюда: ");
            if (!int.TryParse(Console.ReadLine(), out int dishId))
            {
                Console.WriteLine("Неверный ID блюда.");
                return;
            }

            int currentQty = order.GetDishQuantity(dishId);
            if (currentQty == 0)
            {
                Console.WriteLine("Такого блюда нет в заказе.");
                return;
            }

            Console.Write($"Сколько порций удалить (всего {currentQty}, по умолчанию 1): ");
            int quantity = 1;
            string qtyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(qtyInput) && !int.TryParse(qtyInput, out quantity))
            {
                Console.WriteLine("Неверное количество. Используется 1.");
                quantity = 1;
            }
            if (quantity <= 0) quantity = 1;

            order.RemoveDish(dishId, quantity);
            Console.WriteLine($"Удалено {quantity} порций из заказа #{orderId}.");
        }

        static void EditOrderInteractive()
        {
            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                Console.WriteLine("Нельзя изменять закрытый заказ.");
                return;
            }

            Console.WriteLine($"Текущий комментарий: '{order.Comment}'");
            Console.Write("Новый комментарий (оставьте пустым, чтобы не менять): ");
            string newComment = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newComment))
            {
                order.EditOrder(order.OrderId, order.TableId, order.WaiterId, newComment);
                Console.WriteLine("Комментарий обновлён.");
            }
            else
            {
                Console.WriteLine("Комментарий не изменён.");
            }
        }

        static void ShowOrderInfoInteractive()
        {
            Console.Write("ID заказа (оставьте пустым для всех): ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                if (_orders.Count == 0)
                {
                    Console.WriteLine("Нет заказов.");
                    return;
                }
                Console.WriteLine("\n=== Все заказы ===");
                foreach (var order in _orders)
                {
                    order.ShowInfo();
                }
            }
            else if (int.TryParse(input, out int orderId))
            {
                var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    Console.WriteLine($"Заказ с ID {orderId} не найден.");
                    return;
                }
                Console.WriteLine("\n=== Информация о заказе ===");
                order.ShowInfo();
            }
            else
            {
                Console.WriteLine("Неверный ввод.");
            }
        }

        static void DeleteOrderInteractive()
        {
            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                Console.WriteLine("Закрытые заказы нельзя удалять.");
                return;
            }

            Console.WriteLine($"Вы уверены, что хотите удалить заказ #{orderId}? (y/n)");
            string confirm = Console.ReadLine()?.ToLower();
            if (confirm == "y" || confirm == "yes")
            {
                _orders.Remove(order);
                Console.WriteLine($"Заказ #{orderId} удалён.");
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        static void CloseOrderInteractive()
        {
            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
                return;
            }

            if (order.IsClosed)
            {
                Console.WriteLine("Заказ уже закрыт.");
                return;
            }

            if (order.OrderItems.Count == 0)
            {
                Console.WriteLine("Нельзя закрыть пустой заказ.");
                return;
            }

            order.CloseOrder();
            Console.WriteLine($"Заказ #{orderId} закрыт в {order.ClosedAt:HH:mm dd.MM.yyyy}. Итог: {order.TotalCost} руб.");
        }

        static void PrintReceiptInteractive()
        {
            Console.Write("ID заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Неверный ID заказа.");
                return;
            }

            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Заказ с ID {orderId} не найден.");
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
                    Console.WriteLine("Ошибка ввода, пожалуйста, повторите попытку");
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
                Console.WriteLine("Дата не может быть пустой.");
                return false;
            }

            // Поддерживаем форматы: "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm"
            string[] formats = { "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm", "dd.MM.yyyy H:mm" };

            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                if (result < DateTime.Now)
                {
                    Console.WriteLine("Нельзя забронировать столик в прошлом.");
                    return false;
                }
                return true;
            }

            Console.WriteLine("Неверный формат даты. Используйте, например: 05.11.2025 19:00");
            return false;
        }
        
        static void AdminLogin()
        {
            Console.WriteLine("Введите пароль");
            string AdminPassword = Console.ReadLine();

            try
            {
                if (AdminPassword != "admin")
                {
                    Console.WriteLine("Неверный пароль.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }

            ifAdminLogged = true;
            Console.WriteLine("Вы вошли в режим администратора!");
        }

        static void Debugging()
        {
            Console.WriteLine("Режим отладки запущен!");
        }
    }
}