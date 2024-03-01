using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ThomassPuzzle.Enums;
using ThomassPuzzle.Helpers.Base;
using UnityEngine;
using ThomassPuzzle.Extensions;
using ThomassPuzzle.Models;
using TMPro;
namespace ThomassPuzzle.Helpers
{
    public class ColorsHelper
    {
        #region Fields
        private WaterColorEnum[] NotDuplicatedColors;

        public List<WaterColorEnum> DuplicatedColors;

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates colors for flasks. for example if flasks count is 5 method returns (5 * 4)
        /// </summary>
        /// <param name="flasksCount">Count of not empty flasks</param>
        /// <returns></returns>
        public List<WaterColorEnum> ColorsDevelopment(bool isEmptyFlasks, int flasksCount)
        {
            if (!isEmptyFlasks)
                return GetRandomColors(flasksCount);
            else
                return new List<WaterColorEnum> { };
        }

        public List<WaterColorEnum> ColorsForFlask(List<int> enumNumbers)
        {
            List<WaterColorEnum> colors = new List<WaterColorEnum>();

            for (int i = 0; i < enumNumbers.Count; i++)
                colors.Add((WaterColorEnum)enumNumbers[i]);

            return colors;
        }

        public IEnumerable<int> GetColorsIndexes(List<WaterColorEnum> chosenColors)
        {
            for (int i = 0; i < 4; i++)
            {
                var randomizedColor = chosenColors.GetRandom();
                chosenColors.Remove(randomizedColor);
                yield return (int)randomizedColor;
            }
        }
        public static ColorModel GetColor(WaterColorEnum chosenColor)
        {
            ColorModel flask = new ColorModel();

            flask.Name = Enum.GetName(typeof(WaterColorEnum), chosenColor);

            flask.Color = GetChosenColor(chosenColor);

            flask.ColorEnum = chosenColor;

            return flask;
        }


        #endregion

        #region Private Methods
        private static Color GetChosenColor(WaterColorEnum color)
        {
            switch (color)
            {
                case WaterColorEnum.BlueJeans:
                    return FindColorByHex("#64bbd9");
                case WaterColorEnum.DarkTangerine:
                    return FindColorByHex("#f2a715");
                case WaterColorEnum.LightCoral:
                    return FindColorByHex("#e58688");
                case WaterColorEnum.Shandy:
                    return FindColorByHex("#fce46a");
                case WaterColorEnum.RoseQuartzPink:
                    return FindColorByHex("#bf509c");
                case WaterColorEnum.Asparagus:
                    return FindColorByHex("#80b07b");
                case WaterColorEnum.MacaroniAndCheese:
                    return FindColorByHex("#f6b690");
                case WaterColorEnum.Iris:
                    return FindColorByHex("#5058bf");
                case WaterColorEnum.AmericanBrown:
                    return FindColorByHex("#7d353f");
                case WaterColorEnum.WintergreenDream:
                    return FindColorByHex("#5b7981");
                case WaterColorEnum.DesertSand:
                    return FindColorByHex("#ecd2af");
                case WaterColorEnum.Axolotl:
                    return FindColorByHex("#587050");
                case WaterColorEnum.MaximumBluePurple:
                    return FindColorByHex("#a1a3fe");
                case WaterColorEnum.LightPastelPurple:
                    return FindColorByHex("#B4C9A1");
                case WaterColorEnum.Topaz:
                    return FindColorByHex("#ffc76e");
                case WaterColorEnum.BurntSienna:
                    return FindColorByHex("#f07853");
                case WaterColorEnum.BittersweetShimmer:
                    return FindColorByHex("#b95159");
                case WaterColorEnum.Oxley:
                    return FindColorByHex("#6c9482");
                case WaterColorEnum.Brass:
                    return FindColorByHex("#a2a344");
                case WaterColorEnum.Hide:
                    return FindColorByHex("#808080");
                default:
                    return Color.white;
            }
        }
        private static Color FindColorByHex(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color outColor);
            return outColor;
        }
        private WaterColorEnum CreateRandomColor()
        {
            WaterColorEnum color;
            do
            {
                color = RandomColor();
                
                while (color == WaterColorEnum.Hide)
                    color = RandomColor();

                if (!NotDuplicatedColors.Contains(color))
                    return color;
            }
            while (NotDuplicatedColors.Any(o => o == color));

            return WaterColorEnum.None;
        }
        private List<WaterColorEnum> GetRandomColors(int flasksCount)
        {
            DuplicatedColors = new List<WaterColorEnum>();
            NotDuplicatedColors = new WaterColorEnum[flasksCount];

            for (int i = 0; i < NotDuplicatedColors.Length; i++)
            {
                NotDuplicatedColors[i] = CreateRandomColor();

                for (int j = 0; j < 4; j++)
                    DuplicatedColors.Add(NotDuplicatedColors[i]);

            }
            return DuplicatedColors;
        }
        private WaterColorEnum RandomColor() => RandomationEnum.GetRandomEnum<WaterColorEnum>();

        #endregion
    }
}
