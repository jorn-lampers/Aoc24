using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24
{
    internal class Day6
    {
        string[] Grid;

        public Day6()
        {
            Grid = File.ReadAllLines("input6");
        }

        public void Run()
        {
            int[] guardPos = FindGuardPos(Grid);

            while (!MoveGuard1(Grid))
            { }

            int visitedTileCount = Grid
                .SelectMany(x => x)
                .Count(x => x == 'X');

            Console.WriteLine($"Guard visited {visitedTileCount} tiles");
        }

        /// <summary>
        /// If the guard leaves the grid, return true, otherwise return false
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Whether or not the guard left the grid</returns>
        public bool MoveGuard1(string[] grid)
        {
            var guardPos = FindGuardPos(grid);
            var nextPos = FindNextPos(guardPos, grid);

            if (grid.Length <= nextPos[1] || grid[0].Length <= nextPos[0] || nextPos[0] < 0 || nextPos[1] < 0)
            {
                grid[guardPos[1]] = grid[guardPos[1]].Remove(guardPos[0], 1).Insert(guardPos[0], "X");
                return true;
            }

            char guardTile = grid[guardPos[1]][guardPos[0]];
            char nextTile = grid[nextPos[1]][nextPos[0]];

            if (nextTile == '#')
            {
                RotateGuardCW90(grid);
                return false;
            }

            grid[guardPos[1]] = grid[guardPos[1]].Remove(guardPos[0], 1).Insert(guardPos[0], "X");
            grid[nextPos[1]] = grid[nextPos[1]].Remove(nextPos[0], 1).Insert(nextPos[0], $"{guardTile}");

            return false;
        }

        public int[] FindNextPos(int[] guardPos, string[] grid)
        {
            // Find the next position of the guard
            int x = guardPos[0];
            int y = guardPos[1];

            if (grid[y][x] == '^')
            {
                y--;
            }
            else if (grid[y][x] == '>')
            {
                x++;
            }
            else if (grid[y][x] == 'v')
            {
                y++;
            }
            else if (grid[y][x] == '<')
            {
                x--;
            }

            return new int[] { x, y };
        }

        public void RotateGuardCW90(string[] grid)
        {
            // Rotate the guard 90 degrees clockwise
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '^')
                    {
                        grid[y] = grid[y].Remove(x, 1).Insert(x, ">");
                        return;
                    }
                    else if (grid[y][x] == '>')
                    {
                        grid[y] = grid[y].Remove(x, 1).Insert(x, "v");
                        return;
                    }
                    else if (grid[y][x] == 'v')
                    {
                        grid[y] = grid[y].Remove(x, 1).Insert(x, "<");
                        return;
                    }
                    else if (grid[y][x] == '<')
                    {
                        grid[y] = grid[y].Remove(x, 1).Insert(x, "^");
                        return;
                    }
                }
            }

            throw new Exception("Guard not found");
        }

        public int[] FindGuardPos(string[] grid)
        {
            // Find the guard position, denoted by either ^, > , v or <
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '^' || grid[y][x] == '>' || grid[y][x] == 'v' || grid[y][x] == '<')
                    {
                        return new int[] { x, y };
                    }
                }
            }

            throw new Exception("Guard not found");
        }
    }
}
