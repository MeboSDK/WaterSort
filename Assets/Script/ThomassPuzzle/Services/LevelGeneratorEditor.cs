using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Extensions;
using ThomassPuzzle.Helpers;
using ThomassPuzzle.Models.Level;
using Random = UnityEngine.Random;

namespace ThomassPuzzle.Services
{
#if UNITY_EDITOR

    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor
    {
        private SerializedProperty _targetGroupRangeProperty;
        private SerializedProperty _extraHolderRangeProperty;
        private SerializedProperty _targetLevelCountProperty;

        private void OnEnable()
        {
            _targetGroupRangeProperty = serializedObject.FindProperty(LevelGenerator.TARGET_GROUP_RANGE);
            _extraHolderRangeProperty = serializedObject.FindProperty(LevelGenerator.EXTRA_HOLDER_RANGE);
            _targetLevelCountProperty = serializedObject.FindProperty(LevelGenerator.TARGET_LEVEL_COUNT);
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                // Target LVL Count (TLC)
                var lvlCount = _targetLevelCountProperty.intValue;

                // Target Group Range Input (TGR)
                var targetGroupRange = _targetGroupRangeProperty.vector2IntValue;

                // Extra Holder Range Input (ELC)
                var extraHolderRange = _extraHolderRangeProperty.vector2IntValue;

                //If TLC or ELC is 0 or less exit method

                if (lvlCount <= 0 || extraHolderRange.x <= 0)
                    return;

                // Creating levels list
                var levels = new List<Level>();

                for (var i = 0; i < lvlCount; i++)
                {
                    var expectGroup = Mathf.RoundToInt(
                                            Mathf.Lerp(
                                                  targetGroupRange.x,
                                                  targetGroupRange.y,
                                                  (float)i / lvlCount
                                                       )
                                            );

                    var targetGroup = Mathf.RoundToInt(
                                            Mathf.Clamp(
                                                Random.Range(expectGroup - 1, expectGroup + 1),
                                                targetGroupRange.x,
                                                targetGroupRange.y
                                                )
                                            );

                    var emptyFlask = Mathf.RoundToInt(
                                            Mathf.Lerp(
                                                  extraHolderRange.x,
                                                  extraHolderRange.y,
                                                  (float)i / lvlCount
                                                       )
                                            );


                    var generateLevel = GenerateLevel(targetGroup, emptyFlask);

                    levels.Add(new Level
                    {
                        map = generateLevel.Select(items => new LevelColumn
                        {
                            values = items.ToList()
                        }).ToList(),
                        no = i + 1
                    });
                }


                var path = EditorUtility.SaveFilePanel("Save File As Json", "", "levels.json", ".json");

                if (path.Length > 0)
                {
                    System.IO.File.WriteAllText(path, JsonUtility.ToJson(new LevelGroup
                    {
                        levels = levels
                    }));
                }
            }
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
#endif
}