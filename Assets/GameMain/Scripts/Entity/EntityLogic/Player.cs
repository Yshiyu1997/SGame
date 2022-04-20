using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// 玩家实体
    /// </summary>
    public class Player : Entity
    {

        /// <summary>
        ///  玩家的实体数据
        /// </summary>
        private PlayerData m_PlayerData = null;

        /// <summary>
        ///  身上的操作脚本
        /// </summary>
        private Creature m_Creature = null;

        /// <summary>
        ///  获得网格贴图
        /// </summary>
        private SkinnedMeshRenderer m_SkinnedMeshRender = null;

        /// <summary>
        ///  刚体
        /// </summary>
        private Rigidbody m_Rigidbody = null;

        /// <summary>
        ///  玩家血量
        /// </summary>
        private float m_PlayerHealth = 0;

        /// <summary>
        ///  无敌时间时长
        /// </summary>
        private float m_InvincibleTimer = 3;

        /// <summary>
        ///  无敌时间计时
        /// </summary>
        private float m_InvincibleTime = 0;

        /// <summary>
        ///  攻击声音ID
        /// </summary>
        public int m_AttackSoundId = 0;


        private bool m_IsShowFailedForm = false;// 展示失败界面（防止多次展示）
        private bool m_IsEvolveIng = false;// 是否在进化中

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = (PlayerData)userData;
            m_Creature = this.GetComponent<Creature>();
            m_SkinnedMeshRender = this.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            m_Rigidbody = this.GetComponent<Rigidbody>();
            Log.Debug("显示霸王龙" + this.gameObject.name);
            //Invoke("DealTyrannosaurusRex",3f);
            DealPlayer();

            m_IsEvolveIng = false;
            m_IsShowFailedForm = false;

            // 初始化所有的值
            float getDNAVal = (float)GameEntry.DataNode.GetNode("DNAGet").GetData<VarDouble>();
            GameEntry.HPBar.ShowHPBar(this, m_PlayerData.RequireDNA, getDNAVal);
            GameEntry.HPBar.m_AllGetValue = getDNAVal;
         
            Manager._instance.isCanMove = true;
            m_PlayerHealth = m_PlayerData.LifeValue;

            m_AttackSoundId = m_PlayerData.AttackSound;

            // 显示DNA条 的 监听事件
            GameEntry.Event.Sbscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // 升级事件的监听
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // 复活事件的监听
            GameEntry.Event.Sbscribe(ResurgenceEventArgs.EventId,OnResurgence);
            // 重新开始游戏的监听
            GameEntry.Event.Sbscribe(RestartEventArgs.EventId,OnRestartGame);
            // 进化事件的监听
            GameEntry.Event.Sbscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_PlayerHealth != m_Creature.Health&&m_Creature.Health>0)
            {
                Log.Debug("受伤无敌"+m_PlayerHealth+"伤害"+ m_Creature.Health);
                // 表示已经受伤了  展示无敌状态 免受攻击
                GameEntry.Sound.PlaySound(m_PlayerData.HitSound);
                m_PlayerHealth = m_Creature.Health;
                Manager._instance.isInvincible = true;
                // 关闭碰撞体
                m_Creature.PlayerColliderIsTrigger(true);
            }

            if (Manager._instance.isInvincible)
            {
                m_InvincibleTime += elapseSeconds;
                if (m_InvincibleTime < m_InvincibleTimer)
                {
                    float remainer = m_InvincibleTime % 0.2f; // 值越小 闪烁越快
                    m_SkinnedMeshRender.enabled = remainer > 0.12f;
                }
                else
                {
                    m_SkinnedMeshRender.enabled = true;
                    Manager._instance.isInvincible = false;
                    m_InvincibleTime = 0;
                    // 开启碰撞体
                    m_Creature.PlayerColliderIsTrigger(false);
                }
            }

            if (m_Creature.Health <= 0)
            {
                // 死亡 取消刚体禁止移动     展示失败界面
                m_Rigidbody.isKinematic = true;
               

                if (!m_IsShowFailedForm)
                {
                    m_IsShowFailedForm = true;

                    StartCoroutine(ShowFailedForm());
                }
            }
            else
            {
                m_Rigidbody.isKinematic = false;
            }

        }

        /// <summary>
        ///  添加对应的霸王龙 （一级）
        /// </summary>
        private void DealPlayer()
        {
            //GameEntry.DataNode.GetOrAddNode("Level").SetData<VarInt32>();
            //if (Manager._instance.level == 1)
            //{
            //    Manager._instance.isFirstGame = true;
            //}
            //else
            //{
            //    Manager._instance.isFirstGame = false;
            //}
            Log.Debug("皮肤编号"+ m_PlayerData.SkinIndex);
            Manager._instance.OnAddSpecies(this.gameObject, m_PlayerData.IsAI, m_PlayerData.PosValue,m_PlayerData.SkinIndex);
            SetScale(m_PlayerData.Scale);
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
            
            if (!isShutdown&&!m_IsEvolveIng)
            {
                Manager._instance.isFirstGame = false;
                Manager._instance.isCanChangePos = false;
                // 生成更高等级生物
                GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), GameUtil._instance.beginPlayerTypeId + Manager._instance.level - 1, this.transform.position, Manager._instance.playerSkinIndexArr[Manager._instance.level - 1]));
            }
            // 取消监听肉块消失
            GameEntry.Event.Unsubscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // 取消升级事件的监听
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // 取消复活事件的监听
            GameEntry.Event.Unsubscribe(ResurgenceEventArgs.EventId, OnResurgence);
            // 取消重新开始游戏的监听
            GameEntry.Event.Unsubscribe(RestartEventArgs.EventId, OnRestartGame);
            // 进化事件的监听
            GameEntry.Event.Unsubscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        /// <summary>
        ///  进化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEvolve(object sender, GameEventArgs e)
        {
            m_IsEvolveIng = true;
            Manager._instance.isCanChangePos = false;
            // 清除生物列表对应生物
            Manager._instance.creaturesList.Remove(this.gameObject);
            GameEntry.Entity.HideEntity(this);
           
        }

        /// <summary>
        ///  显示DNA经验条
        /// </summary>
        private void OnShowDNAItem(object sender, GameEventArgs e)
        {
            GameEntry.DNA.ShowDNA(this);

            // 更新DNA值
            GameEntry.HPBar.UpdateDNAValue(10);
        }
        /// <summary>
        ///  升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLevelUp(object sender, GameEventArgs e)
        {
            // 清除生物列表对应生物
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
        }

        /// <summary>
        ///  延迟显示失败界面
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowFailedForm()
        {
            yield return new WaitForSeconds(3f);
            GameEntry.UI.OpenUIForm(UIFormId.FailedForm, this);
        }

        /// <summary>
        ///  复活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResurgence(object sender, GameEventArgs e)
        {
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
          
        }

        /// <summary>
        ///  重新开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRestartGame(object sender, GameEventArgs e)
        {
            GameEntry.HPBar.m_AllGetValue = 0;
            // 记录新的节点
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
            Manager._instance.DNAValue = 0;
            Manager._instance.level = 1;
            Log.Debug("当前对象"+this.gameObject);
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);

          

            //GameEntry.Entity.HideAllLoadedEntities();

        }
    }
}
