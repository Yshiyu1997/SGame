using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class Death_Particle : Entity
    {

        /// <summary>
        ///  陆地生物死亡特效
        /// </summary>
        private Death_ParticleData m_Death_ParticleData = null;

        /// <summary>
        ///  特效销毁时效
        /// </summary>
        private float m_HideTime = 3f;

        /// <summary>
        ///  特效销毁计时器
        /// </summary>
        private float m_HideTimer = 0f;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Death_ParticleData = (Death_ParticleData)userData;

            transform.position = m_Death_ParticleData.PosValue;

            // 重置时效
            m_HideTime = 3f;

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


    }
}
