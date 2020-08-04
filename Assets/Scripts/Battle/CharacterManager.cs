using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;
using BattleCommon;

public class CharacterManager : MonoBehaviour
{
    public bool ConsoleDebug = false;
    public float UnitEnterenceTime = 2;

    public enum CHARACTER_STATE
    {
        PLAYER_UNIT_ENTERENCE,
        PLAYER_UNIT_WALKOUT,
        SPAWN_GEM,
        ATTACK,
        WAIT,
        CHECK_START_BUFF,
        CHECK_END_BUFF
    }

    public static CharacterManager instance;

    CHARACTER_STATE character_state;
    public CHARACTER_STATE State { get => character_state; }
    public delegate void StateEndAction();
    Dictionary<CHARACTER_STATE, StateEndAction> mStateEndActions;
    
    public enum TURN
    {
        PLAYER,
        ENEMY,
        NONE,
    }

    TURN turn;
    public TURN Turn { get => turn; set => turn = value; }

    List<Unit> units;
    List<Unit> enemies;

    public List<int> GetIDs(CHARACTER_CAMP camp)
    {
        List<int> ids = new List<int>();
        var targets = camp == CHARACTER_CAMP.PLAYER ? units : enemies;

        foreach (var item in targets)
        {
            if(item == null)
            {
                ids.Add(-1);
            }
            else
            {
                ids.Add(item.mStatus.unitID);
            }
        }
        return ids;
    }

    public Unit GetUnit(int index) { return (index < 5) ? units[index] : enemies[index-5]; }

    Queue<int> attack_queue;
    Queue<Unit> die_queue;

    
    private void Awake()
    {
        instance = this;

        units = new List<Unit>(5);
        enemies = new List<Unit>(5);

        for (int i = 0; i < 5; i++)
        {
            units.Add(null);
            enemies.Add(null);
        }

        attack_queue = new Queue<int>();
        die_queue = new Queue<Unit>();
    }

    public void LoadStageInfoFromDataBase(int stageNum)
    {
        List<UnitInitData> datas;
        //StageInfo stage = GameManager.instance.CurentStage;

        if (stageNum == 0)
        {
            // Character -> stage 0 한정
            datas = BattleDataManager.instance.LoadPlayerUnits();

            foreach (var data in datas)
            {
                GameObject go = Resources.Load(data.prefabPath) as GameObject;
                Unit character = GameObject.Instantiate(go).GetComponent<Unit>();
                character.name += data.position_index;

                character.SetUpCharacter(data.camp, StageManager.instance.UnitOut[data.position_index].position, data);
                
                units[data.position_index] = character;
                BattleUIManager.instance.BattleUI.AddUnitButton(data.position_index, character.mUIController.button);
            }

        }

        // Enemey
        datas = BattleDataManager.instance.LoadEnemyUnits(stageNum);
        foreach (var data in datas)
        {
            GameObject go = Resources.Load(data.prefabPath) as GameObject;
            Unit character = GameObject.Instantiate(go,StageManager.instance.EnemyPositions[stageNum][data.position_index]).GetComponent<Unit>();
            character.name += data.position_index;
            character.SetUpCharacter(data.camp, Vector3.zero,data);

            enemies[data.position_index] = character;
            BattleUIManager.instance.BattleUI.AddUnitButton(data.position_index + 5, character.mUIController.button);
        }

    }



    #region About Recipes

