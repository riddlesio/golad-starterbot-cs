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

using System;
using System.Collections.Generic;
using System.Linq;

using GoladBot.Move;
using GoladBot.Field;

namespace GoladBot.Bot
{
    /**
    * Magic happens here. You should edit this file, or more specifically
    * the DoMove() method to make your bot do more than random moves.
    */
    public class BotStarter
    {
        private Random Random;
        public BotStarter()
        {
            Random = new Random();
        }

        /**
         * Performs a Birth or a Kill move, currently returns a random move.
         * Implement this to make the bot smarter.
         */
        public AbstractMove DoMove(BotState state)
        {
            AbstractMove move;
            Dictionary<string, List<Point>> cellMap = state.Field.GetCellMapping();

            if (Random.NextDouble() < 0.5)
            {
                move = DoRandomBirthMove(state, cellMap);
            }
            else
            {
                move = DoRandomKillMove(state, cellMap);
            }

            return move;
        }

        /**
         * Selects one dead cell and two of own living cells a random to birth a new cell
         * on at the point of the dead cell
         */
        private AbstractMove DoRandomBirthMove(BotState state, Dictionary<string, List<Point>> cellMap)
        {
            string myId = state.Field.MyId;
            List<Point> deadCells = cellMap["."];
            List<Point> myCells = new List<Point>(cellMap[myId]);

            if (deadCells.Count <= 0 || myCells.Count < 2)
            {
                return DoRandomKillMove(state, cellMap);
            }

            Point randomBirth = deadCells[Random.Next(deadCells.Count)];

            List<Point> sacrificePoints = new List<Point>();
            for (int i = 0; i < 2; i++)
            {
                int randomIndex = Random.Next(myCells.Count);
                Point randomSacrifice = myCells[randomIndex];
                sacrificePoints.Add(randomSacrifice);
                myCells.RemoveAt(randomIndex);
            }

            return new BirthMove(randomBirth, sacrificePoints);
        }

        /**
         * Selects one random living cell on the field and kills it
         */
        private AbstractMove DoRandomKillMove(BotState state, Dictionary<string, List<Point>> cellMap)
        {
            string myId = state.Field.MyId;
            string opponentId = state.Field.OpponentId;
            List<Point> livingCells = cellMap[myId].Concat(cellMap[opponentId]).ToList();

            if (livingCells.Count <= 0)
            {
                return new PassMove();
            }

            Point randomLiving = livingCells[Random.Next(livingCells.Count)];

            return new KillMove(randomLiving);
        }
    }
}