using HackTheFuture2021_B3_Hard;
using System.Text.Json;

/// OPDRACHT
/// Jan liet niets aan het blote oog over. 
/// We hebben enkele geëncrypteerde woorden gevonden en ook een raster van een heleboel letters.
/// Een senior developer binnen Involved wist ons te vertellen dat Jan 
/// de laatste weken voor zijn dood onderzoek heeft gedaan naar Caesar Cipher.
/// Bij deze techniek verschuif je alle letters van een woord met een bepaalde hoeveelheid. 
/// Kan jij de woorden omvormen naar een leesbaar formaat en kan je een verband zoeken in het raster?
/// 
/// TECHNISCH
/// Deze opdracht bestaat uit twee delen:
/// Je begint met het eerste deel, hier zal je een lijst van woorden krijgen die geëncrypteerd zijn met een onbekende verschuiving. 
/// Deze woorden moet je omvormen naar bestaande woorden. 
/// Hiernaast krijg je ook een raster met letters. 
/// Zoek de gevonden woorden in dat raster en stuur een lijst van de overgebleven letters terug 
/// als antwoord (in volgorde van ID) naar de bijhorende SampleAPI endpoint.
/// Bij het laatste deel zal je hetzelfde moeten doen als bij het eerste deel maar deze 
/// keer is ook het raster geëncrypteerd met een onbekende verschuiving. 
/// Zoek de gevonden woorden in het raster. 
/// Daarna stuur je een lijst met alle overgebleven letters van het originele raster terug 
/// als antwoord (in volgorde van ID) naar de bijhorende Puzzle API endpoint.
/// Deze lijst vormt ook de zin die het motief van de moord bevat!

var puzzle = JsonSerializer.Deserialize<Puzzle>(Data.Puzzle, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

var cypheredWords = puzzle.CipheredWords;
/// De eerste stap is om de verschuiving van de woorden te zoeken
/// Aangezien alle woorden met dezelfde hoeveelheid vershoven zijn kunnen we al mogelijke verschuivingen printen en manueel zien wat de juiste is
//for (int i = 1; i <= 25; i++)
//{
//    Console.WriteLine($"Shift {i,2}: {cypheredWords[0]} => {cypheredWords[0].CypherString(i)}");
//}

/// In dit geval is de verschuiving 20
var decryptedWords = cypheredWords.Select(x => x.CypherString(20)).ToList();
decryptedWords.ForEach(direction => Console.Write($"{direction}|"));

/// We houden de originele lijst van tiles bij
var originalTiles = puzzle.Grid;
/// Een visuele voorstelling van de grid
DrawPuzzle(originalTiles);

/// Door de Tile class een record te maken kunnen we gebruik maken van het with keywoord om een kopie te maken (we willen de originele objecten niet aanpassen)
/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression
var shiftedTiles = originalTiles.Select(x => x with { }).ToList();
/// We setten de neighbors van alle tiles
SetNeighbors(shiftedTiles);

/// Om de gevonde antwoorde bij te houden gebruiken we een tuple, zo kunnen we meer informatie bijhouden zonder een class te maken
/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
var foundWords = new List<(string word, int shift, List<Tile> tiles)>();

/// De raster verschuiving is te moeilijk om manueel te zien dus moeten we alle mogelijkheiden overlopen
for (int i = 1; i < 26; i++)
{
    /// We verschuiven alle tiles met 1
    shiftedTiles.ForEach(x => x.Content = x.Content.CypherLetter(1));
    // We zoeken alle woorden in het raster
    foreach (var word in decryptedWords)
    {
        /// We halen alle tiles op die beginnen met de eerste letter van het woord
        var possibleTiles = shiftedTiles.Where(x => word.StartsWith(x.Content));

        /// We overlopen alle tiles
        foreach (var tile in possibleTiles)
        {
            /// We kijken of de tile het antwoord heeft
            var result = CheckWord(tile, word);
            /// Als het result niet null is hebben we het woord gevondens
            if (result != null)
            {
                /// We voegen het gevonden woord + de shift index + de lijst met de tiles toe aan onze foundWords lijst
                foundWords.Add(new (word, i, result));
                break;
            }
        }
    }
    /// Als we alle woorden gevonden hebben kunnen we de loop stoppen
    if (foundWords.Count == decryptedWords.Count)
    {
        break;
    }
}

/// We zetten onze lijst met oplossingen om naar een lijst van tiles
/// Dat kunnen we nog vereenvoudigen door alleen de id's over te laten 
var tilesUsed = foundWords.SelectMany(x => x.tiles).Select(x => x.Id);

/// In de opdracht staat dat we alle niet gebruikte letters moeten verzamelen en sorteren op id
var solutionTiles = originalTiles.Where(x => !tilesUsed.Contains(x.Id)).OrderBy(x => x.Id);

/// Zie jij de geheime zin?
foreach (var tile in solutionTiles)
{
    Console.Write($"{tile.Content} ");
}

List<Tile>? CheckWord(Tile tile, string word)
{
    /// We overlopen alle mogelijke richtingen beginnend vanuit de doorgegeven tile
    foreach (var direction in Extension.GetValues<Direction>())
    {
        var result = CheckDirection(tile, word, direction);
        /// Als het result niet null is hebben het woord gevonden
        if (result != null)
        {
            /// Als we het woord gevonden hebben geven we de lijst terug
            return result;
        }
    }
    /// Indien het woord niet gevonden is geven we null terug
    return null;

    List<Tile>? CheckDirection(Tile tile, string word, Direction direction)
    {
        /// We houden een lijst bij van alle tiles die gematch worden
        var matchingTiles = new List<Tile>();
        /// We beginnen met de tile die is doorgegeven
        var nextTile = tile;
        /// We gaan doorheen het woord en kijken of alle letters overeenkomen
        for (int i = 0; i < word.Length; i++)
        {
            /// We kijken of de tile content hetzelfde is als de letter van het woord
            if (nextTile?.Content == word[i])
            {
                matchingTiles.Add(nextTile);
                /// De volgende tile wordt geupdate
                nextTile = nextTile.Neighbors[direction];
            }
            else
            {
                /// Zodra er 1 letter niet overeenkomt geven we null terug
                return null;
            }
        }
        /// Als de code hier geraakt betekent het dat alle letters overeenkomen en kunnen we dus de lijst met tiles terug geven
        return matchingTiles;
    }
}



/// Set all tiles rondom iedere tile
void SetNeighbors(List<Tile> tiles)
{
    foreach (var tile in tiles)
    {
        var neighbors = new Dictionary<Direction, Tile>();
        // We voegen alle naburige tiles toe, indien er geen buur tile is zal de waarde null zijn
        neighbors.Add(Direction.Up, tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1));
        neighbors.Add(Direction.UpRight, tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y - 1));
        neighbors.Add(Direction.Right, tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y));
        neighbors.Add(Direction.DownRight, tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y + 1));
        neighbors.Add(Direction.Down, tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1));
        neighbors.Add(Direction.DownLeft, tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y + 1));
        neighbors.Add(Direction.Left, tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y));
        neighbors.Add(Direction.UpLeft, tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y - 1));
        tile.Neighbors = neighbors;
    }
}

