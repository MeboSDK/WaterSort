using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ThomassPuzzle.Models;

namespace ThomassPuzzle.Models.Level
{
    [Serializable]
    public struct LevelGroup : IEnumerable<Level>
    {
        public List<Level> levels;
        public IEnumerator<Level> GetEnumerator()
        {
            return levels?.GetEnumerator() ?? Enumerable.Empty<Level>().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
