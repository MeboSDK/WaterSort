using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Helpers;
using ThomassPuzzle.Models.Level;
using UnityEngine;

namespace ThomassPuzzle.Services
{
    public class LevelGeneratorService
    {
        public static LevelGroup GenerateLevels(int lvlCount, Vector2Int colorsGroupsRange, Vector2Int emptyHoldersRange)
        {
            if (lvlCount <= 0 || emptyHoldersRange.x <= 0)
                return default;

            // Creating levels list
            var levels = new List<Level>();

            for (var i = 0; i < lvlCount; i++)
            {
                var expectGroup = Mathf.RoundToInt(
                                        Mathf.Lerp(
                                              colorsGroupsRange.x,
                                              colorsGroupsRange.y,
                                              (float)i / lvlCount
                                                   )
                                        );

                var targetGroup = Mathf.RoundToInt(
                                        Mathf.Clamp(
                                            Random.Range(expectGroup - 1, expectGroup + 1),
                                            colorsGroupsRange.x,
                                            colorsGroupsRange.y
                                            )
                                        );

                var emptyFlask = Mathf.RoundToInt(
                                        Mathf.Lerp(
                                              emptyHoldersRange.x,
                                              emptyHoldersRange.y,
                                              (float)i / lvlCount
                                                   )
                                        );


                var generateLevel = GenerateLevel(targetGroup, emptyFlask);

                levels.Add(new Level
                {
                    flaks = generateLevel.Select(items => new LevelColumn
                    {
                        colors = items.ToList()

                    }).ToList(),
                    lvl = i + 1,
                    hide = i == lvlCount - 1,
                    timeLimit = 40
                });
            }

            return new LevelGroup
            {
                levels = levels
            };
        }
        public static IEnumerable<int[]> GenerateLevel(int holderCount, int emptyFlasks)
        {
            // Creating empty holders.
            var holders = Enumerable.Range(0, holderCount).Select(i => new List<int>()).ToList();

            ColorsHelper colorsHelper = new ColorsHelper();
            List<WaterColorEnum> choosedColors = colorsHelper.ColorsDevelopment(false, holders.Count);

            // Set groups and attach values
            for (var i = 0; i < holders.Count; i++)
                holders[i].AddRange(colorsHelper.GetColorsIndexes(choosedColors));


            for (int j = 0; j < emptyFlasks; j++)
                holders.Add(new());

            return holders.Select(items => items.ToArray());
        }
    }
}
