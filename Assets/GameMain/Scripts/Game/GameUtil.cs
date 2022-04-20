using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class GameUtil : MonoBehaviour
    {
        public static GameUtil _instance = null;


        /// <summary>
        ///  刚开始游戏时候的玩家游戏TypeId
        /// </summary>
        public int beginPlayerTypeId = 0;

        /// <summary>
        ///  点击开始游戏按钮后获得的当前的关卡等级
        /// </summary>
        public int beginEvolveLevel = 0;

        /// <summary>
        ///  能够进化新物种
        /// </summary>
        public bool isCanEvole = false;

        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {

        }

        /// <summary>
        /// 手机震动 （安卓）
        /// </summary>
        public static void PhoneVibrate(float time)
        {
            return;
#if UNITY_ANDROID
            AndroidJavaObject m_activity = new AndroidJavaObject("com.ex.phonevibrate.PhoneVibrate");
            AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            m_activity.CallStatic("vibrator", context, 80);
#endif
        }


    }

    /// <summary>
    ///  卡牌奖励信息
    /// </summary>
    public class CardAward
    {
        public bool IsCard = false; // 是否是卡牌碎片奖励  或者金币奖励
        public SpeciesBaseInfo MSpeciesBaseInfo = null;// 卡牌碎片的物种信息
        public int CoinNum = 0;// 奖励的金币数量 或者 卖碎片的金钱

        public CardAward(bool isCard,SpeciesBaseInfo mSpeciesBaseInfo,int coinNum)
        {
            IsCard = isCard;
            MSpeciesBaseInfo = mSpeciesBaseInfo;
            CoinNum = coinNum;
        }
    }


}
