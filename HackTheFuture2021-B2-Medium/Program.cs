/// <summary>
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
/// </summary>
