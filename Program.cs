using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace On_line_Shop_ConsoleApp

{
    public class User
    {
        public string Name;
        public string Password;
        public User(string name)
        {
            Name = name;
        }
    }
    public class Product
    {
        public string Name;
        public decimal Price;
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public void Print()
        {
            Console.WriteLine($"{Name} {Price}");
        }
    }
    public class Order
    {
        public List<Product> Products;
        public decimal FullPrice;
        public int GoodsCount;
        public int OrderNumber;
        public Order(List<Product> products)
        {
            Products = products;
            FullPrice = 0;
            GoodsCount = 0;

            Random random = new Random();
            OrderNumber = random.Next(100, 200);

            foreach (var product in products)
            {
                FullPrice += product.Price;
                GoodsCount++;
            }
        }
    }
    public class Action
    {
        public int Position;
        public string Text;
        public Action(string text)
        {
            Text = text;
        }
        public void Print()
        {
            Console.WriteLine($"{Text}");
        }
    }
    public class Store
    {
        public List<Product> Products = new List<Product>();
        public List<Product> Basket = new List<Product>();
        public List<Action> Actions = new List<Action>();
        public Store()
        {
            Product product1 = new Product("Хлеб", 25);
            Product product2 = new Product("Молоко", 100);
            Product product3 = new Product("Печенье", 50);
            Product product4 = new Product("Масло", 250);
            Product product5 = new Product("Йогурт", 300);
            Product product6 = new Product("Сок", 80);

            Products.Add(product1);
            Products.Add(product2);
            Products.Add(product3);
            Products.Add(product4);
            Products.Add(product5);
            Products.Add(product6);

            Action action1 = new Action("Показать каталог продуктов");
            Action action2 = new Action("Добавить продукт в корзину");
            Action action3 = new Action("Посмотреть корзину");
            Action action4 = new Action("Оформить заказ");
            Action action5 = new Action("Добавить новый продукт");
            Action action6 = new Action("Убрать продукт из корзины");
            Action action7 = new Action("Выйти");

            Actions.Add(action1);
            Actions.Add(action2);
            Actions.Add(action3);
            Actions.Add(action4);
            Actions.Add(action5);
            Actions.Add(action6);
            Actions.Add(action7);
        }
        public void ShowCatalog()
        {
            Console.WriteLine();
            Console.WriteLine("Каталог продуктов:");
            ShowProducts(Products);
        }
        public void AddToBasket(int numberProduct)
        {
            var goodsCount = new Order(Basket);
            var orderNumber = new Order(Basket);

            if (numberProduct <= Products.Count && numberProduct != 0)
            {
                Basket.Add(Products[numberProduct - 1]);
                goodsCount.GoodsCount++;
                orderNumber.OrderNumber++;
                Console.WriteLine();
                Console.WriteLine($"Продукт {Products[numberProduct - 1].Name} успешно добавлен в корзину.");
                Console.WriteLine();
                Console.WriteLine($"Кол-во товаров в корзине {Basket.Count} ");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Такого товара нет!");
            }
        }
        private void DeleteProductFromBasket(int position)
        {
            var goodsCount = new Order(Basket);

            if (position <= Basket.Count && position != 0)
            {
                Basket.RemoveAt(position - 1);
                goodsCount.GoodsCount--;

                Console.WriteLine();
                Console.WriteLine($"Продукт успешно удалён из корзины.");
                Console.WriteLine();
                Console.WriteLine($"Кол-во товаров в корзине {Basket.Count} ");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Такого товара нет!");
            }
        }
        public void ShowBasket()
        {
            if (Basket.Count > 0)

            {
                Console.WriteLine();
                Console.WriteLine("Содержимое корзины: ");
                ShowProducts(Basket);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("В корзине ничего нет.");
            }
        }
        public void ShowProducts(List<Product> products)
        {
            int number = 1;
            foreach (var product in products)
            {
                Console.Write(number + ". ");
                product.Print();
                number++;
            }
        }
        public void AddProductToCatalog(string name, int price)
        {
            Products.Add(new Product(name, price));
        }
        public void CreateOrder(string userBasketPath)
        {
            var reader = new StreamReader(userBasketPath, Encoding.UTF8);
            var fileData = reader.ReadToEnd();
            reader.Close();

            var lines = fileData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("{0,-10} {1,-15} {2,-30} {3,-15}", "Имя", "Номер заказа", "Количество товаров", "Сумма заказа");

            foreach (var line in lines)
            {
                var data = line.Split(' ');
                Console.WriteLine("{0,-10} {1,-15} {2,-30} {3,-15}", data[0], data[1], data[2], data[3]);
            }
        }
        public void MakeActions(List<Action> actions)
        {
            int number = 1;
            foreach (var action in actions)
            {
                Console.Write(number + ". ");
                action.Print();
                number++;
            }
        }
        public void ShowActions()
        {
            Console.WriteLine();
            Console.WriteLine("Здравствуйте! Выберите действие: ");
            Console.WriteLine();
            MakeActions(Actions);
            Console.WriteLine();
            Console.WriteLine("Выберите номер действия, которое хотите совершить. ");
        }
        static bool IsYes(string answer)
        {
            if (answer.ToLower() == "да")
            {
                return true;
            }
            if (answer.ToLower() == "нет")
            {
                return false;
            }
            else
            {
                while (answer.ToLower() != "да" || answer.ToLower() != "нет")
                {
                    Console.WriteLine("Вы выполнили ввод неправильно. Введите Да или Нет. ");
                    answer = Console.ReadLine();

                    if (answer.ToLower() == "да")
                    {
                        return true;
                    }
                    if (answer.ToLower() == "нет")
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public void SaveOrder(User user, string userBasketPath)
        {
            Order order = new Order(Basket);

            var formattedData = $"{user.Name} {order.OrderNumber} {order.GoodsCount} {order.FullPrice}";
            StreamWriter writer = new StreamWriter(userBasketPath, true, Encoding.UTF8);
            writer.WriteLine(formattedData);
            writer.Close();
        }
        public void Admin()
        {
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            while (password != "000")
            {
                Console.WriteLine("Вы ввели неправильный пароль");
                Console.WriteLine("Введите пароль:");
                password = Console.ReadLine();
            }
        }
        public void ChooseAction(User user)
        {
            var userOrderPath = "UserOrderPath.txt";
            bool yes = false;
            Store onlineStore = new Store();

            while (true)
            {
                onlineStore.ShowActions();
                var resultNumberAction = int.TryParse(Console.ReadLine(), out int numberAction);
                while (!resultNumberAction && numberAction <= Actions.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("Вы выполнили ввод неправильно. Введите число. ");
                    resultNumberAction = int.TryParse(Console.ReadLine(), out numberAction);
                }

                switch (numberAction)
                {
                    case 1:
                        onlineStore.ShowCatalog();
                        Console.WriteLine();
                        Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Жаль, что вы решили покинуть магазин!");
                            return;
                        }

                    case 2:
                        onlineStore.ShowCatalog();
                        Console.WriteLine();
                        Console.WriteLine("Напишите номер продукта, который нужно добавить в корзину");
                        var productNumber = int.TryParse(Console.ReadLine(), out int rightProductNumber);

                        while (!productNumber && rightProductNumber > Products.Count)
                        {
                            Console.WriteLine("Вы выполнили ввод неправильно. Введите число. ");
                            productNumber = int.TryParse(Console.ReadLine(), out rightProductNumber);
                        }

                        onlineStore.AddToBasket(rightProductNumber);
                        Console.WriteLine();
                        Console.WriteLine("Хотите добавить продукт в корзину? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            goto case 2;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет.");
                            yes = IsYes(Console.ReadLine());

                            if (yes)
                            {
                                break;
                            }
                            else
                            {
                                goto case 3;
                            }
                        }

                    case 3:
                        Console.WriteLine();
                        Console.WriteLine("Хотите посмотреть корзину? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            onlineStore.ShowBasket();
                        }

                        Console.WriteLine();
                        Console.WriteLine("Хотите добавить товар в корзину? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            goto case 2;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Перейти к оформлению? Наберите Да или Нет.");
                            yes = IsYes(Console.ReadLine());

                            if (yes)
                            {
                                goto case 4;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет.");
                                yes = IsYes(Console.ReadLine());

                                if (yes)
                                {
                                    break;
                                }
                                else
                                {
                                    goto case 3;
                                }
                            }
                        }

                    case 4:
                        Console.WriteLine();
                        Console.WriteLine("Хотите оформить заказ? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            Console.WriteLine("Оформление заказа:");
                            onlineStore.SaveOrder(user, userOrderPath);
                            onlineStore.CreateOrder(userOrderPath);
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет.");
                            yes = IsYes(Console.ReadLine());

                            if (yes)
                            {
                                break;
                            }
                            else
                            {
                                goto case 3;
                            }
                        }

                    case 5:
                        Console.WriteLine();
                        Console.WriteLine("Хотите добавить новый продукт в каталог? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            Console.WriteLine("Введите название продукта: ");
                            string name = Console.ReadLine();

                            if (name == string.Empty)
                            {
                                Console.WriteLine("Вы выполнили ввод неправильно. Введите название продукта: ");
                            }

                            Console.WriteLine("Введите цену: ");
                            var rightPrice = int.TryParse(Console.ReadLine(), out int price);

                            while (!rightPrice && price > 0)
                            {
                                Console.WriteLine("Вы выполнили ввод неправильно. Введите число. ");
                                rightPrice = int.TryParse(Console.ReadLine(), out price);
                            }

                            onlineStore.AddProductToCatalog(name, price);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет.");
                            yes = IsYes(Console.ReadLine());

                            if (yes)
                            {
                                break;
                            }
                            else
                            {
                                goto case 3;
                            }
                        }

                        break;

                    case 6:
                        Console.WriteLine();
                        Console.WriteLine("Хотите убрать продукт из корзины? Наберите Да или Нет.");
                        yes = IsYes(Console.ReadLine());

                        if (yes)
                        {
                            onlineStore.ShowBasket();
                            Console.WriteLine("Введите номер позиции, которую нужно удалить: ");
                            var rightPosition = int.TryParse(Console.ReadLine(), out int position);

                            while (!rightPosition && position > 0)
                            {
                                Console.WriteLine("Вы выполнили ввод неправильно. Введите число. ");
                                rightPosition = int.TryParse(Console.ReadLine(), out position);
                            }

                            onlineStore.DeleteProductFromBasket(position);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Хотите выйти в главное меню? Наберите Да или Нет.");
                            yes = IsYes(Console.ReadLine());

                            if (yes)
                            {
                                break;
                            }
                            else
                            {
                                goto case 3;
                            }
                        }

                        break;

                    case 7:
                        Environment.Exit(0);

                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Такого действия не существует!"); break;
                }
            }
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Как вас зовут?");
            var userName = Console.ReadLine();
            var user = new User(userName);

            Store onlineStore1 = new Store();

            onlineStore1.Admin();

            onlineStore1.ChooseAction(user);
        }
    }
}
