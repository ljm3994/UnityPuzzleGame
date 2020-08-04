using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCommon;
using BattleUnit;

public class BattleManager : MonoBehaviour
{
    public bool ConsoleDebug = false;
    public static BattleManager instance;
    

    IBattleState        mCurrentState;
    State_Stage_In      mState_stage_in;
    State_Stage_Out     mState_stage_out;
    State_Turn_Player   mState_turn_player;
    State_Turn_Enemy    mState_turn_enemy;

    
    
    public int STAGE_MAX = 5;
    private Observe<int> numStage = new Observe<int>(0);
    public int StageNum { get => numStage.Value; set => numStage.Value =  Mathf.Clamp(value, 0, STAGE_MAX); }

    public int LINK_MAX = 3;
    public Observe<int> numLink = new Observe<int>(0);
    public int LinkNum { get => numLink.Value; set => numLink.Value = Mathf.Clamp(value, 0, LINK_MAX); }

    public int MANA_GAUGE_MAX = 100;
    public Observe<int> mana = new Observe<int>(0);
    public int Mana { get => mana.Value; set => mana.Value = Mathf.Clamp(value, 0, MANA_GAUGE_MAX); }

    public int MANABUBBLE_MAX = 3;
    public Observe<int> manaBubble = new Observe<int>(0);
    public int ManaBubble { get => manaBubble.Value; set => manaBubble.Value =  Mathf.Clamp(value, 0, MANABUBBLE_MAX); }


    private void Awake()
    {
        instance = this;

        mState_stage_in = new State_Stage_In();
        mState_stage_out = new State_Stage_Out();
        mState_turn_player = new State_Turn_Player();
        mState_turn_enemy = new State_Turn_Enemy();

        numStage.Value = 0;
        numLink.Value = LINK_MAX;
        mana.Value = 0;
        manaBubble.Value = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        BattleDataManager.instance.LoadBattleStage();

        Setup();
        //Invoke("Setup",1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            string logString = "Current Battle State : ";

            if(mCurrentState.Equals(mState_stage_in))
            {
                logString += "Stage In";
                logString += " State : " + mState_stage_in.State;

            }
            else if(mCurrentState.Equals(mState_stage_out))
            {
                logString += "Stage Out";
                logString += " State : " + mState_stage_out.State;
            }
            else if(mCurrentState.Equals(mState_turn_player))
            {
                logString += "Stage Turn Player";
                logString += " State : " + mState_turn_player.State;
            }
            else if(mCurrentState.Equals(mState_turn_enemy))
            {
                logString += "Stage Turn Enemy";
                logString += " State : " + mState_turn_enemy.State;
            }

            logString += "\n PuzzleManager State : " + PuzzleManager.instance.State + "\n CharacterManager State : " + CharacterManager.instance.State;

            Debug.Log(logString);
        }
    }

    public void SetLobbyScene()
    {
        GameManager.instance.SetLobbyScene();
        numStage.Value = 0;
    }
    
    void Setup()
    {
        BattleUIManager.instance.CentralUI.SetActive(false);
        ChangeState(EBattleState.STAGE_IN);
    }

    public void ChangeState(EBattleState state)
    {
        switch(state)
        {
            case EBattleState.STAGE_IN:
                mCurrentState = mState_stage_in;
            break;
            case EBattleState.STAGE_OUT:
                mCurrentState = mState_stage_out;
            break;
            case EBattleState.TURN_ENEMY:
                mCurrentState = mState_turn_enemy;
            break;
            case EBattleState.TURN_PLAYER: 
                mCurrentState = mState_turn_player;
            break;
        }
        mCurrentState.Setup(); 
        mCurrentState.Changed();
    }

    public void StageEnd(CHARACTER_CAMP camp_win)
    {
        mState_stage_out.SetWinSide(camp_win);
        ChangeState(EBattleState.STAGE_OUT);
    }

}
