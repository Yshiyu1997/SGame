using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  地图管理
    /// </summary>
    public class MapManager : MonoBehaviour
    {

        public static MapManager _instance;

        /// <summary>
        ///  地图上的树木  （不需要的关卡进行隐藏）
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
        ///  根据不同关卡进行选择隐藏树木
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
        ///  改变树木的隐藏状态
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
