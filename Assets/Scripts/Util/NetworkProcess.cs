using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NETWORK_TYPE
{
    /// <summary>
    /// 연결 안되있음
    /// </summary>
    NOTCONNECTED_TYPE, 
    /// <summary>
    /// 와이파이 연결
    /// </summary>
    WIFI_TYPE,
    /// <summary>
    /// 모바일 데이터(LTE, 5G등)
    /// </summary>
    MOBILE_TYPE
}
public class NetworkProcess
{
    /// <summary>
    /// 현재 연결된 네트워크의 상태 값을 가져온다.
    /// </summary>
    /// <returns>상태 타입</returns>
    public static NETWORK_TYPE GetNetWorkConnectType()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            return NETWORK_TYPE.NOTCONNECTED_TYPE;
        }
        else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return NETWORK_TYPE.MOBILE_TYPE;
        }
        else
        {
            return NETWORK_TYPE.WIFI_TYPE;
        }
    }
}
