using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  进化事件
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
