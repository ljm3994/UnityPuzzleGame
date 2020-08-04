using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCommon;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    #region Inspector
    public float ChapterEnterenceTime = 1;
    [Space]
    public MainCharacter MainCharacter;
    public Transform MainCharacterOut;
    public Transform MainCharacterIn;
    [Space]
    public List<Transform> UnitOut;
    public List<Transform> UnitIn;
    [Space]
    public Transform EnemyCenter;
    public Transform Stage;

    public Dictionary<int, List<Transform>> EnemyPositions;
    #endregion

    public delegate void StageAction();
    public Observe<Vector3> stage_angle = new Observe<Vector3>(Vector3.zero);


    private void Awake()
    {
        instance = this;
        EnemyPositions = new Dictionary<int, List<Transform>>();
        stage_angle.Value = EnemyCenter.rotation.eulerAngles;
    }

    private void Start()
    {
        SetupEnemyPositions();
        stage_angle.Value = EnemyCenter.rotation.eulerAngles;
    }

    public void SetupStage(int stageNum, int floor)
    {
        int mapNum;
        if(floor <= 5)
        {
            mapNum = 1;
        }
        else if(floor <=10)
        {
            mapNum = 2;
        }
        else if(floor <= 15)
        {
            mapNum = 3;
        }
        else if(floor <= 19)
        {
            mapNum = 4;
        }
        else
        {
            mapNum = 5;
        }

        Instantiate((GameObject)Resources.Load("Prefabs/Stage/Stage" + stageNum + "/Stage" + mapNum), Stage);
    }

    void SetupEnemyPositions()
    {
        for(int i = 0; i< 5; ++i)
        {
            Transform chapter = EnemyCenter.Find("chapter" + i);
            List<Transform> enemyT = new List<Transform>();
            for(int j = 0; j< 5; ++j)
            {
                enemyT.Add(chapter.Find("enemyUnit" + j));
            }
            EnemyPositions.Add(i, enemyT);
        }
    }

    IEnumerator RotateStage()
    {
        float deltaTime = 0;
        Vector3 angle_origin = Stage.rotation.eulerAngles;
        Vector3 stage_dest_angle = angle_origin - (new Vector3(0, -60, 0));

        Vector3 enemy_angle = EnemyCenter.rotation.eulerAngles;
        Vector3 enemy_dest_angle = enemy_angle - (new Vector3(0, -60, 0));
        

        while(deltaTime < ChapterEnterenceTime)
        {
            deltaTime += Time.deltaTime;

            Vector3 angle = Vector3.Lerp(angle_origin, stage_dest_angle, deltaTime / ChapterEnterenceTime);
            Stage.rotation = Quaternion.Euler(angle);

            stage_angle.Value = Vector3.Lerp(enemy_angle, enemy_dest_angle, deltaTime / ChapterEnterenceTime);
            EnemyCenter.rotation = Quaternion.Euler(stage_angle.Value);
            yield return null;
        }

        MainCharacter.SetIdle();
        Stage.rotation = Quaternion.Euler(stage_dest_angle);
        stage_angle.Value = enemy_dest_angle;
        EnemyCenter.rotation = Quaternion.Euler(enemy_dest_angle);
    }

    public void SetNextStage(StageAction action)
    {
        StartCoroutine(cSetNextStage(action));
    }
    IEnumerator cSetNextStage(StageAction action)
    {
        yield return StartCoroutine(MainCharacter.CharacterEnterence(MainCharacter.STATE.ENTERENCE));

        yield return StartCoroutine(RotateStage());

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(MainCharacter.CharacterEnterence(MainCharacter.STATE.WALKOUT));

        action.Invoke();
    }


}
