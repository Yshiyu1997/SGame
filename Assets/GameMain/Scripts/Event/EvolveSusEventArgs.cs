using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  �����ɹ��¼�
    /// </summary>
    public class EvolveSusEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EvolveSusEventArgs).GetHashCode();

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
