using System;
using System.Collections.Generic;


namespace DishClass
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


    class Dish
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
            Console.WriteLine($"ID заказа: {ID}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Вес: {Weight}");
            Console.WriteLine($"Цена:{Price}");
            Console.WriteLine($"Категория: {Category}");
            Console.WriteLine($"Время приготовления : {CookingTime}");
            Console.WriteLine($"Типы: {string.Join(",", Types)}");
            Console.WriteLine("------------------------");
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
                        if (item.Dish.Id == this.Id)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}

