using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoMenuUnitController : MonoBehaviour
{
    public Text LevelUpCountText;

    public void Load()
    {
        int Count = UIDataProcess.GetUnitInventory().GetUnitLevelUpCount();

        if(Count <= 0)
        {
            LevelUpCountText.text = "";
        }
        else
        {
            LevelUpCountText.text = Count.ToString();
        }
    }
}
