using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

abstract class Task
{
    protected string text = "";
    protected string checker = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    public string Text
    {
        get => text;
        protected set => text = value;
    }

    public virtual void Solution() { }
    public Task(string text)
    {
        this.text = text;
    }
}

class Task1 : Task
{
    private char answer;
    public char Answer
    {
        get => answer;
        protected set => answer = value;
    }
    public Task1(string text) : base(text)
    {
        answer = ' ';
    }
    public override void Solution()
    {
        int[] letters = new int[33];
        for (int i = 0; i < text.Length; i++)
        {
            if (checker.Contains(text.ToUpper()[i]))
            {
                letters[text.ToUpper()[i] - 'А']++;
            }
        }
        int mx = 0;
        for (int i = 0; i < 33; ++i)
        {
            if (mx < letters[i])
            {
                mx = letters[i];
                answer = Convert.ToChar('А' + i);
            }
        }

    }

    public override string ToString()
    {
        Solution();
        return Convert.ToString(answer);
    }
}
class Task2 : Task
{
    private string answer;
    public string Answer
    {
        get => answer;
    }
    public Task2(string text) : base(text)
    {
        answer = "";
    }
    public override void Solution()
    {
        for (int i = 0; i < text.Length; ++i)
        {
            string word = "";
            if (checker.Contains(text.ToUpper()[i]))
            {
                word += text[i];
                while (i + 1 < text.Length && checker.Contains(text.ToUpper()[i + 1]))
                {
                    word += text[i + 1];
                    ++i;
                }
            }
            char[] arr = word.ToCharArray();
            Array.Reverse(arr);
            word = new string(arr);
            if (word != "")
                answer += word;
            else
                answer += text[i];
        }
    }
    public override string ToString()
    {
        Solution();
        return answer;
    }
}

class Program
{
    static void Main()
    {
        string text = "Яблоко, климат. погода: дьявол.";
        Task[] tasks = {
            new Task1(text),
            new Task2(text)
        };
        Console.WriteLine(tasks[0]);
        Console.WriteLine(tasks[1]);

        string path = @"C:\Users\user\Documents"; //путь до рабочего стола
        string folderName = "Control work";
        path = Path.Combine(path, folderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName1 = "task_1.json";
        string fileName2 = "task_2.json";


        fileName1 = Path.Combine(path, fileName1);
        fileName2 = Path.Combine(path, fileName2);
        if (!File.Exists(fileName1))
        {
            File.Create(fileName1);
        }
        if (!File.Exists(fileName2))
        {
            File.Create(fileName2);
        }

    }
}