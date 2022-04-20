using DG.Tweening;
using SpeciesGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework;

namespace SpeciesGame
{
    /// <summary>
    /// ������������
    /// </summary>
    public class EvoLuScForm : UGuiForm
    {
        private float UI_Alpha = 1;    //��ʼ��ʱ��UI��ʾ
        public float alphaSpeed = 2f;   //�������Ե��ٶ�

        //����
        public GameObject PlayerGroup;  // ģ�͸��ڵ�

        public GameObject[] playerPre;   // ��Ӧģ��

        public GameObject Star1;

        public GameObject Star2;

        public Image Mask;

        public bool iszero = false;
        public Color close;
        CanvasGroup C1;
        CanvasGroup C2;
        Image S1;
        Image S2;

        private Vector3 createPos = new Vector3(-0.2088432f, -2.740666f, -5.842199f);  //λ��
        private Vector3 createRotation = new Vector3(7.117f, 201.832f, 1.496f); // ƫ��
        private Quaternion m_Rotation;

        private int preSpeciesIndex = 0;// ǰһ�������±�
        private int nextSpeciesIndex = 0;// ��һ���ֵ��±�
        private bool isFirstSpecies = true;// �Ƿ��ǵ�һ������

        private GameObject preObj;// ����ǰ
        private GameObject nextObj;// ������

        private bool isCanClose = false;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            nextSpeciesIndex = (int)userData;
            Debug.Log("�������Ǹ�" + nextSpeciesIndex);
            C1 = Star1.GetComponent<CanvasGroup>();
            C2 = Star2.GetComponent<CanvasGroup>();
            S1 = Star1.GetComponent<Image>();
            S2 = Star2.GetComponent<Image>();
            close = new Color(1, 1, 1, 0);
            PlayerGroup = GameObject.Find("EvolveSpecies").gameObject;
            m_Rotation = Quaternion.Euler(createRotation);
            isFirstSpecies = true;
            isCanClose = false;

            GetEvolveSpeciesIndex();

            InitTxtData();
            ShowPlayer();
        }

        public float fadeSpeed = 1.5f; // ������������
        private bool sceneStarting = true; // ��ʾ�����Ƿ�ʼ������ʼ����Ҫ�н���Ч��

        // Lerp������update�е���
        void Update()
        {
            if (iszero)
            {
                FadeToBlack();
            }
            else
            {
                FadeToClear();
            }
        }
        // ����
        private void FadeToClear()
        {
            S1.color = Color.Lerp(S1.color, close, fadeSpeed * Time.deltaTime);
            S2.color = Color.Lerp(S2.color, Color.white, fadeSpeed * Time.deltaTime);
            if (S1.color.a <= 0.05f)
            {
                iszero = true;
            }
        }

        // ����
        private void FadeToBlack()
        {
            S1.color = Color.Lerp(S1.color, Color.white, fadeSpeed * Time.deltaTime);
            S2.color = Color.Lerp(S2.color, close, fadeSpeed * Time.deltaTime);
            if (S1.color.a >= 0.95f)
            {
                iszero = false;
            }
        }

        private void ShowPlayer()
        {
            // �ɷ��������¼�
            GameEntry.Event.Fire(this, ReferencePool.Acquire<EvolveEventArgs>());
            //GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000 + Manager._instance.level - 1, PlayerGroup.transform.position, Manager._instance.playerSkinIndexArr[Manager._instance.level - 1]));
            CreateSpecies();
        }

        /// <summary>
        ///  ��������
        /// </summary>
        private void CreateSpecies()
        {
            if (isFirstSpecies)
            {
                isFirstSpecies = false;
                // ����ǰ������
                preObj = Object.Instantiate(playerPre[preSpeciesIndex], createPos, m_Rotation, PlayerGroup.transform);
                Debug.Log("����ǰ" + preSpeciesIndex);
                //yym�޸�
                preObj.transform.localScale = new Vector3(35, 35, 35);
                preObj.transform.GetChild(2).gameObject.transform.localScale = new Vector3(20f, 20f, 20f);
                preObj.transform.GetChild(2).gameObject.SetActive(true);
                Invoke("ShowPar", 5.8f);
                //StartCoroutine(CreateNextSpecies());
                //Invoke("CreateNextSpecies", 7f);
            }
            else
            {
                //StopCoroutine(CreateNextSpecies());
                // �����������
                Debug.Log("���ɺ�" + nextSpeciesIndex);
                nextObj = Object.Instantiate(playerPre[nextSpeciesIndex], createPos, m_Rotation, PlayerGroup.transform);
                nextObj.transform.localScale = new Vector3(35, 35, 35);
                if (nextObj.transform.childCount >= 4)
                {
                    nextObj.transform.GetChild(3).gameObject.SetActive(true);
                }
                isCanClose = true;
            }

        }

        /// <summary>
        ///  ���ɽ���������
        /// </summary>
        /// <returns></returns>
        //IEnumerator CreateNextSpecies()
        //{
        //    Debug.Log("����Ǯ");
        //    yield return new WaitForSeconds(7f);
        //    Debug.Log("����Ǯ_1");
        //    preObj.transform.GetChild(2).gameObject.SetActive(false);
        //    // ���ɽ���������
        //    Destroy(preObj);
        //    CreateSpecies();
        //}

        private void ShowPar()
        {
            Debug.Log("???!!!!!!!!!!!!!");
            Invoke("CreateNextSpecies", 0.5f);
            //Pingpong(1, 1, 2);
        }


        //void Pingpong(float fromValue, float toValue, float duration)
        //{
        //    Debug.Log("???!!!!!!!!!!!!!222222222");
        //    preObj.transform.gameObject.SetActive(false);
        //    Color temColor = Mask.GetComponent<Image>().color;
        //    temColor.a = fromValue;
        //    Tweener tweener = DOTween.ToAlpha(() => temColor, x => temColor = x, toValue, duration);
        //    tweener.onUpdate = () => { Mask.GetComponent<Image>().color = temColor; };
        //    tweener.onPlay = () =>
        //    {
        //        //Color temColor = Mask.GetComponent<Image>().color;
        //        //temColor.a = toValue;
        //        //Tweener tweener = DOTween.ToAlpha(() => temColor, x => temColor = x, fromValue, 1);
        //        //tweener.onUpdate = () => { Mask.GetComponent<Image>().color = temColor; };
        //        Invoke("CreateNextSpecies", 0.5f);
        //    };
        //}

        private void CreateNextSpecies()
        {
            Debug.Log("����Ǯ" + preSpeciesIndex);
            //preObj.transform.GetChild(2).gameObject.SetActive(false);
            // ���ɽ���������
            Destroy(preObj);
            CreateSpecies();
        }

        /// <summary>
        ///  ��ý���ǰ������
        /// </summary>
        private void GetEvolveSpeciesIndex()
        {
            switch (nextSpeciesIndex)
            {
                case 1:
                case 3:
                case 5:
                case 9:
                    preSpeciesIndex = nextSpeciesIndex - 1;
                    break;
                case 7:
                    preSpeciesIndex = nextSpeciesIndex - 5;
                    break;
            }
        }

        /// <summary>
        ///  ��ʼ��һЩ������ʾ
        /// </summary>
        private void InitTxtData()
        {

        }

        public void OnCloseForm()
        {
            if (!isCanClose) return;
            Close(true);
            Destroy(nextObj);
            GameEntry.HPBar.isHasLevelUp = false;
            // �ɷ������ɹ����¼�
            GameEntry.Event.Fire(this, ReferencePool.Acquire<EvolveSusEventArgs>());
        }

    }
}


