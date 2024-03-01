using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject ExitPanel;
    public GameObject Pauseicon;
    public GameObject TopPanel;

   public void ExitpopUp()
   {
     ExitPanel.SetActive(true);
     Pauseicon.SetActive(false);
     Time.timeScale = 0;
   }

   public void CloseExitPopUp()
   {
    ExitPanel.SetActive(false);
    Pauseicon.SetActive(true);
    Time.timeScale = 1;
  }

   public void Leavebutton()
   {
        //Tornikes dasaweria aq
   }

   public void Levelfailed()
   {
    ExitPanel.SetActive(false);
    Pauseicon.SetActive(false);
    TopPanel.SetActive(false);
   }

}
