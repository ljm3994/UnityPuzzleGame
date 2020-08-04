using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
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
