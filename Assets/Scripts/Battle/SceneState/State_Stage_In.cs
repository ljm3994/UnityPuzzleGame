using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stage_In : IBattleState
{
    public bool ConsoleDebug = false;
    /*
     * 캐릭터 생성
     */
    public enum STAGE_IN_STATE
    {
        MAIN_CHARACTER_ENTERNECE,
        SHOW_STAGE_TEXT,
        MAIN_CHARACTER_WALKOUT,
        UNIT_ENTERENCE,
        END,
    }

    STAGE_IN_STATE stage_in_state;
    public STAGE_IN_STATE State { get => stage_in_state; }

    public void Setup()
    {
        PuzzleManager.instance.Cover = true;
        ChangeState(STAGE_IN_STATE.MAIN_CHARACTER_ENTERNECE);
    }


    public void ChangeState(STAGE_IN_STATE state)
    {
        stage_in_state = state;
        switch (state) 
        {
            case STAGE_IN_STATE.MAIN_CHARACTER_ENTERNECE:
                CharacterManager.instance.LoadStageInfoFromDataBase(BattleManager.instance.StageNum);
                StageManager.instance.SetNextStage(() => { ChangeState(STAGE_IN_STATE.SHOW_STAGE_TEXT); });
                break;
            case STAGE_IN_STATE.SHOW_STAGE_TEXT:
                BattleUIManager.instance.BattleUI.ShowStageText("STAGE IN", () => { ChangeState(STAGE_IN_STATE.UNIT_ENTERENCE); });
                break;
            case STAGE_IN_STATE.UNIT_ENTERENCE:
                CharacterManager.instance.ShowGaugeBar(true);
                CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.PLAYER_UNIT_ENTERENCE, 
                    () => {
                    BattleManager.instance.ChangeState(EBattleState.TURN_PLAYER);
                });
            break;
            default:
            break;
        }
    }
    

    public void Changed()
    {
    }

}
