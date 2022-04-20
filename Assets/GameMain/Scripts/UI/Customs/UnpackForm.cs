using DG.Tweening;
using SpeciesGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// �������
    /// </summary>
    public class UnpackForm : UGuiForm
    {

        /// <summary>
        ///  ����ͼƬ
        /// </summary>
        public Image m_SpeciesImg;

        /// <summary>
        ///  ����Ʒ�ʿ�
        /// </summary>
        public Image m_QualityImg;

        /// <summary>
        /// ���ƽ���
        /// </summary>
        public Text Des;

        /// <summary>
        /// ��������
        /// </summary>
        public Text Name;

        /// <summary>
        /// ����Ʒ��
        /// </summary>
        public Text Quality;

        /// <summary>
        /// ���ƽ���1
        /// </summary>
        public Text P_text;

        /// <summary>
        /// ���ƽ���2
        /// </summary>
        public Text P_text2;

        /// <summary>
        ///  ����
        /// </summary>
        public Image m_Box;

        /// <summary>
        ///  ����ľ���ͼƬ
        /// </summary>
        public Sprite[] m_BoxArr;

        /// <summary>
        /// ���ƺͿ�����Ϣ
        /// </summary>  
        public GameObject Card;

        public GameObject CardInfo;

        public Vector3 BornPoint;

        public Vector3[] PointList;

        /// <summary>
        /// �������� 1����һ�� 3��������
        /// </summary>  
        public int OpenNum;

        /// <summary>
        ///  ����Ҫ��ʾ��Ϣ�����ֱ��ID
        /// </summary>
        private int m_CardSpeciesTypeId = 0;

        /// <summary>
        ///  �ܷ�ֹͣ���䶯��
        /// </summary>
        private bool isCanStopBoxAni = false;

        /// <summary>
        ///  �Ѿ��򿪴���
        /// </summary>
        private int hasOpenTimes = 0;

        private bool isCanClickBox = true;// �ܷ�������

        /// <summary>
        ///  ���ƽ�����Ϣ
        /// </summary>
        private List<CardAward>  m_CardAward=new List<CardAward>();

        private int nextSpeciesIndex = 0;  //  ����һ����֧ 

        /// <summary>
        /// ���������жϵĶ�������
        /// </summary>  
        public Queue<IEnumerator> Anique = new Queue<IEnumerator>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OpenNum = (int)userData;
            BornPoint = Card.transform.position;
            PointList = new Vector3[1];
            PointList[0] = BornPoint;

            m_CardAward.Clear();
            isCanStopBoxAni = false;
            isCanClickBox = true;
            hasOpenTimes = 0;
            BoxAni();

            GetOtherBranch();

            InitTxtData();
        }


        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData()
        {
            //m_CardSpeciesTypeId = GameUtil._instance.beginPlayerTypeId;
            //SpeciesTree speciesTree = SpeciesInfo._instance.JudgeSpeciesType(m_CardSpeciesTypeId);
            //SpeciesBaseInfo speciesBaseInfo = SpeciesInfo._instance.GetSpeciesName(speciesTree);
            //Des.text = speciesBaseInfo.Dec;
            //Name.text = speciesBaseInfo.Name;
            //Quality.text = SpeciesInfo._instance.GetQualityDec(speciesBaseInfo.CQuality);
            //m_SpeciesImg.sprite = SpeciesInfo._instance.m_SpeciesCardArr[speciesBaseInfo.SkinIndex];
            //m_QualityImg.sprite = SpeciesInfo._instance.m_QualitySprArr[(int)speciesBaseInfo.CQuality];

            //// ���ǽ�Ǯ�Ľ�������ʽ
            //m_CardAward.Add(new CardAward(false,speciesBaseInfo,0));
            InitCardInfo(GameUtil._instance.beginPlayerTypeId);
        }

        private void InitCardInfo(int speciesIndex)
        {
            m_CardSpeciesTypeId = speciesIndex;
            SpeciesTree speciesTree = SpeciesInfo._instance.JudgeSpeciesType(m_CardSpeciesTypeId);
            SpeciesBaseInfo speciesBaseInfo = SpeciesInfo._instance.GetSpeciesName(speciesTree);
            Des.text = speciesBaseInfo.Dec;
            Name.text = speciesBaseInfo.Name;
            Quality.text = SpeciesInfo._instance.GetQualityDec(speciesBaseInfo.CQuality);
            m_SpeciesImg.sprite = SpeciesInfo._instance.m_SpeciesCardArr[speciesBaseInfo.SkinIndex];
            m_QualityImg.sprite = SpeciesInfo._instance.m_QualitySprArr[(int)speciesBaseInfo.CQuality];

            // ���ǽ�Ǯ�Ľ�������ʽ
            m_CardAward.Add(new CardAward(false, speciesBaseInfo, 0));
        }

        /// <summary>
        /// �ȵ�������֧
        /// </summary>
        private  void GetOtherBranch()
        {
            if (OpenNum > 1)
            {
                // �����֧  
                // ����
                nextSpeciesIndex= GameUtil._instance.beginPlayerTypeId+6;
            }
        }

        /// <summary>
        ///  ���ſ�����Ч
        /// </summary>
        //public void PlayAni()
        //{
        //    Card.SetActive(true);
        //    Card.transform.DOMove(new Vector2(Card.transform.position.x, Card.transform.position.y + 300),2f).OnStart(() =>
        //    {
        //        //Debug.Log("�ƶ���ϣ�������");
        //        //Debug.Log(Card.transform.localPosition);
        //        //Debug.Log(Card.transform.position);
        //       Card.transform.localScale = new Vector2(0.3f, 0.3f);
        //       Card.transform.DOScale(new Vector2(1,1), 3f).OnStart(() =>
        //        {

        //        });
        //    });
        //}

        //public void Update()
        //{
        //    Debug.Log(Card.transform.localPosition);
        //    Debug.Log(Card.transform.position);
        //}

        IEnumerator PlayAni()
        {
            for (int i = 0; i < OpenNum; i++)
            {
                Card.SetActive(true);
                Card.transform.localScale = Vector3.zero;
                Card.transform.DOMoveY(Card.transform.position.y + 3, 1f).SetEase(Ease.Flash);
                Card.transform.DOBlendableScaleBy(Vector3.one, 2f);
                Sequence se = DOTween.Sequence();
                se.Append(Card.transform.DORotate(new Vector3(0, 180, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(10));
                se.AppendInterval(1.5f);
                se.AppendCallback(delegate { MoveBack(Card); });
                yield return new WaitForSeconds(4f);
            }

        }

        void MoveBack(GameObject obj)
        {
            Sequence se = DOTween.Sequence();
            se.Append(obj.transform.DOMove(new Vector2(obj.transform.position.x - 1.5f, obj.transform.position.y - 1.5f), 0.3f).SetEase(Ease.OutQuad));
            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            se.AppendInterval(0.1f);
            se.AppendCallback(delegate { InitCardInfo(); });
            StopCoroutine(PlayAni());

            //se.Append(obj.transform.DOBlendableScaleBy(-Vector3.one, 1f));
            //if (OpenNum == 1)
            //{
            //    Anique.Clear();
            //}
            //else if (OpenNum == 3)
            //{
            //    Anique.Dequeue();
            //}
            Debug.Log("�ƶ�������������������������������������");
        }

        /// <summary>
        ///  ��ʼ����Ƭ��Ϣ
        /// </summary>
        void InitCardInfo()
        {
            hasOpenTimes++;
            CardInfo.SetActive(true);
            if (OpenNum < 2)
            {
            }
            else
            {
                Sequence se = DOTween.Sequence();
                se.Append(Card.transform.DOMove(new Vector2(Card.transform.position.x, Card.transform.position.y), 0f).SetEase(Ease.OutQuad));
                se.AppendInterval(1f);
                se.AppendCallback(delegate { ResetData(); InitCardInfo(nextSpeciesIndex); });
            }
        }

        /// <summary>
        ///  �������
        /// </summary>
        public void OnClickBox()
        {
            if (isCanStopBoxAni) return;
            isCanStopBoxAni = true;
            Debug.Log("�������");
            DOTween.Pause("Box");
            DOTween.Kill("Box");
            m_Box.transform.localScale = Vector3.one;
            m_Box.sprite = m_BoxArr[1];
            StartCoroutine(PlayAni());
        }

        /// <summary>
        ///  ���䶯��
        /// </summary>
        private void BoxAni()
        {
            if (isCanStopBoxAni) return;
            m_Box.transform.DOShakeScale(1, new Vector3(0.25f, 0.2f, 0f), 1, 10).OnComplete(() => {
                if (isCanStopBoxAni)
                {
                  
                }
            }).SetLoops(-1, LoopType.Yoyo).SetId<Tween>("Box");
        }

        /// <summary>
        ///  ��������
        /// </summary>
        private void ResetData()
        {
            Card.transform.position = BornPoint;
            Card.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Card.SetActive(false);
            CardInfo.SetActive(false);
            Debug.Log("��������");
        }

        public void OnCloseForm()
        {
            m_Box.sprite = m_BoxArr[0];
            if (hasOpenTimes < OpenNum) return;
            ResetData();
            Close();
            GameEntry.UI.OpenUIForm(UIFormId.AwardForm, m_CardAward);
        }

    }

}
