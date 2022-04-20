using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class CardState : MonoBehaviour
    {

        public TreeBranch m_OwnTreeBranch=TreeBranch.MainTree;// 所属的树或者分支

        public int index=0;// 下标

        public GameObject m_Black; // 未解锁的黑色蒙板
        public GameObject m_Lock;// 锁


        public GameObject[] m_RelevanceArrowArr;  // 和当前的卡牌关卡相关联的箭头
       
        void Start()
        {
            InitState();
        }

        /// <summary>
        ///  初始化状态
        /// </summary>
        private void InitState()
        {
            string mainTreeUnlockState = GameEntry.Setting.GetString("MainTreeUnLockState");
            string dinosaurUnlockState = GameEntry.Setting.GetString("DinosaurUnLockState");
            string seaFishUnlockState = GameEntry.Setting.GetString("SeaUnLockState");
            switch (m_OwnTreeBranch)
            {
                case TreeBranch.MainTree:
                    if (index >= mainTreeUnlockState.Length) index--;
                    // 主线
                    if (mainTreeUnlockState[index] == '1')
                    {
                        SetLockState(false);
                        SetArrowState(false);
                    }
                    else
                    {
                        SetLockState(true);
                        SetArrowState(true);
                    }
                    break;
                case TreeBranch.DinaoSaurBranch:
                    if (index >= dinosaurUnlockState.Length) index--;
                    // 恐龙支线
                    if (dinosaurUnlockState[index] == '1')
                    {
                        SetLockState(false);
                        SetArrowState(false);
                    }
                    else
                    {
                        SetLockState(true);
                        SetArrowState(true);
                    }
                    break;
                case TreeBranch.SeaBranch:
                    if (index >= seaFishUnlockState.Length) index--;
                    // 现代海洋支线
                    if (seaFishUnlockState[index] == '1')
                    {
                        SetLockState(false);
                        SetArrowState(false);
                    }
                    else
                    {
                        SetLockState(true);
                        SetArrowState(true);
                    }
                    break;
            }
        }
       
        /// <summary>
        ///  设置解锁的状态
        /// </summary>
        /// <param name="isUnLock"></param>
        private void SetLockState(bool isUnLock)
        {
            m_Black.SetActive(isUnLock);
            m_Lock.SetActive(isUnLock);
        }

        /// <summary>
        ///  选择箭头是否高亮
        /// </summary>
        /// <param name="isHighLight"></param>
        private void SetArrowState(bool isHighLight)
        {
            // 已经解锁了  释放箭头为 亮
            if (m_RelevanceArrowArr.Length != 0)
            {
                for (int i = 0; i < m_RelevanceArrowArr.Length; i++)
                {
                    m_RelevanceArrowArr[i].transform.GetChild(0).gameObject.SetActive(isHighLight);
                }
            }
        }
       
    }
}
