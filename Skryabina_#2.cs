using System;
using System.Collections.Generic;
using System.Linq;

namespace Embrasures
{
    public abstract class Embrasure
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }

        public abstract double CalculatePrice();
    }

    public class Window : Embrasure
    {
        public int NumberOfPanes { get; set; }

        public override double CalculatePrice()
        {
            return 100 * Width * Length * Thickness + 50 * NumberOfPanes;
        }
    }

    public class Door : Embrasure
    {
        public bool HasPattern { get; set; }
        public bool HasGlass { get; set; }

        public override double CalculatePrice()
        {
            double price = 200 * Width * Length * Thickness;
            if (HasPattern) price += 50;
            if (HasGlass) price += 100;
            return price;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            Window[] windows = new Window[5];
            Door[] doors = new Door[5];

            
            for (int i = 0; i < 5; i++)
            {
                windows[i] = new Window
                {
                    Name = $"Окно {i + 1}",
                    Width = 1.2 + i * 0.2,
                    Length = 1.5 + i * 0.3,
                    Thickness = 0.1 + i * 0.01,
                    NumberOfPanes = 2 + i
                };

                doors[i] = new Door
                {
                    Name = $"Дверь {i + 1}",
                    Width = 0.9 + i * 0.2,
                    Length = 2.0 + i * 0.3,
                    Thickness = 0.15 + i * 0.02,
                    HasPattern = i % 2 == 0,
                    HasGlass = i % 3 == 0
                };
            }

            
            Array.Sort(windows, (x, y) => x.CalculatePrice().CompareTo(y.CalculatePrice()));

            
            Array.Sort(doors, (x, y) => x.CalculatePrice().CompareTo(y.CalculatePrice()));

            
            Console.WriteLine("Таблица окон:");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10}", "Название", "Ширина", "Длина", "Толщина", "Цена");
            foreach (Window window in windows)
            {
                Console.WriteLine("{0,-20} {1,-10:F2} {2,-10:F2} {3,-10:F2} {4,-10:F2}", window.Name, window.Width, window.Length, window.Thickness, window.CalculatePrice());
            }

            
            Console.WriteLine("\nТаблица дверей:");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10}", "Название", "Ширина", "Длина", "Толщина", "Цена");
            foreach (Door door in doors)
            {
                Console.WriteLine("{0,-20} {1,-10:F2} {2,-10:F2} {3,-10:F2} {4,-10:F2}", door.Name, door.Width, door.Length, door.Thickness, door.CalculatePrice());
            }

            
            Embrasure[] embrasures = new Embrasure[windows.Length + doors.Length];
            Array.Copy(windows, embrasures, windows.Length);
            Array.Copy(doors, 0, embrasures, windows.Length, doors.Length);

            
            Array.Sort(embrasures, (x, y) => x.CalculatePrice().CompareTo(y.CalculatePrice()));

            
            Console.WriteLine("\nИнформация обо всех окнах и дверях:");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10}", "Тип", "Название", "Ширина", "Длина", "Толщина", "Цена");
            foreach (Embrasure embrasure in embrasures)
            {
                if (embrasure is Window)
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-10:F2} {3,-10:F2} {4,-10:F2} {5,-10:F2}", "Окно", embrasure.Name, embrasure.Name, embrasure.Width, embrasure.Length, embrasure.Thickness, embrasure.CalculatePrice());
                }
                else if (embrasure is Door)
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-10:F2} {3,-10:F2} {4,-10:F2} {5,-10:F2}", "Дверь", embrasure.Name, embrasure.Width, embrasure.Length, embrasure.Thickness, embrasure.CalculatePrice());
                }
            }
        }
    }
}