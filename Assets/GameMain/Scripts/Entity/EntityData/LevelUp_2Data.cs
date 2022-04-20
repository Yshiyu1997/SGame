using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ������Ч��ʵ������
    /// </summary>
    public class LevelUp_2Data : EntityData
    {
        /// <summary>
        ///  ���ɵ�λ��
        /// </summary>
        public Vector3 PosValue { get; private set; }


        public LevelUp_2Data(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }
    }
}
