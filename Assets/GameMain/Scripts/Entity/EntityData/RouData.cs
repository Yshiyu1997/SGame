using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class RouData : EntityData
    {

        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }

        public RouData(int entityId, int typeId,Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }
    }
}
