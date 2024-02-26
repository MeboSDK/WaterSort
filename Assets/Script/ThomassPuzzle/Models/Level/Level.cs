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
        public int lvl;

        public List<LevelColumn> flaks;
    }
}
