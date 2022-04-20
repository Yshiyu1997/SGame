using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  升级特效二
    /// </summary>
    public class LevelUp_2 : Entity
    {

        /// <summary>
        ///  升级特效二实体数据
        /// </summary>
        private LevelUp_2Data m_LevelUp_2Data = null;

        /// <summary>
        ///  特效销毁时效
        /// </summary>
        private float m_HideTime = 2f;

        /// <summary>
        ///  特效销毁计时器
        /// </summary>
        private float m_HideTimer = 0f;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_LevelUp_2Data = (LevelUp_2Data)userData;

            transform.position = m_LevelUp_2Data.PosValue;
         

            // 重置时效
            m_HideTime = 2f;

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


            m_HideTimer += elapseSeconds;
            if (m_HideTimer >= m_HideTime)
            {
                m_HideTimer = 0;
                // 销毁 特效
                GameEntry.Entity.HideEntity(this);
            }

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            //Log.Debug("升级派发事件");
            //// 升级
            //GameEntry.HPBar.isHasLevelUp = false;
            //GameEntry.Event.Fire(this, ReferencePool.Acquire<LevelUpEventArgs>());
            //Manager._instance.level++;
            //GameEntry.Setting.SetInt("Level",Manager._instance.level);
            //GameEntry.Setting.Save();
            //GameEntry.Sound.PlaySound(30001);
        }
    }


}
