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
    /// 提示界面
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
        ///  初始化一些数据显示
        /// </summary>
        private void InitTxtData()
        {
            t_Tips.text = "金币不足是否观看广告获得？";

            b_buy.text = "购买";

            b_cancel.text = "取消";

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
