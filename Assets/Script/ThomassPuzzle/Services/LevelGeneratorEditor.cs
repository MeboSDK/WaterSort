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

                LevelGroup levelGroup = LevelGeneratorService.GenerateLevels(lvlCount,targetGroupRange,extraHolderRange);

                var path = EditorUtility.SaveFilePanel("Save File As Json", "", "levels.json", ".json");

                if (path.Length > 0)
                    System.IO.File.WriteAllText(path, JsonUtility.ToJson(levelGroup));
            }
        }
    }
#endif
}