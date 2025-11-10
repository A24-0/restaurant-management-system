using RestaurantManagementSystem;
using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem
{
    //enum категория для блюд
    public enum DishCategory
    {
        Напитки,
        Салаты,
        ХолодныеЗакуски,
        ГорячиеЗакуски,
        Супы,
        ГорячиеБлюда,
        Десерт
    }


    public class Dish
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Compostion { get; set; }
        public string Weight { get; set; }
        public decimal Price { get; set; }
        public DishCategory Category { get; set; }
        public int CookingTime { get; set; }
        public string[] Types { get; set; }

        public Dish(int id, string name, string compostion, string weight, decimal price, DishCategory category, int cookingtime, string[] types)

        {
            ID = id;
            Name = name;
            Compostion = compostion;
            Weight = weight;
            Price = price;
            Category = category;
            CookingTime = cookingtime;
            Types = types;
        }
        //создание блюда
        public static Dish CreateDish(int id, string name, string compostion, string weight, decimal price, DishCategory category, int cookingtime, string[] types)
        {
            return new Dish(id, name, compostion, weight, price, category, cookingtime, types);
        }

        //редактирование блюда
        public void EditDish(int id, string name, string compostion, string weight, decimal price, DishCategory category, int cookingtime, string[] types)
        {
            Name = name;
            Compostion = compostion;
            Weight = weight;
            Price = price;
            Category = category;
            CookingTime = cookingtime;
            Types = types;
        }
        //показать инфу
        public void ShowInfo()
        {
            ConsoleTheme.DrawCard(
                $"Блюдо {ID:00}",
                new[]
                {
                    $"Название: {Name}",
                    $"Состав: {Compostion}",
                    $"Вес: {Weight}",
                    $"Цена: {Price}",
                    $"Категория: {Category}",
                    $"Время приготовления: {CookingTime} минут",
                    $"Типы: {string.Join(", ", Types)}"
                },
                ConsoleColor.DarkGreen);
        }

        //Удалить заказ
        public bool DeleteOrder(List<Order> activeOrders)
        {
            foreach (var order in activeOrders)
            {
                if (!order.IsClosed)
                {
                    // Проверяем, есть ли это блюдо в активном заказе
                    foreach (var item in order.OrderItems)
                    {
                        if (item.Dish.ID == this.ID)
                            return false;
                    }
                }
            }
            return true;
        }

        private static readonly List<Dish> _allDishes = new List<Dish>();

        public static List<Dish> GetAllDishes() => new List<Dish>(_allDishes);
        public static void AddDish(Dish dish) => _allDishes.Add(dish);
        public static Dish FindById(int id) => _allDishes.FirstOrDefault(d => d.ID == id);
    }
}