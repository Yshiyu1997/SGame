//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using DG.Tweening;
using GameFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class HPBarItem : MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 0.4f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField]
        private Slider m_HPBar = null;

        [SerializeField]
        private Text m_RequireDNA = null;
        [SerializeField]
        private Text m_GetDNA = null;
        [SerializeField]
        private Image m_Fill = null;
        [SerializeField]
        private Sprite[] m_FillSprArr =null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        private int m_CardNum = 0;// 需要弹出的卡牌数量

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner, Canvas parentCanvas, float fromHPRatio, float toHPRatio)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }
            //Debug.Log("huode对异性的");
            m_ParentCanvas = parentCanvas;

            gameObject.SetActive(true);
            StopAllCoroutines();
            m_Fill.sprite = m_FillSprArr[0];
            m_CachedCanvasGroup.alpha = 1f;
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_HPBar.value = toHPRatio/fromHPRatio;
                m_Owner = owner;
                m_OwnerId = owner.Id;

                m_RequireDNA.text = "/" + fromHPRatio;
                m_GetDNA.text = "" + toHPRatio;
            }

            if (m_HPBar.value == 0)
            {
                m_Fill.gameObject.SetActive(false);
            }

            Refresh();

            //StartCoroutine(HPBarCo(toHPRatio, AnimationSeconds, KeepSeconds, FadeOutSeconds));
        }

        public bool Refresh()
        {
            //if (m_CachedCanvasGroup.alpha <= 0f)
            //{
            //    return false;
            //}

            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.forward*2;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.localPosition = position;
                }
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            m_CachedCanvasGroup.alpha = 1f;
            m_HPBar.value = 1f;
            m_Owner = null;
            //gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_CachedTransform = GetComponent<RectTransform>();
            if (m_CachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            m_CachedCanvasGroup = GetComponent<CanvasGroup>();
            if (m_CachedCanvasGroup == null)
            {
                Log.Error("CanvasGroup is invalid.");
                return;
            }
        }
        /// <summary>
        ///  更新值
        /// </summary>
        /// <param name="val"></param>
        public void UpdateValue(float val)
        {
            if (GameEntry.HPBar.m_AllGetValue < 50)
            {
                GameEntry.HPBar.m_AllGetValue += val;
                Manager._instance.DNAValue += val;
            }
            m_GetDNA.text = "" + GameEntry.HPBar.m_AllGetValue;
            m_HPBar.value = GameEntry.HPBar.m_AllGetValue / 50;
            m_Fill.gameObject.SetActive(true);
            m_GetDNA.color =new Color(252,255,0,255);
            m_GetDNA.rectTransform.DOScale(new Vector2(1.13f,1.13f), 0.2f).OnComplete(() =>
            {
                m_GetDNA.color = Color.white;
                m_GetDNA.rectTransform.DOScale(new Vector2(1f, 1f), 0.2f);
            });
            //m_Fill.sprite = m_FillSprArr[1];
            //this.GetComponent<RectTransform>().DOScale(new Vector2(1.5f,1.5f),0.3f).SetEase(Ease.InSine).OnComplete(()=> {
            //    m_Fill.sprite = m_FillSprArr[0];
            //    this.GetComponent<RectTransform>().DOScale(new Vector2(1f, 1f), 0.3f);
            //});

            if (GameEntry.HPBar.m_AllGetValue == 50&&!GameEntry.HPBar.isHasLevelUp)
            {
                // 升级  更换玩家模型或者贴图
                // 派发升级的事件
                //GameEntry.Event.Fire(this, ReferencePool.Acquire<LevelUpEventArgs>());
                //Manager._instance.level++;
                GameEntry.HPBar.isHasLevelUp = true;
                //Log.Debug("展示升级特效一");
                Manager._instance.isCanMove = false;
                if (Manager._instance.level < 3)
                {
                    GameEntry.Entity.ShowLevelUp_1_Particle(new LevelUp_1Data(GameEntry.Entity.GenerateSerialId(), 40001, Manager._instance.playerObj.transform.position));
                }
                else
                {
                    DealChangeNextLevel();
                }
            }
        }

        /// <summary>
        ///  当前关卡已经升满级了  转到下一个物种  (主线 或者 支线)  区分主线还是支线可以根据生物编号或者关卡编号
        /// </summary>
        private void DealChangeNextLevel()
        {
            UpdateLevelASpecies();

            // 判断是否要存入本地数据
            int evolveLevel_Save = GameEntry.Setting.GetInt("EvolveLevel");
            if (evolveLevel_Save < GameUtil._instance.beginEvolveLevel)
            {
                GameEntry.Setting.SetInt("EvolveLevel", GameUtil._instance.beginEvolveLevel);
            }
            int playerTypeId_Save = GameEntry.Setting.GetInt("BeginPlayerTypeId");
            if (playerTypeId_Save < GameUtil._instance.beginPlayerTypeId)
            {
                GameEntry.Setting.SetInt("BeginPlayerTypeId", GameUtil._instance.beginPlayerTypeId);
            }

            // 展示开宝箱界面  
            //GameUtil._instance.isCanEvole = true;
            GetCardNum();
            GameEntry.UI.OpenUIForm(UIFormId.UnpackForm, m_CardNum);
            m_CachedCanvasGroup.alpha = 0f;
            // 隐藏生成的实体

        }

        /// <summary>
        ///  更新关卡等级和物种
        /// </summary>
        private void UpdateLevelASpecies()
        {
            if (GameUtil._instance.beginEvolveLevel != 4)
            {
                // 正常递进关卡
                GameUtil._instance.beginEvolveLevel++;
                GameUtil._instance.beginPlayerTypeId += 3;
            }
            else
            {
                // 暴龙变哥斯拉  （后期做兼容处理）
            }
        }

        /// <summary>
        ///  需要宝箱弹出的卡牌数量
        /// </summary>
        private void GetCardNum()
        {
            switch (GameUtil._instance.beginEvolveLevel)
            {
                case 3:
                    m_CardNum = 2;
                    break;
                default:
                    m_CardNum = 1;
                    break;
            }
        }


        private IEnumerator HPBarCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
        {
            yield return m_HPBar.SmoothValue(value, animationDuration);
            yield return new WaitForSeconds(keepDuration);
            //yield return m_CachedCanvasGroup.FadeToAlpha(1f, fadeOutDuration);
        }
    }
}
