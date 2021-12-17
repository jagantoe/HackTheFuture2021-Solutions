using HackTheFuture2021_A3_Hard;
using System.Text.Json;

/// OPDRACHT
/// We betreden de woning en komen in een kamer terecht waar elke tegel een pijl heeft.
/// Om aan de andere kant van de kamer te geraken, moet je een pad over de tegels vinden aan de hand van de pijlen.
/// De bedoeling is om een pad te vinden dat over elke tegel gaat voordat je op het eindpunt komt. 
/// We mogen over tegels heen stappen (dus je moet niet de eerstvolgende tegel bewandelen).
/// In de verte zien we aan de andere kant van de kamer een bebloed object liggen. 
/// Kan jij aan de overkant geraken en het object onderzoeken?
/// 
/// TECHNISCH
/// Via een API call krijg je een raster waarop het start-(linksboven) en eindpunt(rechtsonder) is aangeduid. 
/// Elk vakje (exclusief het eindpunt) heeft een pijl in een bepaalde richting.
/// De richting zijn: horizontaal, verticaal en diagonaal.
/// Elk vakje heeft een ID. 
/// De ID’s verlopen van linksboven tot rechtsonder.
/// Je houdt een lijst bij van elk vakje zijn ID, in volgorde van bewandeling.
/// Deze lijst stuur je door als antwoord.
/// Je krijgt een Sample API endpoint (/api/path/2/hard/Sample) om je algoritme op toe te passen. 
/// Als je algoritme werkt, kan je overgaan naar de Puzzle API endpoint (/api/path/2/hard/Puzzle) om het antwoord door te sturen.
/// Let op! Bij het indienen van je antwoord bij de Puzzle API endpoint wordt je aantal pogingen opgeteld.
/// De opdrachtwordt niet gereset na een foutievepoging.

