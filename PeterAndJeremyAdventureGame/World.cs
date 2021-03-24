using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PeterAndJeremyAdventureGame
{
    public class World
    {
        private string[,] Grid;
        private int Rows;
        private int Columns;

        public World(string[,] grid)
        {
            Grid = grid;
            Rows = grid.GetLength(0);
            Columns = grid.GetLength(1);
        }

        public void Draw()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    string element = Grid[y, x];
                    SetCursorPosition(x, y);

                    if (element == "┌" || element == "└" || element == "┐" || element == "┘" || element == "│" || element == "─" || element == "┴" || element == "┬" || element == "┤" || element == "├")
                    {
                        ForegroundColor = ConsoleColor.Red;
                    }
                    //else if (element == "%")
                    //{
                    //    ForegroundColor = ConsoleColor.Black;
                    //}
                    else
                    {
                        ForegroundColor = ConsoleColor.White;
                    }

                    Write(element);
                }
            }
        }

        public bool IsPositionWalkable(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Columns || y >= Rows)
            {
                return false;
            }

            return Grid[y, x] == " " || Grid[y, x] == "%";
        }

        public string GetElementAt(int x, int y)
        {
            return Grid[y, x];
        }

        public void DeleteElementAtLocation(int x, int y)
        {
            Grid[y, x] = " ";
        }
    }
}
