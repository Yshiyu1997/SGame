using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  敌人生物
    /// </summary>
    public class EnemyData : EntityData
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

        /// <summary>
        ///  死亡特效
        /// </summary>
        public int m_DeadEffectId { get; private set; }

        /// <summary>
        ///  死亡音效
        /// </summary>
        public int m_DeadSoundId { get; private set; }

        /// <summary>
        ///  大小
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        ///  攻击音效
        /// </summary>
        public int AttackSound { get; private set; }

        /// <summary>
        ///  受击音效
        /// </summary>
        public int HitSound { get; private set; }

        /// <summary>
        ///  能否攻击
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
