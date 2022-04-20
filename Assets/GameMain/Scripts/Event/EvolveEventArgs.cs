using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  �����¼�
    /// </summary>
    public class EvolveEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EvolveEventArgs).GetHashCode();

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
