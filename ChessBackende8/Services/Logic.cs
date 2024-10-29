using Chess;
using System.Runtime.ConstrainedExecution;



namespace ChessBackende8.Services



{
    public class Logic
    {

        
        public List<string> GetFens(string pgn)
        {
            string formattedPgn = PgnFormat(pgn);
            var moves = PgnToMoves(formattedPgn);
            var movesFinal = SplitMoves(moves);
            var fens = PgnToFen(movesFinal, new ChessBoard());
            var fensFinal = new List<string>();
            foreach(var fen in fens)
            {
                
                fensFinal.Add(FenNormalizer(fen));
            }
            return fensFinal;
        }

        public string FenNormalizer(string fen)
        {
            string ret = string.Empty;
            int c = 0;
            for(int i = 0; i < fen.Length; i++)
            {
                if(fen[i] == ' ')
                {
                    c++;
                }
                if(c == 3)
                {
                    ret = fen.Substring(i);
                    if (ret.Any(char.IsLetter))
                    {
                        c--;
                    }
                    else
                    {
                        ret += fen.Substring(0, i);
                    }
                }

            }
            return ret;
        }

        private string Cutter(string move)
        {
            int i = 0;
            int c = 0;

            foreach (var letter in move)
            {

                if (int.TryParse(letter + "", out int j))
                {
                    c++;
                    if (c > 1)
                    {
                        return move.Substring(0, i).Trim();
                    }
                }
                else
                {
                    if (c > 0)
                    {
                        c = 0;
                    }
                }
                i++;
            }
            return move.Trim();
        }
        private string PgnFormat(string pgn)
        {
            string ret = string.Empty;
            string final = string.Empty;
            
            for (int i = 0; i < pgn.Length; i++)
            {
                if (pgn[i] == '1' && pgn[i + 1] == '.')
                {
                    ret = pgn.Substring(i + 2, pgn.Length - (i + 2));
                    break;
                }
            }
            for(int i = 0; i < ret.Length; i++)
            {
                if (ret[i] == '{')
                {
                    while (ret[i] != '}')
                    {
                        i++;
                    }

                }
                else
                {
                    final += ret[i];
                    
                }
            }


            
            return final;

        }

        private List<string> SplitMoves(List<string> moves)
        {
            var ret = new List<string>();
            foreach (var move in moves)
            {
                var moveFinal = Cutter(move);

                
                
                for (int i = 0; i < moveFinal.Length; i++)
                {
                    if(moveFinal[i] == ' ')
                    {
                        string second = moveFinal.Substring(i + 1).Replace(" ", "");
                        int c = 0;
                        for(int j = 0; j < second.Length; j++)
                        {
                            if (int.TryParse(second[j] + "", out int k))
                            {
                                c++;
                                if(c > 1)
                                {
                                    second = second.Substring(0, j);
                                    break;
                                }
                                if (second[j-1] == 'O' || second[j-1] == '+')
                                {
                                    
                                    second = second.Substring(0, j);
                                    break;
                                }
                            }

                        }

                        ret.Add(moveFinal.Substring(0, i));
                        ret.Add(second);
                        break;
                    }
                    
                    
                    
                }
            }
            return ret;
        }
        private List<string> PgnToMoves(string pgn)
        {
            var moves = new List<string>();
            string move = string.Empty;
            string temp = pgn;
            int len = 0;
            for (int i = 0; i < pgn.Length; i++)
            {
                if (pgn[i] == '.')
                {

                    move = temp.Substring(0, len - 1);
                    temp = temp.Substring(len + 1);
                    len = 0;
                    i++;
                    moves.Add(move);
                }
                len++;
            }

            return moves;
        }


        private List<string> PgnToFen(List<string> moves, ChessBoard board)
        {

            var ret = new List<string>();
            ret.Add(board.ToFen());
            foreach (var move in moves)
            {


                if (!int.TryParse(move[0] + "", out int j))
                {
                    board.Move(move);
                }
                
                ret.Add(board.ToFen());

            }
            return ret;



        }


    }

}

