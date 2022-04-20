using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ������Чһʵ������
    /// </summary>
    public class Death_ParticleData : EntityData
    {

        /// <summary>
        ///  ���ɵ�λ��
        /// </summary>
        public Vector3 PosValue { get; private set; }

    

        public Death_ParticleData(int entityId, int typeId, Vector3 posValue) : base(entityId, typeId)
        {
            PosValue = posValue;
        }


       
    }
}
