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

using System.Text;
using System.Collections.Generic;

using GoladBot.Field;

namespace GoladBot.Move
{
    public class BirthMove: AbstractMove
    {
        public Point BirthPoint { get; private set; }
        public List<Point> SacrificePoints { get; private set; }

        public BirthMove(Point birthPoint, List<Point> sacrificePoints) {
            MoveType = MoveType.Birth;
            BirthPoint = birthPoint;
            SacrificePoints = sacrificePoints;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append(MoveType.ToString()).Append(" ").Append(PointToString(BirthPoint));

            foreach (Point point in SacrificePoints)
            {
                builder.Append(" ").Append(PointToString(point));
            }

            return builder.ToString();
        }
    }
}