var puzzle = JsonSerializer.Deserialize<Puzzle>(Data.Puzzle, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

/// Om de puzzel visueel voor te stellen
DrawPuzzle();
/// Maken een tiles variabele zodat we niet altijd puzzle.Tiles moeten doen
var tiles = puzzle.Tiles;
SetPossibleJumps();

/// De setup om het gemakkelijk te maken
/// We halen de start tile eruit, altijd degene met x = 1 en y = 1
var initialTile = tiles.First(t => t.X == 1 && t.Y == 1);
/// We beginnen met 1 path dat begint op de start tile
var initialPath = new Path(initialTile);
/// We maken een lijst om alle paths in bij te houden terwijl we zoeken voor een oplossing
var paths = new List<Path>();
/// Voeg het initiele path toe aan de lijst
paths.Add(initialPath);
/// In dit voorbeeld houden we alle doodgelopen paths bij,
/// let op dat dit wel veel geheugen kan innemen dus als je dat wil vermijden kan je alleen de geldige paths
var completedPaths = new List<Path>();

/// Onze loop blijft gaan tot we alle paths hebben doorlopen (dit kan even duren),
/// voor jullie oplossing zou het genoeg zijn om te gaan tot je 1 oplossing hebt
/// Doordat we alle paths in een lijst bijhouden (heap geheugen) zijn we alleen gelimiteerd door ons werk geheugen
/// Met recursie zou je snel op een stack overflow error lopen
while (paths.Count > 0)
{
    /// We halen de eerste path in de lijst op
    var path = paths.First();
    /// We kijken wat de mogelijke volgende tiles zijn
    var possibleJumps = path.PossibleNextJumps;
    /// Als er volgende tiles zijn gaan we ze allemaal doorlopen
    if (possibleJumps.Count() > 0)
    {
        /// We gaan door alle mogelijkheiden en voegen ze toe aan de lijst
        foreach (var jump in possibleJumps)
        {
            /// Hier maken we een nieuwe path waar we de huidige path aan meegeven samen met de volgende tile
            paths.Add(new Path(path, jump));
        }
    }
    else
    {
        /// Er zijn geen verdere stappen die genomen kunnen worden dus is deze path tot zijn einde gekomen
        completedPaths.Add(path);
    }
    /// We zijn klaar met het huidige path en verwijderen die dus uit de lijst
    paths.RemoveAt(0);
}

/// Als indien je alle gevonden paths eens wilt zien
/// Disclaimer: het zijn er veel
//foreach (var path in completedPaths)
//{
//    Console.WriteLine(String.Join(",", path.PreviousTiles.Select(t => t.Id.ToString())));
//}

/// Aangezien we iedere tile 1 keer moeten bezoeken moet een geldige oplossing evenveel tiles bevatten
var size = tiles.Count;
/// We filteren alle gevonden paths om alleen de geldige paths te krijgen
var correctPaths = completedPaths.Where(p => p.PreviousTiles.Count == size);

Console.WriteLine($"Er zijn {completedPaths.Count} paths gevonden en {correctPaths.Count()} geldige paths gevonden:");
Console.ForegroundColor = ConsoleColor.Green;
foreach (var path in correctPaths)
{
    Console.WriteLine(String.Join(",", path.PreviousTiles.Select(t => t.Id.ToString())));
}

/// Stap voor stap voorstelling van een oplossing
AnimatedDrawSolution(correctPaths.First());

/// Wat je zal merken is dat bij veel van de methodes het return type void is,
/// dat is omdat ik vaak de objecten rechtstreeks aanpas en er dus geen nood is om een waarde te returnen
void SetPossibleJumps()
{
    foreach (var tile in tiles)
    {
        // Per tegel maken we een lijst die bijhoudt wat mogelijke volgende sprongen zijn van die tegel
        var jumps = new List<Tile>();
        // We voegen tijdelijk de tegel zelf toe aan de lijst
        jumps.Add(tile);
        // Gebruiken deze methode om alle tiles te setten
        SetPossibleTileJumps(jumps, tile.Direction);
        // Hier halen we die initiele tegel er weer uit
        jumps.RemoveAt(0);
        // Wijzen de mogelijke sprongen toe
        tile.PossibleJumps = jumps;
    }
}
void SetPossibleTileJumps(List<Tile> jumps, Direction direction)
{
    // We halen de laatst toegevoegde tegel op
    var tile = jumps.Last();
    Func<Tile,bool> nextTilePredicate;
    // Afhankelijk van de richting hebben we een verschillende formule nodig om de volgende tile te vinden
    switch (direction)
    {
        case Direction.Up:
            nextTilePredicate = t => t.X == tile.X && t.Y == tile.Y - 1;
            break;
        case Direction.UpRight:
            nextTilePredicate = t => t.X == tile.X + 1 && t.Y == tile.Y - 1;
            break;
        case Direction.Right:
            nextTilePredicate = t => t.X == tile.X + 1 && t.Y == tile.Y;
            break;
        case Direction.DownRight:
            nextTilePredicate = t => t.X == tile.X + 1 && t.Y == tile.Y + 1;
            break;
        case Direction.Down:
            nextTilePredicate = t => t.X == tile.X && t.Y == tile.Y + 1;
            break;
        case Direction.DownLeft:
            nextTilePredicate = t => t.X == tile.X - 1 && t.Y == tile.Y + 1;
            break;
        case Direction.Left:
            nextTilePredicate = t => t.X == tile.X - 1 && t.Y == tile.Y;
            break;
        case Direction.UpLeft:
            nextTilePredicate = t => t.X == tile.X - 1 && t.Y == tile.Y - 1;
            break;
        case Direction.Finish:
        default:
            nextTilePredicate = t => false;
            break;
    }
    // We maken een tegel voor de mogelijke volgende tile, indien er geen volgende tile is zal de waarde null zijn
    Tile? nextTile = tiles.FirstOrDefault(nextTilePredicate);
    if (nextTile != null)
    {
        // We voegen de volgende tile toe
        jumps.Add(nextTile);
        // Gebruiken recursie om steeds de volgende tile op te halen
        SetPossibleTileJumps(jumps, direction);
    }
    // Als er geen volgende tiles zijn hebben we alles en stopt de recursie
    // Waarom moeten we niets terug geven? De lijst die we aan het aanpassen zijn is buiten deze scope aangemaakt en bevat alle veranderingen
}

void DrawPuzzle()
{
    puzzle.Directions.ForEach(direction => Console.WriteLine(direction));
    for (int y = 0; y < 5; y++)
    {
        Console.WriteLine(new String('-', 56));
        Console.Write("|");
        for (int x = 0; x < 5; x++)
        {
            Console.Write(" {0,8} |",puzzle.Tiles[x + (y * 5)]);
        }
        Console.WriteLine();
    }
    Console.WriteLine(new String('-', 56));
}
void AnimatedDrawSolution(Path solution)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    Console.WriteLine("Druk op enter om een animatie te zien van de oplossing.");
    Console.ReadLine();
    var shownTiles = new List<Tile>();
    foreach (var step in solution.PreviousTiles)
    {
        shownTiles.Add(step);
        Console.Clear();
        for (int y = 0; y < 5; y++)
        {
            Console.WriteLine(new String('-', 56));
            Console.Write("|");
            for (int x = 0; x < 5; x++)
            {
                if (shownTiles.Contains(puzzle.Tiles[x + (y * 5)]))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(" {0,8} |", puzzle.Tiles[x + (y * 5)]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
        Console.WriteLine(new String('-', 56));
        Thread.Sleep(1000);
    }
}

/// Het object dat we binnen krijgen
public class Puzzle
{
    // Een lijst die uitlegt wat de directions zijn
    public List<string> Directions { get; set; }
    
    // Een lijst met alle tiles
    public List<Tile> Tiles { get; set; }
}
/// De tiles die we gebruiken
public class Tile
{
    /// Een id die uniek is per tile
    public int Id { get; set; }

    /// De x coordinaat van de tile gaande van links naar rechts
    public int X { get; set; }

    /// De y coordinaat van de tile gaande van boven naar onder
    public int Y { get; set; }

    /// De richting die de tile aanwijst
    public Direction Direction { get; set; }

    /// Een lijst van mogelijke sprongen vanuit de huidige tile
    /// Vullen we zelf op aan het begin van het programma
    public List<Tile> PossibleJumps { get; set; }

    /// We overriden de ToString voor de draw methode
    public override string? ToString()
    {
        return Direction.ToString();
    }
}
/// Een enum om de directions iets makkelijk te maken om mee te werken
public enum Direction
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft,
    Finish
}
/// Dit gebruiken we om een path voor te stellen
public class Path
{
    /// Een lijst met alle tiles die we al hebben bezocht in dit path
    public List<Tile> PreviousTiles { get; set; }

    /// De huidige tile van het path is altijd de laatste, maken gebruik van expression bodied members
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members
    public Tile CurrentTile => PreviousTiles.Last();
    
    /// Een lijst van mogelijke volgende sprongen voor dit path
    /// We weten onze huidige tile en die heeft een aantal mogelijke volgende tiles,
    /// het kan zijn dat we sommige van die tiles al bezocht hebben dus filteren we de lijst met onze PreviousTiles
    public IEnumerable<Tile> PossibleNextJumps => CurrentTile.PossibleJumps.Where(t => !PreviousTiles.Contains(t));
    
    /// Een constructor voor het initiele path
    public Path(Tile startTile)
    {
        PreviousTiles = new List<Tile>();
        PreviousTiles.Add(startTile);
    }
    /// Een constructor waarmee we een bestaand path kunnen overnemen met een volgende stap
    public Path(Path path, Tile nextTile)
    {
        // Per pad moeten we een nieuwe lijst maken anders verwijzen alle paths naar dezelfde lijst
        PreviousTiles = new List<Tile>(path.PreviousTiles);
        PreviousTiles.Add(nextTile);
    }
}