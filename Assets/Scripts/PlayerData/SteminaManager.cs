using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteminaManager : MonoBehaviour
{
    public delegate void ChargeEvent(int Time);
    public delegate void SteminaUpEvent(int Stemina);
    private int SteminaChargeTime;
    private Coroutine TimerCoroutine;
    private static SteminaManager _Instance;
    public static SteminaManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType(typeof(SteminaManager)) as SteminaManager;
                if (!_Instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SteminaManagerContainer";
                    _Instance = container.AddComponent(typeof(SteminaManager)) as SteminaManager;
                }
            }

            return _Instance;
        }
    }

    public int SteminaChargeTimer { get => SteminaChargeTime; set => SteminaChargeTime = value; }

    private void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(this);
        SteminaChargeTimer = 0;
    }
    /// <summary>
    /// 데이터 로드시 사용되는 함수로 세이브시 시간과 현재 시간을 비교하여
    /// 해당 차이만큼 스테미너를 채워준다.
    /// </summary>
    /// <param name="QuitTime">세이브시 저장된 시간값</param>
    /// <param name="Respone">완료시 반환 받을 함수(현재 사용되지 않음)</param>
    public void SetLoadTimer(DateTime QuitTime, int NextCoolTime, Action Respone = null)
    {
        if (TimerCoroutine != null)
        {
            StopCoroutine(TimerCoroutine);
        }
        int CoolTime = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStaminaCoolTime;
        int Max = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina;
        int DiffereceInSec = (int)((DateTime.Now.ToLocalTime() - QuitTime).TotalSeconds);
        var Stemina = Math.Floor((double)(DiffereceInSec / CoolTime));
        var Timer = DiffereceInSec % CoolTime;
        if (PlayerDataManager.PlayerData.Pdata.iStamina < Max)
        {
            PlayerDataManager.PlayerData.Pdata.iStamina += int.Parse(Stemina.ToString());

            if (PlayerDataManager.PlayerData.Pdata.iStamina >= Max)
            {
                PlayerDataManager.PlayerData.Pdata.iStamina = Max;
            }
            else
            {
                if(NextCoolTime > Timer)
                {
                    Timer = NextCoolTime - Timer;
                }
                else
                {
                    Timer = Timer - NextCoolTime;
                }
                TimerCoroutine = StartCoroutine(SteminaTimer(Timer, Respone));
            }
        }
    }
    /// <summary>
    /// 스테미너 사용 함수로 정해진 양의 스테미너를 플레이어 데이터에서 사용하고
    /// iStaminaCoolTime시간 동안 최대 스테미너 양까지 자동으로 계속해서 채워준다.
    /// </summary>
    /// <param name="DeleteStemina">사용할 스테미너 양</param>
    /// <param name="Respone">완료시 반환 함수</param>
    /// <returns></returns>
    public bool UseStemina(int DeleteStemina, Action Respone = null)
    {
        if(PlayerDataManager.PlayerData.Pdata.iStamina < DeleteStemina)
        {
            return false;
        }

        int CoolTime = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStaminaCoolTime;
        int Max = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina;
        PlayerDataManager.PlayerData.Pdata.iStamina -= DeleteStemina;

        if (TimerCoroutine == null && PlayerDataManager.PlayerData.Pdata.iStamina < Max)
        {
            TimerCoroutine = StartCoroutine(SteminaTimer(CoolTime, Respone));
        }

        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE, (succed) => {
            
        });
        return true;
    }
    private IEnumerator SteminaTimer(int Time, Action Respone = null)
    {
        int CoolTimeDelay = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStaminaCoolTime;
        int Max = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina;

        if (Time <= 0)
        {
            SteminaChargeTimer = CoolTimeDelay;
        }
        else
        {
            SteminaChargeTimer = Time;
        }

        while(SteminaChargeTimer > 0)
        {
            SteminaChargeTimer--;
            PlayerDataManager.PlayerData.Pdata.INextCoolTime = SteminaChargeTimer;
            yield return new WaitForSeconds(1.0f);
        }

        PlayerDataManager.PlayerData.Pdata.iStamina++;
        if (PlayerDataManager.PlayerData.Pdata.iStamina >= Max)
        {
            
            PlayerDataManager.PlayerData.Pdata.iStamina = GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina;
            SteminaChargeTimer = 0;
            PlayerDataManager.PlayerData.Pdata.INextCoolTime = 0;
            TimerCoroutine = null;
        }
        else
        {
            TimerCoroutine = StartCoroutine(SteminaTimer(CoolTimeDelay, Respone));
        }
    }
}
