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
        [SerializeField] TextMeshProUGUI LvlName;
        [SerializeField] TextMeshProUGUI LvlTitle;
        [SerializeField] TextMeshProUGUI TimeLimit;

        [SerializeField] GameObject TopPanel;
        [SerializeField] RectTransform PauseLevelPanel;
        [SerializeField] GameObject PauseIcon;
        [SerializeField] RectTransform GameOverPanel;
        [SerializeField] RectTransform NextLevelPanel;
        [SerializeField] GameObject AllLevelCompleted;

        #region Help Setting

        [Space]
        [Header("Help Setting")]
        [SerializeField] int AddableMaxFlasks;
        [SerializeField] int AddTimeSeconds;
        [SerializeField] int AddTimeCount;
        [SerializeField] int MaxUndoCount;

        #endregion

        #region Texts

        [Space]
        [Header("Texts")]
        [SerializeField] TextMeshProUGUI CompletedTotalTimeText;
        [SerializeField] TextMeshProUGUI FailedTotalTimeText;
        [SerializeField] TextMeshProUGUI CompletedTimeText;
        [SerializeField] TextMeshProUGUI BonusTimeText;

        #endregion

        [SerializeField] TextAsset jsonData;
        private LevelGroup _levelData;

        private int _addedFlasksCount;
        private int _addedTimesCount;

        private int _seconds;
        private int _bonusTime;
        private int _completeTime;
        private int _totalTime;


        #region Level Generator
        [Space]
        [Header("Level Generator")]
        [SerializeField] int LevelsCount;
        [SerializeField] Vector2Int ColorsGroupsRange;
        [SerializeField] Vector2Int EmptyHoldersRange;
        #endregion

        #region FPS Properties
        [Space]
        [Header("FPS")]
        [SerializeField] LimitsEnum Limit;
        #endregion
        #endregion

        #region Methods
        void Start()
        {
            Application.targetFrameRate = (int)Limit;

            //_levelData = JsonUtility.FromJson<LevelGroup>(jsonData.text);
            _levelData = LevelGeneratorService.GenerateLevels(LevelsCount, ColorsGroupsRange, EmptyHoldersRange);

            PlayerModel.SetPlayerCurrentLvl(1);

            GenerateLvl();
        }

        public void GenerateLvl()
        {
            NextLevelPanel.gameObject.SetActive(false);
            GameOverPanel.gameObject.SetActive(false);
            AllLevelCompleted.gameObject.SetActive(false);

            Space.gameObject.SetActive(true);
            TopPanel.gameObject.SetActive(true);
            PauseIcon.SetActive(true);

            var currentLevel = GetCurrentLvl();

            //Write information about level
            LvlName.text = PlayerModel.CurrentLevel.ToString() + "/" + _levelData.Count();

            //Set default settings
            _bonusTime = 0;
            _completeTime = 0;
            _seconds += currentLevel.timeLimit;

            Space.DisabledTouch = false;

            Space.CreateFlasks(currentLevel);

            Space.CalculateGridConstraint();


            StartCoroutine(MinusTime());
        }
        public void LevelCompleted()
        {
            StopAllCoroutines();

            _bonusTime = _seconds;

            TimeLimit.text = ToFormattedTime(_seconds);

            CompletedTimeText.text = ToFormattedTime(_completeTime);
            BonusTimeText.text = ToFormattedTime(_bonusTime);
            CompletedTotalTimeText.text = ToFormattedTime(_totalTime);

            TopPanel.gameObject.SetActive(false);
            PauseIcon.SetActive(false);
            Space.gameObject.SetActive(false);

            if (_levelData.levels.Count == PlayerModel.CurrentLevel)
                AllLevelCompleted.SetActive(true);
            else
                NextLevelPanel.gameObject.SetActive(true);
        }
        public bool IsLvlDone(List<Flask> flasks)
        {
            var LiquidsList = flasks.Select(o => o.GetLiquidObjects());
            var filledLiquids = LiquidsList.Where(o => o.Any(o => o.IsFilled()));
            var isSameColors = filledLiquids.All(o => o.DistinctBy(o => o.GetImage().color).Count() == 1);
            return isSameColors;
        }
        private void FailedLevel()
        {
            TMGameService.RestartActions();

            TopPanel.SetActive(false);

            print(_totalTime);
            FailedTotalTimeText.text = ToFormattedTime(_totalTime);

            Space.gameObject.SetActive(false);
            GameOverPanel.gameObject.SetActive(true);
            PauseIcon.SetActive(false);
        }
        private Level GetCurrentLvl()
        {
            var playerCurrentLvl = PlayerModel.CurrentLevel;
            var level = _levelData.FirstOrDefault(o => o.lvl == playerCurrentLvl);

            return level;
        }
        private static string ToFormattedTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                time.Hours,
                                time.Minutes,
                                time.Seconds);


            if (formattedTime.StartsWith(":"))
                formattedTime = formattedTime.Remove(0, 1);

            return formattedTime;
        }
        private IEnumerator MinusTime()
        {
            while (_seconds > 0)
            {
                TimeLimit.text = ToFormattedTime(_seconds);

                yield return new WaitForSeconds(1f);

                _seconds--;
                _completeTime++;
                _totalTime++;
            }
            print(_completeTime);
            print(_totalTime);
            Space.DisabledTouch = true;
            yield return new WaitUntil(() => !Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()));

            if (IsLvlDone(Space.AllFlasks.Where(o => o.isActiveAndEnabled).ToList()))
                LevelCompleted();
            else
                FailedLevel();
        }

        #region Buttons
        public void UndoActionButton()
        {
            if (MaxUndoCount > 0)
                MaxUndoCount--;
            else
                return;

            if (NextLevelPanel.gameObject.activeSelf)
                return;

            //Logic of ad
            Debug.Log("Undo Button Ad Logic here");

            //End 

            TMGameService.UndoAction();
        }
        public void AddFlaskButton()
        {
            if (NextLevelPanel.gameObject.activeSelf)
                return;

            if (AddableMaxFlasks == _addedFlasksCount)
                return;

            //If some of cup is in action
            if (Space.SelectedFlasks.Exists(o => o != null && o.IsInAction()))
                return;

            //Logic of ad
            Debug.Log("Add Flask Button Ad Logic here");

            //End 

            var flask = Space.CreateFlask();

            _addedFlasksCount++;

            flask.ClearFlask();

            var selectedFlask = Space.SelectedFlasks.FirstOrDefault(o => o != null && !o.IsInAction() && o.IsMovedUp());

            if (selectedFlask != default)
                Space.FailedTry(selectedFlask);

            Space.CalculateGridConstraint();
        }
        public void PauseButton()
        {
            Time.timeScale = 0;
            PauseLevelPanel.gameObject.SetActive(true);
            PauseIcon.SetActive(false);
        }
        public void AddSecondsButton()
        {
            if (NextLevelPanel.gameObject.activeSelf)
                return;

            if (AddTimeCount == _addedTimesCount)
                return;

            //Logic of ad
            Debug.Log("Add Seconds Button Ad Logic here");
            //End 

            _seconds += AddTimeSeconds;
            _addedTimesCount++;
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
            PauseLevelPanel.gameObject.SetActive(false);
            PauseIcon.SetActive(true);
            Time.timeScale = 1;
        }
        public void LeaveButton()
        {
            //Logic of leave
            Debug.Log("Leave logic here");
            //End 
        }
        public void LevelFailedButton()
        {
            PauseLevelPanel.gameObject.SetActive(false);
            PauseIcon.SetActive(false);
            TopPanel.SetActive(false);
        }

        #endregion

        #endregion
    }
}