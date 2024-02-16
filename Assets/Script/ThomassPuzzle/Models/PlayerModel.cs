using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThomassPuzzle.Models
{
    public class PlayerModel
    {
        public static int CurrentLevel{
            get
            {
                return GetPlayerCurrentLvl();
            }
            set{
                SetPlayerCurrentLvl(value);
            } 
        }

        public static void SetPlayerCurrentLvl(int lvl)
        {
            PlayerPrefs.SetInt("CurrentLvl", lvl);
            PlayerPrefs.Save();
        }
        public static int GetPlayerCurrentLvl()
        {
            int crtLvl = PlayerPrefs.GetInt("CurrentLvl");
            if (crtLvl != 0)
                return crtLvl;
            else
                return 1;
        }
    }
}
