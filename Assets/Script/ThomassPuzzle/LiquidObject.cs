using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Models;
namespace ThomassPuzzle
{
    public class LiquidObject : MonoBehaviour
    {
        #region Fields
        [SerializeField] Image Image;
        [SerializeField] bool Filled;
        private WaterColorEnum colorEnum;
        public Flask LastFlask;
        public static float delay = 0;
        #endregion

        #region Methods
        public void Fill(ColorModel colorModel, int fillAmount)
        {
            Image.color = colorModel.Color;
            name = colorModel.Name;
            gameObject.SetActive(true);
            Image.fillAmount = fillAmount;
            Filled = true;

            SetColorEnum(colorModel.ColorEnum);
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
                MinimizeHeigth(this);
            }

            Clear();
        }
        public IEnumerator MaximazeLiquid()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(delay == 0 ? .05f : delay);
                MaximazeHeigth(this);
            }
        }
        private void MinimizeHeigth(LiquidObject liquid)
        {
            liquid.GetImage().fillAmount -= 0.1f;
        }
        private void MaximazeHeigth(LiquidObject liquid)
        {
            liquid.GetImage().fillAmount += 0.1f;
        }

        #endregion 
    }
}