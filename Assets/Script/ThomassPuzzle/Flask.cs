using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Helpers;
namespace ThomassPuzzle
{
    [System.Serializable]
    public class WaterRationSegment
    {
        public float Degree;
        public float x, y;
        public float heigth;
    }

    [System.Serializable]
    public class WaterRationSegments
    {
        public List<WaterRationSegment> WaterBoundList;
    }

    [System.Serializable]
    public class WaterRationBound
    {
        public List<WaterRationSegments> WaterBoundLists;

        public void SetupRect(ref float degree, LiquidObject[] L)
        {
            degree %= 360;

            int sign = 1;

            if (degree > 90)
            {
                degree -= 360;
                degree = -degree;
            }
            else
                sign = -1;


            for (int i = 0; i < WaterBoundLists.Count; i++)
            {

                WaterRationSegment start = WaterBoundLists[i].WaterBoundList[0], end = WaterBoundLists[i].WaterBoundList[2];

                foreach (var segment in WaterBoundLists[i].WaterBoundList)
                {
                    if (segment.Degree <= degree)
                        start = segment;

                    if (segment.Degree >= degree)
                    {
                        end = segment;
                        break;
                    }
                }

                var rect = L[i].GetImage().rectTransform;

                float enami = degree - start.Degree;
                float denami = end.Degree - start.Degree;
                float ratio = 0;
                if (denami > 0)
                    ratio = enami / denami;

                float newHeight = ratio * (end.heigth - start.heigth) + start.heigth;
                float newx = ratio * (end.x - start.x) + start.x;
                float newy = ratio * (end.y - start.y) + start.y;

                Vector2 pos = rect.localPosition;
                pos.x = newx * sign;
                pos.y = newy;
                rect.localPosition = pos;

                Vector2 szDelta = rect.sizeDelta;
                szDelta.y = newHeight;
                rect.sizeDelta = szDelta;

                Vector3 rot = rect.localEulerAngles;
                rot.z = degree * sign;
                rect.localEulerAngles = rot;
            }
        }
    }

    public class Flask : MonoBehaviour
    {
        #region Fields

        [SerializeField] WaterRationBound RatioBound;

        [SerializeField] LiquidObject[] LiquidObjects;

        [SerializeField] Image Content;

        [SerializeField] RectTransform RectTransform;

        [SerializeField] RectTransform DotForLiquidLine;

        [SerializeField] GameObject FinishedFlask;

        [DoNotSerialize] public Button Button;
        
        private Vector2 FixedPosition;

        private FlasksSpace _parentSpace;

        private List<WaterColorEnum> _choosedColors;

        private bool isActivated = false;



        #endregion

        #region  Methods
        /*        public void Update()
                {
                    var crtRadius = transform.localEulerAngles.z;
                    GetRatioBound().SetupRect(ref crtRadius, LiquidObjects);
                }*/
        private void Awake() => Invoke(nameof(Activate), 1);
        private void Activate() => isActivated = true;

        public void HandleClick()
        {
      /*      if (!isActivated)
            { return; }*/
            _parentSpace.SelectFlask(this);
        }

        public void ClearFlask()
        {
            for (int i = 0; i < LiquidObjects.Length; i++)
            {
                LiquidObjects[i].name = i.ToString();
                LiquidObjects[i].Clear();
                LiquidObjects[i].LastFlask = null;
            }

            if (isActiveAndEnabled)
                StartCoroutine(FlaskIsFinished(false));
        }
        public void FillFlask()
        {
            for (int i = 0; i < LiquidObjects.Length; i++)
            {
                var color = ColorsHelper.GetColor(_choosedColors[i]);
                LiquidObjects[i].Fill(color, 1);
                LiquidObjects[i].LastFlask = this;
            }
        }
        public int TopLiquidItemIndex()
        {
            for (int i = 3; i >= 0; i--)
                if (LiquidObjects[i].IsFilled())
                    return i;

            return -1;
        }
        public void MoveUp() =>
            transform.DOMoveY(transform.position.y + .5f, 0.1f);
        public void MoveDown(float delay = 0.1f) =>
            RectTransform.DOAnchorPosY(FixedPosition.y, delay).OnComplete(() =>
            {
                Button.enabled = true;
            });
        public void ReturnBack(float delay = 0.1f) =>
            RectTransform.DOAnchorPos(FixedPosition, delay).OnComplete(() =>
            {
                Button.enabled = true;
            });
        public void SetFlaskSpace(FlasksSpace flastSpace) => _parentSpace = flastSpace;
        public LiquidObject[] GetLiquidObjects() => LiquidObjects;
        public void SetChoosedColors(List<WaterColorEnum> choosedColors) => _choosedColors = choosedColors;
        public void SetFixedPosition(Vector2 position) => FixedPosition = position;
        public Vector2 GetFixedPosition() => FixedPosition;
        public RectTransform GetRect() => RectTransform;
        public RectTransform GetDotRectForLiquidLine() => DotForLiquidLine;
        public Image GetContent() => Content;
        public WaterRationBound GetRatioBound() => RatioBound;

        public bool CheckFinishedFlask()
        {
            var liquidObjs = GetLiquidObjects();
            var color = liquidObjs[0].GetColorEnum();
            if (liquidObjs.Any(o => o.GetColorEnum() != color) || color== WaterColorEnum.None)
            {
                StartCoroutine(FlaskIsFinished(false));
                return false;
            }

            StartCoroutine(FlaskIsFinished(true));
            return true;
        }
        public IEnumerator FlaskIsFinished(bool isFinished)
        {
            if (isFinished)
                yield return new WaitUntil(() => GetLiquidObjects().All(o => o.GetImage().fillAmount == 1));
           
            FinishedFlask.gameObject.SetActive(isFinished);
        }

        #endregion

    }
}