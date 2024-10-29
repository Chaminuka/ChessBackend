namespace ChessBackende8.Controllers.Entities
{
    public class GameMe
    {
        public int ID { get; set; } 
        public bool isWhite { get; set; }  = false;
        public bool whiteWon { get; set; } = false;
        public string positions { get; set; } = string.Empty;
        public List<string> currPosition { get; set; }  = new List<string>();

    }
}
