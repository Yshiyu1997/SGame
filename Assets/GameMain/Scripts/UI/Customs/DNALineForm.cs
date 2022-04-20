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
        ///  ���DNA����
        /// </summary>
        public Text m_DNAText = null;

        /// <summary>
        ///  Ӧ�û�õ�DNA����
        /// </summary>
        private int m_DNANum = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_DNANum = 20;

            m_DNAText.text = "DNA+" + m_DNANum;

            // Ȼ�󲥷Ŷ���
            transform.DOMoveY(2f,2f).OnComplete(()=> {
                // ��ɺ������ý���
                Close();
            }).onKill();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);


        }

    }
}
