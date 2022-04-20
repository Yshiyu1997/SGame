using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  升级特效二实体数据
    /// </summary>
    public class LevelUp_2Data : EntityData
    {
        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }


        public LevelUp_2Data(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }
    }
}
