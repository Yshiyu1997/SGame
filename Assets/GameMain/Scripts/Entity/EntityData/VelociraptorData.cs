using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  迅猛龙实体数据
    /// </summary>
    public class VelociraptorData : EntityData
    {

        /// <summary>
        ///  是否是AI或者玩家
        /// </summary>
        public bool IsAI { get; private set; }

        /// <summary>
        ///  生命值
        /// </summary>
        public float LifeValue { get; private set; }

        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }


        public VelociraptorData(int entityId, int typeId, bool isAI, float lifeValue,Vector3 posValue) : base(entityId, typeId)
        {
            IsAI = isAI;
            LifeValue = lifeValue;
            PosValue = posValue;
        }
    }
}
