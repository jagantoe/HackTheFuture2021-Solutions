/// <summary>
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
/// De opdrachtwordt niet gereset na een foutievepoging.
/// </summary>

Console.WriteLine();

