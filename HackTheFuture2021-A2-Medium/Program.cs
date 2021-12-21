using HackTheFuture2021_A2_Medium;
using System.Text.Json;

/// OPDRACHT
/// De enige optie om naar de juiste verdieping van het appartement te gaan, 
/// is door deze speciale lift te nemen.
/// De gebruiksaanwijzingen staan op de muur in de lift: 
/// “Deze lift stopt niet zomaar op elke verdieping. 
/// Om op de gewenste verdieping terecht te komen, dien je rekening te houden met het volgende: 
/// je start op het gelijkvloers met de mogelijkheid om één verdieping omhoog of omlaag te gaan. 
/// Bij elke beweging (omhoog of omlaag) die je maakt,zal de lift zijn aantal stappen verhogen met één.
/// Dus eerst kan je 1 verdieping naar boven of beneden, van dat punt kan je dan 2 verdiepingen naar boven of beneden, dan 3 verdiepingen...”.
/// Kan jij op de juiste verdieping terechtkomen?
/// 
/// TECHNISCH
/// Via een API call krijg je de verdieping waar je terecht moet komen.
/// Als oplossing stuur je naar de bijhorende API endpoint een lijst met verdiepingen
/// waarbij het eerste getal nul is (gelijkvloers) en het laatste getal de verdieping waar je terecht moet komen.
/// Als je op verdieping 9 moet geraken, stuur je een lijst met getallen: 0, 1, 3, 0, 4, 9.
/// Je krijgt een Sample API endpoint (/api/path/1/medium/Sample) om je algoritme op toe te passen.
/// Als je algoritme werkt, kan je overgaan naar de Puzzle API endpoint (/api/path/1/medium/Puzzle) om het antwoord door te sturen.
/// Let op! Bij het indienen van je antwoord bij de Puzzle API endpoint wordt je aantal pogingen opgeteld.
/// De opdracht wordt niet gereset na een foutieve poging.

