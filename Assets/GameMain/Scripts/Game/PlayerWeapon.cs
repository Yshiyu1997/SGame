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
        ///  不同关卡小型生物的TypeId（可攻击的）
        /// </summary>
        private List<int> m_MinCreature = new List<int>();

        /// <summary>
        ///  不同关卡大型生物的TypeId（可攻击的）
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
            Debug.Log("碰到生物" + collision.transform.tag);
            if (collision.transform.tag == "Creature"&&this.GetComponent<Creature>().Health>0)
            {
                Debug.Log("碰到生物类型" + collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId);
                if (JudgeIsCanAttack(collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId) == 1)
                {
                    // 碰到小型生物不用攻击动作 直接死亡   比如：迅猛龙
                    collision.gameObject.GetComponent<Creature>().Health = 0;
                }else if(JudgeIsCanAttack(collision.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId) == 2)
                {
                    // 碰到相对大型的需要攻击动作    
                    
                    transform.GetComponent<Creature>().PlayerAttack();
                    GameEntry.Sound.PlaySound(this.GetComponent<Player>().m_AttackSoundId);
                    
                }
                else
                {
                    // 遇到不可以攻击的生物 只能逃跑
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
          
           
        }


        /// <summary>
        ///  判断能否攻击当前生物  返回值类型：1：有小型  2：有大型  0:不可以攻击的
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
        ///  确定所有的生物在不同关卡的不同身份（可分为：小型生物和大型生物）
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
