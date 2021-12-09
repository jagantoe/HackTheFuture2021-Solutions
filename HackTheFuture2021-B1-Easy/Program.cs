/// <summary>
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
/// </summary>
Console.WriteLine();
