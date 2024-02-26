using System.Linq;
using ThomassPuzzle.Helpers;
using ThomassPuzzle.Models;

namespace ThomassPuzzle.Services
{
    public class TMGameService
    {
        public static void UndoAction()
        {
            var flaskSpace = FlasksSpace.Instance;

            if (flaskSpace.SavedGamePlays.Count == 0)
                return;

            if (flaskSpace.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return;

            var flask = flaskSpace.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());
            if (flask != default)
                flaskSpace.FailedTry(flask);

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

        public static bool RestartActions()
        {
            var flaskSpace = FlasksSpace.Instance;

            if (flaskSpace.SavedGamePlays.Count == 0)
                return false;

            if (flaskSpace.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return false;

            var flask = flaskSpace.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());

            if (flask != default)
                flaskSpace.FailedTry(flask);

            flaskSpace.LiquidLines.ForEach(o =>
            {
                o.gameObject.SetActive(false);
            });

            flaskSpace.AllFlasks.ForEach(o =>
            {
                o.SetFixedPosition(default);
            });

            ClearSavedActions();

            return true;
        }

        public static void ClearSavedActions()
        {
            var flaskSpace = FlasksSpace.Instance;

            flaskSpace.SavedGamePlays.Clear();
        }

    }
}
