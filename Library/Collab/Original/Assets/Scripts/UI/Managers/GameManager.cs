using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Queue<GameObject> PrevObject;
    public GameObject CurrentObject;
    static public GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        PrevObject = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
    }
    
    public void BackButton()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                if (PrevObject.Count > 0 && CurrentObject)
                {
                    SwapPanel(PrevObject.Dequeue());
                }
            }
        }
    }

    public void SwapPanel(GameObject target)
    {
        // In Screen
        // Panel A => Panel B
        UI_Panel panelA = CurrentObject.GetComponent<UI_Panel>();
        panelA.panelBack = null;
        UI_Panel panelB = target.GetComponent<UI_Panel>();

        Vector3 temp_pos;
        temp_pos = panelA.transform.localPosition;
        panelA.transform.localPosition = panelB.transform.localPosition;
        panelB.transform.localPosition = temp_pos;
        panelB.panelBack = panelA;

        UI_DATA.StartLoadHierarchy(panelB.transform);
    }

    public void SetBattleScene()
    {
        SceneManager.LoadScene("BattleScene");

    }
    
    public void SetLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
