using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class LevelUp_1 : Entity
    {

        /// <summary>
        ///  ʵ������
        /// </summary>
        private LevelUp_1Data m_LevelUp_1Data = null;

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

            m_LevelUp_1Data = (LevelUp_1Data)userData;

            transform.position = m_LevelUp_1Data.PosValue;

            // ����ʱЧ
            m_HideTime = 1f;

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

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Log.Debug("�����ɷ��¼�");
            // ����
            GameEntry.HPBar.isHasLevelUp = false;
            GameEntry.Event.Fire(this, ReferencePool.Acquire<LevelUpEventArgs>());
            Manager._instance.level++;
            GameEntry.Setting.SetInt("Level", Manager._instance.level);
            GameEntry.Setting.Save();
            GameEntry.Sound.PlaySound(30001);
            // ���ŵڶ���������Ч
            GameEntry.Entity.ShowLevelUp_2_Particle(new LevelUp_2Data(GameEntry.Entity.GenerateSerialId(), 40002, Manager._instance.playerObj.transform.position));
        }
    }
}
