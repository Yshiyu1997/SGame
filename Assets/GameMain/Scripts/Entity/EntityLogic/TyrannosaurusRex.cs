using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class TyrannosaurusRex :Entity
    {
        /// <summary>
        ///  ������������ʵ������
        /// </summary>
        private TyrannosaurusRexData m_TyrannosaurusRexData = null;

        /// <summary>
        ///  ���ϵĲ����ű�
        /// </summary>
        private Creature m_Creature = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TyrannosaurusRexData = (TyrannosaurusRexData)userData;
            m_Creature = this.GetComponent<Creature>();
            Log.Debug("��ʾ������"+ this.gameObject.name);
            //Invoke("DealTyrannosaurusRex",3f);
            DealTyrannosaurusRex();

            // ��ʼ�����е�ֵ
            GameEntry.HPBar.ShowHPBar(this, 50, 0f);
            GameEntry.HPBar.m_AllGetValue = 0;
           

            // ��ʾDNA�� �� �����¼�
            GameEntry.Event.Sbscribe(RouHideEventArgs.EventId,OnShowDNAItem);
            // �����¼��ļ���
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
        }

        /// <summary>
        ///  ��Ӷ�Ӧ�İ����� ��һ����
        /// </summary>
        private void DealTyrannosaurusRex()
        {
            //GameEntry.DataNode.GetOrAddNode("Level").SetData<VarInt32>();
            Manager._instance.isFirstGame = true;
            Manager._instance.OnAddSpecies(this.gameObject,m_TyrannosaurusRexData.IsAI,m_TyrannosaurusRexData.PosValue,0);
            SetScale(0.05f);
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

            // ȡ�����������ʧ
            GameEntry.Event.Unsubscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // ȡ�������¼��ļ���
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
        }

        /// <summary>
        ///  ��ʾDNA������
        /// </summary>
        private void OnShowDNAItem(object sender,GameEventArgs e)
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
            // ���ɸ��ߵȼ�����
            GameEntry.Entity.ShowBaryonyx(new BaryonyxData(GameEntry.Entity.GenerateSerialId(), 10001, false, 3f, this.transform.position));
            // ��������б��Ӧ����
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
        }

    }
}
