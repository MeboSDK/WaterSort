using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ThomassPuzzle
{
    public class LiquidLine : MonoBehaviour
    {

        [SerializeField] RectTransform Rect;
        [SerializeField] Image Line;
        [SerializeField] RectTransform TopRect;
        [SerializeField] RectTransform BottomRect;

        private Vector2 _fixedSizeDelta;
        private int _fixedIndex;

        [DoNotSerialize] public bool Used = false;
        public void SetDeltaSize(Vector2 selected)
        {
            if (_fixedSizeDelta != default(Vector2))
            {
                Rect.sizeDelta = _fixedSizeDelta;
                return;
            }

            float potentialHeight = selected.y < 0 ? selected.y * -1 : selected.y;

            float topPointY = Rect.anchoredPosition.y + potentialHeight;

            float height = potentialHeight - (topPointY - selected.y);

            float newHeight = Mathf.Max(height, 0.0f);

            Rect.sizeDelta = new Vector2(5, newHeight);

            SetFixedSizeDelta(Rect.sizeDelta);
        }
        public void ShowLiquidLine(bool show, Flask target = null, Color color = default)
        {
            if (show)
            {
                PlaceLiquidLine(target);

                EnableLiquidLine(color);

            }
            else
            {
                Line.fillOrigin = 1;
                Line.DOFillAmount(0, 0.3f).OnComplete(() =>
                {
                    Used = false;
                    Line.enabled = false;
                    gameObject.SetActive(false);
                    Rect.SetSiblingIndex(GetFixedIndex());

                });
            }

        }
        public void EnableLiquidLine(Color color = default)
        {
            if (!Line.enabled)
            {
                Line.fillAmount = 0;
                Line.color = color;
                Line.enabled = true;
                Line.fillOrigin = 0;
                Line.DOFillAmount(1, 0.2f);
                gameObject.SetActive(true);
            }
        }
        public void PlaceLiquidLine(Flask flask)
        {
            if (flask == null)
                return;

            Rect.SetSiblingIndex(0);
            Rect.position =
                new Vector3(flask.GetDotRectForLiquidLine().position.x, flask.GetDotRectForLiquidLine().position.y, 0);


            var rect = Rect.transform.localEulerAngles;
            rect.z = -180;
            Rect.transform.localEulerAngles = rect;
        }
        public RectTransform GetRect() => Rect;
        public bool IsEnabledLineImage() => Line.enabled;
        public void SetFixedSizeDelta(Vector2 sizeDelta) => _fixedSizeDelta = sizeDelta;
        public Vector2 GetFixedSizeDelta() => _fixedSizeDelta; 
        public void SetFixedIndex(int index) => _fixedIndex = index;
        public int GetFixedIndex() => _fixedIndex;
    }
}
