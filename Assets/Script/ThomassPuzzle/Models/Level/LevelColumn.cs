using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomassPuzzle.Models.Level
{
    [Serializable]
    public struct LevelColumn : IEnumerable<int>
    {
        public List<int> values;

        public IEnumerator<int> GetEnumerator()
        {
            return values?.GetEnumerator() ?? Enumerable.Empty<int>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
