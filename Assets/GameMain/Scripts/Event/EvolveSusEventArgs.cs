using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  进化成功事件
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
