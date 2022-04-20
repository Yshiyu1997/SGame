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
    /// ��ý�������
    /// </summary>
    public class ChoiceForm : UGuiForm
    {

        /// <summary>
        /// ������
        /// </summary>
        public int CardNum = 0;

        public GameObject CardItem;

        public Transform CardGroup;

        public Text Title;

        public Text Tips;

        public Text BtnText;

        private List<GameObject> ItemList;

        private List<SpeciesBaseInfo> ItemInfoList;

        int CurChoice;

        private int selectIndex = 0;// ȷ�ϵĵ���±�

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ItemList = new List<GameObject>();
            ItemInfoList = new List<SpeciesBaseInfo>();

            JudgeCardNum();
            LoadItem();
            InitTxtData();
        }

        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData()
        {
           
        }

        private void LoadItem()
        {
            for (int i = 0; i < CardNum; i++)
            {
                GameObject obj = Object.Instantiate(CardItem, CardGroup);
                obj.SetActive(true);
                ItemList.Add(obj);
                LoadCardInfo(obj, i);
            }
        }

        private void LoadCardInfo(GameObject obj,int i)
        {
            GameObject select = obj.transform.GetChild(0).gameObject;
            Image speciesImg = obj.transform.GetChild(1).GetComponent<Image>();
            Image qualityImg = obj.transform.GetChild(2).GetComponent<Image>();
            Text Name = obj.transform.Find("Name").gameObject.GetComponent<Text>();
            Text Des = obj.transform.Find("Des").gameObject.GetComponent<Text>();
            Button btn = obj.GetComponent<Button>();
            btn.onClick.AddListener(delegate { ItemClick(i); });
            if (i == 0) select.SetActive(true);
            else select.SetActive(false);
            speciesImg.sprite = SpeciesInfo._instance.m_SpeciesCardArr[ItemInfoList[i].SkinIndex];
            qualityImg.sprite = SpeciesInfo._instance.m_QualitySprArr[(int)ItemInfoList[i].CQuality];
            Name.text = ItemInfoList[i].Name;
            Des.text = ItemInfoList[i].Dec;
        }

        private void ItemClick(int i)
        {
            Debug.Log("��ǰ�������" + i);
            //CurChoice = i;
            selectIndex = i;
            RefreshItem(i);
        }

        private void RefreshItem(int index)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                GameObject Obj = ItemList[i].transform.Find("Q_lizi").gameObject;
                if (i == index)
                {
                    Obj.SetActive(true);
                }
                else
                {
                    Obj.SetActive(false);
                }
            }
        }

        /// <summary>
        ///  �жϸ����ѡ����Ŀ
        /// </summary>
        private void JudgeCardNum()
        {
            if (GameUtil._instance.beginEvolveLevel == 3)  // ������֧�ڵ� ��ʳ������Ϊ ��������������
            {
                CardNum = 2;
                ItemInfoList.Add(SpeciesInfo._instance.GetSpeciesName(SpeciesInfo._instance.JudgeSpeciesType(GameUtil._instance.beginPlayerTypeId)));
                ItemInfoList.Add(SpeciesInfo._instance.GetSpeciesName(SpeciesInfo._instance.JudgeSpeciesType(GameUtil._instance.beginPlayerTypeId+6)));
            }
            else
            {
                CardNum = 1;
                ItemInfoList.Add(SpeciesInfo._instance.GetSpeciesName(SpeciesInfo._instance.JudgeSpeciesType(GameUtil._instance.beginPlayerTypeId)));
            }
        }

        private void ClearItem()
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                Destroy(ItemList[i]);
            }

            ItemInfoList.Clear();
        }

        public void OnClickOk()
        {
            // ����
            OnCloseForm();
            if (selectIndex != 0)
            {
                // ������֧ �ܵ���������
                GameUtil._instance.beginPlayerTypeId += 6;
                GameUtil._instance.beginEvolveLevel += 2;
            }
            JudgeSelectIndex();
            Debug.Log("�������Ǹ�"+ CurChoice);
            GameEntry.UI.OpenUIForm(UIFormId.EvoLuScForm, CurChoice);
        }

        /// <summary>
        ///  �ж�ѡ�������
        /// </summary>
        private void JudgeSelectIndex()
        {
            switch (GameUtil._instance.beginPlayerTypeId)
            {
                case 10003: // ��������ʳ��
                    CurChoice = 1;
                    break;
                case 10006: // ����������
                    CurChoice = 3;
                    break;
                case 10009: // ����������
                    CurChoice = 5;
                    break;
                case 10012: // ����������
                    CurChoice = 7;
                    break;
                case 10015: // ����������
                    CurChoice = 9;
                    break;
            }
        }

        public void OnCloseForm()
        {
            Close(true);
            ClearItem();
        }

    }

}
