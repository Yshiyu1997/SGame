using DG.Tweening;
using SpeciesGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// 宝箱界面
    /// </summary>
    public class BoxForm : UGuiForm
    {

        public static BoxForm _instance = null;
        /// <summary>
        ///  普通宝箱标题
        /// </summary>
        public Text n_Title;

        /// <summary>
        ///  描述
        /// </summary>
        public Text n_Dec;

        /// <summary>
        ///   花费金币
        /// </summary>
        public Text n_Num;

        /// <summary>
        ///  神奇宝箱标题
        /// </summary>
        public Text m_Title;

        /// <summary>
        ///  描述
        /// </summary>
        public Text m_Dec;

        /// <summary>
        ///   花费金币
        /// </summary>
        public Text m_Num;

        /// <summary>
        ///   玩家当前金币
        /// </summary>
        public static int c_Coin;

        public int evolveLevel;

        public static Transform c_Coin_num;
        public static Text Coin_text;

        protected override void OnOpen(object userData)
        {
            _instance = this;
            base.OnOpen(userData);
            c_Coin_num = transform.Find("Coin/num");
            Coin_text = c_Coin_num.GetComponent<Text>();
            c_Coin = GameEntry.Setting.GetInt("Coin");

            InitTxtData();
            Test();

        }

        /// <summary>
        ///  初始化一些数据显示
        /// </summary>
        private void InitTxtData()
        {

            n_Title.text = "普通宝箱";

            n_Dec.text = "打开后可获得一件物品";

            n_Num.text = "1000";

            m_Title.text = "神奇宝箱";

            m_Dec.text = "打开后可获得四件物品，更有机会获得神话碎片。";

            m_Num.text = "3000";

            Coin_text.text = c_Coin.ToString();
        }

        public void OnBuyNormolBox()
        {
            if (c_Coin < 1000)
            {
                GameEntry.UI.OpenUIForm(UIFormId.TipsForm);
            }
            else
            {
                c_Coin = c_Coin - 1000;
                GameEntry.Setting.SetInt("Coin", c_Coin);
                Debug.Log("购买了普通宝箱");
                UpdateCoin(c_Coin);
                GameEntry.UI.OpenUIForm(UIFormId.UnpackForm,1);
            }
        }

        public void OnBuyMagicBox()
        {
            if (c_Coin < 3000)
            {
                GameEntry.UI.OpenUIForm(UIFormId.TipsForm,3);
            }
            else
            {
                c_Coin = c_Coin - 3000;
                GameEntry.Setting.SetInt("Coin", c_Coin);
                Debug.Log("购买了神奇宝箱");
                UpdateCoin(c_Coin);
                GameEntry.UI.OpenUIForm(UIFormId.UnpackForm, 3);
            }
        }

        public void UpdateCoin(int val)
        {
            c_Coin = val;
            Coin_text.text = c_Coin.ToString();
        }

        public void OnCloseForm()
        {
            Close(true);
        }

        void Test()
        {
            evolveLevel = GameEntry.Setting.GetInt("EvolveLevel", 1);
            IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            DRLevel drLevel = dtLevel.GetDataRow(evolveLevel);

            Debug.Log(drLevel.Enemy_Count + "       test!!!????");
        }

    }

}
