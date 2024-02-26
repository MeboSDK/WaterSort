using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomassPuzzle.Models
{
    public class OperationModel
    {
        public OperationModel(ThomassPuzzle.Flask selectedFlask, ThomassPuzzle.Flask targetFlask)
        {
            SelectedFlask = selectedFlask;
            TargetFlask = targetFlask;
        }
        private int _targetTopIndex = -1;
        public ThomassPuzzle.Flask SelectedFlask;
        public ThomassPuzzle.Flask TargetFlask;
        public int SelectedTopIndex => SelectedFlask.TopLiquidItemIndex();
        public int TargetTopIndex
        {
            get
            {
                if (_targetTopIndex == -1)
                    return TargetFlask.TopLiquidItemIndex();
                else
                    return _targetTopIndex;
            }
            set =>
                _targetTopIndex = value;
        }
        public LiquidObject[] SelectedLiquidObjects => SelectedFlask?.GetLiquidObjects();
        public LiquidObject[] TargetLiquidObjects => TargetFlask?.GetLiquidObjects();
    }
}
