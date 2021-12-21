using HackTheFuture2021_B1_Easy;
using System.Text.Json;
using System.Text.RegularExpressions;

/// OPDRACHT
/// Jan beschikte over een smartwatch waarmee hij zijn hartslag ten alle tijden kon raadplegen.
/// Deze smartwatch heeft dus ook geregistreerd wanneer Jan zijn hart stopte met kloppen. 
/// Het tijdstip is niet accuraat, want de smartwatch deed willekeurig een hartslagmeting. 
/// Uit de gegevens zouden we dus kunnen achterhalen wanneer Jan nog een hartslag had en wanneer niet.
/// Echter, wanneer we deze twee tijdstippen ophalen, blijkt dat het formaat van elk tijdstip anders. 
/// Kan jij het ontcijferen?
/// 
/// TECHNISCH
/// Via een API call krijg je twee tijdstippen die elks willekeurig geformatteerd zijn. 
/// De bedoeling is dat je deze twee tijdstippen omvormt naar een bruikbaar formaat en tussen deze twee tijdstippen het verschil in seconden ophaalt. 
/// Dit verschil stuur je door als antwoord naar de bijhorende API endpoint.
/// Je krijgt een Sample API endpoint (/api/path/2/easy/Sample) om je algoritme op toe te passen.
/// Als je algoritme werkt, kan je overgaan naar de Puzzle API endpoint (/api/path/2/easy/Puzzle) om het antwoord door te sturen.
/// Let op! Indien je een verkeerd antwoord geeft, wordt de opdracht gereset.

/// De input komt als een object terug dus maken we een klasse om het te deserializen
/// Om onze property namen met hoofdletter te kunnen gebruiken zetten we de PropertyNameCaseInsensitive op true
var puzzle = JsonSerializer.Deserialize<Puzzle>(Data.Puzzle, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

var firstDate = ConvertToDate(puzzle.Date1);
var secondDate = ConvertToDate(puzzle.Date2);

DateTime ConvertToDate(string mixedDate)
{
    var year = GetNumbersWithRegex(mixedDate, "YYYY", 4);
    var month = GetNumbersWithRegex(mixedDate, "MM", 2);
    var day = GetNumbersWithRegex(mixedDate, "DD", 2);
    var hours = GetNumbersWithRegex(mixedDate, "hh", 2);
    var minutes = GetNumbersWithRegex(mixedDate, "mm", 2);
    var seconds = GetNumbersWithRegex(mixedDate, "ss", 2);

    return new DateTime(year, month, day, hours, minutes, seconds);

    /// Deze methode neemt een text om in te zoeken, de text die gezogt moet worden en het aantal cijfers dat je wilt hebben
    int GetNumbersWithRegex(string searchText, string textToFind, int numberLength)
    {
        /// Maken een Regex object
        var textRegex = new Regex($"\\d{{{numberLength}}}" + textToFind);
        /// Zoeken de text voor een match en slagen de value op
        var match = textRegex.Match(searchText).Value;
        /// Indien er geen match gevonden is geven we 0 terug, als het wel gevonden is converten we de nummers en sturen we ze terug
        /// match[..numberLength] is een korte notatie voor match[Range.EndAt(2)]
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges
        return match == string.Empty ? 0 : Convert.ToInt32(match[..numberLength]);
    }
}

/// We nemen het verschil van de twee datums en zetten dat om naar de absolute waarde
var difference = Math.Abs((firstDate - secondDate).TotalSeconds);

Console.WriteLine($"Answer: {difference}");

public class Puzzle
{
    public string Date1 { get; set; }
    public string Date2 { get; set; }
}