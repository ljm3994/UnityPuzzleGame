using BattleUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCommon
{
    public delegate void Noti();
    public class Observe<T>
    {
        public Observe(T value)
        {
            val = value;
        }

        T val;
        public delegate void Noti(T t);
        Noti noti;
        public bool canNoti = true;

        public T Value{
            get => val;
            set
            {
                val = value;
                if(noti != null && canNoti)
                {
                    noti.Invoke(val);
                }
            }
        }

        public void AddNoti(Noti n)
        {
            if (noti == null)
            {
                noti = n;
            }
            else
            {
                noti += n;
            }

            noti.Invoke(val);
        }
    }


    public class BattleCommons
    {
        public static Vector3 GetCenterPosition(List<Unit> units)
        {
            Vector3 position = new Vector3();

            foreach (var item in units)
            {
                position += item.transform.position;
            }

            return position / units.Count;
        }
    }
}