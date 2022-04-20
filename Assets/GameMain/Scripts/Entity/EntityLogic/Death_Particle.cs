using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class Death_Particle : Entity
    {

        /// <summary>
        ///  ½������������Ч
        /// </summary>
        private Death_ParticleData m_Death_ParticleData = null;

        /// <summary>
        ///  ��Ч����ʱЧ
        /// </summary>
        private float m_HideTime = 3f;

        /// <summary>
        ///  ��Ч���ټ�ʱ��
        /// </summary>
        private float m_HideTimer = 0f;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Death_ParticleData = (Death_ParticleData)userData;

            transform.position = m_Death_ParticleData.PosValue;

            // ����ʱЧ
            m_HideTime = 3f;

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


            m_HideTimer += elapseSeconds;
            if (m_HideTimer >= m_HideTime)
            {
                m_HideTimer = 0;
                // ���� ��Ч
                GameEntry.Entity.HideEntity(this);
            }

        }


    }
}
