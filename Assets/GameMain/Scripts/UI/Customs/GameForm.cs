using DG.Tweening;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class GameForm : UGuiForm
    {

        private ProcedureMain m_ProcedureMain = null;

        /// <summary>
        ///  ������ϵĿ��ƽű�
        /// </summary>
        private Creature m_PlayerCreature = null;

        [SerializeField]
        private Image m_DNAValue = null;
        [SerializeField]
        private Sprite[] m_DNAValueSprArr = null;
        [SerializeField]
        private Text m_Level = null;

        [SerializeField]
        private Image m_Blood_1 = null;
        [SerializeField]
        private Image m_Blood_2 = null;
        [SerializeField]
        private Image m_Blood_3 = null;


        private float m_GetDNAValue = 0;// ��õ�DNAֵ

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            //m_ProcedureMain = (ProcedureMain)userData;
            //if (m_ProcedureMain == null)
            //{
            //    Log.Warning("ProcedureMenu is invalid when open MenuForm.");
            //    return;
            //}    

            Log.Debug("��ʾ��Ϸ����");
            Manager._instance.isShowGameFormUI = true;
            m_PlayerCreature = Manager._instance.playerObj.GetComponent<Creature>();

            m_Level.text = "" + Manager._instance.level;
            ResetAllBlood();
            //m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(EvolveEventArgs.EventId, OnEvolve);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!Manager._instance.isCanChangePos) return;

            m_DNAValue.fillAmount = Manager._instance.DNAValue / 150;
            if (m_GetDNAValue != Manager._instance.DNAValue)
            {
                m_GetDNAValue =Manager._instance.DNAValue;
                // ����չʾ
                m_DNAValue.sprite = m_DNAValueSprArr[1];
                m_DNAValue.rectTransform.DOScale(new Vector2(1.08f,1.08f),0.2f).SetEase(Ease.InBack).OnComplete(()=> {
                    m_DNAValue.rectTransform.DOScale(new Vector2(1, 1), 0.2f);
                    m_DNAValue.sprite = m_DNAValueSprArr[0];
                });
            }

            m_PlayerCreature = Manager._instance.playerObj.GetComponent<Creature>();
            if (m_PlayerCreature.Health <= 0)
            {
                // ����
                m_Blood_1.gameObject.SetActive(false);
            }else if(m_PlayerCreature.Health<=66.6&& m_PlayerCreature.Health > 33.3)
            {
                m_Blood_3.gameObject.SetActive(false);
            }else if (m_PlayerCreature.Health <= 33.3 && m_PlayerCreature.Health > 0)
            {
                m_Blood_2.gameObject.SetActive(false);
            }
            else 
            {
                ResetAllBlood();
            }
        }


#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            m_ProcedureMain = null;

            base.OnClose(isShutdown, userData);

            // ȡ�������¼��ļ���
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
            // �����¼��ļ���
            GameEntry.Event.Unsubscribe(EvolveEventArgs.EventId, OnEvolve);
        }

        /// <summary>
        ///  ����
        /// </summary>
        public void OnAttack()
        {
            m_PlayerCreature.PlayerAttack();
        }

        /// <summary>
        ///  ȡ������
        /// </summary>
        public void OnCancelAttack()
        {
            m_PlayerCreature.CancelPlayerAttack();
        }

        /// <summary>
        ///  �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEvolve(object sender, GameEventArgs e)
        {
            Manager._instance.creaturesList.Clear();
            Close(true);
        }

        /// <summary>
        ///  ����������ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLevelUp(object sender, GameEventArgs e)
        {
            m_Level.text = "" + Manager._instance.level;
            Log.Debug("�ָ�Ѫ��");
            m_Blood_1.gameObject.SetActive(true);
            m_Blood_2.gameObject.SetActive(true);
            m_Blood_3.gameObject.SetActive(true);
        }
        /// <summary>
        ///  ����Ѫ����ʾ
        /// </summary>
        private void ResetAllBlood()
        {
            m_Blood_1.gameObject.SetActive(true);
            m_Blood_2.gameObject.SetActive(true);
            m_Blood_3.gameObject.SetActive(true);
        }
    }
}
