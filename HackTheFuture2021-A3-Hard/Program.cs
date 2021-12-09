/// <summary>
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
/// </summary>
Console.WriteLine();
