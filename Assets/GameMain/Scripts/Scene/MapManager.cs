using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ��ͼ����
    /// </summary>
    public class MapManager : MonoBehaviour
    {

        public static MapManager _instance;

        /// <summary>
        ///  ��ͼ�ϵ���ľ  ������Ҫ�Ĺؿ��������أ�
        /// </summary>
        public GameObject[] m_TreeArr;

        private void Awake()
        {
            if (!_instance) _instance = this;
        }

        void Start()
        {
            JudgeIsHideTree();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        ///  ���ݲ�ͬ�ؿ�����ѡ��������ľ
        /// </summary>
        public void JudgeIsHideTree()
        {
            int evolveLevel = GameUtil._instance.beginEvolveLevel;
            switch (evolveLevel)
            {
                case 1:
                case 3:
                case 4:
                    ChangeTreeState(true);
                    break;
                case 2:
                case 5:
                case 6:
                    ChangeTreeState(false);
                    break;
                default:
                    ChangeTreeState(true);
                    break;
            }
        }


        /// <summary>
        ///  �ı���ľ������״̬
        /// </summary>
        /// <param name="isActive"></param>
        private void ChangeTreeState(bool isActive)
        {
            for(int i = 0; i < m_TreeArr.Length; i++)
            {
                m_TreeArr[i].SetActive(isActive);
            }
        }
    }
}
