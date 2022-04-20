using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class Enemy : Entity
    {

        /// <summary>
        ///  ���������ʵ������
        /// </summary>
        public EnemyData m_EnemyData = null;

        /// <summary>
        ///  ���ϵĲ����ű�
        /// </summary>
        private Creature m_Creature = null;
        /// <summary>
        ///  ��ʧ��λ��
        /// </summary>
        private Vector3 m_DiePos = Vector3.zero;

        // �Ѿ�������
        private bool isHasHide = false;

        /// <summary>
        ///  �Ƿ�Ҫ�������
        /// </summary>
        private bool isCanCreateRou = false;

        /// <summary>
        ///  ������Ч
        /// </summary>
        public int m_AttackSoundId = 0;

        /// <summary>
        ///  ������ҵ������
        /// </summary>
        private float m_MaxDistance = 10;

        /// <summary>
        ///  ��������  1��ֱ����ʧ���κ���������߱���  2��ֲ������  3���������
        /// </summary>
        private int m_DieType=0;

        private bool m_IsEvolveIng = false;// �Ƿ��ڽ�����

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EnemyData = (EnemyData)userData;
            m_Creature = this.GetComponent<Creature>();
            DealEnemy();

            isHasHide = false;
            m_IsEvolveIng = false;

            m_AttackSoundId = m_EnemyData.AttackSound;
           
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if (m_Creature.Health <= 0&&!isHasHide)
            {
                isHasHide = true;
                // ����
                m_DiePos = transform.position;
                //// ��������б��Ӧ����
                //Manager._instance.creaturesList.Remove(this.gameObject);
                //if (Manager._instance.selected != 0)
                //    Manager._instance.selected--;
                //GameEntry.Entity.HideEntity(this);
                //// չʾ������Ч
                //GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(), 10007, m_DiePos));
                StartCoroutine(DealEnemyDieIe(DealEnemyDie()));
            
            }

            JudgePlayerDistance();
   
        }

        /// <summary>
        ///  ������˲������������ӳ���ʧ
        /// </summary>
        private float DealEnemyDie()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.HitSound);
            switch (m_EnemyData.TypeId)
            {
                case 20000:  // ��ʬ   
                case 20001:  // ����  
                case 20002:  // Ģ��  
                case 20006:  // Ѹ����  
                    return 0f;
                case 20003: // ����
                    return 0.8f;
                case 20004: // ������
                    return 1f;
                case 20007: // ������
                case 20008: // ������

                    return 2f;
            }

            return 0f;
        } 

        IEnumerator DealEnemyDieIe(float delayTime)
        {
            DealFruiterTree();
            yield return new WaitForSeconds(delayTime);
            // ��������б��Ӧ����
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0&&Manager._instance.creaturesList.IndexOf(this.gameObject)<Manager._instance.selected)
                Manager._instance.selected--;
            JudgeIsCreateRou();
            isCanCreateRou = m_DieType.Equals(3)?true:false;
            GameEntry.Entity.HideEntity(this);
            // չʾ������Ч
            GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(), 40000, m_DiePos));
        }

        /// <summary>
        ///  ������ʾ��Ӧ��ʵ��
        /// </summary>
        private void DealEnemy()
        {
            Manager._instance.OnAddSpecies(this.gameObject, m_EnemyData.IsAI, m_EnemyData.PosValue,0);
            SetScale(m_EnemyData.Scale);
            if (m_Creature) m_Creature.CanAttack = m_EnemyData.IsCanAttack;
        }

        /// <summary>
        ///  ����ģ�ʹ�С
        /// </summary>
        private void SetScale(float setSize)
        {
            setSize = JudgeSize();
            m_Creature.SetScale(setSize);
        }

        /// <summary>
        ///  �ж��ر𲻺ϵĴ�С
        /// </summary>
        private float JudgeSize()
        {
            switch (m_EnemyData.TypeId)
            {
                case 20008:  //������
                    if(GameUtil._instance.beginEvolveLevel==2|| GameUtil._instance.beginEvolveLevel == 3)
                    {
                        return 0.06f;
                    }
                    break;
            }
            return m_EnemyData.Scale;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Log.Debug("��������������" + isCanCreateRou);
            if (!isShutdown&&!m_IsEvolveIng)
            {
                if (isCanCreateRou)
                {
                    //if (Manager._instance.isHasAttack)
                    //{
                    //    // ���ȡ������
                    //    Manager._instance.playerObj.GetComponent<Creature>().CancelPlayerAttack();
                    //    Manager._instance.isHasAttack = false;
                    //}
                    // �������  ���ݲ�ͬ�������ɲ�ͬ���������
                    //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                    //GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 10006, m_DiePos));
                    for (int i = 0; i <= 1; i++)
                    {
                        GameEntry.Entity.ShowRou(new RouData(GameEntry.Entity.GenerateSerialId(), 30000, new Vector3(m_DiePos.x, 58.7f, m_DiePos.z)));
                    }
                }
                else
                {
                    if (m_DieType == 1)
                    {
                        m_DieType = 0;
                        GameEntry.Sound.PlaySound(30000);
                        GameUtil.PhoneVibrate(80);
                        // �ɷ���þ�����¼�
                        GameEntry.Event.Fire(this, ReferencePool.Acquire<RouHideEventArgs>());
                    }
                    else if (m_DieType == 2)
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            GameEntry.Entity.ShowGuoShi(new GuoShiData(GameEntry.Entity.GenerateSerialId(), 30001, new Vector3(m_DiePos.x, 58.7f, m_DiePos.z)));
                        }
                    }
                   
                }

                // �����µ�һ������
                Manager._instance.DistributeEnemy_Out();
                Log.Debug("��������λ��" + Manager._instance.enemyPos);
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), m_EnemyData.TypeId, 
                    Manager._instance.enemyPos));

            }



            // �����¼��ļ���
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // �����¼��ļ���
            GameEntry.Event.Unsubscribe(EvolveEventArgs.EventId, OnEvolve);

        }

        /// <summary>
        ///  ������Һ͵��˵ľ��� Ȼ���жϵ����Ƿ��Ի�
        /// </summary>
        private void JudgePlayerDistance()
        {
            Vector3 targetDir = Manager._instance.playerPos - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            //Log.Debug("�������");
            if (Vector3.Distance(Manager._instance.playerPos, transform.position) * Mathf.Cos(angle) >= m_MaxDistance)
            {
                // �������ƾ��� Ȼ���������˵��ǲ�������� �����µĵ�������Ҹ���
                isCanCreateRou = false;
                // ��������б��Ӧ����
                Manager._instance.creaturesList.Remove(this.gameObject);
                if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                    Manager._instance.selected--;
                GameEntry.Entity.HideEntity(this);
            }
        }

        /// <summary>
        ///  �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEvolve(object sender, GameEventArgs e)
        {
            m_IsEvolveIng = true;
            // ��������б��Ӧ����
            Manager._instance.creaturesList.Remove(this.gameObject);
            //if (Manager._instance.selected != 0)
            //    Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
        }

        private void OnLevelUp(object sender, GameEventArgs e)
        {
            if (Manager._instance.level >= 2)
            {
                m_Creature.Health = 10f;
            }
        }
        /// <summary>
        ///  �ж��Ƿ���Ҫ�������
        /// </summary>
        private void JudgeIsCreateRou()
        {
            switch (m_EnemyData.TypeId)
            {
                case 20001:   // ����
                case 20002:  // Ģ��    ���õ����κζ���
                    m_DieType = 1;
                    break;
                case 20000:  // ��ʬ
                case 20004:   // ������֮���  ������͵�
                case 20005:  // ����
                case 20006:  // Ѹ����
                case 20007:  //������
                case 20008:  //������
                    m_DieType = 3;
                    break;
                case 20003:  // ���� ����
                    m_DieType = 2;
                    break;
                default:
                    m_DieType = 0;
                    break;
            }
        }



        /// <summary>
        ///  ���⴦������Ķ���
        /// </summary>
        private void DealFruiterTree()
        {
            if (m_EnemyData.TypeId == 20003)
            {
                // ����
                this.GetComponent<FruiterTree>().ShowAnimation();
            }
        }
    }

}