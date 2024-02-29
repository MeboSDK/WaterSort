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
using System;
using System.Collections;
namespace ThomassPuzzle
{
    public class TPGameManager : Singleton<TPGameManager>
    {
        #region Fields

        [SerializeField] FlasksSpace Space;
        [SerializeField] Text LvlName;
        [SerializeField] Text LvlTitle;
        [SerializeField] Text WholeLvl;
        [SerializeField] RectTransform NextLevelPanel;
        [SerializeField] RectTransform GameOverPanel;
        [SerializeField] TextMeshProUGUI TimeLimit;
        [SerializeField] RectTransform PauseLevelPanel;

        public TextAsset jsonData;
        private LevelGroup _levelData;
        private bool _topPanelButtonsAreDisabled;
        private int _seconds;
        
        private bool _pausedGame { get; set; }

        #region Level Generator
        [Space]
        [Header("Level Generator")]
        [SerializeField] int LevelsCount;
        [SerializeField] Vector2Int ColorsGroupsRange;
        [SerializeField] Vector2Int EmptyHoldersRange;
        private LevelGeneratorEditor _levelGeneratorEditor;
        #endregion
        
        #region FPS Properties
        [Space]
        [Header("FPS")]
        [SerializeField] LimitsEnum Limit;
        [SerializeField] TextMeshProUGUI FpsText;
        private float pollingTime = 1f;
        private float time;
        private int frameCount;
        #endregion

        #endregion


        #region Methods
        void Start()
        {
            Application.targetFrameRate = (int)Limit;

            //_levelData = JsonUtility.FromJson<LevelGroup>(jsonData.text);
            _levelData = LevelGeneratorService.GenerateLevels(LevelsCount,ColorsGroupsRange,EmptyHoldersRange);

            PlayerModel.SetPlayerCurrentLvl(1);

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
            //Turn off next level pop up
            NextLevelPanel.gameObject.SetActive(false);
            //Turn on space for flasks
            Space.gameObject.SetActive(true);

            //Write information about level
            LvlName.text = "Level " + PlayerModel.CurrentLevel.ToString();
            WholeLvl.text = PlayerModel.CurrentLevel.ToString() + "/" + _levelData.Count();

            var currentLevel = GetCurrentLvl();
        
            //Set default settings
            _pausedGame = false;
            _seconds = currentLevel.timeLimit;
            _topPanelButtonsAreDisabled = false;

            Space.DisabledTouch = false;

            Space.CreateFlasks(currentLevel);
            
            Space.CalculateGridConstraint();
            
            GameOverPanel.gameObject.SetActive(false);
            
            StartCoroutine(MinusTime());
        }
        public void DoneLevel()
        {

            NextLevelPanel.gameObject.SetActive(true);
            _topPanelButtonsAreDisabled = true;
            _pausedGame = true;
            StopAllCoroutines();
        }
        public bool IsLvlDone(List<Flask> flasks)
        {
            var LiquidsList = flasks.Select(o => o.GetLiquidObjects());
            var filledLiquids = LiquidsList.Where(o => o.Any(o => o.IsFilled()));
            var isSameColors = filledLiquids.All(o => o.DistinctBy(o => o.GetImage().color).Count() == 1);
            return isSameColors;
        }
        private static string ToFormattedTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                time.Hours,
                                time.Minutes,
                                time.Seconds);

            // Remove hours and minutes if they are zero
            formattedTime = formattedTime.TrimStart('0', ':');
            if (formattedTime.StartsWith(":"))
            {
                formattedTime = formattedTime.Remove(0, 1);
            }

            return formattedTime;
        }
        private IEnumerator MinusTime()
        {
            while (_seconds >= 0)
            {
                if (_pausedGame)
                    yield return new WaitUntil(() => !_pausedGame);

                TimeLimit.text = ToFormattedTime(_seconds);

                _seconds--;

                yield return new WaitForSeconds(1f);
            }
            Space.DisabledTouch = true;
            yield return new WaitUntil(() => !Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()));

            if (IsLvlDone(Space.AllFlasks.Where(o => o.isActiveAndEnabled).ToList()))
                DoneLevel();
            else
                FailedLevel();
        }
        private Level GetCurrentLvl()
        {
            var playerCurrentLvl = PlayerModel.CurrentLevel;
            var level = _levelData.FirstOrDefault(o => o.lvl == playerCurrentLvl);
            return level;
        }

        private void FailedLevel()
        {
            TMGameService.RestartActions();

            Space.AllFlasks.ForEach(f =>
            {
                if (f.isActiveAndEnabled)
                    f.gameObject.SetActive(false);
            });
            Space.LiquidLines.ForEach(l => l.gameObject.SetActive(false));
            //Space.gameObject.SetActive(false);

            GameOverPanel.gameObject.SetActive(true);
            _topPanelButtonsAreDisabled = true;
        }

        #region Buttons
        public void UndoActionButton()
        {
            if (_topPanelButtonsAreDisabled)
                return;

            if (NextLevelPanel.gameObject.activeSelf)
                return;

            //Logic of ad


            //End 

            TMGameService.UndoAction();
        }
        public void AddFlaskButton()
        {
            if (_topPanelButtonsAreDisabled)
                return;

            if (NextLevelPanel.gameObject.activeSelf)
                return;

            //If some of cup is in action
            if (Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return;

            //Logic of ad


            //End 

            var flask = Space.CreateFlask();

            flask.ClearFlask();

            var selectedFlask = Space.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());

            if (selectedFlask != default)
                Space.FailedTry(selectedFlask);

            Space.CalculateGridConstraint();
        }
        public void PauseButton()
        {
            if (_topPanelButtonsAreDisabled)
                return;

            if (NextLevelPanel.gameObject.activeSelf)
                return;

            if (Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return;

            //Logic of ad


            //End 

            var selectedFlask = Space.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());

            if (selectedFlask != default)
                Space.FailedTry(selectedFlask);

            _topPanelButtonsAreDisabled = true;
            _pausedGame = true;
            PauseLevelPanel.gameObject.SetActive(true);


            Space.AllFlasks.ForEach(o =>
            {
                if (o.isActiveAndEnabled)
                    o.Button.enabled = false;
            });

        }
        public void AddSecondsButton()
        {
            if (_topPanelButtonsAreDisabled)
                return;

            if (NextLevelPanel.gameObject.activeSelf)
                return;

            
            //Logic of ad


            //End 
            
            _seconds += 15;
        }
        public void ResetSceneButton() => SceneManager.LoadScene("ThomassPuzzle");
        public void NextLevelButton()
        {
            if (_levelData.levels.Count >= PlayerModel.CurrentLevel + 1)
                ++PlayerModel.CurrentLevel;

            TMGameService.ClearSavedActions();

            GenerateLvl();
        }
        public void RestartButton()
        {
            TMGameService.RestartActions();

            GenerateLvl();
        }
        public void ContinueButton()
        {
            _topPanelButtonsAreDisabled = false;
            _pausedGame = false;
            PauseLevelPanel.gameObject.SetActive(false);

            Space.AllFlasks.ForEach(o =>
            {
                if (o.isActiveAndEnabled)
                    o.Button.enabled = true;
            });

        }

        #endregion

        #endregion
    }
}