/// Visualiseert het woordraster
void DrawPuzzle(List<Tile> tiles)
{
    Console.WriteLine();
    for (int y = 0; y < 15; y++)
    {
        Console.WriteLine(new String('-', 61));
        Console.Write("|");
        for (int x = 0; x < 15; x++)
        {
            Console.Write($" {tiles[x + (y * 15)].Content} |");
        }
        Console.WriteLine();
    }
    Console.WriteLine(new String('-', 61));
}

/// Het object dat we binnen krijgen
public record Puzzle
{
    /// De lijst van geencrypteerde woorden
    public List<string> CipheredWords { get; set; }
    /// De lijst van tiles
    public List<Tile> Grid { get; set; }
}
/// De tiles die we gebruiken
public record Tile
{
    /// Een id die uniek is per tile
    public int Id { get; set; }

    /// De x coordinaat van de tile gaande van links naar rechts
    public int X { get; set; }

    /// De y coordinaat van de tile gaande van boven naar onder
    public int Y { get; set; }

    /// De letter van de tile
    public char Content { get; set; }

    // Alle tiles rondom de tile aan de hand van hun direction
    public Dictionary<Direction, Tile> Neighbors { get; set; }
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
    UpLeft
}
/// Een static Extension class zodat we handige extension methodes kunnen maken voor string en char
/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
public static class Extension
{
    /// Een lijst met alle letters
    public static List<char> Alphabet = Enumerable.Range('a', 26).Select(x => (char)x).ToList();
    
    /// Een methode om een string te shiften met een bepaald aantal
    public static string CypherString(this string word, int shift)
    {
        /// In geval dat de shift groter is dan 26
        shift = shift % 26;
        /// Met de Select kunnen we over ieder character gaan en het shiften
        /// Dat geeft ons een char[] terug die we dan terug aan elkaar kunnen plakken met string.Join
        return string.Join("", word.Select(l => l.CypherLetter(shift)));
    }

    /// Een method om een char te shiften met een bepaald aantal
    public static char CypherLetter(this char character, int shift)
    {
        /// In geval er een overshoot is beginnen we terug van het begin
        var index = (Alphabet.IndexOf(character) + shift) % 26;
        return Alphabet[index];
    }

    /// Een methode om een lijst van alle opties in een enum te krijgen, T kan eender welke enum zijn
    /// https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/generics
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}