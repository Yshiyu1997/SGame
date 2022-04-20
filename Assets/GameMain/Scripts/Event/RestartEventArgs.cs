using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ���¿�ʼ��Ϸ�¼�
    /// </summary>
    public class RestartEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(RestartEventArgs).GetHashCode();


        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public override void Clear()
        {
            
        }
    }

}