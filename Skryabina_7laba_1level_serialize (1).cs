using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7Laba_1Level_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var resultsList = new List<Result>();
            string[] personOfYearAnswers = { "Person A", "Person B", "Person C", "Person D", "Person E" };
            string[] discoveryOfYearAnswers = { "Discovery X", "Discovery Y", "Discovery Z", "Discovery Q", "Discovery W" };

            PersonOfYear personOfYear = new PersonOfYear(personOfYearAnswers);
            DiscoveryOfYear discoveryOfYear = new DiscoveryOfYear(discoveryOfYearAnswers);

            var personsDetermine = personOfYear.DetermineTopResponses();
            foreach (var personDetermine in personsDetermine)
            {
                resultsList.Add(new Result { Description = personDetermine });
            }
            var discoveryesDetermine = discoveryOfYear.DetermineTopResponses();
            foreach (var discoveryDetermine in discoveryesDetermine)
            {
                resultsList.Add(new Result { Description = discoveryDetermine });
            }
			string fileXml = "results.xml";
			var xml = new MyXmlSerializer();
			xml.Serialize(resultsList, fileXml);
			var xmlList = xml.Deserialize(fileXml);
			foreach (var result in xmlList)
			{
				Console.WriteLine(result.Description);
			}
			string fileJson = "results.json";
			var json = new MyJsonSerializer();
			json.Serialize(resultsList, fileJson);
			var jsonList = json.Deserialize(fileJson);
			foreach (var result in jsonList)
			{
				Console.WriteLine(result.Description);
			}
			Console.ReadLine();
        }
    }

    abstract class CategoryOfTheYear
    {
        protected string[] answers;

        public CategoryOfTheYear(string[] answers)
        {
            this.answers = answers;
        }

        public abstract IList<string> DetermineTopResponses();
    }

    class PersonOfYear : CategoryOfTheYear
    {
        public PersonOfYear(string[] answers) : base(answers) { }

        public override IList<string> DetermineTopResponses()
        {
            var determibeTopResonses = new List<string>();
            Random rand = new Random();
            var groupedAnswers = answers.GroupBy(x => x)
                                        .Select(group => new { Answer = group.Key, Count = group.Count() })
                                        .OrderByDescending(x => x.Count)
                                        .Take(5)
                                        .ToList();

            determibeTopResonses.Add("Таблица ответов для Человека Года:");

            double sumPercentage = 0;
            foreach (var item in groupedAnswers)
            {
                double percentage = rand.Next(0, 100);
                while (sumPercentage + percentage > 100)
                {
                    percentage = rand.Next(0, 100);
                }

                sumPercentage += percentage;

                determibeTopResonses.Add($"{item.Answer} - {percentage:f2}%");
            }
            return determibeTopResonses;
        }
    }

    class DiscoveryOfYear : CategoryOfTheYear
    {
        public DiscoveryOfYear(string[] answers) : base(answers) { }

        public override IList<string> DetermineTopResponses()
        {
            var determibeTopResonses = new List<string>();
            Random rand = new Random();
            var groupedAnswers = answers.GroupBy(x => x)
                                        .Select(group => new { Answer = group.Key, Count = group.Count() })
                                        .OrderByDescending(x => x.Count)
                                        .Take(5)
                                        .ToList();

            determibeTopResonses.Add("Таблица ответов для Открытия Года:");

            double sumPercentage = 0;
            foreach (var item in groupedAnswers)
            {
                double percentage = rand.Next(0, 100);
                while (sumPercentage + percentage > 100)
                {
                    percentage = rand.Next(0, 100);
                }

                sumPercentage += percentage;

                determibeTopResonses.Add($"{item.Answer} - {percentage:f2}%");
            }
            return determibeTopResonses;
        }
    }

    public class Result
    {
        public string Description { get; set; }
        public Result()
        {

        }
    }

    public class ResultsViewModel : DependencyObject
    {


        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(ICollectionView), typeof(ResultsViewModel), new PropertyMetadata(null));

        public ResultsViewModel(IList<Result> results)
        {
            Items = CollectionViewSource.GetDefaultView(results);
        }
    }
	public class MyJsonSerializer : Serializer
        {
            public MyJsonSerializer()
            {
                
            }

            public override IList<Result> Deserialize(string fileName)
            {
                if (File.Exists(fileName))
                {
                    return JsonSerializer.Deserialize<IList<Result>>(File.ReadAllText(fileName));
                }
                else
                {
                    throw new Exception("Файл не найден!");
                }
            }

            public override void Serialize(IList<Result> results, string fileName)
            {
                try
                {
                    File.WriteAllText(fileName, JsonSerializer.Serialize(results));
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }
        }

        public class MyXmlSerializer : Serializer
        {
            public MyXmlSerializer()
            {

            }

            public override IList<Result> Deserialize(string fileName)
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        var xml = new XmlSerializer(typeof(IList<Result>));
                        return xml.Deserialize(stream) as List<Result>;
                    }
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }

            public override void Serialize(IList<Result> results, string fileName)
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        var xml = new XmlSerializer(typeof(IList<Result>));
                        xml.Serialize(stream, results);
                    }
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }
			public abstract class Serializer
        {
            protected Serializer() 
            {
                
            }
            public abstract void Serialize(IList<Result> results, string fileName);
            public abstract IList<Result> Deserialize(string fileName);
        }

        public class MyJsonSerializer : Serializer
        {
            public MyJsonSerializer()
            {
                
            }

            public override IList<Result> Deserialize(string fileName)
            {
                if (File.Exists(fileName))
                {
                    return JsonSerializer.Deserialize<IList<Result>>(File.ReadAllText(fileName));
                }
                else
                {
                    throw new Exception("Файл не найден!");
                }
            }

            public override void Serialize(IList<Result> results, string fileName)
            {
                try
                {
                    File.WriteAllText(fileName, JsonSerializer.Serialize(results));
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }
        }

        public class MyXmlSerializer : Serializer
        {
            public MyXmlSerializer()
            {

            }

            public override IList<Result> Deserialize(string fileName)
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        var xml = new XmlSerializer(typeof(IList<Result>));
                        return xml.Deserialize(stream) as List<Result>;
                    }
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }

            public override void Serialize(IList<Result> results, string fileName)
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        var xml = new XmlSerializer(typeof(IList<Result>));
                        xml.Serialize(stream, results);
                    }
                }
                catch
                {
                    throw new Exception("Запись в файл не удалась");
                }
            }
        }
	}
}