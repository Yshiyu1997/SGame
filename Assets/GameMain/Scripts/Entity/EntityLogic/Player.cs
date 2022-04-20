using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// ���ʵ��
    /// </summary>
    public class Player : Entity
    {

        /// <summary>
        ///  ��ҵ�ʵ������
        /// </summary>
        private PlayerData m_PlayerData = null;

        /// <summary>
        ///  ���ϵĲ����ű�
        /// </summary>
        private Creature m_Creature = null;

        /// <summary>
        ///  ���������ͼ
        /// </summary>
        private SkinnedMeshRenderer m_SkinnedMeshRender = null;

        /// <summary>
        ///  ����
        /// </summary>
        private Rigidbody m_Rigidbody = null;

        /// <summary>
        ///  ���Ѫ��
        /// </summary>
        private float m_PlayerHealth = 0;

        /// <summary>
        ///  �޵�ʱ��ʱ��
        /// </summary>
        private float m_InvincibleTimer = 3;

        /// <summary>
        ///  �޵�ʱ���ʱ
        /// </summary>
        private float m_InvincibleTime = 0;

        /// <summary>
        ///  ��������ID
        /// </summary>
        public int m_AttackSoundId = 0;


        private bool m_IsShowFailedForm = false;// չʾʧ�ܽ��棨��ֹ���չʾ��
        private bool m_IsEvolveIng = false;// �Ƿ��ڽ�����

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = (PlayerData)userData;
            m_Creature = this.GetComponent<Creature>();
            m_SkinnedMeshRender = this.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            m_Rigidbody = this.GetComponent<Rigidbody>();
            Log.Debug("��ʾ������" + this.gameObject.name);
            //Invoke("DealTyrannosaurusRex",3f);
            DealPlayer();

            m_IsEvolveIng = false;
            m_IsShowFailedForm = false;

            // ��ʼ�����е�ֵ
            float getDNAVal = (float)GameEntry.DataNode.GetNode("DNAGet").GetData<VarDouble>();
            GameEntry.HPBar.ShowHPBar(this, m_PlayerData.RequireDNA, getDNAVal);
            GameEntry.HPBar.m_AllGetValue = getDNAVal;
         
            Manager._instance.isCanMove = true;
            m_PlayerHealth = m_PlayerData.LifeValue;

            m_AttackSoundId = m_PlayerData.AttackSound;

            // ��ʾDNA�� �� �����¼�
            GameEntry.Event.Sbscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(ResurgenceEventArgs.EventId,OnResurgence);
            // ���¿�ʼ��Ϸ�ļ���
            GameEntry.Event.Sbscribe(RestartEventArgs.EventId,OnRestartGame);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_PlayerHealth != m_Creature.Health&&m_Creature.Health>0)
            {
                Log.Debug("�����޵�"+m_PlayerHealth+"�˺�"+ m_Creature.Health);
                // ��ʾ�Ѿ�������  չʾ�޵�״̬ ���ܹ���
                GameEntry.Sound.PlaySound(m_PlayerData.HitSound);
                m_PlayerHealth = m_Creature.Health;
                Manager._instance.isInvincible = true;
                // �ر���ײ��
                m_Creature.PlayerColliderIsTrigger(true);
            }

            if (Manager._instance.isInvincible)
            {
                m_InvincibleTime += elapseSeconds;
                if (m_InvincibleTime < m_InvincibleTimer)
                {
                    float remainer = m_InvincibleTime % 0.2f; // ֵԽС ��˸Խ��
                    m_SkinnedMeshRender.enabled = remainer > 0.12f;
                }
                else
                {
                    m_SkinnedMeshRender.enabled = true;
                    Manager._instance.isInvincible = false;
                    m_InvincibleTime = 0;
                    // ������ײ��
                    m_Creature.PlayerColliderIsTrigger(false);
                }
            }

            if (m_Creature.Health <= 0)
            {
                // ���� ȡ�������ֹ�ƶ�     չʾʧ�ܽ���
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
        ///  ��Ӷ�Ӧ�İ����� ��һ����
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
            Log.Debug("Ƥ�����"+ m_PlayerData.SkinIndex);
            Manager._instance.OnAddSpecies(this.gameObject, m_PlayerData.IsAI, m_PlayerData.PosValue,m_PlayerData.SkinIndex);
            SetScale(m_PlayerData.Scale);
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
            
            if (!isShutdown&&!m_IsEvolveIng)
            {
                Manager._instance.isFirstGame = false;
                Manager._instance.isCanChangePos = false;
                // ���ɸ��ߵȼ�����
                GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), GameUtil._instance.beginPlayerTypeId + Manager._instance.level - 1, this.transform.position, Manager._instance.playerSkinIndexArr[Manager._instance.level - 1]));
            }
            // ȡ�����������ʧ
            GameEntry.Event.Unsubscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // ȡ�������¼��ļ���
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // ȡ�������¼��ļ���
            GameEntry.Event.Unsubscribe(ResurgenceEventArgs.EventId, OnResurgence);
            // ȡ�����¿�ʼ��Ϸ�ļ���
            GameEntry.Event.Unsubscribe(RestartEventArgs.EventId, OnRestartGame);
            // �����¼��ļ���
            GameEntry.Event.Unsubscribe(EvolveEventArgs.EventId, OnEvolve);
        }


        /// <summary>
        ///  �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEvolve(object sender, GameEventArgs e)
        {
            m_IsEvolveIng = true;
            Manager._instance.isCanChangePos = false;
            // ��������б��Ӧ����
            Manager._instance.creaturesList.Remove(this.gameObject);
            GameEntry.Entity.HideEntity(this);
           
        }

        /// <summary>
        ///  ��ʾDNA������
        /// </summary>
        private void OnShowDNAItem(object sender, GameEventArgs e)
        {
            GameEntry.DNA.ShowDNA(this);

            // ����DNAֵ
            GameEntry.HPBar.UpdateDNAValue(10);
        }
        /// <summary>
        ///  ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLevelUp(object sender, GameEventArgs e)
        {
            // ��������б��Ӧ����
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
        }

        /// <summary>
        ///  �ӳ���ʾʧ�ܽ���
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowFailedForm()
        {
            yield return new WaitForSeconds(3f);
            GameEntry.UI.OpenUIForm(UIFormId.FailedForm, this);
        }

        /// <summary>
        ///  ����
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
        ///  ���¿�ʼ��Ϸ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRestartGame(object sender, GameEventArgs e)
        {
            GameEntry.HPBar.m_AllGetValue = 0;
            // ��¼�µĽڵ�
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
            Manager._instance.DNAValue = 0;
            Manager._instance.level = 1;
            Log.Debug("��ǰ����"+this.gameObject);
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0 && Manager._instance.creaturesList.IndexOf(this.gameObject) < Manager._instance.selected)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);

          

            //GameEntry.Entity.HideAllLoadedEntities();

        }
    }
}
