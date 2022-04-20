using DG.Tweening;
using SpeciesGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public class TipsForm : UGuiForm
    {

        public Text t_Tips;

        public Text b_buy;

        public Text b_cancel;

        public int Coin;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            InitTxtData();

        }

        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData()
        {
            t_Tips.text = "��Ҳ����Ƿ�ۿ�����ã�";

            b_buy.text = "����";

            b_cancel.text = "ȡ��";

        }

        public void OnGetCoin()
        {
            Coin = GameEntry.Setting.GetInt("Coin");
            Coin = Coin + 1000;
            GameEntry.Setting.SetInt("Coin", Coin);
            BoxForm._instance.UpdateCoin(Coin);
            Close(true);
        }

        public void OnCloseForm()
        {
            Close(true);
        }

    }

}
