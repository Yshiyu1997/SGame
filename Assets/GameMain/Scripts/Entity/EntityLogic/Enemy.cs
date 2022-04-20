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
        ///  敌人生物的实体数据
        /// </summary>
        public EnemyData m_EnemyData = null;

        /// <summary>
        ///  身上的操作脚本
        /// </summary>
        private Creature m_Creature = null;
        /// <summary>
        ///  消失的位置
        /// </summary>
        private Vector3 m_DiePos = Vector3.zero;

        // 已经隐藏了
        private bool isHasHide = false;

        /// <summary>
        ///  是否要生成肉块
        /// </summary>
        private bool isCanCreateRou = false;

        /// <summary>
        ///  攻击声效
        /// </summary>
        public int m_AttackSoundId = 0;

        /// <summary>
        ///  距离玩家的最长距离
        /// </summary>
        private float m_MaxDistance = 10;

        /// <summary>
        ///  死亡类型  1：直接消失无任何生成物或者表现  2：植物类型  3：肉块类型
        /// </summary>
        private int m_DieType=0;

        private bool m_IsEvolveIng = false;// 是否在进化中

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EnemyData = (EnemyData)userData;
            m_Creature = this.GetComponent<Creature>();
            DealEnemy();

            isHasHide = false;
            m_IsEvolveIng = false;

            m_AttackSoundId = m_EnemyData.AttackSound;
           
            // 升级事件的监听
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // 进化事件的监听
            GameEntry.Event.Sbscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if (m_Creature.Health <= 0&&!isHasHide)
            {
                isHasHide = true;
                // 死亡
                m_DiePos = transform.position;
                //// 清除生物列表对应生物
                //Manager._instance.creaturesList.Remove(this.gameObject);
                //if (Manager._instance.selected != 0)
                //    Manager._instance.selected--;
                //GameEntry.Entity.HideEntity(this);
                //// 展示死亡特效
                //GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(), 10007, m_DiePos));
                StartCoroutine(DealEnemyDieIe(DealEnemyDie()));
            
            }

            JudgePlayerDistance();
   
        }

        /// <summary>
        ///  处理敌人播放死亡动画延迟消失
        /// </summary>
        private float DealEnemyDie()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.HitSound);
            switch (m_EnemyData.TypeId)
            {
                case 20000:  // 躺尸   
                case 20001:  // 龙蛋  
                case 20002:  // 蘑菇  
                case 20006:  // 迅猛龙  
                    return 0f;
                case 20003: // 果树
                    return 0.8f;
                case 20004: // 三角龙
                    return 1f;
                case 20007: // 副栉龙
                case 20008: // 霸王龙

                    return 2f;
            }

            return 0f;
        } 

        IEnumerator DealEnemyDieIe(float delayTime)
        {
            DealFruiterTree();
            yield return new WaitForSeconds(delayTime);
            // 清除生物列表对应生物
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0&&Manager._instance.creaturesList.IndexOf(this.gameObject)<Manager._instance.selected)
                Manager._instance.selected--;
            JudgeIsCreateRou();
            isCanCreateRou = m_DieType.Equals(3)?true:false;
            GameEntry.Entity.HideEntity(this);
            // 展示死亡特效
            GameEntry.Entity.ShowDeath_Particle(new Death_ParticleData(GameEntry.Entity.GenerateSerialId(), 40000, m_DiePos));
        }

        /// <summary>
        ///  处理显示对应的实体
        /// </summary>
        private void DealEnemy()
        {
            Manager._instance.OnAddSpecies(this.gameObject, m_EnemyData.IsAI, m_EnemyData.PosValue,0);
            SetScale(m_EnemyData.Scale);
            if (m_Creature) m_Creature.CanAttack = m_EnemyData.IsCanAttack;
        }

        /// <summary>
        ///  设置模型大小
        /// </summary>
        private void SetScale(float setSize)
        {
            setSize = JudgeSize();
            m_Creature.SetScale(setSize);
        }

        /// <summary>
        ///  判断特别不合的大小
        /// </summary>
        private float JudgeSize()
        {
            switch (m_EnemyData.TypeId)
            {
                case 20008:  //霸王龙
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
            Log.Debug("消除敌人生物龙" + isCanCreateRou);
            if (!isShutdown&&!m_IsEvolveIng)
            {
                if (isCanCreateRou)
                {
                    //if (Manager._instance.isHasAttack)
                    //{
                    //    // 玩家取消攻击
                    //    Manager._instance.playerObj.GetComponent<Creature>().CancelPlayerAttack();
                    //    Manager._instance.isHasAttack = false;
                    //}
                    // 生成肉块  根据不同生物生成不同数量的肉块
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
                        // 派发获得经验的事件
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

                // 生成新的一个生物
                Manager._instance.DistributeEnemy_Out();
                Log.Debug("敌人生成位置" + Manager._instance.enemyPos);
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), m_EnemyData.TypeId, 
                    Manager._instance.enemyPos));

            }



            // 升级事件的监听
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // 进化事件的监听
            GameEntry.Event.Unsubscribe(EvolveEventArgs.EventId, OnEvolve);

        }

        /// <summary>
        ///  根据玩家和敌人的距离 然后判断敌人是否自毁
        /// </summary>
        private void JudgePlayerDistance()
        {
            Vector3 targetDir = Manager._instance.playerPos - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            //Log.Debug("计算距离");
            if (Vector3.Distance(Manager._instance.playerPos, transform.position) * Mathf.Cos(angle) >= m_MaxDistance)
            {
                // 超出限制距离 然后消除敌人但是不生成肉块 生成新的敌人在玩家附近
                isCanCreateRou = false;
                // 清除生物列表对应生物
                Manager._instance.creaturesList.Remove(this.gameObject);
                if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                    Manager._instance.selected--;
                GameEntry.Entity.HideEntity(this);
            }
        }

        /// <summary>
        ///  进化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEvolve(object sender, GameEventArgs e)
        {
            m_IsEvolveIng = true;
            // 清除生物列表对应生物
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
        ///  判断是否需要生成肉块
        /// </summary>
        private void JudgeIsCreateRou()
        {
            switch (m_EnemyData.TypeId)
            {
                case 20001:   // 龙蛋
                case 20002:  // 蘑菇    不用掉落任何东西
                    m_DieType = 1;
                    break;
                case 20000:  // 躺尸
                case 20004:   // 三角龙之类的  肉块类型的
                case 20005:  // 腕龙
                case 20006:  // 迅猛龙
                case 20007:  //副栉龙
                case 20008:  //霸王龙
                    m_DieType = 3;
                    break;
                case 20003:  // 测试 果树
                    m_DieType = 2;
                    break;
                default:
                    m_DieType = 0;
                    break;
            }
        }



        /// <summary>
        ///  特殊处理果树的动画
        /// </summary>
        private void DealFruiterTree()
        {
            if (m_EnemyData.TypeId == 20003)
            {
                // 果树
                this.GetComponent<FruiterTree>().ShowAnimation();
            }
        }
    }

}