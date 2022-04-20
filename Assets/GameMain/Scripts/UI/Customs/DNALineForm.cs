using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeciesGame
{
    public class DNALineForm : UGuiForm
    {



        /// <summary>
        ///  获得DNA经验
        /// </summary>
        public Text m_DNAText = null;

        /// <summary>
        ///  应该获得的DNA数量
        /// </summary>
        private int m_DNANum = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_DNANum = 20;

            m_DNAText.text = "DNA+" + m_DNANum;

            // 然后播放动画
            transform.DOMoveY(2f,2f).OnComplete(()=> {
                // 完成后消除该界面
                Close();
            }).onKill();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);


        }

    }
}
