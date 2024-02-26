using System;
using System.Numerics;
using System.Reflection.PortableExecutable;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        if(direction == 'U')
            Position.Y = Math.Max(0, Position.Y - 1);
        else if (direction == 'D')
            Position.Y = Math.Min(5, Position.Y + 1);
        else if (direction == 'L')
            Position.X = Math.Max(0, Position.X - 1);
        else if (direction == 'R')
            Position.X = Math.Min(5, Position.X + 1);

        
    }
}

class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

class Board
{
    public Cell[,] Grid { get; }

    public Player Player1 { get; }
    public Player Player2 { get; }

    public Board()
    {
        Grid = new Cell[6, 6];

        // Initialize players
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        Grid[Player1.Position.Y, Player1.Position.X] = new Cell("P1");
        Grid[Player2.Position.Y, Player2.Position.X] = new Cell("P2");

        // Place gems and obstacles randomly
        PlaceElements('G', 5);  // 5 gems
        PlaceElements('O', 6);  // 6 obstacles

        
    }

    private void PlaceElements(char element, int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = random.Next(0, 6);
                y = random.Next(0, 6);
            } while (Grid[y, x] != null);

            Grid[y, x] = new Cell(element.ToString());
        }
    }

    public void Display()
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Console.Write(Grid[y, x]?.Occupant ?? "-");
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        if (direction == 'U') newY = Math.Max(0, newY - 1);
        else if (direction == 'D') newY = Math.Min(5, newY + 1);
        else if (direction == 'L') newX = Math.Max(0, newX - 1);
        else if (direction == 'R') newX = Math.Min(5, newX + 1);

        if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newY, newX]?.Occupant != "O")
            return true;

      

        return false;


    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.Y, player.Position.X]?.Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.Y, player.Position.X] = null;
        }
    }
}

class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    

    public Game()
    {
        Board = new Board();
        Player1 = Board.Player1;
        Player2 = Board.Player2;
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns == 30;
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
            Console.WriteLine("Player 1 wins! "+"Total gems Collected by Player 1 are: "+ Player1.GemCount);
        else if (Player1.GemCount < Player2.GemCount)
            Console.WriteLine("Player 2 wins! "+ "Total gems Collected by Player 2 are: "+ Player2.GemCount);
        else
            Console.WriteLine("It's a Tie!"+"\nBoth Players collected same Gems: "+ Player1.GemCount);
    }

    public void Start()
    {
        
        while (!IsGameOver())
        {
            Board.Display();
            Console.Write("\nCurrentTurn: " + TotalTurns + "\n");
            Console.Write($"\nIt's {CurrentTurn.Name}'s turn.\n");
            Console.WriteLine("\nEnter movement (U for Up movement / D for Down movement/ L for Left movement/ R for right movement): ");
            char move = char.ToUpper(Console.ReadKey().KeyChar);
            char[] chars = CurrentTurn.Name.ToCharArray();
           


            if (Board.IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
             

                Board.CollectGem(CurrentTurn);
                TotalTurns++;
                SwitchTurn();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\nInvalid move. Try again.");
            }
        }

        Console.Clear();
        Board.Display();
        AnnounceWinner();
    }
}

class Program
{
    
    static void Main()
    {
        Game gemHuntersGame = new Game();
        gemHuntersGame.Start();
    }
}