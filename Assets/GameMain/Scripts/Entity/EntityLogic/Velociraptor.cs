using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class Velociraptor : Entity
    {

        /// <summary>
        ///  Ѹ����һ����ʵ������
        /// </summary>
        private VelociraptorData m_VelociraptorData = null;
        /// <summary>
        ///  ���ϵĲ����ű�
        /// </summary>
        private Creature m_Creature = null;
        /// <summary>
        ///  ��ʧ��λ��
        /// </summary>
        private Vector3 m_DiePos = Vector3.zero;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_VelociraptorData = (VelociraptorData)userData;
            m_Creature = this.GetComponent<Creature>();
            DealVelociraptor();
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_Creature) m_Creature.CanAttack = false;
            if (m_Creature.Health <= 0)
            {
                // ����
                m_DiePos = transform.position;
                // ��������б��Ӧ����
                Manager._instance.creaturesList.Remove(this.gameObject);
                if(Manager._instance.selected!=0)
                    Manager._instance.selected--;
                GameEntry.Entity.HideEntity(this);
                // չʾ������Ч
                GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(),10007,m_DiePos));
            }
        }

        /// <summary>
        ///  ������ʾ��Ӧ��Ѹ����һ��ʵ��
        /// </summary>
        private void DealVelociraptor()
        {
            Manager._instance.OnAddSpecies(this.gameObject, m_VelociraptorData.IsAI, m_VelociraptorData.PosValue,0);
            SetScale(0.08f);
        }

        /// <summary>
        ///  ����ģ�ʹ�С
        /// </summary>
        private void SetScale(float setSize)
        {
            m_Creature.SetScale(setSize);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Log.Debug("����Ѹ����");
            if (!isShutdown)
            {
                // �������  ���ݲ�ͬ�������ɲ�ͬ���������
                //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                for(int i = 0; i <= 1; i++)
                {
                    GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, new Vector3(m_DiePos.x, 58.7f, m_DiePos.z)));
                }
            }
         
        }
    }
}
