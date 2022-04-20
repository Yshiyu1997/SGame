using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class CardState : MonoBehaviour
    {

        public TreeBranch m_OwnTreeBranch=TreeBranch.MainTree;// �����������߷�֧

        public int index=0;// �±�

        public GameObject m_Black; // δ�����ĺ�ɫ�ɰ�
        public GameObject m_Lock;// ��


        public GameObject[] m_RelevanceArrowArr;  // �͵�ǰ�Ŀ��ƹؿ�������ļ�ͷ
       
        void Start()
        {
            InitState();
        }

        /// <summary>
        ///  ��ʼ��״̬
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
                    // ����
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
                    // ����֧��
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
                    // �ִ�����֧��
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
        ///  ���ý�����״̬
        /// </summary>
        /// <param name="isUnLock"></param>
        private void SetLockState(bool isUnLock)
        {
            m_Black.SetActive(isUnLock);
            m_Lock.SetActive(isUnLock);
        }

        /// <summary>
        ///  ѡ���ͷ�Ƿ����
        /// </summary>
        /// <param name="isHighLight"></param>
        private void SetArrowState(bool isHighLight)
        {
            // �Ѿ�������  �ͷż�ͷΪ ��
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
