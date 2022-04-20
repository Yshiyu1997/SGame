using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ����
    /// </summary>
    public class FruiterTree : Creature
    {
        public GameObject m_Stand; // ��������

        public GameObject m_FallDown; // ���е��¶������� 

        private void OnEnable()
        {
            m_Stand.SetActive(true);
            m_FallDown.SetActive(false);
        }

        /// <summary>
        ///  ������
        /// </summary>
        public void ShowAnimation()
        {
            m_Stand.SetActive(false);
            m_FallDown.SetActive(true);
        }

    }
}
