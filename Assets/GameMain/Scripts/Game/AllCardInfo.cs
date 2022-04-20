using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using UnityEditor;


namespace SpeciesGame
{
    public enum CardQuality
    {
        Normal,  //��ͨ
        Special,  //����
        Myth,  //��
    }

    public class AllCardInfo : MonoBehaviour
    {
        public static AllCardInfo _instance = null;

        private void Awake()
        {
            _instance = this;

        }

        /// <summary>
        ///  ����������ص��±��ȡ��������Ʒ��
        /// </summary>
        public static CardQuality GetCardQuality(int i)
        {
            switch (i)
            {
                case 0:
                    return CardQuality.Normal;
                case 1:
                    return CardQuality.Special;
                case 2:
                    return CardQuality.Myth;
            }

            return 0;
        }

        /// <summary>
        ///  ���俨��Ʒ�����  CardProArr = { 35, 55, 10 };//��ͨ ���� ��
        /// </summary>
        public static int RandCard(int[] RandGroup, int total)
        {
            int rand = Random.Range(0, total);
            for (int i = 0; i < RandGroup.Length; i++)
            {
                rand -= RandGroup[i];
                if (rand <= 0)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        ///  ͨ���������Ʒ�ʴӿ��Ƴ����������ȡһ�ſ���
        /// </summary>
        public static int GetCardByQuality(int Quality)
        {
            int Id;
            List<int> CardGroup = CardBaseInfo.GetCardGroup(Quality);
            if (CardGroup.Count > 0)
            {
                int Rang = Random.Range(0, CardGroup.Count);
                Id = CardGroup[Rang];
                return Id;
            }
            return -1;
        }
    }

    [System.Serializable]
    public class Data
    {
        public CardBaseInfo[] CardBaseInfo;
        public List<int> NormalCardGroup;//��ͨ���Ƴ���
        public List<int> SpecialCardGroup;//���⿨�Ƴ���
        public List<int> MythCardGroup;//�񻰿��Ƴ���
    }

    [System.Serializable]
    public class CardBaseInfo
    {
        public string Name = ""; // ���� 
        public string Dec = "";  // ����
        public int ID;
        public int Quality; //Ʒ��
        public int GetNum; //��ǰ���ռ�����
        public int Limit; //��������

        public static CardBaseInfo _instance = null;

        private void Awake()
        {
            _instance = this;

        }

        //������ؿ���JsON��Ϣ
        public static void CreateCardJson(DRPlayer[] PlayerArr)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            Data data = new Data();
            data.CardBaseInfo = new CardBaseInfo[PlayerArr.Length];
            Debug.Log(fileUrl + "  ��ǰ����JSon·����������");
            //���Ƴ���
            List<int> NormalCardGroup = new List<int>();
            List<int> SpecialCardGroup = new List<int>();
            List<int> MythCardGroup = new List<int>();


            if (File.Exists(fileUrl))
            {
                return;
            }

            for (int i = 0; i < PlayerArr.Length; i++)
            {
                CardBaseInfo info = new CardBaseInfo();
                info.ID = PlayerArr[i].Id;
                info.Quality = Random.Range(0, 3);
                info.GetNum = 0;
                info.Limit = 99;

                switch (info.Quality)
                {
                    case 0:
                        NormalCardGroup.Add(info.ID);
                        break;
                    case 1:
                        SpecialCardGroup.Add(info.ID);
                        break;
                    case 2:
                        MythCardGroup.Add(info.ID);
                        break;
                }

                data.CardBaseInfo[i] = info;
            }

            data.NormalCardGroup = NormalCardGroup;
            data.SpecialCardGroup = SpecialCardGroup;
            data.MythCardGroup = MythCardGroup;

            string js = JsonUtility.ToJson(data);
            using (StreamWriter sw = new StreamWriter(fileUrl))
            {
                sw.WriteLine(js);
                sw.Close();
                sw.Dispose();
            }

#if UNITY_EDITOR
            //ˢ���ļ��б�
            AssetDatabase.Refresh();
#endif 
        }

        //���ݿ���Ʒ�ʻ�ȡ���Ƴ���
        public static List<int> GetCardGroup(int type)
        {
            //��ȡ����
            string jsonData = ReadCardData();
            //��������
            Data info = JsonUtility.FromJson<Data>(jsonData);

            switch (type)
            {
                case 0:
                    return info.NormalCardGroup;
                case 1:
                    return info.SpecialCardGroup;
                case 2:
                    return info.MythCardGroup;
            }

            return null;
        }

        //����ID��ȡ��������
        public static int GetCardNumByID(int Id)
        {
            //��ȡ����
            string jsonData = ReadCardData();
            //��������
            Data info = JsonUtility.FromJson<Data>(jsonData);

            foreach (CardBaseInfo card in info.CardBaseInfo)
            {
                if (card.ID == Id)
                {
                    return card.GetNum;
                }
            }

            return -1;
        }

        //����ID��ȡ����Ʒ��
        public static int GetCardQualityByID(int Id)
        {
            //��ȡ����
            string jsonData = ReadCardData();
            //��������
            Data info = JsonUtility.FromJson<Data>(jsonData);

            foreach (CardBaseInfo card in info.CardBaseInfo)
            {
                if (card.ID == Id)
                {
                    return card.Quality;
                }
            }

            return -1;
        }

        //���ݿ���ID�޸Ŀ��ƻ������
        public static void AmendCardInfo(int Id, int GetNum)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //��ȡ����
            string jsonData = ReadCardData();
            //��������
            Data info = JsonUtility.FromJson<Data>(jsonData);
            //�ҵ������޸�����
            foreach (CardBaseInfo card in info.CardBaseInfo)
            {
                if (card.ID == Id)
                {
                    card.GetNum = card.GetNum + GetNum;
                }
            }
            //ת��json��Ȼ�󱣴�����
            string json = JsonUtility.ToJson(info);
            AlterCardData(json);
        }

        //��ȡ����
        public static string ReadCardData()
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //string���͵����ݳ���
            string readData;

            //��ȡ�ļ�
            using (StreamReader sr = File.OpenText(fileUrl))
            {
                //���ݱ���
                readData = sr.ReadToEnd();
                sr.Close();
            }
            //��������
            return readData;
        }

        //�����޸ĺ������
        public static void AlterCardData(string content)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //��ȡ�ļ�
            using (StreamWriter sr = new StreamWriter(fileUrl))
            {
                //���ݱ���
                sr.WriteLine(content);
                sr.Close();
            }
        }
    }

}
