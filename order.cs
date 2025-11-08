using RestaurantManagementSystem;
using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem
{
    public class Order
    {
        public class OrderItem
        {
            public Dish Dish { get; set; }
            public int Quantity { get; set; }
            public OrderItem(Dish dish, int quantity)
            {
                Dish = dish ?? throw new ArgumentNullException(nameof(dish));
                if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть больше 0.");
                Quantity = quantity;
            }
            public decimal TotalPrice => Dish.Price * Quantity;
        }
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WaiterId { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosedAt { get; private set; }


        public Order(int orderId, int tableId, int waiterId, string comment = "")
        {
            OrderId = orderId;
            TableId = tableId;
            WaiterId = waiterId;
            Comment = comment;
            CreatedAt = DateTime.Now;
            IsClosed = false;
            ClosedAt = null;
            OrderItems = new List<OrderItem>();
            TotalCost = 0;
        }
        public static Order CreateOrder(int orderId, int tableId, int waiterId, string comment = "")
        {
            return new Order(orderId, tableId, waiterId, comment);
        }
        public void EditOrder(int orderId, int tableId, int waiterId, string comment = "")
        {
            if (!string.IsNullOrEmpty(comment))
                Comment = comment;
        }
        // Добавить блюдо в заказ
        public void AddDish(Dish dish, int quantity = 1)
        {
            // Проверить, есть ли уже такое блюдо в заказе
            var existingItem = OrderItems.FirstOrDefault(d => d.Dish.ID == dish.ID);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                OrderItems.Add(new OrderItem(dish, quantity));
            }
            CalculateTotalCost();
        }

        // Удалить блюдо из заказа
        public void RemoveDish(int dishId, int quantity = 1)
        {
            var item = OrderItems.FirstOrDefault(d => d.Dish.ID == dishId);
            if (item != null)
            {
                if (item.Quantity <= quantity)
                {
                    OrderItems.Remove(item);
                }
                else
                {
                    item.Quantity -= quantity;
                }
                CalculateTotalCost();
            }
        }

        // Показать информацию о заказе
        public void ShowInfo()
        {
            Console.WriteLine($"Заказ ID: {OrderId}");
            Console.WriteLine($"Столик: {TableId}");
            Console.WriteLine($"Официант: {WaiterId}");
            Console.WriteLine($"Создан: {CreatedAt:HH:mm dd.MM.yyyy}");
            Console.WriteLine($"Статус: {(IsClosed ? "Закрыт" : "Открыт")}");
            if (IsClosed)
            {
                Console.WriteLine($"Закрыт: {ClosedAt:HH:mm dd.MM.yyyy}");
            }
            Console.WriteLine($"Общая стоимость: {TotalCost} руб.");
            Console.WriteLine($"Комментарий: {Comment}");
            Console.WriteLine("Блюда:");

            foreach (var item in OrderItems)
            {
                Console.WriteLine($"  {item.Dish.Name} x{item.Quantity} = {item.TotalPrice} руб.");
            }
            Console.WriteLine("------------------------");
        }

        // Закрыть заказ
        public void CloseOrder()
        {
            if (!IsClosed)
            {
                IsClosed = true;
                ClosedAt = DateTime.Now;
                CalculateTotalCost();
            }
        }

        // Вывести чек (только для закрытых заказов)
        public void PrintReceipt()
        {
            if (!IsClosed)
            {
                Console.WriteLine("Чек можно вывести только для закрытого заказа!");
                return;
            }

            Console.WriteLine("=== ЧЕК ===");
            Console.WriteLine($"Столик: {TableId}");
            Console.WriteLine($"Официант: {WaiterId}");
            Console.WriteLine($"Период обслуживания: с {CreatedAt:HH:mm} по {ClosedAt:HH:mm}");
            Console.WriteLine();

            // Группировка блюд по категориям
            var dishesByCategory = OrderItems
                .GroupBy(d => d.Dish.Category)
                .OrderBy(g => g.Key);

            foreach (var categoryGroup in dishesByCategory)
            {
                Console.WriteLine($"{categoryGroup.Key}:");
                foreach (var item in categoryGroup)
                {
                    Console.WriteLine($"  {item.Dish.Name} {item.Quantity}*{item.Dish.Price}={item.TotalPrice}");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Итог счета: {TotalCost}");
            Console.WriteLine("======================");
        }

        // Расчет общей стоимости
        private void CalculateTotalCost()
        {
            TotalCost = OrderItems.Sum(item => item.TotalPrice);
        }

        // Получить количество конкретного блюда в заказе
        public int GetDishQuantity(int dishId)
        {
            var item = OrderItems.FirstOrDefault(d => d.Dish.ID == dishId);
            return item?.Quantity ?? 0;
        }
    }
}
//а тут просто мяу