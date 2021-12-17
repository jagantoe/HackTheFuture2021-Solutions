using HackTheFuture2021_A1_Easy;
using System.Text.Json;

/// OPDRACHT
/// De oude badge ontcijferen lukt niet zo gemakkelijk. 
/// De methode waarop Involved nu de codes genereert en ontcijfert is niet meer zoals vroeger.
/// Een oud-werknemer herinnert zich nog de oude methode: een optelmethode die je bij het technisch gedeelte zal ontdekken. 
/// De oud-werknemer wist ook waarom deze methode achterhaald was: 
/// het resultaat van de ontcijfering is maar één cijfer en kan niet meer gebruikt worden in een bedrijf met meer dan negen personeelsleden.
/// Kan jij het ontcijferen?
/// 
/// TECHNISCH
/// Via een API call krijg je een lijst met getallen. 
/// Elk getal tel je met elkaar op. 
/// Hierna tel je elk cijfer van de som met elkaar op. 
/// Als de som hiervan meer dan één cijfer bevat, doe je de vorige stap opnieuw. 
/// Dit doe je tot je één cijfer bekomt.
/// Dit cijfer stuur je door naar de bijhorende API endpoint.
/// Je krijgt een Sample APIendpoint (/api/path/1/easy/Sample) om je algoritme op toe te passen. 
/// Als je algoritme werkt, kan je overgaan naar de Puzzle API endpoint (/api/path/1/easy/Puzzle) om het antwoord door te sturen.
/// Let op! Indien je een verkeerd antwoord geeft, wordt de opdracht gereset.

var numbers = JsonSerializer.Deserialize<List<double>>(Data.Puzzle);

/// Alle getallen in de list optellen optellen
var total = numbers.Sum();

/// De loop blijft gaan tot ons totaal maar 1 character bevat
while (total.ToString().Length > 1)
{
    /// ToString maakt van het totaal een string, een string is een collectie van characters
    /// Met de Select kunnen we alle elementen in een lijst omzetten naar een nieuwe vorm, in dit geval een lijst van doubles
    /// Overview: double => list<char> => list<doubles>
    var digits = total.ToString().Select(x => Char.GetNumericValue(x));

    /// De lijst van cijfers tellen we dan op voor ons nieuwe totaal
    total = digits.Sum();
}

Console.WriteLine($"Answer: {total}");

/// Anderzijds als je modulo 9 van het totaal neemt komt je ook op het antwoord uit
/// Als je 0 krijgt is het antwoord 9
var alternative = numbers.Sum() % 9;
if (alternative == 0) alternative = 9;
Console.WriteLine($"Alternative: {alternative}");

/// Extra informatie https://en.wikipedia.org/wiki/Digital_root