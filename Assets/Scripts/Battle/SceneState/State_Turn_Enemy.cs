using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;

public class State_Turn_Enemy : IBattleState
{
    public bool ConsoleDebug = false;
    public enum TURN_ENEMY_STATE
    {
        SHOW_TURN_TEXT,
        CHECK_START_BUFF,
        ENEMY_ATTACK,
        CHECK_END_BUFF,
        END,
    }
    TURN_ENEMY_STATE mCurState;
    public TURN_ENEMY_STATE State { get => mCurState; }

    // Start is called before the first frame update
    public void Setup()
    {
        PuzzleManager.instance.Cover = true;
        ChangeTurnState(TURN_ENEMY_STATE.SHOW_TURN_TEXT);
    }


    public void ChangeTurnState(TURN_ENEMY_STATE state)
    {
        mCurState = state;
        switch (mCurState)
        {
            case TURN_ENEMY_STATE.SHOW_TURN_TEXT:
            {
                    CharacterManager.instance.Turn = CharacterManager.TURN.ENEMY;
                    BattleUIManager.instance.BattleUI.ShowStageText("Enemy Turn",()=> { ChangeTurnState(TURN_ENEMY_STATE.CHECK_START_BUFF); });
            }
            break;
            ///  EFFECT 공격 턴 시작 시 버프 효과 및 차감
            case TURN_ENEMY_STATE.CHECK_START_BUFF:
            {
                    CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.CHECK_START_BUFF,
                        () =>
                        {
                            ChangeTurnState(TURN_ENEMY_STATE.ENEMY_ATTACK);
                        });
            }
            break;
            case TURN_ENEMY_STATE.ENEMY_ATTACK:
            {
                    CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.ATTACK,()=>
                    {
                        CharacterManager.TURN isEnd = CharacterManager.instance.IsAllDie();
                        if (isEnd != CharacterManager.TURN.NONE)
                        {
                            BattleManager.instance.StageEnd(isEnd == CharacterManager.TURN.PLAYER ? CHARACTER_CAMP.ENEMY : CHARACTER_CAMP.PLAYER);
                        }
                        else
                        {
                            ChangeTurnState(TURN_ENEMY_STATE.CHECK_END_BUFF);
                        }
                    });
            }
            break;
            ///  EFFECT : 공격 턴 종료 시 버프 효과 및 차감
            case TURN_ENEMY_STATE.CHECK_END_BUFF:
            {
                    CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.CHECK_END_BUFF,
                        () => {
                            BattleManager.instance.ChangeState(EBattleState.TURN_PLAYER);
                        });
            }
            break;
            default:
                break;
        }
    }

    public void Changed()
    {
    }

}
