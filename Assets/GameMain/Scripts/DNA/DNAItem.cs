using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class DNAItem : MonoBehaviour
    {

        /// <summary>
        ///  ���DNA����
        /// </summary>
        public Text m_DNAText = null;

        /// <summary>
        ///  ����һ
        /// </summary>
        public Image m_Value_1 = null;
        /// <summary>
        ///  ���ֶ�
        /// </summary>
        public Image m_Value_2 = null;
        /// <summary>
        ///  ������
        /// </summary>
        public Image m_Value_3 = null;

        /// <summary>
        ///  ����ͼƬ����
        /// </summary>
        public Sprite[] m_ValueSprArr = null;

        /// <summary>
        ///  Ӧ�û�õ�DNA����
        /// </summary>
        private int m_DNANum = 0;


        /// <summary>
        ///  ��ʾ��������
        /// </summary>
        private Entity m_Owner = null;
        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        private RectTransform m_DNARectTransform = null;
        private Canvas m_ParentCanvas = null;


        private void Awake()
        {
            m_DNARectTransform = this.GetComponent<RectTransform>();

        }

        public void Init(Entity owner, Canvas parentCanvas)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }
            m_Owner = owner;
            m_ParentCanvas = parentCanvas;
            // ����DNAֵ�����ݲ�ͬ�����õ�DNA��ͬ
            m_DNANum = 10;
            InitValues(m_DNANum);
            m_DNAText.text = "DNA+" + m_DNANum;
            gameObject.SetActive(true);
            SetStartPos();
            // Ȼ�󲥷Ŷ���
            if (m_DNARectTransform)
            {
                m_DNARectTransform.DOLocalMove(new Vector2(0, 200f), 1.5f).OnComplete(() =>
                {
                    // ��ɺ�����DNA
                    m_Owner = null;
                    gameObject.SetActive(false);
                    m_DNARectTransform.DOScale(new Vector2(1,1),0.1f);
                 });

                m_DNARectTransform.DOScale(new Vector2(1.7f,1.7f),1.5f);
            }
        }

        /// <summary>
        ///  ��ʼ�����е�Valueֵ
        /// </summary>
        private void InitValues(int value)
        {
            m_Value_1.gameObject.SetActive(true);
            m_Value_2.gameObject.SetActive(true);
            m_Value_3.gameObject.SetActive(true);
            if (value < 10&&value>=0)
            {
                // ��λ
                m_Value_2.gameObject.SetActive(false);
                m_Value_3.gameObject.SetActive(false);
                m_Value_1.sprite = m_ValueSprArr[value];
            }
            else if(value>=10&&value<100)
            {
                // ˫λ
                m_Value_3.gameObject.SetActive(false);
                m_Value_1.sprite = m_ValueSprArr[value/10];
                m_Value_2.sprite = m_ValueSprArr[value%10];
            }
            else
            {
                m_Value_1.sprite = m_ValueSprArr[value / 100];
                m_Value_2.sprite = m_ValueSprArr[(value-(value/100)*100)/10];
                m_Value_3.sprite = m_ValueSprArr[(value - (value / 100) * 100) % 10];
            }
        }

        /// <summary>
        ///  ���ó�ʼλ��
        /// </summary>
        private void SetStartPos()
        {
            if (m_Owner != null && Owner.Available )
            {
                Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.forward;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_DNARectTransform.localPosition = position;
                }
            }
        }
    }
}