var puzzle = JsonSerializer.Deserialize<Puzzle>(Data.Puzzle, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

Console.WriteLine($"Expected destination: {puzzle.Destination}");

/// De start is altijd 0 dus ik ga alleen naar de destination kijken, als de start toch anders zou zijn dan werk je simpelweg met een offset

/// Deze method geeft altijd een weg naar de bestemming, het duurt wel zeer lang
var guaranteedPath = GetGuaranteedPath(puzzle.Destination);
/// Deze methode geeft de beste/kortste path
/// Disclaimer: misschien bestaat er nog een betere manier, dit is beste dat ik heb kunnen bedenken
var bestPath = GetBestPath(puzzle.Destination);

List<int> GetGuaranteedPath(int destination)
{
    /// We nemen de absolute waarde van de bestemming zodat we ons niet druk moeten maken over richting
    var adjustedDestination = Math.Abs(destination);
    var path = new List<int>();
    /// We maken gebruik van de ternary operator om een predicate te kiezen
    /// Als onze bestemming even is => ga naar boven als i even is en naar beneden als i oneven is
    /// Als onze bestemming oneven is => ga naar beneden als i even is en naar boven als i oneven is
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
    /// https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=net-6.0
    Predicate<int> predicate = 
        destination % 2 == 0 ?
        (x) => x % 2 == 0
        :
        (x) => x % 2 == 1;
    for (int i = 0; i <= adjustedDestination; i++)
    {
        if (predicate(i))
        {
            path.Add(i);
        }
        else
        {
            path.Add(i * -1);
        }
    }
    /// Hier kijken we dan of de bestemming onder 0 is, zo ja dan inverteren we iedere stap simpelweg
    if (destination < 0)
    {
        path = InvertList(path);
    }
    return path;
}

/// Getest op alles van 1 t.e.m. 100 000
List<int> GetBestPath(int destination)
{
    /// We nemen de absoluute waarde van de bestemming zodat we ons niet druk moeten maken over richting
    var adjustedDestination = Math.Abs(destination);
    /// We maken een path rechtstreeks naar de bestemming
    var path = CreatePath();
    /// Als we meteen op onze eindbestemming zijn geraakt dan zijn we klaar
    /// Zo niet dan moeten we onze path nog bijstellen
    if (path.Last() != adjustedDestination)
    {
        /// We berekenen hoe ver we voorbij onze bestemming zijn gegaan
        var overshoot = Math.Abs(adjustedDestination - path.Last());
        /// We gaan opnieuw een path maken maar nu geven we de overshoot mee
        path = CreatePath(overshoot);
        
    }
    /// Hier kijken we dan of de bestemming onder 0 is, zo ja dan inverteren we iedere stap simpelweg
    if (destination < 0)
    {
        path = InvertList(path);
    }
    return path;

    /// De eerste keer dat we deze methode oproepen geven we geen overshoot mee
    /// De overshoot gebruiken we straks om te zeggen hoe ver we over onze bestemming zijn gegaan
    List<int> CreatePath(int overshoot = 0)
    {
        var path = new List<int>();
        /// We beginnen altijd op 0
        var currentLevel = 0;
        path.Add(currentLevel);
        /// Onze eerste stap is altijd 1
        var index = 1;
        /// Als we geen overshoot hebben meegegeven dan gaan we rechtstreeks naar de bestemming
        if (overshoot == 0)
        {
            /// De loop stop wanneer we op of voorbij onze bestemming zijn
            do
            {
                /// Verhogen onze verdieping met de stap groote
                currentLevel += index;
                /// Voegen onze huidige verdieping toe
                path.Add(currentLevel);
                /// Onze stap vergroot telkens met 1
                index++;
            } while (path.Last() < destination);
            return path;
        }
        /// Als we een overshoot hebben meegegeven kunnen we bepalen wanneer we een stap naar beneden moeten doen
        /// We delen de overshoot door 2 omdat voor iedere stap die we naar beneden doen we erna ook terug naar boven moeten doen
        /// Dus als ik 5 stappen naar beneden zou gaan dan moet ik later ook nog eens 5 stappen naar boven doen om die in te halen
        /// In geval dat de overshoot oneven is kunnen we een komma getal krijgen maar we kunnen geen halve stappen doen dus ronden we af naar beneden
        var switchJump = (int)Math.Floor(overshoot / 2.0);
        /// Om dan toch rekening te kunnen houden met de overblijvende stap maken we een boolean die straks gebruiken
        var lowerByOne = overshoot % 2 == 1;
        do
        {
            /// Hier gaan we exact hetzelfde doe als hierboven met uitzondering dat als we op onze switchJump komen we die stap naar beneden moeten doen
            if (index == switchJump)
            {
                currentLevel -= index;
            }
            else
            {
                currentLevel += index;
            }
            path.Add(currentLevel);
            index++;

        } while (path.Last() < destination);
        /// Als de overshoot oneven was dan gaan we op dit punt ons 1 verdieping boven onze bestemming bevinden
        /// Om dan 1 naar benden te gaan gaan we simpelweg 1 stap naarboven en dan 1 stap naar beneden
        /// Want de tweede stap is altijd 1 groter dan de eerste dus krijgen we => x1 - x2 = -1
        if (lowerByOne)
        {
            currentLevel += index;
            path.Add(currentLevel);
            index++;
            currentLevel -= index;
            path.Add(currentLevel);
        }
        return path;
    }
}

Console.WriteLine($"Best Path:");
foreach (var item in bestPath)
{
    Console.Write($"{item} |");
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine($"Guaranteed Path:");
foreach (var item in guaranteedPath)
{
    Console.Write($"{item} |");
}


/// Inverteert alle getallen in een lijst
List<int> InvertList(List<int> list)
{
    return list.Select(x => x * -1).ToList();
}

public class Puzzle
{
    public int Start { get; set; }
    public int Destination { get; set; }
}