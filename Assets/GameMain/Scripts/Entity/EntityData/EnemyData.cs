using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ��������
    /// </summary>
    public class EnemyData : EntityData
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

        /// <summary>
        ///  ������Ч
        /// </summary>
        public int m_DeadEffectId { get; private set; }

        /// <summary>
        ///  ������Ч
        /// </summary>
        public int m_DeadSoundId { get; private set; }

        /// <summary>
        ///  ��С
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        ///  ������Ч
        /// </summary>
        public int AttackSound { get; private set; }

        /// <summary>
        ///  �ܻ���Ч
        /// </summary>
        public int HitSound { get; private set; }

        /// <summary>
        ///  �ܷ񹥻�
        /// </summary>
        public bool IsCanAttack { get; private set; }



        public EnemyData(int entityId, int typeId,Vector3 posValue) : base(entityId,typeId)
        {
            IDataTable<DREnemy> dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            DREnemy drEnemy = dtEnemy.GetDataRow(TypeId);
            if (drEnemy == null)
            {
                return;
            }

            IsAI = drEnemy.IsAi;
            LifeValue = drEnemy.LifeValue;
            m_DeadEffectId = drEnemy.DeadEffectId;
            m_DeadSoundId = drEnemy.DeadSoundId;
            Scale = drEnemy.Scale;
            AttackSound = drEnemy.AttackSound;
            HitSound = drEnemy.HitSound;
            IsCanAttack = drEnemy.IsCanAttack;

            PosValue = posValue;
            
          
        }
    }
}
