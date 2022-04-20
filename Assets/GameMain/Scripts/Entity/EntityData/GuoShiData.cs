using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class GuoShiData : EntityData
    {
        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }

        public GuoShiData(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }
    }
}
