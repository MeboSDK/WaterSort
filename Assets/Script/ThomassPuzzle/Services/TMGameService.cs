using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Helpers;
using ThomassPuzzle.Models;

namespace ThomassPuzzle.Services
{
    public class TMGameService
    {
        public static void UndoAction()
        {
            var flaskSpace = FlasksSpace.Instance;

            if (flaskSpace.SelectedFlasks.Count > 0)
                return;

            if (flaskSpace.SavedGamePlays.Count == 0)
                return;

            var lastAction = flaskSpace.SavedGamePlays.Peek();

            ColorModel colorModel = ColorsHelper.GetColor(lastAction.Color);
            while(lastAction.TargetLiquidIndex.Count > 0)
            {
                lastAction.TargetFlask.GetLiquidObjects()[lastAction.TargetLiquidIndex.Peek()].Clear();
                lastAction.TargetLiquidIndex.Pop();
                lastAction.SelectedFlask.GetLiquidObjects()[lastAction.SelectedLiquidIndex.Peek()].Fill(colorModel, 1);
                lastAction.SelectedLiquidIndex.Pop();
            }

            lastAction.TargetFlask.CheckFinishedFlask();
            lastAction.SelectedFlask.CheckFinishedFlask();

            flaskSpace.SavedGamePlays.Pop();
        }

        public static void ClearSavedActions()
        {
            var flaskSpace = FlasksSpace.Instance;

            flaskSpace.SavedGamePlays.Clear();
        }

        public static bool RestartActions()
        {
            var flaskSpace = FlasksSpace.Instance;

            if (flaskSpace.SelectedFlasks.Count > 0)
                return false;


            flaskSpace.LiquidLines.ForEach(o =>
            {
                o.gameObject.SetActive(false);
            });

            flaskSpace.AllFlasks.ForEach(o =>
            {
                o.SetFixedPosition(default);
            });

            return true;
        }
    }
}
