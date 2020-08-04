using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GoogleSheatData : MonoBehaviour
{
    private const string TableKey = @"1e5lZdK2qs86S_1QmCsrm2fSlVbIm0vYBoUUaKlvv3_4";
    private const string strUrlBase = @"https://spreadsheets.google.com/a/google.com/tq?key={0}&gid={1}";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void StartLoad<T>(int TableId, SerializableDictionary<int, T> refContainer, System.Action<bool> OnRespone)
    {
        // 코루틴 실행
        StartCoroutine(Request(TableId, refContainer, OnRespone));
    }

    IEnumerator Request<T>(int TableId, SerializableDictionary<int, T> refContainer, System.Action<bool> OnRespone)
    {
        bool Result = true;
        string strURL = string.Format(strUrlBase, TableKey, TableId);

        WWW wWW = new WWW(strURL);

        while(wWW.isDone == false)
        {
            yield return null;
        }

        if(string.IsNullOrEmpty(wWW.error) == false)
        {
            if(OnRespone != null)
            {
                Debug.LogError("접속 오류.....");
                OnRespone(false);
                // 접속이 실패하면 코루틴을 종료한다.
                yield break;
            }
        }

        string strText = wWW.text;
        Debug.Log(strText);
        try
        {
            // 불필요한 문자열 제거
            int nStart = strText.IndexOf("(");
            int nEnd = strText.IndexOf(");");
            ++nStart;

            // 불필요한 문자열 부분을 제외한 문자만 가져와서 사용
            string strData = strText.Substring(nStart, nEnd - nStart);

            List<string> ValueName = new List<string>();
            List<List<string>> Values = new List<List<string>>();

            // 역직렬화를 통해 스프레드 시트의 값들을 가져온다.
            var mapParsed = JsonParsing.Deserialize(strData) as Dictionary<string, object>;
            // 가져온 값중에 테이블 부분만 가져온다.
            var map = (Dictionary<string, object>)mapParsed["table"];

            var Name = (List<object>)map["cols"];
            var ValuesRow = (List<object>)map["rows"];

            // 각 컬럼값 캐싱
            for(int i = 0; i < Name.Count; i++)
            {
                var m = (Dictionary<string, object>)Name[i];
                ValueName.Add((string)m["label"]);
            }

            // 각 로우 값 캐싱
            for(int i = 0; i < ValuesRow.Count; i++)
            {
                var Temp = (Dictionary<string, object>)ValuesRow[i];
                var RowTemp = (List<object>)Temp["c"];

                Values.Add(new List<string>());

                for(int j = 0; j < ValueName.Count; j++)
                {
                    var vRow = (Dictionary<string, object>)RowTemp[j];

                    if (vRow != null && vRow["v"] != null)
                    {
                        Values[i].Add(vRow["v"].ToString());
                    }
                    else
                    {
                        Values[i].Add("-");
                    }
                }
            }

            // 캐싱해온 값으로 데이터 클래스를 구성한다.
            int nVal = Values.Count;

            for (int i = 0; i < nVal; i++)
            {
                T val = (T)DataProcess.GetClassInit(typeof(T).FullName, Values[i].ToArray());
                refContainer.Add(int.Parse(Values[i][0]), val);
            }
        }
        // 오류 발생시 예외처리
        catch(Exception ex)
        {
            Debug.LogError("에러 발생 : " + ex.Message);
            Result = false;
        }

        // 반환 함수가 존재한다면
        if(OnRespone != null)
        {
            OnRespone(Result);
        }
    }

    public object GetInstance(string strFullName, string[] arr)
    {
        Type type = Type.GetType(strFullName);
        if(type != null)
        {
            return Activator.CreateInstance(type, arr);
        }

        foreach(var appdomain in AppDomain.CurrentDomain.GetAssemblies())
        {           
            type = appdomain.GetType(strFullName);
            if(type != null)
            {
                return Activator.CreateInstance(type, arr);
            }
        }

        return null;
    }
}
