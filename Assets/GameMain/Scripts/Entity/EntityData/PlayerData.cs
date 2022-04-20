using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ��ҵ�ʵ������
    /// </summary>
    public class PlayerData : EntityData
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
        ///  �ٶ�
        /// </summary>
        public float Speed { get; private set; }


        /// <summary>
        ///  ��������Ч
        /// </summary>
        public int DeadEffectId { get; private set; }


        /// <summary>
        ///  ��������Ч
        /// </summary>
        public int DeadSoundId { get; private set; }

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
        ///  ��������ľ���
        /// </summary>
        public int RequireDNA { get; private set; }

        /// <summary>
        ///  ���ɵ�λ��
        /// </summary>
        public Vector3 PosValue { get; private set; }

        /// <summary>
        ///  Ƥ���±�
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
