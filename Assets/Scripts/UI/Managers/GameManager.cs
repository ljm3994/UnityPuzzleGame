using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private StageInfo _CurentStage;
    static public GameManager instance;

    public StageInfo CurentStage { get => _CurentStage; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetBattleScene(StageInfo stageInfo)
    {
        _CurentStage = stageInfo;
        SceneManager.LoadScene("BattleScene");
    }
    
    public void SetLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
