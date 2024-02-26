using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThomassPuzzle;
using ThomassPuzzle.Enums;

namespace ThomassPuzzle.Models
{
    public class SaveGamePlay
    {
        public ThomassPuzzle.Flask SelectedFlask;
        public ThomassPuzzle.Flask TargetFlask;
        public WaterColorEnum Color;
        public Stack<int> SelectedLiquidIndex = new Stack<int>();
        public Stack<int> TargetLiquidIndex = new Stack<int>();
    }
}
