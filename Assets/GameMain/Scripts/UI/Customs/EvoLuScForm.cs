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
    /// 进化场景界面
    /// </summary>
    public class EvoLuScForm : UGuiForm
    {
        private float UI_Alpha = 1;    //初始化时让UI显示
        public float alphaSpeed = 2f;   //渐隐渐显的速度

        //容器
        public GameObject PlayerGroup;  // 模型父节点

        public GameObject[] playerPre;   // 对应模型

        public GameObject Star1;

        public GameObject Star2;

        public Image Mask;

        public bool iszero = false;
        public Color close;
        CanvasGroup C1;
        CanvasGroup C2;
        Image S1;
        Image S2;

        private Vector3 createPos = new Vector3(-0.2088432f, -2.740666f, -5.842199f);  //位置
        private Vector3 createRotation = new Vector3(7.117f, 201.832f, 1.496f); // 偏移
        private Quaternion m_Rotation;

        private int preSpeciesIndex = 0;// 前一个物种下标
        private int nextSpeciesIndex = 0;// 下一物种的下标
        private bool isFirstSpecies = true;// 是否是第一个生物

        private GameObject preObj;// 进化前
        private GameObject nextObj;// 进化后

        private bool isCanClose = false;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            nextSpeciesIndex = (int)userData;
            Debug.Log("升级到那个" + nextSpeciesIndex);
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

        public float fadeSpeed = 1.5f; // 渐隐渐现速率
        private bool sceneStarting = true; // 表示场景是否开始，若开始，需要有溅现效果

        // Lerp函数在update中调用
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
        // 渐隐
        private void FadeToClear()
        {
            S1.color = Color.Lerp(S1.color, close, fadeSpeed * Time.deltaTime);
            S2.color = Color.Lerp(S2.color, Color.white, fadeSpeed * Time.deltaTime);
            if (S1.color.a <= 0.05f)
            {
                iszero = true;
            }
        }

        // 溅现
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
            // 派发进化的事件
            GameEntry.Event.Fire(this, ReferencePool.Acquire<EvolveEventArgs>());
            //GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000 + Manager._instance.level - 1, PlayerGroup.transform.position, Manager._instance.playerSkinIndexArr[Manager._instance.level - 1]));
            CreateSpecies();
        }

        /// <summary>
        ///  生成物种
        /// </summary>
        private void CreateSpecies()
        {
            if (isFirstSpecies)
            {
                isFirstSpecies = false;
                // 进化前的生物
                preObj = Object.Instantiate(playerPre[preSpeciesIndex], createPos, m_Rotation, PlayerGroup.transform);
                Debug.Log("生成前" + preSpeciesIndex);
                //yym修改
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
                // 进化后的生物
                Debug.Log("生成后" + nextSpeciesIndex);
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
        ///  生成进化后生物
        /// </summary>
        /// <returns></returns>
        //IEnumerator CreateNextSpecies()
        //{
        //    Debug.Log("生成钱");
        //    yield return new WaitForSeconds(7f);
        //    Debug.Log("生成钱_1");
        //    preObj.transform.GetChild(2).gameObject.SetActive(false);
        //    // 生成进化后生物
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
            Debug.Log("生成钱" + preSpeciesIndex);
            //preObj.transform.GetChild(2).gameObject.SetActive(false);
            // 生成进化后生物
            Destroy(preObj);
            CreateSpecies();
        }

        /// <summary>
        ///  获得进化前的物种
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
        ///  初始化一些数据显示
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
            // 派发进化成功的事件
            GameEntry.Event.Fire(this, ReferencePool.Acquire<EvolveSusEventArgs>());
        }

    }
}