    public void ShowGaugeBar(bool b)
    {
        foreach (var unit in units)
        {
            if (unit == null) continue;
            unit.ShowGaugeBar(b);
        }

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.ShowGaugeBar(b);
        }
    }

    IEnumerator GenerateRecipes()
    {
        for (int i = 0; i < units.Count; ++i)
        {
            if (units[i] == null) continue;

            yield return StartCoroutine(units[i].mStatus.puzzleRecipe.GenerateRandom(units));
        }

        SetStateEnd();
    }
    public void ClearRecipes()
    {
        foreach (var unit in units)
        {
            if (unit == null) continue;

            unit.mStatus.puzzleRecipe.ClearRecipe();
        }
    }

    public List<Unit> CheckPuzzle(List<List<int>> selected_nodes)
    {
        List<Unit> character_match = new List<Unit>();
        bool[] checks = new bool[units.Count];
        for (int i = 0; i < checks.Length; i++)
            checks[i] = false;

        var tiles = PuzzleManager.instance.tiles;

        foreach (var nodes in selected_nodes)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] == null) continue;
                if (checks[i]) continue;

                bool match = units[i].mStatus.puzzleRecipe.CheckRecipe(tiles[nodes[0]].node.Type, tiles[nodes[1]].node.Type, tiles[nodes[2]].node.Type);    
                if (match)
                {
                    /// TODO:  mana 증가
                    if(BattleManager.instance.ManaBubble != BattleManager.instance.MANABUBBLE_MAX)
                        BattleManager.instance.Mana += 20;

                    checks[i] = true;
                    character_match.Add(units[i]);
                    attack_queue.Enqueue(i);
                    break;
                }
            }
        }

        bool matching = false;
        foreach (var check in checks)
        {
            if(check)
            {
                matching = true;
                break;
            }
        }

        if (!matching) character_match.Clear();

        return character_match;
    }

    #endregion

    #region UnitAttack

    IEnumerator UnitAttack(TURN turn)
    {
        List<Unit> attackers;
        List<Unit> targets;
        if (TURN.PLAYER == turn)
        {
            attackers = units;
            targets = enemies;
        }
        else
        {
            int numAttack = Random.Range(GameManager.instance.CurentStage.IStageMonsterMinAtkNum, GameManager.instance.CurentStage.IStageMonsterMaxAtkNum+1);

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;

                attack_queue.Enqueue(i);

                if (attack_queue.Count == numAttack) break;
            }
            attackers = enemies;
            targets = units;
        }

        while (attack_queue.Count != 0)
        {
            Unit attacker = attackers[attack_queue.Dequeue()];

            while (die_queue.Count != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            if(attacker.mStatus.ContainsBuff(EFFECT.FAINT_EFFECT))
            {
                attacker.mUIController.GenerateBuffFont(EFFECT.FAINT_EFFECT);
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                yield return StartCoroutine(attacker.Attack());
            }
        }

        while(die_queue.Count != 0)
        {
            yield return new WaitForEndOfFrame();
        }

        SetStateEnd();
    }
    
    public List<Unit> FindTarget(Unit caster,EFFECT effectID, TARGET targetType, TARGETOBJECT targetObjType, RANGE rangeType)
    {
        List<Unit> targets;
        switch (targetType)
        {
            case TARGET.FRIENDLY_TARGET:
                targets = (Turn == TURN.PLAYER) ? units : enemies;
            break;
            case TARGET.ENEMY_TARGET:
                targets = (Turn == TURN.PLAYER) ? enemies : units;
                break;
            case TARGET.ALL_TARGET:
                targets = new List<Unit>();
                targets.AddRange(units);
                targets.AddRange(enemies);
            break;
            default:
                return null;
            break;
        }

        return FindTarget(caster,targets, effectID, targetObjType,rangeType);
    }

    List<Unit> FindTarget(Unit caster, List<Unit> targets, EFFECT effectID, TARGETOBJECT targetObjType, RANGE rangeType)
    {
        List<Unit> selected_target = new List<Unit>();
        switch (rangeType)
        {
            case RANGE.SINGLE_RANGE:
                {
                    Unit unit = null;
                    
                    if(caster != null)
                    {
                        if (effectID != EFFECT.HEAL_EFFECT && effectID != EFFECT.REINCARNATION_EFFECT)
                        {
                            List<Unit> highPrioritys = new List<Unit>();
                            foreach (var item in targets)
                            {
                                if (item == null) continue;
                                if (item == caster) continue;

                                if (item.mStatus.ContainsBuff(EFFECT.CONCENTRATION_EFFECT))
                                {
                                    highPrioritys.Add(item);
                                }
                            }

                            if (highPrioritys.Count > 0)
                            {
                                selected_target.Add(highPrioritys[Random.Range(0, highPrioritys.Count)]);
                                return selected_target;
                            }
                        }
                    }

                    List<Unit> nNullUnit = new List<Unit>();
                    foreach (var item in targets)
                    {
                        if (item == null) continue;

                        nNullUnit.Add(item);
                    }

                    int whileCount = 0;
                    while(true)
                    {
                        whileCount++;
                        switch (targetObjType)
                        {
                            case TARGETOBJECT.NORMAL_TARGETOBJ:
                                {
                                    unit = nNullUnit[Random.Range(0, nNullUnit.Count)];
                                }
                                break;
                            case TARGETOBJECT.LOWHEALTH_TARGETOBJ:
                                {
                                    foreach (var item in targets)
                                    {
                                        if (item == null) continue;
                                        if (item == caster) continue;

                                        if(unit == null)
                                        {
                                            unit = item;
                                            continue;
                                        }

                                        if(item.mStatus.HP <= unit.mStatus.HP)
                                        {
                                            unit = item;
                                        }
                                    }
                                }
                                break;
                            case TARGETOBJECT.HIGHHEALTH_TARGETOBJ:
                                {
                                    foreach (var item in targets)
                                    {
                                        if (item == null) continue;
                                        if (item == caster) continue;

                                        if (unit == null)
                                        {
                                            unit = item;
                                            continue;
                                        }

                                        if (item.mStatus.HP >= unit.mStatus.HP)
                                        {
                                            unit = item;
                                        }
                                    }
                                }
                                break;
                            default:
                                return null;
                            break;
                        }

                        if (whileCount > 100) break;

                        if(unit != null)
                        {
                            if(caster == null) // playerSkill
                            {
                                break;
                            }
                            else
                            {
                                if(unit != caster && unit.mStatus.ContainsBuff(EFFECT.STEALTH_EFFECT) == false)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    selected_target.Add(unit);
                    return selected_target;
                }
                break;
            case RANGE.MULTI_RANGE:
                {
                    switch (targetObjType)
                    {
                        case TARGETOBJECT.FRONTLINE_TARGETOBJ:
                            {
                                for(int i = 0; i< targets.Count; ++i)
                                {
                                    if (i % 5 != 4) continue;

                                    /// ???? 모두
                                    if(targets[i] != null /*&& targets[i] != caster*/)
                                    {
                                        if(effectID != EFFECT.HEAL_EFFECT && effectID != EFFECT.REINCARNATION_EFFECT)
                                        {
                                            if (targets[i].mStatus.ContainsBuff(EFFECT.STEALTH_EFFECT) && caster != null) continue;
                                        }

                                        selected_target.Add(targets[i]);
                                    }
                                }
                            }
                            break;
                        case TARGETOBJECT.MIDDLELINE_TARGETOBJ:
                            {
                                for(int i = 0; i< targets.Count; ++i)
                                {
                                    if (i % 5 != 2 && i % 5 != 3) continue;

                                    if(targets[i] != null /*&& targets[i] != caster*/)
                                    {
                                        if (effectID != EFFECT.HEAL_EFFECT && effectID != EFFECT.REINCARNATION_EFFECT)
                                        {
                                            if (targets[i].mStatus.ContainsBuff(EFFECT.STEALTH_EFFECT) && caster != null) continue;
                                        }

                                        selected_target.Add(targets[i]);
                                    }
                                }
                            }
                            break;
                        case TARGETOBJECT.ENDLINE_TARGETOBJ:
                            {
                                for(int i = 0; i< targets.Count; ++i)
                                {
                                    if (i % 5 != 0 && i % 5 != 1) continue;

                                    if(targets[i] != null /*&& targets[i] != caster*/)
                                    {
                                        if (effectID != EFFECT.HEAL_EFFECT && effectID != EFFECT.REINCARNATION_EFFECT)
                                        {
                                            if (targets[i].mStatus.ContainsBuff(EFFECT.STEALTH_EFFECT) && caster != null) continue;
                                        }

                                        selected_target.Add(targets[i]);
                                    }
                                }
                            }
                            break;
                        default:
                            {
                                foreach (var item in targets)
                                {
                                    if (item == null) continue;
                                    if (effectID != EFFECT.HEAL_EFFECT && effectID != EFFECT.REINCARNATION_EFFECT)
                                    {
                                        if (item.mStatus.ContainsBuff(EFFECT.STEALTH_EFFECT) && caster != null) continue;
                                    }

                                    selected_target.Add(item);
                                }
                            }
                            break;
                    }

                    return selected_target;
                }
                break;
            default:
                return null;
            break;
        }
    }

    #endregion

    #region About Unit

    IEnumerator PlayerUnitMove(CHARACTER_STATE state)
    {

        for (int i = 0; i < 5; ++i)
        {
            if (units[i] == null) continue;
            if (state == CHARACTER_STATE.PLAYER_UNIT_ENTERENCE)
                StartCoroutine(units[i].MoveToPos(StageManager.instance.UnitIn[i].position, UnitEnterenceTime));
            else if (state == CHARACTER_STATE.PLAYER_UNIT_WALKOUT)
                StartCoroutine(units[i].MoveToPos(StageManager.instance.UnitOut[i].position, UnitEnterenceTime));
        }

        yield return new WaitForSeconds(UnitEnterenceTime + 0.5f);

        SetStateEnd();
    }

    IEnumerator ExecuteBuff(BUFFTIME time)
    {
        var targets = Turn == TURN.PLAYER ? units : enemies;

        for(int i = 0; i< targets.Count; ++i)
        {
            if (targets[i] == null) continue;

            yield return (targets[i] == null) ? null : StartCoroutine(targets[i].mStatus.ExecuteBuff(time));
        }

        SetStateEnd();
    }

    public IEnumerator DecreaseSkillBubble(TARGET targetType, int value)
    {
        List<Unit> targets = null;
        switch (targetType)
        {
            case TARGET.FRIENDLY_TARGET:
                targets = (Turn == TURN.PLAYER) ? units : enemies;
                break;
            case TARGET.ENEMY_TARGET:
                targets = (Turn == TURN.PLAYER) ? enemies : units;
                break;
            case TARGET.ALL_TARGET:
                targets = new List<Unit>();
                targets.AddRange(units);
                targets.AddRange(enemies);
                break;
            default:
                break;
        }

        if(targets != null)
        {
            foreach (var item in targets)
            {
                if (item == null) continue;

                for (int i = 0; i < value; ++i)
                {
                    item.mStatus.SkillBubble--;
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }

    #endregion

    #region PlayerSkill&Item
    public IEnumerator FirePlayerEffect(ItemInfo item)
    {
        switch ((EFFECT)item.IEffectID)
        {
            case EFFECT.HEAL_EFFECT:
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        var unit = units[i];
                        if (unit == null) continue;

                        unit.mStatus.GetHeal(unit.mStatus.HP * (item.IEffectValue / 100f));
                        yield return new WaitForSeconds(0.2f);
                    }
                    SetStateEnd();
                }
                break;
            case EFFECT.SKILLGAUGEDECREASE_EFFECT:
                {
                    yield return StartCoroutine(DecreaseSkillBubble(item.Target, item.IEffectValue));
                    SetStateEnd();
                }
                break;
            case EFFECT.MANABUBBLEINCREASE_EFFECT:
                {
                    for (int i = 0; i < item.IEffectValue; ++i)
                    {
                        BattleManager.instance.ManaBubble += 1;
                        yield return new WaitForSeconds(0.5f);
                    }
                    SetStateEnd();
                }
                break;
            case EFFECT.EQUALITY_EFFECT:
                {
                    if (item.Target == TARGET.ALL_TARGET)
                    {
                        float averHP = 0;
                        int count = 0;
                        for (int i = 0; i < 5; ++i)
                        {
                            var unit = units[i];
                            if (unit == null) continue;
                            count++;
                            averHP += unit.mStatus.HP;
                        }
                        averHP /= count;

                        for (int i = 0; i < 5; i++)
                        {
                            var unit = units[i];
                            if (unit == null) continue;
                            
                            if(unit.mStatus.HP > averHP)
                            {
                                unit.mStatus.GetDamage(unit.mStatus.HP - averHP);
                            }
                            else
                            {
                                unit.mStatus.GetHeal(averHP - unit.mStatus.HP);
                            }
                        }

                        averHP = 0;
                        count = 0;
                        for (int i = 0; i < 5; ++i)
                        {
                            var unit = enemies[i];
                            if (unit == null) continue;
                            count++;
                            averHP += unit.mStatus.HP;
                        }
                        averHP /= count;

                        for (int i = 0; i < 5; i++)
                        {
                            var unit = enemies[i];
                            if (unit == null) continue;

                            if (unit.mStatus.HP > averHP)
                            {
                                unit.mStatus.GetDamage(unit.mStatus.HP - averHP);
                            }
                            else
                            {
                                unit.mStatus.GetHeal(averHP - unit.mStatus.HP);
                            }
                        }
                        SetStateEnd();
                    }
                }
                break;
            case EFFECT.PUZZLEREINITIALIZATION_EFFECT:
                {
                    PuzzleManager.instance.ClearPuzzle();
                    PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.FILL,null);
                    PuzzleManager.instance.MoveEndAction(PuzzleManager.PUZZLE_STATE.MATCH, PuzzleManager.PUZZLE_STATE.FILL);
                }
                break;
            default:
                break;
        }
    }
    public IEnumerator FirePlayerEffect(PlayerSkillInfo playerSkillInfo)
    {
        if (playerSkillInfo.ISkillEffectID1 != 0)
        {
            RANGE range = RANGE.NO_RANGE;
            switch (playerSkillInfo.TargetObj1)
            {
                case TARGETOBJECT.NORMAL_TARGETOBJ:
                case TARGETOBJECT.FRONTLINE_TARGETOBJ:
                case TARGETOBJECT.MIDDLELINE_TARGETOBJ:
                case TARGETOBJECT.ENDLINE_TARGETOBJ:
                    range = RANGE.MULTI_RANGE;
                    break;
                case TARGETOBJECT.LOWHEALTH_TARGETOBJ:
                case TARGETOBJECT.HIGHHEALTH_TARGETOBJ:
                    range = RANGE.SINGLE_RANGE;
                    break;
                default:
                    break;
            }
            var targets = FindTarget(null, (EFFECT)playerSkillInfo.ISkillEffectID1, playerSkillInfo.Target1, playerSkillInfo.TargetObj1, range);
            BattlePacket packet = new BattlePacket(null, playerSkillInfo.ISkillId, playerSkillInfo.ISkillEffectID1, playerSkillInfo.ISkillEffectTurn1, playerSkillInfo.ISkillEffectValue1);

            foreach (var item in targets)
            {
                if (item == null) continue;
                item.mStatus.AddBuff(packet);
                yield return new WaitForSeconds(0.5f);
            }
        }
        SetStateEnd();
    }

    public void FirePlayerEffect(ItemInfo item, int targetIndex)
    {
        var target = (targetIndex < 5) ? units[targetIndex] : enemies[targetIndex - 5];

        switch ((EFFECT)item.IEffectID)
        {
            case EFFECT.HEAL_EFFECT:

                if (target == null) break;

                target.mStatus.GetHeal(target.mStatus.HP * (item.IEffectValue / 100f));
                
                SetStateEnd();
                break;
            case EFFECT.PUZZLEREMOVE_EFFECT:

                PuzzleManager.instance.ClearPuzzle(targetIndex);
                PuzzleManager.instance.ChangeState(PuzzleManager.PUZZLE_STATE.FILL, null);
                PuzzleManager.instance.MoveEndAction(PuzzleManager.PUZZLE_STATE.MATCH, PuzzleManager.PUZZLE_STATE.FILL);
                break;
            default:
                break;
        }
    }

    public void FirePlayerEffect(PlayerSkillInfo playerSkillInfo, int targetIndex)
    {
        List<BattlePacket> packets = new List<BattlePacket>();

        if (playerSkillInfo.ISkillEffectID1 != 0)
        {
            BattlePacket packet = new BattlePacket(null, playerSkillInfo.ISkillId, playerSkillInfo.ISkillEffectID1, playerSkillInfo.ISkillEffectTurn1, playerSkillInfo.ISkillEffectValue1);
            packets.Add(packet);
        }

        if(playerSkillInfo.ISkillEffectID2 != 0)
        {
            BattlePacket packet = new BattlePacket(null, playerSkillInfo.ISkillId, playerSkillInfo.ISkillEffectID2, playerSkillInfo.ISkillEffectTurn2, playerSkillInfo.ISkillEffectValue2);
            packets.Add(packet);
        }

        if (playerSkillInfo.ISkillEffectID3 != 0)
        {
            BattlePacket packet = new BattlePacket(null, playerSkillInfo.ISkillId, playerSkillInfo.ISkillEffectID3, playerSkillInfo.ISkillEffectTurn3, playerSkillInfo.ISkillEffectValue3);
            packets.Add(packet);
        }

        for(int i = 0; i< packets.Count;++i)
        {
            var target = (targetIndex < 5) ? units[targetIndex] : enemies[targetIndex - 5];
            if (target != null)
            {
                if (packets[i].eID == (int)EFFECT.DAMAGE_EFFECT)
                {
                    float sumAtk = 0;
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
                        sumAtk += unit.mStatus.ATK;
                    }

                    target.mStatus.GetDamage(packets[i].eValue/100f * sumAtk);
                }
                else if (packets[i].eID == (int)EFFECT.HEAL_EFFECT)
                {
                    target.mStatus.GetHeal(target.mStatus.MAXHP * packets[i].eValue / 100f);
                }
                else
                {
                    target.mStatus.AddBuff(packets[i]);
                }
            }
        }

        SetStateEnd();            
    }

    #endregion

    #region Unit Dies

    public void SetCharacterDie(Unit c)
    {
        die_queue.Enqueue(c);
        c.mSpineEventHandler.AddEndAction(SPINE_ANIMATION_TYPE.SPINE_DIE, 
            () => {
                Unit character = die_queue.Dequeue();
                Destroy(character.gameObject);
                CheckStageEnd();
            });
        
        for (int i = 0; i < units.Count; ++i)
        {
            if (units[i] == c)
            {
                units[i] = null;
                BattleUIManager.instance.BattleUI.unitButtons.Remove(i);
                return;
            }
        }

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] == c)
            {
                enemies[i] = null;
                BattleUIManager.instance.BattleUI.unitButtons.Remove(i+5);
                return;
            }
        }
    }


    public TURN IsAllDie()
    {
        if (IsAllDie(units)) return TURN.PLAYER;
        if (IsAllDie(enemies)) return TURN.ENEMY;

        return TURN.NONE;
    }

    public bool IsAllDie(List<Unit> characters)
    {
        bool check = true;
        for (int i = 0; i < characters.Count; ++i)
        {
            if (characters[i] != null)
            {
                check = false;
                break;
            }
        }
        return check;
    }

    public void ClearCharacters(CHARACTER_CAMP camp)
    {
        List<Unit> clearTargets;
        clearTargets = (camp == CHARACTER_CAMP.PLAYER) ? units : enemies;

        for (int i = 0; i < clearTargets.Count; ++i)
            clearTargets[i] = null;
    }
    public void CheckStageEnd()
    {
        bool check = true;
        for (int i = 0; i < units.Count; ++i)
        {
            if (units[i] != null)
            {
                check = false;
                break;
            }
        }

        if (check)
            SetStateEnd();

        check = true;
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
            {
                check = false;
                break;
            }
        }

        if (check)
            SetStateEnd();
    }

    #endregion

    #region ChangeState

    public void AddEndAction(CHARACTER_STATE animType, StateEndAction animNoti)
    {
        if (mStateEndActions == null) mStateEndActions = new Dictionary<CHARACTER_STATE, StateEndAction>();

        StateEndAction noti;
        if (mStateEndActions.TryGetValue(animType, out noti))
        {
            bool isIn = false;
            foreach (var n in noti.GetInvocationList())
            {
                if (n.Equals(animNoti))
                {
                    isIn = true;
                    break;
                }
            }
            if (!isIn) noti += animNoti;
        }
        else
        {
            mStateEndActions.Add(animType, animNoti);
        }
    }
    public void ClearEndAction()
    {
        if (mStateEndActions == null) return;
        mStateEndActions.Clear();
    }
    public void ClearEndAction(CHARACTER_STATE state)
    {
        if (mStateEndActions == null) return;

        if (mStateEndActions.ContainsKey(state))
        {
            mStateEndActions.Remove(state);
        }
    }
    public void ChangeState(CHARACTER_STATE state, StateEndAction endAction)
    {

        if (endAction != null)
        {
            ClearEndAction();
            AddEndAction(state, endAction);
        }

        character_state = state;
        switch (character_state)
        {
            case CHARACTER_STATE.PLAYER_UNIT_ENTERENCE:
                StartCoroutine(PlayerUnitMove(CHARACTER_STATE.PLAYER_UNIT_ENTERENCE));
                break;
            case CHARACTER_STATE.PLAYER_UNIT_WALKOUT:
                StartCoroutine(PlayerUnitMove(CHARACTER_STATE.PLAYER_UNIT_WALKOUT));
                break;
            case CHARACTER_STATE.SPAWN_GEM:
                StartCoroutine(GenerateRecipes());
                break;
            case CHARACTER_STATE.CHECK_START_BUFF:
                StartCoroutine(ExecuteBuff(BUFFTIME.STARTTURN));
                break;
            case CHARACTER_STATE.CHECK_END_BUFF:
                foreach (var item in turn == TURN.PLAYER ? units : enemies)
                {
                    if (item == null) continue;

                    item.mStatus.PrevDamage = -1;
                }
                StartCoroutine(ExecuteBuff(BUFFTIME.ENDTURN));
                break;
            case CHARACTER_STATE.ATTACK:
                StartCoroutine(UnitAttack(turn));
                break;
            case CHARACTER_STATE.WAIT:

                break;
            default:
                break;
        }
    }

    public void SetStateEnd()
    {
        StateEndAction noti;
        if (mStateEndActions.TryGetValue(character_state, out noti))
        {
            CHARACTER_STATE state = character_state;


            noti.Invoke();
            mStateEndActions.Remove(state);

        }
    }

    #endregion

}
