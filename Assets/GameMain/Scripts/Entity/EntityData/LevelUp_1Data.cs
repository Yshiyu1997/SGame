using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  升级特效一实体数据
    /// </summary>
    public class LevelUp_1Data : EntityData
    {
        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }


        public LevelUp_1Data(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }
    }
}
