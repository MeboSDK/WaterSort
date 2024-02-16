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
        public ThomassPuzzle.Flask SelectedFlask { get; set; }
        public ThomassPuzzle.Flask TargetFlask { get; set; }
        public int SelectedTopIndex
        {
            get
            {
                return SelectedFlask.TopLiquidItemIndex();
            }
        }
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
        public LiquidObject[] SelectedLiquidObjects
        {
            get
            {
                return SelectedFlask?.GetLiquidObjects();
            }
        }
        public LiquidObject[] TargetLiquidObjects
        {
            get
            {
                return TargetFlask?.GetLiquidObjects();
            }
        }
    }
}
