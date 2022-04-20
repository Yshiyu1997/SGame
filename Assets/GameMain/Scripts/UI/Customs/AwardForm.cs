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
    /// 获得奖励界面
    /// </summary>
    public class AwardForm : UGuiForm
    {

        /// <summary>
        ///  恭喜标签
        /// </summary>
        public Transform m_Title;

        /// <summary>
        ///  奖励数据
        /// </summary>
        private List<CardAward> m_AwardDataList;

        /// <summary>
        /// 获得奖励次数
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
        ///  根据奖励数量生成卡牌
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
        ///  初始化一些数据显示
        /// </summary>
        private void InitTxtData(GameObject obj,int index)
        {
            if (!m_AwardDataList[index].IsCard)
            {
                // 不是金币奖励
                obj.GetComponent<Image>().sprite= SpeciesInfo._instance.m_SpeciesCardArr[m_AwardDataList[index].MSpeciesBaseInfo.SkinIndex];
                obj.transform.GetChild(0).GetComponent<Image>().sprite= SpeciesInfo._instance.m_QualitySprArr[(int)m_AwardDataList[index].MSpeciesBaseInfo.CQuality];
                obj.transform.GetChild(1).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Dec;
                obj.transform.GetChild(2).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Name;
                obj.transform.GetChild(3).GetComponent<Text>().text = m_AwardDataList[index].MSpeciesBaseInfo.Name+"碎片x1";
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
            // 打开选择升级界面
            GameEntry.UI.OpenUIForm(UIFormId.ChoiceForm);
        }

    }

}
