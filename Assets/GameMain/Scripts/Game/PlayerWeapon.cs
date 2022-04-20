using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class PlayerWeapon : MonoBehaviour
    {

        public static PlayerWeapon _instance = null;

        /// <summary>
        ///  ��ͬ�ؿ�С�������TypeId���ɹ����ģ�
        /// </summary>
        private List<int> m_MinCreature = new List<int>();

        /// <summary>
        ///  ��ͬ�ؿ����������TypeId���ɹ����ģ�
        /// </summary>
        private List<int> m_MaxCreature = new List<int>();


        private void Awake()
        {
            _instance = this;
        }



        private void Start()
        {
            InitAllCreatureTypeId();
        }

      

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("��������" + collision.transform.tag);
            if (collision.transform.tag == "Creature"&&this.GetComponent<Creature>().Health>0)
            {
                Debug.Log("������������" + collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId);
                if (JudgeIsCanAttack(collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId) == 1)
                {
                    // ����С�����ﲻ�ù������� ֱ������   ���磺Ѹ����
                    collision.gameObject.GetComponent<Creature>().Health = 0;
                }else if(JudgeIsCanAttack(collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId) == 2)
                {
                    // ������Դ��͵���Ҫ��������    
                    
                    transform.GetComponent<Creature>().PlayerAttack();
                    GameEntry.Sound.PlaySound(this.GetComponent<Player>().m_AttackSoundId);
                    
                }
                else
                {
                    // ���������Թ��������� ֻ������
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
          
           
        }


        /// <summary>
        ///  �ж��ܷ񹥻���ǰ����  ����ֵ���ͣ�1����С��  2���д���  0:�����Թ�����
        /// </summary>
        public int JudgeIsCanAttack(int typeId)
        {
            foreach(int val in m_MinCreature)
            {
                if (val == typeId)
                {
                    return 1;
                }
            }

            foreach (int val in m_MaxCreature)
            {
                if (val == typeId)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        ///  ȷ�����е������ڲ�ͬ�ؿ��Ĳ�ͬ��ݣ��ɷ�Ϊ��С������ʹ������
        /// </summary>
        private void InitAllCreatureTypeId()
        {
            //int level= GameEntry.Setting.GetInt("Level");
            if (m_MinCreature.Count != 0)
            {
                m_MinCreature.Clear();
            }
            if (m_MaxCreature.Count != 0)
            {
                m_MaxCreature.Clear();
            }
            switch (Manager._instance.level)
            {
                case 1:
                    m_MinCreature.Add(20000);
                    m_MinCreature.Add(20001);
                    m_MinCreature.Add(20002);
                    m_MinCreature.Add(20006);
                    m_MinCreature.Add(20003);
                    break;
                case 2:
                    m_MinCreature.Add(20000);
                    m_MinCreature.Add(20001);
                    m_MinCreature.Add(20002);
                    m_MinCreature.Add(20006);
                    m_MinCreature.Add(20003);
                    m_MaxCreature.Add(20004);
                    m_MaxCreature.Add(20007);
                    break;
                case 3:
                    m_MinCreature.Add(20000);
                    m_MinCreature.Add(20001);
                    m_MinCreature.Add(20002);
                    m_MinCreature.Add(20006);
                    m_MinCreature.Add(20003);
                    m_MinCreature.Add(20004);
                    m_MinCreature.Add(20007);
                    m_MaxCreature.Add(20005);
                    m_MaxCreature.Add(20008);
                    break;
            }
        }

    }
}
