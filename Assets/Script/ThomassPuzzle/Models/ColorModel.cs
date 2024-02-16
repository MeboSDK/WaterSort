using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThomassPuzzle.Enums;

namespace ThomassPuzzle.Models
{
    public class ColorModel
    {
        public string Name { get; set; }
        public Color Color {  get; set; }
        public WaterColorEnum ColorEnum{ get; set; }

    }
}
