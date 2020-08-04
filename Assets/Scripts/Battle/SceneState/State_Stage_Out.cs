using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;

public class State_Stage_Out : IBattleState
{
    public bool ConsoleDebug = false;

    public enum STATE_STAGE_OUT
    {
        SHOW_STAGE_OUT_TEXT,
        STAGE_END,
        END
    }
    STATE_STAGE_OUT mCurState;
    public STATE_STAGE_OUT State { get => mCurState; }

    public CHARACTER_CAMP camp_win;
    public void SetWinSide(CHARACTER_CAMP side) { camp_win = side; }

    // Start is called before the first frame update
    public void Setup()
    {
        PuzzleManager.instance.Cover = true;
        // 적 승리
        if (camp_win == CHARACTER_CAMP.ENEMY)
        {
            // 적 승리 팝업
            BattleUIManager.instance.Popup("StageLosePopup", 0);
            return;
        }

        {
            /// TODO : Item Drop
            /// 
            BattleDataManager.instance.ItemDrop();
        }

        // 플레이어 최종 STAGE까지 승리
        if(BattleManager.instance.STAGE_MAX <= BattleManager.instance.StageNum)
        {
            // 플레이어 승리 팝업
            BattleUIManager.instance.Popup("StageFinishPopup", 0);
            return;
        }

        // 다음 Stage로 이동
        BattleManager.instance.StageNum++;
        CharacterManager.instance.ClearCharacters(CHARACTER_CAMP.ENEMY);

        BattleUIManager.instance.Popup("StageEndPopup", 0);
    }


    public void Changed()
    {
    }

}
