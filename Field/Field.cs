/*
 * Copyright 2017 riddles.io (developers@riddles.io)
 *
 *     Licensed under the Apache License, Version 2.0 (the "License");
 *     you may not use this file except in compliance with the License.
 *     You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 *     Unless required by applicable law or agreed to in writing, software
 *     distributed under the License is distributed on an "AS IS" BASIS,
 *     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *     See the License for the specific language governing permissions and
 *     limitations under the License.
 *
 *     For the full copyright and license information, please view the LICENSE
 *     file that was distributed with this source code.
 */

using System.Collections.Generic;

using GoladBot.Field;

namespace GoladBot.Field
{
    /**
     * Stores all information about the game field and
     * contains methods to perform calculations on it
     */
    public class Field
    {
        public string MyId { get; set; }
        public string OpponentId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private string[,] Cells;

        /**
         * Parses the input string given by the engine
         */
        public void ParseFromString(string input)
        {
            Cells = new string[Width, Height];
            int x = 0;
            int y = 0;

            foreach (string cell in input.Split(','))
            {
                Cells[x,y] = cell;

                if (++x == Width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        /**
         * Get the points on the field per cell type
         */
        public Dictionary<string, List<Point>> GetCellMapping()
        {
            Dictionary<string, List<Point>> cellMap = new Dictionary<string, List<Point>>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    string cell = Cells[x,y];

                    if (!cellMap.ContainsKey(cell))
                    {
                        cellMap.Add(cell, new List<Point>());
                    }

                    cellMap[cell].Add(new Point(x, y));
                }
            }

            return cellMap;
        }
    }
}