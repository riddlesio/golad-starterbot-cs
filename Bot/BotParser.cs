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
using GoladBot.Move;
using GoladBot.Player;


namespace GoladBot.Bot
{
    /**
     * Main class that will keep reading output from the engine.
     * Will either update the bot state or get actions.
     */
    public class BotParser
    {
        private readonly BotStarter _bot;
        private readonly BotState _currentState;
        
        public BotParser(BotStarter bot)
        {
            _bot = bot;
            _currentState = new BotState();
        }

        /**
         * Keeps consuming all input over the stdin channel
         */
        public void Run()
        {
            string line;

            while ((line = Console.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');

                switch (parts[0])
                {
                    case "settings":
                        ParseSettings(parts[1], parts[2]);
                        break;
                    case "update":
                        if (parts[1].Equals("game"))
                        {
                            ParseGameData(parts[2], parts[3]);
                        }
                        else
                        {
                            ParsePlayerData(parts[1], parts[2], parts[3]);
                        }
                        break;
                    case "action":
                        if (parts[1].Equals("move"))
                        {
                            AbstractMove move = _bot.DoMove(_currentState);
                            Console.WriteLine(move?.ToString() ?? MoveType.Pass.ToString());
                        }
                        break;
                    default:
                        Console.Error.WriteLine("Unknown command");
                        break;
                }
            }
        }

        /**
         * Parses all the game settings given by the engine
         */
        private void ParseSettings(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "timebank":
                        int time = int.Parse(value);
                        _currentState.MaxTimebank = time;
                        _currentState.Timebank = time;
                        break;
                    case "time_per_move":
                        _currentState.TimePerMove = int.Parse(value);
                        break;
                    case "player_names":
                        string[] playerNames = value.Split(',');
                        foreach (string playerName in playerNames)
                            _currentState.Players.Add(playerName, new Player.Player(playerName));
                        break;
                    case "your_bot":
                        _currentState.MyName = value;
                        break;
                    case "your_botid":
                        _currentState.Field.MyId = value;
                        _currentState.Field.OpponentId = (2 - (int.Parse(value) + 1)) + "";
                        break;
                    case "field_width":
                        _currentState.Field.Width = int.Parse(value);
                        break;
                    case "field_height":
                        _currentState.Field.Height = int.Parse(value);
                        break;
                    case "max_rounds":
                        _currentState.MaxRounds = int.Parse(value);
                        break;
                    default:
                        Console.Error.Write($"Cannot parse settings input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse settings value '{value}' for key '{key}'");
            }
        }

        /**
         * Parse data about the game given by the engine
         */
        private void ParseGameData(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "round":
                        _currentState.RoundNumber = int.Parse(value);
                        break;
                    case "field":
                        _currentState.Field.ParseFromString(value);
                        break;
                    default:
                        Console.Error.WriteLine($"Cannot parse game data input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Cannot parse game data value '{value}' for key '{key}'");
            }
        }

        /**
         * Parse data about given player
         */
        private void ParsePlayerData(string playerName, string key, string value)
        {
            Player.Player player;
            
            if (!_currentState.Players.TryGetValue(playerName, out player))
            {
                Console.Error.WriteLine($"Could not find player with name '{playerName}'");
                return;
            }

            try
            {
                switch (key)
                {
                    case "living_cells":
                        player.LivingCells = int.Parse(value);
                        break;
                    case "move":
                        player.previousMove = value;
                        break;
                    default:
                        Console.Error.WriteLine(
                            $"Cannot parse {playerName} data input with key '{key}'");
                        break;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine(
                    $"Cannot parse {playerName} data value '{value}' for key '{key}'");
            }
        }
    }
}