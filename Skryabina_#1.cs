using System;

struct Rectangle
{
    public double Length { get; set; }
    public double Width { get; set; }

    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }

    public double Area()
    {
        return Length * Width;
    }

    public double Perimeter()
    {
        return (Length + Width) * 2;
    }

    public (bool Longer, bool Wider, bool LargerArea) Compare(Rectangle other)
    {
        return (
            Longer: this.Length > other.Length,
            Wider: this.Width > other.Width,
            LargerArea: this.Area() > other.Area()
        );
    }
}

class Program
{
    static void PrintComparisonTable(Rectangle[] rectangles)
    {
        Console.WriteLine("{0,-12} {1,-8} {2,-8} {3,-8} {4,-12}", "Rectangle", "Length", "Width", "Area", "Perimeter");
        for (int i = 0; i < rectangles.Length; i++)
        {
            Console.WriteLine("Rectangle {0} {1,-8} {2,-8} {3,-8} {4,-12}", i + 1, rectangles[i].Length, rectangles[i].Width, rectangles[i].Area(), rectangles[i].Perimeter());
        }

        Console.WriteLine("\nComparison Results:");
        Console.WriteLine("{0,-12} {1,-8} {2,-8} {3,-12}", "Comparison", "Longer", "Wider", "Larger Area");

        for (int i = 0; i < rectangles.Length; i++)
        {
            for (int j = i + 1; j < rectangles.Length; j++)
            {
                var comp = rectangles[i].Compare(rectangles[j]);
                Console.WriteLine("Rect {0} vs {1} {2,-8} {3,-8} {4,-12}", i + 1, j + 1, comp.Longer, comp.Wider, comp.LargerArea);
            }
        }
    }

    static void Main(string[] args)
    {
        Rectangle[] rectangles = new Rectangle[3];

        for (int i = 0; i < 3; i++)
        {
            Console.Write("Enter length of rectangle {0}: ", i + 1);
            double length = double.Parse(Console.ReadLine());

            Console.Write("Enter width of rectangle {0}: ", i + 1);
            double width = double.Parse(Console.ReadLine());

            rectangles[i] = new Rectangle(length, width);
        }

        PrintComparisonTable(rectangles);
    }
}