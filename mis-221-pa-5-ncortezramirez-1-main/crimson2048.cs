using System;

namespace mis_221_pa_5_ncortezramirez_1
{
    public class Game2048
    {
        private int[] board;
        private int size = 4;
        private Random random;
        private int score = 0;
        private const int DISCOUNT_TARGET = 100; // Low score for demo purposes!

        public Game2048()
        {
            board = new int[size * size];
            random = new Random();
            AddNewTile();
            AddNewTile();
        }

        private int GetIndex(int row, int col) => row * size + col;

        private void AddNewTile()
        {
            int[] emptyCells = new int[size * size];
            int emptyCount = 0;
            for (int i = 0; i < size * size; i++)
            {
                if (board[i] == 0) emptyCells[emptyCount++] = i;
            }
            if (emptyCount > 0)
            {
                int value = (random.Next(10) < 9) ? 2 : 4;
                board[emptyCells[random.Next(emptyCount)]] = value;
            }
        }

        private void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine("=== CRIMSON LOYALTY CHALLENGE ===");
            Console.WriteLine($"Score: {score} / Goal: {DISCOUNT_TARGET}\n");
            Console.WriteLine("┌─────┬─────┬─────┬─────┐");
            for (int i = 0; i < size; i++)
            {
                Console.Write("│");
                for (int j = 0; j < size; j++)
                {
                    int val = board[GetIndex(i, j)];
                    Console.Write(val == 0 ? "     │" : $"{val,4} │");
                }
                Console.WriteLine();
                if (i < size - 1) Console.WriteLine("├─────┼─────┼─────┼─────┤");
            }
            Console.WriteLine("└─────┴─────┴─────┴─────┘");
            Console.WriteLine("Arrows to Move. Q to Quit.");
        }

        // Returns TRUE if discount won, FALSE otherwise
        public bool PlayForDiscount()
        {
            while (true)
            {
                DrawBoard();
                if (score >= DISCOUNT_TARGET)
                {
                    Console.WriteLine("\n🎉 TOUCHDOWN! You won the 10% discount!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return true;
                }
                
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) return false;

                Direction? dir = key switch
                {
                    ConsoleKey.UpArrow => Direction.Up,
                    ConsoleKey.DownArrow => Direction.Down,
                    ConsoleKey.LeftArrow => Direction.Left,
                    ConsoleKey.RightArrow => Direction.Right,
                    _ => null
                };

                if (dir.HasValue && Move(dir.Value)) AddNewTile();
                
                if (IsGameOver())
                {
                    Console.WriteLine("\nGame Over. No discount this time!");
                    Console.ReadKey();
                    return false;
                }
            }
        }

        // --- GAME LOGIC HELPERS ---
        private bool Move(Direction direction)
        {
            int[] oldBoard = (int[])board.Clone();
            bool moved = false;

            if (direction == Direction.Left || direction == Direction.Right)
            {
                for (int i = 0; i < size; i++)
                {
                    int[] row = new int[size];
                    for (int j = 0; j < size; j++) row[j] = board[GetIndex(i, j)];
                    int[] newRow = (direction == Direction.Left) ? MergeLine(row) : ReverseArray(MergeLine(ReverseArray(row)));
                    for (int j = 0; j < size; j++) board[GetIndex(i, j)] = newRow[j];
                }
            }
            else
            {
                for (int j = 0; j < size; j++)
                {
                    int[] col = new int[size];
                    for (int i = 0; i < size; i++) col[i] = board[GetIndex(i, j)];
                    int[] newCol = (direction == Direction.Up) ? MergeLine(col) : ReverseArray(MergeLine(ReverseArray(col)));
                    for (int i = 0; i < size; i++) board[GetIndex(i, j)] = newCol[i];
                }
            }

            for (int i = 0; i < board.Length; i++) if (board[i] != oldBoard[i]) moved = true;
            return moved;
        }

        private int[] MergeLine(int[] line)
        {
            int[] result = new int[size];
            int pos = 0;
            int[] nonZero = new int[size];
            int count = 0;

            foreach (int num in line) if (num != 0) nonZero[count++] = num;

            for (int i = 0; i < count; i++)
            {
                if (i < count - 1 && nonZero[i] == nonZero[i + 1])
                {
                    result[pos++] = nonZero[i] * 2;
                    score += nonZero[i] * 2;
                    i++;
                }
                else result[pos++] = nonZero[i];
            }
            return result;
        }

        private int[] ReverseArray(int[] arr)
        {
            Array.Reverse(arr);
            return arr;
        }

        private bool IsGameOver()
        {
            if (Array.IndexOf(board, 0) != -1) return false;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = board[GetIndex(i, j)];
                    if (j < size - 1 && val == board[GetIndex(i, j + 1)]) return false;
                    if (i < size - 1 && val == board[GetIndex(i + 1, j)]) return false;
                }
            }
            return true;
        }

        enum Direction { Up, Down, Left, Right }
    }
}