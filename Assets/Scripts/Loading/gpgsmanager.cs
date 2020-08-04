using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Android;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Android;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class gpgsmanager : MonoBehaviour
{
    #region INSPECTOR
    [SerializeField]
    string FileName;
    [SerializeField]
    bool TestDebug = false;
    #endregion

    public bool bFileSaving;
    private string SaveData;
    public string _SaveData
    {
        get
        {
            return SaveData;
        }
        set
        {
            SaveData = value;
        }
    }
    private static gpgsmanager _Instance;
    public static gpgsmanager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType(typeof(gpgsmanager)) as gpgsmanager;
                if (!_Instance)
                {
                    GameObject container = new GameObject();
                    container.name = "GpgsManagerContainer";
                    _Instance = container.AddComponent(typeof(gpgsmanager)) as gpgsmanager;
                }
            }

            return _Instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        bFileSaving = false;
        _Instance = this;
        _SaveData = "0";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Debug.Log("AWAKE");    
    }

    void Start()
    {
        Permission.RequestUserPermission("android.permission.ACCESS_NETWORK_STATE");
        Permission.RequestUserPermission("android.permission.INTERNET");
    }

    private void SetCamera()
    {
        float TargetWidthAspect = 9.0f;
        float TargetHeightAspect = 16.0f;

        Camera main = Camera.main;

        main.aspect = TargetWidthAspect / TargetHeightAspect;

        float WidthRatio = Screen.width / TargetWidthAspect;
        float HeightRatio = Screen.height / TargetHeightAspect;

        float HeightAdd = ((WidthRatio / (HeightRatio / 100)) - 100) / 200;
        float WidthAdd = ((HeightRatio / (WidthRatio / 100)) - 100) / 200;

        if (HeightRatio > WidthRatio)
        {
            WidthAdd = 0.0f;
        }
        else
        {
            HeightAdd = 0.0f;
        }

        main.rect = new Rect(main.rect.x + Mathf.Abs(WidthAdd), main.rect.y + Mathf.Abs(HeightAdd), main.rect.width + (WidthAdd * 2),
            main.rect.height + (HeightAdd * 2));
    }
    public void Signin(System.Action<bool> Respone)
    {
        Debug.Log("로그인 진행");
        if (Social.localUser.authenticated)
        {
            PlayerDataManager.PlayerData.Pdata.StrName = Social.localUser.userName;
            Debug.Log(Social.localUser.userName);
            Respone(true);
        }
        else
        {
            Social.localUser.Authenticate((bool Succed) =>
            {
                if (Succed)
                {
                    Debug.Log("DataLoad");
                    PlayerDataManager.PlayerData.Pdata.StrName = Social.localUser.userName;
                    Debug.Log(Social.localUser.userName);
                    Debug.Log("로그인 성공");
                    Respone(true);
                }
                else
                {
                    Debug.Log("로그인 실패");
                    Respone(false);
                }

            });
        }
        //씬변경 임시
    }
    public string GetUserName()
    {
        return Social.localUser.userName;
    }
    public void SignOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Log out");
    }

    public void SaveOpenFile(DATAFILEName FileName, bool bOpenGame, Action<bool> SaveEvent)
    {
        string Id = Social.localUser.id;
        string Name = Enum.GetName(typeof(DATAFILEName), FileName);
        string _FileName = string.Format("{0}.bin", Name);
        Debug.Log(_FileName);
        if (Social.localUser.authenticated)
        {
            bFileSaving = true;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(_FileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (SavedGameRequestStatus Status, ISavedGameMetadata gameMetadata) =>
            {
                if (Status == SavedGameRequestStatus.Success)
                {
                    if (!bOpenGame)
                    {
                        Debug.Log("로드1 성공");
                        LoadGame(gameMetadata, FileName, SaveEvent);
                    }
                    else
                    {
                        Debug.Log("세이브1 성공");
                        SaveGame(gameMetadata, FileName, SaveEvent);
                    }
                }
                else
                {
                    bFileSaving = false;
                    SaveEvent(false);
                    Debug.Log(Status.ToString());
                }
            });
        }
        else
        {
            SaveEvent(false);
            Debug.Log("로그인 실패");
        }

    }

    private void SaveGame(ISavedGameMetadata gameMetadata, DATAFILEName FileName, Action<bool> SaveEvent)
    {
        string Data = "";

        Data = PlayerDataManager.PlayerData.GetDataString(FileName);

        byte[] Databyte = Encoding.ASCII.GetBytes(Data);

        // 현재 세이브 시간을 메타데이터로 저장해주어야 한다.
        SavedGameMetadataUpdate savedGameMetadataUpdate = new SavedGameMetadataUpdate.Builder().Build();

        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(gameMetadata, savedGameMetadataUpdate, Databyte, OnSaveGameDataWrite);


        void OnSaveGameDataWrite(SavedGameRequestStatus Status, ISavedGameMetadata gameMetadata2)
        {
            if (Status == SavedGameRequestStatus.Success)
            {
                bFileSaving = false;
                SaveEvent(true);
                Debug.Log("세이브2 성공");
            }
            else
            {
                SaveEvent(false);
                bFileSaving = false;
            }
        }
    }

    private void LoadGame(ISavedGameMetadata gameMetadata, DATAFILEName FileName, Action<bool> SaveEvent)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(gameMetadata, (SavedGameRequestStatus Status, byte[] Databyte) =>
        {
            if (Status == SavedGameRequestStatus.Success)
            {
                Debug.Log(gameMetadata.LastModifiedTimestamp);
                Debug.Log("로드2 성공");
                string str = "0";

                if (Databyte.Length != 0)
                {
                    str = Encoding.ASCII.GetString(Databyte);
                    PlayerDataManager.PlayerData.SetDataString(FileName, str);
                }
                else
                {
                    switch (FileName)
                    {
                        case DATAFILEName.User_Data:
                            PlayerDataManager.PlayerData.PlayerDataDefault();
                            break;
                        case DATAFILEName.User_Item:                            
                        case DATAFILEName.User_ETCItem:
                            PlayerDataManager.PlayerData.ItemDefaultData();
                            break;
                        case DATAFILEName.User_Skill:
                            PlayerDataManager.PlayerData.SkillDefalultData();
                            break;
                        case DATAFILEName.User_Unit:
                            PlayerDataManager.PlayerData.UnitDefaultData();
                            break;
                    }
                }

                SaveEvent(true);
            }
            else
            {
                SaveEvent(false);
            }
        });

    }

    private void ResolveConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
    {
        if(originalData == null)
        {
            resolver.ChooseMetadata(unmerged);
        }
        else if(unmergedData == null)
        {
            resolver.ChooseMetadata(original);
        }
        else
        {
            string originalstr = Encoding.ASCII.GetString(originalData);
            string unmergedstr = Encoding.ASCII.GetString(unmergedData);

            int OriginalNum = int.Parse(originalstr);
            int unmergedNum = int.Parse(unmergedstr);

            if(OriginalNum > unmergedNum)
            {
                resolver.ChooseMetadata(original);
                return;
            }
            else if(OriginalNum < unmergedNum)
            {
                resolver.ChooseMetadata(unmerged);
                return;
            }
            resolver.ChooseMetadata(original);
        }
    }
}
    
