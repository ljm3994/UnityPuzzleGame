using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;

public class State_Turn_Player : IBattleState
{
    public bool ConsoleDebug = false;
    /*
     * **반복 (링크 횟수 만큼)**
     * 퍼즐 채우기 (PUZZLE_FILL)
     * 캐릭터 스킬 띄우기 
     * 퍼즐 맞추기
     * 퍼즐 - 스킬 매칭
     * 공격
     * **반복 끝**
     * Battle State 변경(Enemy-Turn)
     */
    public enum TURN_PLAYER_STATE
    {
        SHOW_TURN_TEXT,
        CHECK_START_BUFF,
        FILL_PUZZLE,
        GENERATE_CHARACTER_GEM,
        PUZZLE_SKILL_ITEM_TURN,
        CHARACTER_ATTACK,
        CHECK_END_BUFF,
        END,
    }
    TURN_PLAYER_STATE turn_player_state;
    public TURN_PLAYER_STATE State { get => turn_player_state; }


    // Start is called before the first frame update
    public void Setup()
    {
        PuzzleManager.instance.Cover = false;
        ChangeTurnState(TURN_PLAYER_STATE.SHOW_TURN_TEXT);
    }


    public void ChangeTurnState(TURN_PLAYER_STATE state)
    {
        turn_player_state = state;

        switch (state)
        {
            case TURN_PLAYER_STATE.SHOW_TURN_TEXT:
            {
                BattleManager.instance.LinkNum = BattleManager.instance.LINK_MAX;
                //numLink = LINK_MAX;
                //BattleManager.instance.mBattleUI.SetNumLinkText(numLink + 1);
                CharacterManager.instance.Turn = CharacterManager.TURN.PLAYER;
                BattleUIManager.instance.BattleUI.ShowStageText("Player Turn", 
                    ()=> {
                        ChangeTurnState(TURN_PLAYER_STATE.CHECK_START_BUFF);
                    });
            }
            break;
            ///  EFFECT : 공격 턴 시작 시 버프 효과 및 차감
            case TURN_PLAYER_STATE.CHECK_START_BUFF:
            {
                CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.CHECK_START_BUFF,
                    () =>
                    {
                        ChangeTurnState(TURN_PLAYER_STATE.FILL_PUZZLE);
                    });
            }
            break;
            case TURN_PLAYER_STATE.FILL_PUZZLE:
            {
                PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.FILL, 
                    () => {
                    ChangeTurnState(TURN_PLAYER_STATE.GENERATE_CHARACTER_GEM);
                });
            }
            break;
            case TURN_PLAYER_STATE.GENERATE_CHARACTER_GEM:
            {
                PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.WAIT,null);
                CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.SPAWN_GEM,
                    ()=> {
                    ChangeTurnState(TURN_PLAYER_STATE.PUZZLE_SKILL_ITEM_TURN);
                });
            }
            break;
            case TURN_PLAYER_STATE.PUZZLE_SKILL_ITEM_TURN:
            {
                BattleUIManager.instance.CentralUI.SetActive(true);
                PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.MATCH,
                    ()=> {
                        CharacterManager.instance.ClearEndAction(CharacterManager.CHARACTER_STATE.WAIT);
                        ChangeTurnState(TURN_PLAYER_STATE.CHARACTER_ATTACK);
                    });
                CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.WAIT,
                    ()=>
                    {
                        PuzzleManager.instance.ClearEndAction(PuzzleManager.PUZZLE_STATE.MATCH);
                        ChangeTurnState(TURN_PLAYER_STATE.CHARACTER_ATTACK);
                    });
            }
            break;
            case TURN_PLAYER_STATE.CHARACTER_ATTACK:
            {
                BattleUIManager.instance.CentralUI.SetActive(false);
                PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.FILL,
                    ()=> {
                        PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.WAIT, null);
                    }
                    );
                CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.ATTACK,
                    () => {
                        CharacterManager.instance.ClearRecipes();

                        CharacterManager.TURN isEnd = CharacterManager.instance.IsAllDie();
                        if (isEnd != CharacterManager.TURN.NONE)
                        {
                            BattleManager.instance.StageEnd(isEnd == CharacterManager.TURN.PLAYER ? CHARACTER_CAMP.ENEMY : CHARACTER_CAMP.PLAYER);
                        }
                        else
                        {
                            BattleManager.instance.LinkNum--;
                            if (BattleManager.instance.LinkNum <= 0)
                            {
                                ChangeTurnState(TURN_PLAYER_STATE.CHECK_END_BUFF);
                            }
                            else
                            {
                                ChangeTurnState(TURN_PLAYER_STATE.FILL_PUZZLE);
                            }
                        }
                    });
            }
            break;
            ///  EFFECT : 공격 턴 종료 시 버프 효과 및 차감
            case TURN_PLAYER_STATE.CHECK_END_BUFF:
            {
                    CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.CHECK_END_BUFF,
                        ()=>
                        {
                            BattleManager.instance.ChangeState(EBattleState.TURN_ENEMY);
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
