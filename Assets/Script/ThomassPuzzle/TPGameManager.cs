using ThomassPuzzle.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ThomassPuzzle.Models.Level;
using Unity.VisualScripting;
using ThomassPuzzle.Services;
using ThomassPuzzle.Enums;
using TMPro;
using UnityEngine.SceneManagement;
namespace ThomassPuzzle
{
    public class TPGameManager : Singleton<TPGameManager>
    {
        #region Properties
        [SerializeField] FlasksSpace Space;
        [SerializeField] Text LvlName;
        [SerializeField] int TestLevel;
        [SerializeField] Text LvlTitle;
        [SerializeField] Text WholeLvl;
        [SerializeField] RectTransform NextLevelPanel;

        public TextAsset jsonData;
        private LevelGroup _levelData;

        #region FPS Properties
        [Space]
        [Header("FPS")]
        [SerializeField] LimitsEnum limit;
        [SerializeField] TextMeshProUGUI FpsText;
        private float pollingTime = 1f;
        private float time;
        private int frameCount;
        #endregion

        #endregion
        void Start()
        {
            Application.targetFrameRate = (int)limit;

            _levelData = JsonUtility.FromJson<LevelGroup>(jsonData.text);
            GenerateLvl();
        }
        void Update()
        {
            // Update time.
            time += Time.deltaTime;

            // Count this frame.
            frameCount++;

            if (time >= pollingTime)
            {
                // Update frame rate.
                int frameRate = Mathf.RoundToInt((float)frameCount / time);
                FpsText.text = frameRate.ToString() + " fps";

                // Reset time and frame count.
                time -= pollingTime;
                frameCount = 0;
            }
        }
        public void GenerateLvl()
        {
            NextLevelPanel.gameObject.SetActive(false);

            if (TestLevel > 0)
                PlayerModel.SetPlayerCurrentLvl(TestLevel);

            LvlName.text = "Level " + PlayerModel.CurrentLevel.ToString();
            WholeLvl.text = PlayerModel.CurrentLevel.ToString() + "/" + _levelData.Count();
            Space.CreateFlasks(GetCurrentLvl());
            Space.CalculateGridConstraint();
        }
        public bool IsLvlDone(List<Flask> flasks)
        {
            var LiquidsList = flasks.Select(o => o.GetLiquidObjects());
            var filledLiquids = LiquidsList.Where(o => o.Any(o => o.IsFilled()));
            var isSameColors = filledLiquids.All(o => o.DistinctBy(o => o.GetImage().color).Count() == 1);
            return isSameColors;
        }
        public void DoneLevel() => NextLevelPanel.gameObject.SetActive(true);
        private Level GetCurrentLvl()
        {
            var playerCurrentLvl = PlayerModel.CurrentLevel;

            return _levelData.FirstOrDefault(o => o.no == playerCurrentLvl);
        }

        #region Buttons
        public void ResetSceneButton() =>
            SceneManager.LoadScene("ThomassPuzzle");
        public void NextLevelButton()
        {
            if (_levelData.levels.Count >= PlayerModel.CurrentLevel + 1)
                ++PlayerModel.CurrentLevel;

            Space.SavedGamePlays.Clear();
            GenerateLvl();
        }
        public void RestartButton()
        {
            if (!TMGameService.RestartActions() || NextLevelPanel.gameObject.activeSelf)
                return;
            GenerateLvl();
        }
        public void UndoActionButton()
        {
            if(NextLevelPanel.gameObject.activeSelf)
                return;
            TMGameService.UndoAction();
        }
        public void AddFlaskButton()
        {
            if (NextLevelPanel.gameObject.activeSelf)
                return;
            //If some of cup is in action
            if (Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return;

            var flask = Space.CreateFlask();

            flask.ClearFlask();
    
            var selectedFlask = Space.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());

            if (selectedFlask != default)
            {
                selectedFlask.MoveDown();
                Space.FailedTry(selectedFlask);
            }
            
            Space.CalculateGridConstraint();
        }

        #endregion
    }
}