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
    public class AwardForm : UGuiForm
    {

        /// <summary>
        ///  ��ϲ��ǩ
        /// </summary>
        public Transform m_Title;

        /// <summary>
        ///  ��������
        /// </summary>
        private List<CardAward> m_AwardDataList;

        /// <summary>
        /// ��ý�������
        /// </summary>
        public int AwardNum;

        public GameObject AwardItem;

        public Transform AwardGroup;

        private List<GameObject> ItemList;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_AwardDataList = (List<CardAward>)userData;
            AwardNum = m_AwardDataList.Count;
            ItemList = new List<GameObject>();

            LoadItem();
           
        }
        
        /// <summary>
        ///  ���ݽ����������ɿ���
        /// </summary>
        private void LoadItem()
        {
            for (int i = 0; i < AwardNum; i++)
            {
                GameObject obj = Object.Instantiate(AwardItem, AwardGroup);
                obj.SetActive(true);
                ItemList.Add(obj);
                InitTxtData(obj,i);
            }
        }

        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData(GameObject obj,int index)
        {
            if (!m_AwardDataList[index].IsCard)
            {
                // ���ǽ�ҽ���
                obj.GetComponent<Image>().sprite= SpeciesInfo._instance.m_SpeciesCardArr[m_AwardDataList[index].MSpeciesBaseInfo.SkinIndex];
                obj.transform.GetChild(0).GetComponent<Image>().sprite= SpeciesInfo._instance.m_QualitySprArr[(int)m_AwardDataList[index].MSpeciesBaseInfo.CQuality];
                obj.transform.GetChild(1).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Dec;
                obj.transform.GetChild(2).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Name;
                obj.transform.GetChild(3).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Name+"��Ƭx1";
            }
        }


        private void ClearItem()
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                Destroy(ItemList[i]);
            }
        }

        public void OnCloseForm()
        {
            Close(true);
            ClearItem();
            // ��ѡ����������
            GameEntry.UI.OpenUIForm(UIFormId.ChoiceForm);
        }

    }

}
