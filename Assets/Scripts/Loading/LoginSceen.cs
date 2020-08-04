using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoginSceen : MonoBehaviour
{
    [SerializeField]
    private bool bTest;

    private bool bTouch;

    public bool BTest { get => bTest; set => bTest = value; }

    // Start is called before the first frame update
    void Start()
    {
        bTouch = false;
    }
    public void ResponNext(bool Re)
    {
        if(Re)
        {
            SceneManager.LoadScene("Loadding");
        }
    }
    public void GetClick()
    {
        if (!BTest)
        {

            if (!bTouch)
            {
                bTouch = true;
                gpgsmanager.Instance.Signin(ResponNext);
                bTouch = false;
            }
        }
        else
        {

            if (!bTouch)
            {
                bTouch = true;
                PlayerDataManager.PlayerData.SkillDefalultData();
                PlayerDataManager.PlayerData.UnitDefaultData();
                PlayerDataManager.PlayerData.ItemDefaultData();
                PlayerDataManager.PlayerData.PlayerDataDefault();
                SceneManager.LoadScene("Loadding");
                bTouch = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
