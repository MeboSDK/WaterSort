using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = UnityEngine.Random;


namespace ThomassPuzzle.Extensions
{
    public static class LinqExtension
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable, out int index)
        {
            var list = enumerable.ToList();
            index = Random.Range(0, list.Count);
            return list[index];
        }
        public static T GetRandom<T>(this IEnumerable<T> enumerable) =>
        enumerable.GetRandom(out var index);

    }
}
