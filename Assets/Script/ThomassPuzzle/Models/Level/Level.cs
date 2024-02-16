using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomassPuzzle.Models.Level
{
    [Serializable]
    public struct Level
    {
        public int no;

        public List<LevelColumn> map;
        public List<IEnumerable<LiquidData>> LiquidDataMap => map.Select(GetLiquidDatas).ToList();

        public static IEnumerable<LiquidData> GetLiquidDatas(LevelColumn column)
        {
            var list = column.ToList();

            for (var j = 0; j < list.Count; j++)
            {
                var currentGroup = list[j];
                var count = 0;
                for (; j < list.Count; j++)
                {
                    if (currentGroup == list[j])
                    {
                        count++;
                    }
                    else
                    {
                        j--;
                        break;
                    }
                }

                yield return new LiquidData
                {
                    groupId = currentGroup,
                    value = count
                };
            }
        }
    }

    public struct LiquidData
    {
        public int groupId;
        public float value;
    }
}
