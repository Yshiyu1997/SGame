using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  Éý¼¶ÊÂ¼þ
    /// </summary>
    public class LevelUpEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LevelUpEventArgs).GetHashCode();

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
