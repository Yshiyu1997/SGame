using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  进化树分支的种类
    /// </summary>
    public enum TreeBranch
    {
        MainTree=0, // 主线
        SoftwareBranch,// 软体支线
        SeaBranch,// 现代海洋支线
        DinaoSaurBranch, // 恐龙支线
    }

    /// <summary>
    ///  进化树界面
    /// </summary>
    public class EvoluTreeForm :UGuiForm
    {

        /// <summary>
        ///  进化树模块
        /// </summary>
        public GameObject m_TreePanel;

        /// <summary>
        ///  当前分支的主题
        /// </summary>
        public Text m_Theme;
        /// <summary>
        ///  当前卡牌收集进度
        /// </summary>
        public Image m_CardCollect;
        /// <summary>
        ///  当前卡牌收集要求多少
        /// </summary>
        public Text m_Card_RequireGet;
        /// <summary>
        ///  当前卡牌收集多少
        /// </summary>
        public Text m_Card_Get;
        /// <summary>
        ///  当前结局的进度
        /// </summary>
        public Image m_EndCollect;
        /// <summary>
        ///  当前结局的总进度
        /// </summary>
        public Text m_EndRequireGet;
        /// <summary>
        ///  当前结局的进度
        /// </summary>
        public Text m_EndGet;
        /// <summary>
        ///  关键卡牌
        /// </summary>
        public Text m_KeyCardTxt;
        /// <summary>
        ///  金币数量
        /// </summary>
        public Text m_CoinTxt;
        /// <summary>
        ///  全部进化分支按钮
        /// </summary>
        public GameObject m_ReturnAll;
        /// <summary>
        ///  主分支
        /// </summary>
        public GameObject m_MainTree;
        /// <summary>
        ///  恐龙分支
        /// </summary>
        public GameObject m_DinaoSaurBranch;
        /// <summary>
        ///  现代海洋支线
        /// </summary>
        public GameObject m_SeaFishBranch;
 
        /// <summary>
        ///  所有的分支
        /// </summary>
        public GameObject m_AllBranch;

        /// <summary>
        ///  确认弹窗
        /// </summary>
        public GameObject m_SurePanel;

        /// <summary>
        /// 确认弹窗的物种标题
        /// </summary>
        public Text m_TitleTxt;



        [HideInInspector]
        public Dictionary<TreeBranch, int> m_TreeBranchLength = new Dictionary<TreeBranch, int>();


        private int m_AllCardNum = 21;// 当前所有的关卡
        private int m_CollectNum = 0;// 已经收集完卡牌解锁的关卡
        private int m_AllEndNum = 4;// 当前总共的结局数
        private int m_CompeleteEndNum = 0;// 完成的结局数


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);


            // 展示主分支 隐藏其他分支
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(true);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // 返回所有分支界面的按钮动态出现
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797,-419), 0.5f).SetEase(Ease.InOutBack);

            InitTxtData();

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            if (!isShutdown)
            {
                // 返回所有分支界面的按钮动态出现
                m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(1124, -419), 0.5f).SetEase(Ease.Linear);
            }
          
        }

        /// <summary>
        ///  获得进化树和分支的长度
        /// </summary>
        private void GetTreeBranchLength()
        {
            if (m_TreeBranchLength.Count == 0)
            {
                // 分配长度
                m_TreeBranchLength.Add(TreeBranch.MainTree,7);
                m_TreeBranchLength.Add(TreeBranch.SoftwareBranch,5);
                m_TreeBranchLength.Add(TreeBranch.SeaBranch, 7);
                m_TreeBranchLength.Add(TreeBranch.DinaoSaurBranch,7);
            }
        }

        /// <summary>
        ///  初始化一些数据显示
        /// </summary>
        private void InitTxtData()
        {
            m_Theme.text = "灵长类崛起";

            int mainTreeLevel = GameEntry.Setting.GetInt("EvolveLevel");
            int seaBranchLevel = GameEntry.Setting.GetInt("SeaLevel");
            int dinosaurLevel = GameEntry.Setting.GetInt("DinosaurLevel");
            m_CollectNum = mainTreeLevel + seaBranchLevel + dinosaurLevel;
            m_Card_Get.text = "" + m_CollectNum;
            m_CardCollect.fillAmount = (float)m_CollectNum/(float)m_AllCardNum;
            m_Card_RequireGet.text = "/" + m_AllCardNum;
            int compeleteNum = GameEntry.Setting.GetInt("CompeleteEnd");
            m_EndCollect.fillAmount = (float)compeleteNum / (float)m_AllEndNum;
            m_EndGet.text = "" + compeleteNum;
            m_EndRequireGet.text = "/" + m_AllEndNum;

            m_KeyCardTxt.text = "智人";
            int coinNum = GameEntry.Setting.GetInt("Coin");
            m_CoinTxt.text = "" + coinNum;


        }


        /// <summary>
        ///  关闭进化树
        /// </summary>
        public void OnCloseForm()
        {
            Log.Debug("关闭界面");
            Close(true);
        }

        /// <summary>
        ///  返回所有分支界面
        /// </summary>
        public void OnReturnAllBranch()
        {
            m_TreePanel.SetActive(false);
            m_AllBranch.SetActive(true);
            // 返回所有分支界面的按钮动态出现
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(1124, -419), 0.5f).SetEase(Ease.Linear);

        }
        /// <summary>
        ///  显示主线
        /// </summary>
        public void OnShowMainTree()
        {
            m_Theme.text = "灵长类崛起";
            m_KeyCardTxt.text = "智人";
            // 展示主分支 隐藏其他分支
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(true);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // 返回所有分支界面的按钮动态出现
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }

        /// <summary>
        ///  显示恐龙支线
        /// </summary>
        public void OnShowDinosaurBranch()
        {
            m_Theme.text = "怪物之王";
            m_KeyCardTxt.text = "哥斯拉";
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(false);
            m_DinaoSaurBranch.SetActive(true);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // 返回所有分支界面的按钮动态出现
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }
        /// <summary>
        ///  显示海洋支线
        /// </summary>
        public void OnShowSeaBranch()
        {
            m_Theme.text = "吞噬天下&镇守四方";
            m_KeyCardTxt.text = "鲲";
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(false);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(true);
            m_AllBranch.SetActive(false);
            // 返回所有分支界面的按钮动态出现
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }

        /// <summary>
        ///  确认选择该物种进行游戏
        /// </summary>
        public void OnSureSelect()
        {
            // 选择物种 获得选择编号

            m_SurePanel.SetActive(false);
        }

        /// <summary>
        ///  取消选择
        /// </summary>
        public void OnCancleSelect()
        {
            m_SurePanel.SetActive(false);
        }
    }

}