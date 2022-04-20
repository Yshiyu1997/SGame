using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ����������ʵ������
    /// </summary>
    public class BaryonyxData : EntityData
    {
        /// <summary>
        ///  �Ƿ���AI�������
        /// </summary>
        public bool IsAI { get; private set; }

        /// <summary>
        ///  ����ֵ
        /// </summary>
        public float LifeValue { get; private set; }

        /// <summary>
        ///  ���ɵ�λ��
        /// </summary>
        public Vector3 PosValue { get; private set; }


        public BaryonyxData(int entityId, int typeId, bool isAI, float lifeValue, Vector3 posValue) : base(entityId, typeId)
        {
            IsAI = isAI;
            LifeValue = lifeValue;
            PosValue = posValue;
        }
    }
}
