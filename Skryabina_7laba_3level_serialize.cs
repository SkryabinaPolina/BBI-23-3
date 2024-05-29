using System.Text.Json;
using System.Xml.Serialization;

using static Program.Japan;
using System.Text.Json.Serialization;

internal class Program
{
    private static readonly int N = 10000;
    private static readonly double p_answer_russia = 0.8;
    private static readonly double p_answer_japan = 0.7;
    private static void Main(string[] args)
    {
        try
        {
            var questRussia = new Quest(N);
            var questJapan = new Quest(N);
            string[] q1 = { "Кошка", "Журавль", "Дракон", "Аист", "Ёж", "Волк", "Заяц", "Феникс" };
            string[] q2 = { "Уважение к старшим", "Гостеприимство", "Трудолюбие", "Лояльность" };
            string[] q3 = { "Сакура", "Аниме", "Суши", "Самурай", "Рамен" };

            var resultsList = new List<Result>();

            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                string[] answersRussia = { q1[rand.Next(q1.Length)], q2[rand.Next(q2.Length)], q3[rand.Next(q3.Length)] };
                for (int j = 0; j < 3; j++)
                {
                    double p = rand.NextDouble();
                    if (p > p_answer_russia)
                    {
                        answersRussia[j] = "";
                    }
                }
                questRussia.Append(answersRussia);
                string[] answersJapan = { q1[rand.Next(q1.Length)], q2[rand.Next(q2.Length)], q3[rand.Next(q3.Length)] };
                for (int j = 0; j < 3; j++)
                {
                    double p = rand.NextDouble();
                    if (p > p_answer_japan)
                    {
                        answersJapan[j] = "";
                    }
                }
                questJapan.Append(answersJapan);
            }

            Country russia = new Russia(questRussia);
            Country japan = new Japan(questJapan);

            var russiaResults = russia.ShowResults();
            foreach (var russianResult in russiaResults)
            {
                resultsList.Add(new Result { Description = russianResult });
            }
            var japanResults = japan.ShowResults();
            foreach (var japanResult in japanResults)
            {
                resultsList.Add(new Result { Description = japanResult });
            }

            resultsList.Add(new Result { Description = "Общие результаты для обеих стран:" });

            for (int i = 0; i < 3; i++)
            {
                resultsList.Add(new Result { Description = $"Результаты для Вопроса {i + 1}:" });

                var topAnswersRussia = questRussia.GetTop(i);
                var topAnswersJapan = questJapan.GetTop(i);

                for (int j = 0; j < topAnswersRussia.Length && j < topAnswersJapan.Length; j++)
                {
                    var dataRussia = topAnswersRussia[j];
                    var dataJapan = topAnswersJapan[j];

                    if (dataRussia != null && dataJapan != null && dataRussia.Answer == dataJapan.Answer)
                    {
                        double avgPercentage = (dataRussia.Percentage + dataJapan.Percentage) / 2;
                        resultsList.Add(new Result { Description = $"{avgPercentage:f1}% : {dataRussia.Answer} - средний процент ответа" });
                    }
                }
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
            xml.Serialize(resultsList, fileJson);
            var jsonList = json.Deserialize(fileJson);
            foreach (var result in jsonList)
            {
                Console.WriteLine(result.Description);
            }
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public class Quest
    {
        private readonly string[][] _questions;
        private int _currIndex;

        public Quest(int maxQuestions = 100)
        {
            _questions = new string[3][];
            for (int i = 0; i < 3; i++)
            {
                _questions[i] = new string[maxQuestions];
            }
            _currIndex = 0;
        }

        public void Append(string[] answers)
        {
            if (answers.Length != 3)
            {
                throw new Exception("Должно быть 3 ответа");
            }
            for (int i = 0; i < answers.Length; i++)
            {
                _questions[i][_currIndex] = answers[i];
            }
            _currIndex++;
        }

        public AnswerData[] GetTop(int iQuestion)
        {
            AnswerData[] data = new AnswerData[_questions[iQuestion].Length];
            int dataIndex = 0;
            string[] currQuestions = _questions[iQuestion];
            var query = currQuestions.OrderBy(x => x);

            string currQuestion = query.First();
            int currCnt = 0;
            int nullCnt = 0;
            foreach (string item in query)
            {
                if (item == "")
                {
                    nullCnt++;
                }
                if (item == currQuestion)
                {
                    currCnt++;
                }
                else
                {
                    if (currQuestion != "")
                    {
                        data[dataIndex] = new AnswerData(currQuestion, currCnt / (double)(_questions[iQuestion].Length - nullCnt) * 100);
                        dataIndex++;
                    }

                    currCnt = 1;
                    currQuestion = item;
                }
            }

            data[dataIndex] = new AnswerData(currQuestion, currCnt / (double)(_questions[iQuestion].Length - nullCnt) * 100);
            dataIndex++;

            return data;
        }

        public List<string> Show()
        {
            var showResult = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                AnswerData[] data = GetTop(i);
                showResult.Add($"Вопрос #{i + 1}\n---");
                int cnt = 0;
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j] != null)
                    {
                        showResult.Add($"{data[j].Percentage,4:f1}% : {data[j].Answer}");
                        cnt++;
                        if (cnt >= 5)
                        {
                            break;
                        }
                    }
                }
                showResult.Add("-----");
            }
            return showResult;
        }
    }

    public class AnswerData
    {
        public string Answer { get; }
        public double Percentage { get; }

        public AnswerData(string answer, double percentage = 0)
        {
            Answer = answer;
            Percentage = percentage;
        }
    }

    public abstract class Country
    {
        public abstract List<string> ShowResults();
    }

    public class Russia : Country
    {
        private readonly Quest _quest;

        public Russia(Quest quest)
        {
            _quest = quest;
        }

        public override List<string> ShowResults()
        {
            var showResult = new List<string>()
        {
            "Результаты для России:"
        };
            var questsShow = _quest.Show();
            foreach (var questShow in questsShow)
            {
                showResult.Add(questShow);
            }
            return showResult;
        }
    }

    public class Japan : Country
    {
        private readonly Quest _quest;

        public Japan(Quest quest)
        {
            _quest = quest;
        }

        public override List<string> ShowResults()
        {
            var showResult = new List<string>()
        {
            "Результаты для Японии:"
        };
            var questsShow = _quest.Show();
            foreach (var questShow in questsShow)
            {
                showResult.Add(questShow);
            }
            return showResult;
        }

        public class Result
        {
            public string Description { get; set; }
            public Result()
            {

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
