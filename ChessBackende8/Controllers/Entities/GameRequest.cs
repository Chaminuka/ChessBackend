namespace ChessBackende8.Controllers.Entities  // Namespace anpassen, falls nötig
{
    public class NewGameRequest
    {
        public string GamePgn { get; set; }  // Schachpartie im PGN-Format
        public string Name { get; set; }     // Benutzername oder Spielname
    }
}
