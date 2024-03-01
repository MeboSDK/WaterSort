using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Models;
using TMPro;
using ThomassPuzzle.Helpers;
namespace ThomassPuzzle
{
    public class LiquidObject : MonoBehaviour
    {
        #region Fields
        [SerializeField] Image Image;
        [SerializeField] bool Filled;
        [SerializeField] GameObject QuestionImage;
        [SerializeField] RectTransform Rect;
        private WaterColorEnum colorEnum;
        public Flask LastFlask;
        public static float delay = 0;
        #endregion

        #region Methods
        public void Fill(ColorModel colorModel, int fillAmount, bool hided = false)
        {
            Image.color = colorModel.Color;
            gameObject.SetActive(true);
            Image.fillAmount = fillAmount;
            Filled = true;

            if (!hided)
            {
                name = colorModel.Name;
                SetColorEnum(colorModel.ColorEnum);
            }
        }

        public void Clear()
        {
            Filled = false;
            Image.color = Color.white;
            SetColorEnum(WaterColorEnum.None);
            gameObject.SetActive(false);
            Image.fillAmount = 0;
        }
        public bool IsFilled()
        {
            return Filled;
        }
        public void SetColorEnum(WaterColorEnum colorEnum)
        {
            this.colorEnum = colorEnum;
        }
        public WaterColorEnum GetColorEnum()
        {
            return colorEnum;
        }
        public Image GetImage()
        {
            return Image;
        }
        public IEnumerator MinimizeLiquid()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(.05f);
                MinimizeHeight(this);
            }

            Clear();
        }
        public IEnumerator MaximizeLiquid()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(delay == 0 ? .05f : delay);
                MaximizeHeight(this);
            }
        }
        private void MinimizeHeight(LiquidObject liquid)
        {
            liquid.GetImage().fillAmount -= 0.1f;
        }
        private void MaximizeHeight(LiquidObject liquid)
        {
            liquid.GetImage().fillAmount += 0.1f;
        }
        public void ShowQuestions(bool show)
        {
            if (show)
                QuestionImage.SetActive(true);
            else
                QuestionImage.SetActive(false);
        }

        public void ShowLiquidObject(bool show)
        {
            if (show)
            {
                var color = ColorsHelper.GetColor(GetColorEnum());

                Fill(color, 1, false);
                ShowQuestions(false);

            }
            else
            {
                var color = ColorsHelper.GetColor(WaterColorEnum.Hide);
                Fill(color, 1, true);
                ShowQuestions(true);
            }
        }

        public RectTransform GetRect()
        {
            return Rect;
        }
        #endregion 
    }
}