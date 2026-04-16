using System;
using System.Collections.Generic;

namespace ZBSnake.Controller
{
    public class Movement
    {
        private LinkedList<(int row, int col)> snakeBody = new LinkedList<(int, int)>();
        private Random rng = new Random();

        public int Score { get; private set; } = 0;
        public bool IsGameOver { get; private set; } = false;

        private int rows;
        private int cols;

        /// <summary>
        /// Publikus hozzáférés a kígyó testéhez a rajzoláshoz.
        /// </summary>
        public LinkedList<(int row, int col)> SnakeBody => snakeBody;

        public Movement(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }

        /// <summary>
        /// Elhelyezi a kígyót és az almát a pályán az inicializáláskor.
        /// </summary>
        public void InitSnake(int[,] map, int startRow, int startCol)
        {
            snakeBody.Clear();

            // 5 hosszú kígyó
            for (int i = 0; i < 3; i++)
            {
                int col = startCol - i; // balra építjük fel
                snakeBody.AddLast((startRow, col));
                map[startRow, col] = 1;
            }
        }

        /// <summary>
        /// Mozgatja a kígyót a megadott irányba.
        /// Visszatér true-val, ha evett almát (gyorsítás kell).
        /// </summary>
        public bool MoveSnake(int[,] map, ZBSnake.Direction direction)
        {
            if (IsGameOver) return false;

            var head = snakeBody.First.Value;
            int newRow = head.row;
            int newCol = head.col;

            switch (direction)
            {
                case ZBSnake.Direction.Up: newRow--; break;
                case ZBSnake.Direction.Down: newRow++; break;
                case ZBSnake.Direction.Left: newCol--; break;
                case ZBSnake.Direction.Right: newCol++; break;
            }

            // Fal ütközés vizsgálat
            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
            {
                IsGameOver = true;
                return false;
            }

            int cellValue = map[newRow, newCol];

            // Önmagába ütközés vizsgálat
            if (cellValue == 1)
            {
                IsGameOver = true;
                return false;
            }

            bool ateApple = (cellValue == 2);

            // Új fej hozzáadása
            snakeBody.AddFirst((newRow, newCol));
            map[newRow, newCol] = 1;

            if (ateApple)
            {
                Score++;
                // Almánál NEM töröljük a farkat (nő a kígyó)
                SpawnApple(map);
                return true;
            }
            else
            {
                // Farok törlése
                var tail = snakeBody.Last.Value;
                snakeBody.RemoveLast();
                map[tail.row, tail.col] = 0;
                return false;
            }
        }

        /// <summary>
        /// Véletlenszerű helyre rak egy új almát (ahol nincs kígyó).
        /// </summary>
        public void SpawnApple(int[,] map)
        {
            int r, c;
            do
            {
                r = rng.Next(0, rows);
                c = rng.Next(0, cols);
            } while (map[r, c] != 0);

            map[r, c] = 2;
        }
    }
}