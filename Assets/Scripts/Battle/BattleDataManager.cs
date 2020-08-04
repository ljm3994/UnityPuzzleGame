using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;

public class UnitInitData
{
    public UnitInitData()
    {
        basicStatus = new GeneralStatus(0,0,0);
    }

    public string unitName;
    public int unitID;
    public int AtkID;
    public int SkillID;
    public CHARACTER_CAMP camp;
    public int position_index;
    public string prefabPath;
    public int level;
    public GeneralStatus basicStatus;
}


public class BattleDataManager : MonoBehaviour
{
    public static BattleDataManager instance;

    public static int[] unitIDs = { 11201,11204,11207,11212,11224,13203,13220,33215,42214,52102};
    public static int[] enemyIDs = { 11111,11112,11113,12111,15111,15131,15321,16111,16131};

    #region inspector
    public bool ConsoleDebug = false;

    [Space]
    public string mCharacterPrefabPath;
    #endregion

    public List<int> encounterIDList;
    public Dictionary<int, int> totalRewardItems = new Dictionary<int, int>();
    public Dictionary<int, int> stageRewardItems = new Dictionary<int, int>();

    private void Awake()
    {
        instance = this;
    }

    public void moveTotalRewards()
    {
        foreach (var item in stageRewardItems)
        {
            if(totalRewardItems.ContainsKey(item.Key))
            {
                totalRewardItems[item.Key] += item.Value;
            }
            else
            {
                totalRewardItems.Add(item.Key, item.Value);
            }
        }
        stageRewardItems.Clear();
    }

    public void ItemDrop()
    {
        int[] dropPercents = {
            GameManager.instance.CurentStage.IStageDropPercent1, GameManager.instance.CurentStage.IStageDropPercent2, GameManager.instance.CurentStage.IStageDropPercent3,
            GameManager.instance.CurentStage.IStageDropPercent4,GameManager.instance.CurentStage.IStageDropPercent5,GameManager.instance.CurentStage.IStageDropPercent6
        };

        int[] dropItems =
        {
            GameManager.instance.CurentStage.IStageDropItemID1,GameManager.instance.CurentStage.IStageDropItemID2,GameManager.instance.CurentStage.IStageDropItemID3,
            GameManager.instance.CurentStage.IStageDropItemID4,GameManager.instance.CurentStage.IStageDropItemID5,GameManager.instance.CurentStage.IStageDropItemID6
        };

        int[] dropCounts =
        {
            GameManager.instance.CurentStage.IStageDropItemNumber1,GameManager.instance.CurentStage.IStageDropItemNumber2,GameManager.instance.CurentStage.IStageDropItemNumber3,
            GameManager.instance.CurentStage.IStageDropItemNumber4,GameManager.instance.CurentStage.IStageDropItemNumber5,GameManager.instance.CurentStage.IStageDropItemNumber6
        };
        
        for(int i = 0; i < 6; ++i)
        {
            if (dropItems[i] == 0) continue;

            int p = Random.Range(1, 100);
            if(p < dropPercents[i])
            {
                int num = Random.Range(1, dropCounts[i]);

                if(stageRewardItems.ContainsKey(dropItems[i]))
                {
                    stageRewardItems[dropItems[i]] += num;
                }
                else
                {
                    stageRewardItems.Add(dropItems[i], num);
                }
            }
        }

    }

    #region SAVE & LOAD

