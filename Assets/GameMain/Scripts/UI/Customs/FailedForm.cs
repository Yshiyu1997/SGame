using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  失败界面
    /// </summary>
    public class FailedForm : UGuiForm
    {


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Sound.PlaySound(30002);

            // 记录一个DNA节点
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
        }

        /// <summary>
        ///  重新开始
        /// </summary>
        public void OnRestartButtonClick()
        {
            Close(true);
            // 派发重新开始的事件
            GameEntry.Event.Fire(this,ReferencePool.Acquire<RestartEventArgs>());

        }


        /// <summary>
        ///  复活
        /// </summary>
        public void OnResurgenceButtonClick()
        {
            Close(true);
            // 派发复活的事件
            GameEntry.Event.Fire(this,ReferencePool.Acquire<ResurgenceEventArgs>());
        }
    }
}
