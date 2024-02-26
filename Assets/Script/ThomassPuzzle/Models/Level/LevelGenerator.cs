#if UNITY_EDITOR

using UnityEngine;

namespace ThomassPuzzle.Models.Level
{
    public partial class LevelGenerator : ScriptableObject
    {
        [SerializeField] private Vector2Int _targetGroupRange;
        [SerializeField] private Vector2Int _extraHolderRange;
        [SerializeField] private int _targetLevelCount;
    }

    public partial class LevelGenerator
    {
        public const string TARGET_GROUP_RANGE = nameof(_targetGroupRange);
        public const string EXTRA_HOLDER_RANGE = nameof(_extraHolderRange);
        public const string TARGET_LEVEL_COUNT = nameof(_targetLevelCount);
    }

}
#endif
