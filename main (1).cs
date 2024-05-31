using ConsoleApp10.Serializers;
using ProtoBuf;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ConsoleApp10
{
    [Serializable]
    [ProtoContract]
    [ProtoInclude(1, typeof(Athlete))]
    public abstract class Person
    {
        protected string firstName;
        protected string lastName;
        [ProtoMember(2)]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value ?? string.Empty; }
        } // свойство только для чтения, которое возвращает значение поля
        [ProtoMember(3)]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value ?? string.Empty; }
        } // свойство только для чтения, которое возвращает значение поля
        public abstract void Print();
        public Person() { }
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
    [ProtoContract]
    public class Athlete : Person
    {
        private static int nextID = 1;
        public int ID { get; set; }
        public double Score { get; set; }
        public Athlete() : base() { }
        public Athlete(string firstName, string lastName, double score) : base(firstName, lastName) // инициализация полей в базовом классе Person 
        {
            ID = nextID++;
            Score = score; // доп инициализация полей
        }
        public override void Print()
        {
            Console.WriteLine($"ID: {ID}, Имя: {firstName}, Фамилия: {lastName}, Результат: {Score} ");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Athlete[] participants = new Athlete[5]; // создаем массив непроинициализированных переменных типа computerScienceStudents
            participants[0] = new Athlete("Alex", "S", 0);
            participants[1] = new Athlete("Kate", "D", 0);
            participants[2] = new Athlete("Maria", "A", 0.5);
            participants[3] = new Athlete("Andrey", "G", 1);
            participants[4] = new Athlete("Katerina", "K", 1);
            Sort(participants); // сортируем 
            Console.WriteLine("Sorted:");
            foreach (Athlete athlete in participants) // выводим одних
            {
                athlete.Print();
            }
            ISer[] serializers = new ISer[3]
           {
                new JsonSer(),
                new XMLSer(),
                new BinSer()
           };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folder = "Person";
            path = Path.Combine(path, folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] files = new string[3]
            {
                "person.json",
                "person.xml",
                "person.bin"
            };

            for (int i = 0; i < serializers.Length; i++)
            {
                serializers[i].Write(participants, Path.Combine(path, files[i]));
            }

            for (int i = 0; i < serializers.Length; i++)
            {
                participants = serializers[i].Read<Athlete[]>(Path.Combine(path, files[i]));
                foreach (Athlete informatic in participants)
                {
                    informatic.Print();
                }
            }

            Console.ReadKey();
        }
        public static void Sort(Athlete[] students)
        {
            for (int i = 0; i < students.Length - 1; i++)
            {
                for (int j = i; j < students.Length; j++)
                {
                    if (students[i].FirstName.CompareTo(students[j].FirstName) > 0)
                    {
                        Athlete temp = students[i];
                        students[i] = students[j];
                        students[j] = temp;
                    }
                }
            }
        }
    }

}