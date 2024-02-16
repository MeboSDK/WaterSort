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
        public ThomassPuzzle.Flask SelectedFlask { get; set; }
        public ThomassPuzzle.Flask TargetFlask { get; set; }
        public WaterColorEnum Color { get; set; }
        public Stack<int> SelectedLiquidIndex { get; set; } = new Stack<int>();
        public Stack<int> TargetLiquidIndex { get; set; } = new Stack<int>();
    }
}
