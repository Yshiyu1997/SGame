using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class ResurgenceEventArgs : GameEventArgs
    {


        public static readonly int EventId = typeof(ResurgenceEventArgs).GetHashCode();

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