    public void LoadBattleStage()
    {
        //GameManager.instance.CurentStage; 해당 스테이지 정보를 담고 있음
        StageManager.instance.SetupStage(GameManager.instance.CurentStage.IStageNumber,GameManager.instance.CurentStage.IStageFloor);
        BattleManager.instance.STAGE_MAX = GameManager.instance.CurentStage.IStageEncounter - 1;
        BattleManager.instance.LINK_MAX = GameManager.instance.CurentStage.IStagePlayerLinkNumber;
        

        encounterIDList = new List<int>();
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID1);
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID2);
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID3);
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID4);
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID5);
        encounterIDList.Add(GameManager.instance.CurentStage.IStageMonsterID6);
    }

    public List<UnitInitData> LoadPlayerUnits()
    {

        List<UnitInitData> datas = new List<UnitInitData>();

        var unitInventory = UIDataProcess.GetUnitInventory();
        
        for(int i = 0; i< unitInventory.EquipmentUnit.Length; i++)
        {
            var equipUnit = unitInventory.EquipmentUnit[i];
            if (equipUnit != null && equipUnit.iIndex != 0)
            {
                UnitInitData data = new UnitInitData();
                data.unitID = equipUnit.iIndex;

                var skillKeys = UIDataProcess.GetUnitSkillKeys(data.unitID);
                var unitData = GameDataBase.Instance.UnitTable[data.unitID];
                
                if(skillKeys != null)
                {
                    data.AtkID = skillKeys.Length >= 1 ? skillKeys[0] : -1;
                    data.SkillID = skillKeys.Length >= 2 ? skillKeys[1] : -1;
                }
                else
                {
                    data.AtkID = -1;
                    data.SkillID = -1;
                }

                data.unitName = unitData.strName;
                data.camp = CHARACTER_CAMP.PLAYER;
                data.position_index = i;
                data.level = equipUnit.iLevel;
                data.basicStatus.HP = unitData.IUnitHealth + ((data.level - 1) * unitData.IStateIncreaseValue);
                data.basicStatus.ATK = unitData.IUnitAtk + ((data.level - 1) * unitData.IStateIncreaseValue);
                data.basicStatus.DEF = unitData.IUnitDef + ((data.level - 1) * unitData.IStateIncreaseValue);
                
                bool isIn = false;
                foreach (var item in unitIDs)
                {
                    if(item == unitData.iID)
                    {
                        isIn = true;
                        break;
                    }
                }

                if(isIn) data.prefabPath = UIDataProcess.UnitSpinePath + "Unit/" + unitData.iID; 
                else data.prefabPath = UIDataProcess.UnitSpinePath + "Unit/" + "11201";

                datas.Add(data);
            }
        }

        return datas;
    }

    public List<UnitInitData> LoadEnemyUnits(int stageNum)
    {

        List<UnitInitData> datas = new List<UnitInitData>();
        List<int> listEncounter;
        switch (stageNum)
        {
            case 0:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter1;
            break;
            case 1:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter2;
            break;
            case 2:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter3;
            break;
            case 3:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter4;
            break;
            case 4:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter5;
            break;
            case 5:
                listEncounter = GameManager.instance.CurentStage.ListStageEncounter6;
            break;
            default: return null;
        }

        for(int i = 0; i< listEncounter.Count; ++i)
        {
            if(listEncounter[i] >= 0)
            {
                UnitInitData data = new UnitInitData();
                int encounterIndex = listEncounter[i] - 1;
                if(encounterIndex >= 0)
                {
                    data.unitID = encounterIDList[encounterIndex];

                    if (data.unitID != 0)
                    {
                        var skillKeys = UIDataProcess.GetMonsterSkillKeys(data.unitID);
                        var unitData = GameDataBase.Instance.MonsterTable[data.unitID];

                        if (skillKeys != null)
                        {
                            data.AtkID = skillKeys.Length >= 1 ? skillKeys[0] : -1;
                            data.SkillID = skillKeys.Length >= 2 ? skillKeys[1] : -1;
                        }
                        else
                        {
                            data.AtkID = -1;
                            data.SkillID = -1;
                        }
                        data.camp = CHARACTER_CAMP.ENEMY;
                        data.position_index = i;
                        data.level = GameManager.instance.CurentStage.IStageLevel;

                        data.basicStatus.HP = unitData.IMonsterHealth + ((data.level - 1) * unitData.IStateIncreaseValue);
                        data.basicStatus.ATK = unitData.IMonsterAtk + ((data.level - 1) * unitData.IStateIncreaseValue);
                        data.basicStatus.DEF = unitData.IMonsterDef + ((data.level - 1) * unitData.IStateIncreaseValue);

                        bool isIn = false;
                        foreach (var item in enemyIDs)
                        {
                            if (item == unitData.iID)
                            {
                                isIn = true;
                                break;
                            }
                        }
                        if (isIn) data.prefabPath = UIDataProcess.UnitSpinePath + "Enemy/" + unitData.iID;
                        else data.prefabPath = UIDataProcess.UnitSpinePath + "Enemy/" + "11111";

                        datas.Add(data);
                    }
                }
                
            }
        }

        return datas;
    }

    #endregion

}
