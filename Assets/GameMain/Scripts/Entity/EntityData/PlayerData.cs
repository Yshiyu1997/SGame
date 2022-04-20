using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  玩家的实体数据
    /// </summary>
    public class PlayerData : EntityData
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
        ///  速度
        /// </summary>
        public float Speed { get; private set; }


        /// <summary>
        ///  死亡的特效
        /// </summary>
        public int DeadEffectId { get; private set; }


        /// <summary>
        ///  死亡的音效
        /// </summary>
        public int DeadSoundId { get; private set; }

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
        ///  升级所需的经验
        /// </summary>
        public int RequireDNA { get; private set; }

        /// <summary>
        ///  生成的位置
        /// </summary>
        public Vector3 PosValue { get; private set; }

        /// <summary>
        ///  皮肤下标
        /// </summary>
        public int SkinIndex { get; private set; }


        public PlayerData(int entityId, int typeId,Vector3 posValue,int skinIndex) : base(entityId,typeId)
        {

            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drPlayer = dtPlayer.GetDataRow(TypeId);
            if (drPlayer == null)
            {
                return;
            }

            IsAI = drPlayer.IsAi;
            LifeValue = drPlayer.LifeValue;
            Speed = drPlayer.Speed;
            DeadEffectId = drPlayer.DeadEffectId;
            DeadSoundId = drPlayer.DeadSoundId;
            Scale = drPlayer.Scale;
            RequireDNA = drPlayer.RequireDNA;
            AttackSound = drPlayer.AttackSound;
            HitSound = drPlayer.HitSound;

            PosValue = posValue;
            SkinIndex = skinIndex;

        }
    }
}
