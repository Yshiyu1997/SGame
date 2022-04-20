using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  三角龙
    /// </summary>
    public class Triceratops : Entity
    {

        /// <summary>
        ///  三角龙一级的实体数据
        /// </summary>
        private TriceratopsData m_TriceratopsData = null;

        /// <summary>
        ///  身上的操作脚本
        /// </summary>
        private Creature m_Creature = null;
        /// <summary>
        ///  消失的位置
        /// </summary>
        private Vector3 m_DiePos = Vector3.zero;


       

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TriceratopsData = (TriceratopsData)userData;

            m_Creature = this.GetComponent<Creature>();

            DealTriceratops();

            // 升级事件的监听
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_Creature) m_Creature.CanAttack = true;
            if (m_Creature.Health <= 0)
            {
                // 死亡
                m_DiePos = transform.position;
                // 清除生物列表对应生物
                Manager._instance.creaturesList.Remove(this.gameObject);
                if (Manager._instance.selected != 0)
                    Manager._instance.selected--;
                GameEntry.Entity.HideEntity(this);
                // 展示死亡特效
                GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(), 10007, m_DiePos));
            }
        }

        /// <summary>
        ///  显示一级三角龙
        /// </summary>
        private void DealTriceratops()
        {
            Manager._instance.OnAddSpecies(this.gameObject,m_TriceratopsData.IsAI,m_TriceratopsData.PosValue,0);
            SetScale(0.08f);
        }


        /// <summary>
        ///  设置模型大小
        /// </summary>
        private void SetScale(float setSize)
        {
            m_Creature.SetScale(setSize);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Log.Debug("消除三角龙");
            if (!isShutdown)
            {
                // 生成肉块  根据不同生物生成不同数量的肉块
                //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                for (int i = 0; i <= 2; i++)
                {
                    GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, new Vector3(m_DiePos.x,58.7f,m_DiePos.z)));
                }
            }

            // 升级事件的监听
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);

        }

        private void OnLevelUp(object sender, GameEventArgs e)
        {
            if (Manager._instance.level >= 2)
            {
                m_Creature.Health = 10f;
            }
        }




    }
}
