using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  死亡特效一实体数据
    /// </summary>
    public class Death_ParticleData : EntityData
    {

        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }

    

        public Death_ParticleData(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }


       
    }
}
