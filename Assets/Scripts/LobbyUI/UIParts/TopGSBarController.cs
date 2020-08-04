using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopGSBarController : MonoBehaviour
{
    public Text Gold;
    public Text Stemina;
    public Text Timer;

    // Start is called before the first frame update
    void Start()
    {
    }
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Load();
    }

    public void Load()
    {
        int max = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina;
        int Current = PlayerDataManager.PlayerData.Pdata.iStamina;
        Gold.text = PlayerDataManager.PlayerData.Pdata.iCoin.ToString();
        Stemina.text = Current.ToString() + "/" + max.ToString();
        int Time = SteminaManager.Instance.SteminaChargeTimer;
        if (Time == 0 && Current == max)
        {
            Timer.text = "MAX";
        }
        else
        {
            Timer.text = (Time / 60).ToString() + "m " + (Time % 60).ToString() + "s";
        }
    }
}
