using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVMSudoku
{
    //Отдельный класс для генерации поля судоку. Вынес, потому что класс модели представления и так уже оказался излишне забит
    public class SudokuBoard
    {
        private int[,] board;

        public int[,] Board
        {
            get { return board; }
            set { board = value; }
        }

        public SudokuBoard()
        {
        }

        public int[,] GetBoard()
        {
            Board = new int[9, 9];
            FillCells(0, 0);
            return Board;

        }

        private bool FillCells(int x, int y)
        {
            int nextX = x;
            int nextY = y;
            int[] row = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Random r = new Random();
            int tmp = 0;
            int current = 0;

            for (int i = row.Length - 1; i > 0; i--)
            {
                current = r.Next(i);
                tmp = row[current];
                row[current] = row[i];
                row[i] = tmp;
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (IsLegalMove(x, y, row[i]))
                {
                    Board[x, y] = row[i];
                    if (x == 8)
                    {
                        if (y == 8)
                            return true;
                        else
                        {
                            nextX = 0;
                            nextY = y + 1;
                        }
                    }
                    else
                    {
                        nextX = x + 1;
                    }
                    if (FillCells(nextX, nextY))
                        return true;
                }
            }
            Board[x, y] = 0;
            return false;
        }

        private bool IsLegalMove(int x, int y, int current)
        {
            for (int i = 0; i < 9; i++)
            {
                if (current == Board[x, i])
                    return false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (current == Board[i, y])
                    return false;
            }
            int cornerX = 0;
            int cornerY = 0;
            if (x > 2)
                if (x > 5)
                    cornerX = 6;
                else
                    cornerX = 3;
            if (y > 2)
                if (y > 5)
                    cornerY = 6;
                else
                    cornerY = 3;
            for (int i = cornerX; i < 10 && i < cornerX + 3; i++)
                for (int j = cornerY; j < 10 && j < cornerY + 3; j++)
                    if (current == Board[i, j])
                        return false;
            return true;
        }
    }
}
