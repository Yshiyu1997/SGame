using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class DNAComponent : GameFrameworkComponent
    {
        /// <summary>
        ///  Ԥ����
        /// </summary>
        [SerializeField]
        private DNAItem m_DNAItemTemplate = null;

        /// <summary>
        ///  ���ڵ�
        /// </summary>
        [SerializeField]
        private Transform m_DNAInstanceRoot = null;

        /// <summary>
        ///  ������ڶ�������
        /// </summary>
        [SerializeField]
        private int m_InstancePoolCapacity = 5;

        private IObjectPool<DNAItemObject> m_DNAItemObjectPool = null;  // �����
        private List<DNAItem> m_ActiveDNAItems = null;  // DNA����
        private Canvas m_CachedCanvas = null;

        private int useIndex = 0;// ʹ�õ�DNA����

        private void Start()
        {
            if (m_DNAInstanceRoot == null)
            {
                Log.Error("�������þ������ĸ��ڵ�");
                return;
            }

            m_CachedCanvas = m_DNAInstanceRoot.GetComponent<Canvas>();
            m_DNAItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<DNAItemObject>("DNAItem", m_InstancePoolCapacity);
            m_ActiveDNAItems = new List<DNAItem>();

            InitDNAItemObject();

        }
        /// <summary>
        ///  �ȴ���������
        /// </summary>
        private void InitDNAItemObject()
        {
            for (int i = 0; i < m_InstancePoolCapacity; i++)
            {
                CreateDNAItem();
            }
        }

        private void Update()
        {
            
        }
        /// <summary>
        ///  չʾDNA������
        /// </summary>
        public void ShowDNA(Entity entity)
        {
            if (entity == null)
            {
                Log.Error("û�����ɶ���");
                return;
            }

            DNAItem dnaItem = GetActiveDNAItem(entity);
            if (dnaItem == null)
            {
                dnaItem = CreateDNAItem();
                //m_ActiveDNAItems.Add(dnaItem);
            }

            dnaItem.Init(entity,m_CachedCanvas);
        }

        /// <summary>
        ///  ��ö�ӦDNAitem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DNAItem GetActiveDNAItem(Entity entity)
        {
            //for (int i = 0; i < m_ActiveDNAItems.Count; i++)
            //{
            //    if (m_ActiveDNAItems[i])
            //    {
            //        return m_ActiveDNAItems[i];
            //    }
            //}

            if(useIndex>= m_InstancePoolCapacity)
            {
                useIndex = 0;
            }
            useIndex++;
            return m_ActiveDNAItems[useIndex-1];
        }

        /// <summary>
        ///  ������Ӧ��DNAItem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DNAItem CreateDNAItem()
        {
            DNAItem dnaItem = null;
            DNAItemObject dnaItemObject = m_DNAItemObjectPool.Spawn();
            if (dnaItemObject != null)
            {
                dnaItem = (DNAItem)dnaItemObject.Target;
            }
            else
            {
                dnaItem = Instantiate(m_DNAItemTemplate);
                Transform transform = dnaItem.GetComponent<Transform>();
                transform.SetParent(m_DNAInstanceRoot);
                transform.localScale = Vector3.one;
                m_DNAItemObjectPool.Register(DNAItemObject.Create(dnaItem), true);
                m_ActiveDNAItems.Add(dnaItem);
                dnaItem.gameObject.SetActive(false);
            }

            return dnaItem;
        }

    }
}
