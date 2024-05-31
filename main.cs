using LabTaskJsonSerializerLib;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

abstract class Survey
{
    public string animal { get; }
    public string character { get; }
    public string thing { get; }

    [JsonConstructor]
    public Survey(string _animal, string _character, string _thing)
    {
        animal = _animal;
        character = _character;
        thing = _thing;
    }

    public abstract void printSurvey();
}

class SurveyWithCountry : Survey
{
    private string _country;
    public string country { get { return _country; } }

    public SurveyWithCountry(string animal, string character, string thing, string country)
        : base(animal, character, thing)
    {
        _country = country;
    }

    public override void printSurvey()
    {
        Console.WriteLine(
            "Country: {0, 6} | Animal: {1,15} | char: {2,20} | thing: {3,15}",
            country, animal, character, thing);
    }
}

class RussianSurvey : SurveyWithCountry
{
    [JsonConstructor]
    public RussianSurvey(string animal, string character, string thing)
        : base(animal, character, thing, "Russia")
    {
    }
}


class JapanSurvey : SurveyWithCountry
{
    [JsonConstructor]
    public JapanSurvey(string animal, string character, string thing)
        : base(animal, character, thing, "Japan")
    {
    }
}

class Program
{
    static void printElemCountOfArray(KeyValuePair<string, int>[] values, int limit, int totalCount)
    {
        Console.WriteLine();
        for (int index = 0; index < Math.Min(limit, values.Length); ++index)
        {
            Console.WriteLine(
                "value: {0,20} | percent: {1,6}%",
                values[index].Key, ((double)values[index].Value / totalCount * 100).ToString("00.00" +
                "" +
                ""));
        }
    }

    static void CalculatePercentsByCountry(Survey[] survery)
    {
        for (int index = 0; index < survery.Length; ++index)
            survery[index].printSurvey();

        int animalAnswers = survery.Count(surveyItem => !string.IsNullOrEmpty(surveyItem.animal));
        int charAnswers = survery.Count(surveyItem => !string.IsNullOrEmpty(surveyItem.character));
        int thingAnswers = survery.Count(surveyItem => !string.IsNullOrEmpty(surveyItem.thing));

        Dictionary<string, int> animalAnswersCount = new Dictionary<string, int>();
        for (int index = 0; index < survery.Length; ++index)
        {
            if (animalAnswersCount.ContainsKey(survery[index].animal))
                animalAnswersCount[survery[index].animal]++;
            else
                animalAnswersCount[survery[index].animal] = 1;
        }
        var animalAnswersAsArray = animalAnswersCount.ToArray();
        Array.Sort(animalAnswersAsArray, (a, b) => a.Value.CompareTo(b.Value));
        Array.Reverse(animalAnswersAsArray);
        printElemCountOfArray(animalAnswersAsArray, 5, animalAnswers);

        Dictionary<string, int> charAnswersCount = new Dictionary<string, int>();
        for (int index = 0; index < survery.Length; ++index)
        {
            if (charAnswersCount.ContainsKey(survery[index].character))
                charAnswersCount[survery[index].character]++;
            else
                charAnswersCount[survery[index].character] = 1;
        }
        var charAnswersAsArray = charAnswersCount.ToArray();
        Array.Sort(charAnswersAsArray, (a, b) => a.Value.CompareTo(b.Value));
        Array.Reverse(charAnswersAsArray);
        printElemCountOfArray(charAnswersAsArray, 5, charAnswers);
        Dictionary<string, int> thingAnswersCount = new Dictionary<string, int>();
        for (int index = 0; index < survery.Length; ++index)
        {
            if (thingAnswersCount.ContainsKey(survery[index].thing))
                thingAnswersCount[survery[index].thing]++;
            else
                thingAnswersCount[survery[index].thing] = 1;
        }
        var thingAnswersAsArray = thingAnswersCount.ToArray();
        Array.Sort(thingAnswersAsArray, (a, b) => a.Value.CompareTo(b.Value));
        Array.Reverse(thingAnswersAsArray);
        printElemCountOfArray(thingAnswersAsArray, 5, thingAnswers);
    }

    static void Main()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Shashkina", "Lab9_3");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        JapanSurvey[] japanSurvery = {
            new JapanSurvey("кошка", "застенчивость", "рис"),
            new JapanSurvey("собака", "дисциплинированость", "кимоно"),
            new JapanSurvey("рыба", "вежливость", "сакура"),
            new JapanSurvey("дракон", "целеустремлённость", "сакура"),
            new JapanSurvey("дракон", "скромность", "рис")
        };

        RussianSurvey[] russainSurvery = {
            new RussianSurvey("медведь", "трудолюбие", "чай"),
            new RussianSurvey("медведь", "вежливость", "гречка")
        };

        JSSerializer serializer = new JSSerializer();

        string jsonFile = Path.Combine(path, "lab9_3_japan.json");
        serializer.Write<Survey[]>(japanSurvery, jsonFile);
        JapanSurvey[] japanSurveryLoaded = serializer.Read<JapanSurvey[]>(jsonFile);

        jsonFile = Path.Combine(path, "lab9_3_russia.json");
        serializer.Write<Survey[]>(russainSurvery, jsonFile);
        RussianSurvey[] russainSurveryLoaded = serializer.Read<RussianSurvey[]>(jsonFile);

        Console.WriteLine();
        Console.WriteLine("==== Japan ====");
        CalculatePercentsByCountry(japanSurveryLoaded);

        Console.WriteLine();
        Console.WriteLine("==== Russia ====");
        CalculatePercentsByCountry(russainSurveryLoaded);

        Survey[] total = new Survey[japanSurveryLoaded.Length + russainSurveryLoaded.Length];
        russainSurveryLoaded.CopyTo(total, 0);
        japanSurveryLoaded.CopyTo(total, russainSurveryLoaded.Length);


        Console.WriteLine();
        Console.WriteLine("==== total ====");
        CalculatePercentsByCountry(total);
    }
}