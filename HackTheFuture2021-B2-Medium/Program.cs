using HackTheFuture2021_B2_Medium;
using System.Text.Json;

/// OPDRACHT
/// Jan hield een notitieboekje bij waar hij al zijn bevindingen opschreef.
/// Er staan ook heel wat getallen in die op niks slaan.
/// Op de laatste pagina vinden we echter volgende zin:
/// “Vind een patroon in de getallen in dit notitieboekje en je hebt de code van mijn smartphone”.
/// De getallen vertellen ons niks, maar er zijn wel veel duplicaten in te vinden.
/// Misschien is dat wel het patroon dat Jan ons heeft achtergelaten?
/// Vormt dit de code?
/// Het getal is te groot om zo te kunnen ontcijferen.
/// Kan jij alle patronen uit een groot getal vissen?
/// 
/// TECHNISCH
/// Via een API call krijg je een groot getal bestaande uit een heleboel cijfers.
/// De bedoeling is dat je alle mogelijke patronen in dit getal vindt.
/// Een patroon bestaat uit 2 of meerdere cijfers.
/// Eerst en vooral ga je op zoek naar hoeveel verschillende patronen er te vinden zijn.
/// Daarna ga je op zoek naar het meest voorkomende patroon.
/// De hoeveelheid verschillende patronen plak (concatenatie) je aan het 
/// meest voorkomende patroon en dat resultaat stuur je door als antwoord naar de bijhorende API endpoint.
/// Je krijgt een Sample API endpoint (/api/path/2/medium/Sample) om je algoritme op toe te passen. 
/// Als je algoritme werkt, kan je overgaan naar de Puzzle API endpoint (/api/path/2/medium/Puzzle) om het antwoord door te sturen.
/// Let op! Bij het indienen van je antwoord bij de Puzzle API endpoint wordt je aantal pogingen opgeteld.
/// De opdrachtwordt niet gereset na een foutievepoging.

var puzzle = JsonSerializer.Deserialize<string>(Data.Puzzle).ToString();

/// Een lijst om alle blokken in bij te houden
var chunks = new List<string>();

/// Met deze loop willen gaan we alle mogelijke blokken doorlopen van groot naar klein
for (int i = puzzle.Length; i >= 2; i--)
{
    /// We steken alle blokken in de lijst
    chunks.AddRange(GetChunks(puzzle, i));
}

/// We groeperen alle blokken, 
/// deze groepen bevatten een Key waarde (de tekst) en 
/// dan een lijst met alle objecten die daar aan voldoen (het zijn allemaal strings dus alle duplicates)
/// Die kunnen we dan omzetten naar een tuple met de key en het aantal keer dat die voorkomt
var combinations = chunks.GroupBy(x => x).Select(x => (Key: x.Key, Count: x.Count()));

/// We spreken pas over een patroon als het meer dan 2 keer voorkomt dus daarop kunnen we filteren
var patterns = combinations.Where(x => x.Count > 1).ToList();

/// Het aantal verschillende pattronen
var amountOfPatterns = patterns.Count;
/// Het patroon dat het meeste voorkomt
var mostCommonPattern = patterns.MaxBy(x => x.Count);

/// Het antwoord is de concatenatie van het aantal patronen en het patroon dat het meeste voorkomt
Console.WriteLine($"{amountOfPatterns}{mostCommonPattern.Key}");
Console.WriteLine("16221");

List<string> GetChunks(string text, int size)
{
    var chunks = new List<string>();
    /// Het aantal mogelijke blokken is: text length - de blok size + 1
    var possibleChunks = (text.Length - size) + 1;
    for (int i = 0; i < possibleChunks; i++)
    {
        /// We gebruiken Substring om de blokken te maken
        chunks.Add(text.Substring(i, size));
    }
    return chunks;
}