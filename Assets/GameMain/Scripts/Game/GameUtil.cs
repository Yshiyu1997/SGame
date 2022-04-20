using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class GameUtil : MonoBehaviour
    {
        public static GameUtil _instance = null;


        /// <summary>
        ///  �տ�ʼ��Ϸʱ��������ϷTypeId
        /// </summary>
        public int beginPlayerTypeId = 0;

        /// <summary>
        ///  �����ʼ��Ϸ��ť���õĵ�ǰ�Ĺؿ��ȼ�
        /// </summary>
        public int beginEvolveLevel = 0;

        /// <summary>
        ///  �ܹ�����������
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
        /// �ֻ��� ����׿��
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
    ///  ���ƽ�����Ϣ
    /// </summary>
    public class CardAward
    {
        public bool IsCard = false; // �Ƿ��ǿ�����Ƭ����  ���߽�ҽ���
        public SpeciesBaseInfo MSpeciesBaseInfo = null;// ������Ƭ��������Ϣ
        public int CoinNum = 0;// �����Ľ������ ���� ����Ƭ�Ľ�Ǯ

        public CardAward(bool isCard,SpeciesBaseInfo mSpeciesBaseInfo,int coinNum)
        {
            IsCard = isCard;
            MSpeciesBaseInfo = mSpeciesBaseInfo;
            CoinNum = coinNum;
        }
    }


}
