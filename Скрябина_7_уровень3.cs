using System;
using System.Linq;

class Program
{
    class AnswerData
    {
        public string answer { get; set; }
        public double percentage { get; set; }

        public AnswerData(string answer, double percentage = 0)
        {
            this.answer = answer;
            this.percentage = percentage;
        }
    }

    struct Quest
    {
        private string[][] questions;
        private int currIndex;

        public Quest(int max_questions = 100)
        {
            questions = new string[3][];
            for (int i = 0; i < 3; i++)
            {
                questions[i] = new string[max_questions];
            }
            currIndex = 0;
        }

        public void append(string[] answers)
        {
            if (answers.Length != 3)
            {
                Console.WriteLine("Должно быть 3 ответа");
                return;
            }
            for (int i = 0; i < answers.Length; i++)
            {
                questions[i][currIndex] = answers[i];
            }
            currIndex++;
        }

        public AnswerData[] get_top(int i_question)
        {
            AnswerData[] data = new AnswerData[questions[i_question].Length];
            int dataIndex = 0;
            string[] currQuestions = questions[i_question];
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
                        data[dataIndex] = new AnswerData(currQuestion, currCnt / (double)(currQuestions.Length - nullCnt) * 100);
                        dataIndex++;
                    }

                    currCnt = 1;
                    currQuestion = item;
                }
            }

            data[dataIndex] = new AnswerData(currQuestion, currCnt / (double)(currQuestions.Length - nullCnt) * 100);
            dataIndex++;

            return data;
        }

        public void show()
        {
            for (int i = 0; i < 3; i++)
            {
                AnswerData[] data = get_top(i);
                Console.WriteLine("Вопрос #{0}\n---", i + 1);
                int cnt = 0;
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j] != null)
                    {
                        Console.WriteLine("{0, 4:f1}% : {1}", data[j].percentage, data[j].answer);
                        cnt++;
                        if (cnt >= 5)
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("-----", i + 1);
            }
        }
    }

    class Country
    {
        public virtual void ShowResults() { }
    }

    class Russia : Country
    {
        private Quest quest;

        public Russia(Quest quest)
        {
            this.quest = quest;
        }

        public override void ShowResults()
        {
            Console.WriteLine("Результаты для России:");
            quest.show();
        }
    }

    class Japan : Country
    {
        private Quest quest;

        public Japan(Quest quest)
        {
            this.quest = quest;
}

        public override void ShowResults()
        {
            Console.WriteLine("Результаты для Японии:");
            quest.show();
        }
    }

    static void Main()
{
    int N = 10;
    double p_answer_russia = 0.80; 
    double p_answer_japan = 0.75; 

    Quest questRussia = new Quest(N);
    Quest questJapan = new Quest(N);

    string[] q1 = { "Кошка", "Журавль", "Дракон", "Аист", "Ёж", "Волк", "Заяц", "Феникс" };
    string[] q2 = { "Уважение к старшим", "Гостеприимство", "Трудолюбие", "Лояльность" };
    string[] q3 = { "Сакура", "Аниме", "Суши", "Самурай", "Рамен" };

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
        questRussia.append(answersRussia);
        string[] answersJapan = { q1[rand.Next(q1.Length)], q2[rand.Next(q2.Length)], q3[rand.Next(q3.Length)] };
        for (int j = 0; j < 3; j++)
        {
            double p = rand.NextDouble();
            if (p > p_answer_japan)
            {
                answersJapan[j] = "";
            }
        }
        questJapan.append(answersJapan);
    }

    Country russia = new Russia(questRussia);
    Country japan = new Japan(questJapan);

    russia.ShowResults();
    japan.ShowResults();

    Console.WriteLine("Общие результаты для обеих стран:");

    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine("Результаты для Вопроса {0}:", i + 1);

        var topAnswersRussia = questRussia.get_top(i);
        var topAnswersJapan = questJapan.get_top(i);

        for (int j = 0; j < topAnswersRussia.Length && j < topAnswersJapan.Length; j++)
        {
            var dataRussia = topAnswersRussia[j];
            var dataJapan = topAnswersJapan[j];

            if (dataRussia != null && dataJapan != null && dataRussia.answer == dataJapan.answer)
            {
                double avgPercentage = (dataRussia.percentage + dataJapan.percentage) / 2;
                Console.WriteLine("{0:f2}% : {1} - средний процент ответа", avgPercentage, dataRussia.answer);
            }
        }
    }
}
}