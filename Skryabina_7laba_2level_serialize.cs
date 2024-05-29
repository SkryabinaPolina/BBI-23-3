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
            WinterSport[] sports = { new FigureSkating(), new SpeedSkating() };
            var resultsList = new List<Result>();
            foreach (var sport in sports)
            {
                resultsList.Add(new Result { Description = $"Название дисциплины: {sport.DisciplineName}" });

                Participant[] participants = new Participant[5]
                {
                new Participant("Иван", new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7 }, 0),
                new Participant("Петр", new double[] { 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7 }, 0),
                new Participant("Андрей", new double[] { 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7 }, 0),
                new Participant("Ксения", new double[] { 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7 }, 0),
                new Participant("Дарья", new double[] { 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7 }, 0)
                };

                for (int i = 0; i < participants.Length; i++)
                {
                    for (int j = 0; j < participants[i].GetScores().Length; j++)
                    {
                        int place = 1;
                        for (int k = 0; k < participants.Length; k++)
                        {
                            if (k != i && participants[i].GetScores()[j] < participants[k].GetScores()[j])
                            {
                                place++;
                            }
                        }
                        int b = participants[i].GetSumOfPlaces();
                        b += place;
                        participants[i].SetSumOfPlaces(b);
                    }
                }

                var orderedParticipants = participants.OrderBy(p => p.GetSumOfPlaces());

                foreach (var participant in orderedParticipants)
                {
                    resultsList.Add(new Result { Description = $"Имя: {participant.GetName()}, Сумма: {participant.GetSumOfPlaces()}" });
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
