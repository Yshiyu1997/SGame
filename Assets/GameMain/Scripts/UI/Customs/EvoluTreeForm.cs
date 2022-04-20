using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    ///  ��������֧������
    /// </summary>
    public enum TreeBranch
    {
        MainTree=0, // ����
        SoftwareBranch,// ����֧��
        SeaBranch,// �ִ�����֧��
        DinaoSaurBranch, // ����֧��
    }

    /// <summary>
    ///  ����������
    /// </summary>
    public class EvoluTreeForm :UGuiForm
    {

        /// <summary>
        ///  ������ģ��
        /// </summary>
        public GameObject m_TreePanel;

        /// <summary>
        ///  ��ǰ��֧������
        /// </summary>
        public Text m_Theme;
        /// <summary>
        ///  ��ǰ�����ռ�����
        /// </summary>
        public Image m_CardCollect;
        /// <summary>
        ///  ��ǰ�����ռ�Ҫ�����
        /// </summary>
        public Text m_Card_RequireGet;
        /// <summary>
        ///  ��ǰ�����ռ�����
        /// </summary>
        public Text m_Card_Get;
        /// <summary>
        ///  ��ǰ��ֵĽ���
        /// </summary>
        public Image m_EndCollect;
        /// <summary>
        ///  ��ǰ��ֵ��ܽ���
        /// </summary>
        public Text m_EndRequireGet;
        /// <summary>
        ///  ��ǰ��ֵĽ���
        /// </summary>
        public Text m_EndGet;
        /// <summary>
        ///  �ؼ�����
        /// </summary>
        public Text m_KeyCardTxt;
        /// <summary>
        ///  �������
        /// </summary>
        public Text m_CoinTxt;
        /// <summary>
        ///  ȫ��������֧��ť
        /// </summary>
        public GameObject m_ReturnAll;
        /// <summary>
        ///  ����֧
        /// </summary>
        public GameObject m_MainTree;
        /// <summary>
        ///  ������֧
        /// </summary>
        public GameObject m_DinaoSaurBranch;
        /// <summary>
        ///  �ִ�����֧��
        /// </summary>
        public GameObject m_SeaFishBranch;
 
        /// <summary>
        ///  ���еķ�֧
        /// </summary>
        public GameObject m_AllBranch;

        /// <summary>
        ///  ȷ�ϵ���
        /// </summary>
        public GameObject m_SurePanel;

        /// <summary>
        /// ȷ�ϵ��������ֱ���
        /// </summary>
        public Text m_TitleTxt;



        [HideInInspector]
        public Dictionary<TreeBranch, int> m_TreeBranchLength = new Dictionary<TreeBranch, int>();


        private int m_AllCardNum = 21;// ��ǰ���еĹؿ�
        private int m_CollectNum = 0;// �Ѿ��ռ��꿨�ƽ����Ĺؿ�
        private int m_AllEndNum = 4;// ��ǰ�ܹ��Ľ����
        private int m_CompeleteEndNum = 0;// ��ɵĽ����


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);


            // չʾ����֧ ����������֧
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(true);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // �������з�֧����İ�ť��̬����
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797,-419), 0.5f).SetEase(Ease.InOutBack);

            InitTxtData();

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            if (!isShutdown)
            {
                // �������з�֧����İ�ť��̬����
                m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(1124, -419), 0.5f).SetEase(Ease.Linear);
            }
          
        }

        /// <summary>
        ///  ��ý������ͷ�֧�ĳ���
        /// </summary>
        private void GetTreeBranchLength()
        {
            if (m_TreeBranchLength.Count == 0)
            {
                // ���䳤��
                m_TreeBranchLength.Add(TreeBranch.MainTree,7);
                m_TreeBranchLength.Add(TreeBranch.SoftwareBranch,5);
                m_TreeBranchLength.Add(TreeBranch.SeaBranch, 7);
                m_TreeBranchLength.Add(TreeBranch.DinaoSaurBranch,7);
            }
        }

        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData()
        {
            m_Theme.text = "�鳤������";

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

            m_KeyCardTxt.text = "����";
            int coinNum = GameEntry.Setting.GetInt("Coin");
            m_CoinTxt.text = "" + coinNum;


        }


        /// <summary>
        ///  �رս�����
        /// </summary>
        public void OnCloseForm()
        {
            Log.Debug("�رս���");
            Close(true);
        }

        /// <summary>
        ///  �������з�֧����
        /// </summary>
        public void OnReturnAllBranch()
        {
            m_TreePanel.SetActive(false);
            m_AllBranch.SetActive(true);
            // �������з�֧����İ�ť��̬����
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(1124, -419), 0.5f).SetEase(Ease.Linear);

        }
        /// <summary>
        ///  ��ʾ����
        /// </summary>
        public void OnShowMainTree()
        {
            m_Theme.text = "�鳤������";
            m_KeyCardTxt.text = "����";
            // չʾ����֧ ����������֧
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(true);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // �������з�֧����İ�ť��̬����
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }

        /// <summary>
        ///  ��ʾ����֧��
        /// </summary>
        public void OnShowDinosaurBranch()
        {
            m_Theme.text = "����֮��";
            m_KeyCardTxt.text = "��˹��";
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(false);
            m_DinaoSaurBranch.SetActive(true);
            m_SeaFishBranch.SetActive(false);
            m_AllBranch.SetActive(false);
            // �������з�֧����İ�ť��̬����
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }
        /// <summary>
        ///  ��ʾ����֧��
        /// </summary>
        public void OnShowSeaBranch()
        {
            m_Theme.text = "��������&�����ķ�";
            m_KeyCardTxt.text = "��";
            m_TreePanel.SetActive(true);
            m_MainTree.SetActive(false);
            m_DinaoSaurBranch.SetActive(false);
            m_SeaFishBranch.SetActive(true);
            m_AllBranch.SetActive(false);
            // �������з�֧����İ�ť��̬����
            m_ReturnAll.GetComponent<RectTransform>().DOLocalMove(new Vector2(797, -419), 0.5f).SetEase(Ease.InOutBack);
        }

        /// <summary>
        ///  ȷ��ѡ������ֽ�����Ϸ
        /// </summary>
        public void OnSureSelect()
        {
            // ѡ������ ���ѡ����

            m_SurePanel.SetActive(false);
        }

        /// <summary>
        ///  ȡ��ѡ��
        /// </summary>
        public void OnCancleSelect()
        {
            m_SurePanel.SetActive(false);
        }
    }

}