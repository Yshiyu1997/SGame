using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  果树
    /// </summary>
    public class FruiterTree : Creature
    {
        public GameObject m_Stand; // 正常的树

        public GameObject m_FallDown; // 带有倒下动画的树 

        private void OnEnable()
        {
            m_Stand.SetActive(true);
            m_FallDown.SetActive(false);
        }

        /// <summary>
        ///  树倒下
        /// </summary>
        public void ShowAnimation()
        {
            m_Stand.SetActive(false);
            m_FallDown.SetActive(true);
        }

    }
}
