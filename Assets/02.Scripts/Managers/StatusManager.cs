using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SymStatus { Main, Play, OnEvent }

namespace GsDefaultModule
{
    public class StatusManager
    {
        public Action OnMain = null;
        public Action OnPlay = null;
        public Action OnEvent = null;

        SymStatus _symStatus = SymStatus.Main;

        public SymStatus SymStat
        {
            get { return _symStatus; }

            set
            {
                _symStatus = value;

                switch (_symStatus)
                {
                    case SymStatus.Main:
#if UNITY_EDITOR
                    Debug.Log("메인화면 모드입니다. ");
#endif
                        if (OnMain != null)
                            OnMain.Invoke();
                        break;
                    case SymStatus.Play:
#if UNITY_EDITOR
                    Debug.Log("플레이 모드입니다. ");
#endif
                        if (OnPlay != null)
                            OnPlay.Invoke();
                        break;
                    case SymStatus.OnEvent:
#if UNITY_EDITOR
                    Debug.Log("이벤트 모드입니다. ");
#endif
                        if (OnEvent != null)
                            OnEvent.Invoke();
                        break;
                    default:
                        break;
                }
            }


        }
    }
}
