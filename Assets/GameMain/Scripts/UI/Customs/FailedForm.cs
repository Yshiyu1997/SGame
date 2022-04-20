using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  ʧ�ܽ���
    /// </summary>
    public class FailedForm : UGuiForm
    {


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Sound.PlaySound(30002);

            // ��¼һ��DNA�ڵ�
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
        }

        /// <summary>
        ///  ���¿�ʼ
        /// </summary>
        public void OnRestartButtonClick()
        {
            Close(true);
            // �ɷ����¿�ʼ���¼�
            GameEntry.Event.Fire(this,ReferencePool.Acquire<RestartEventArgs>());

        }


        /// <summary>
        ///  ����
        /// </summary>
        public void OnResurgenceButtonClick()
        {
            Close(true);
            // �ɷ�������¼�
            GameEntry.Event.Fire(this,ReferencePool.Acquire<ResurgenceEventArgs>());
        }
    }
}
