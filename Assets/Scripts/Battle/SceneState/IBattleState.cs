using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EBattleState
{
    STAGE_IN,
    TURN_PLAYER,
    TURN_ENEMY,
    STAGE_OUT
}

public delegate void ChangeState(EBattleState battle_state);

public interface IBattleState
{
    void Setup();
    
    void Changed();

}
