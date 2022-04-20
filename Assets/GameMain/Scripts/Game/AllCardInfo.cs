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
        Normal,  //普通
        Special,  //特殊
        Myth,  //神话
    }

    public class AllCardInfo : MonoBehaviour
    {
        public static AllCardInfo _instance = null;

        private void Awake()
        {
            _instance = this;

        }

        /// <summary>
        ///  根据随机返回的下标获取卡牌所属品质
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
        ///  宝箱卡牌品质随机  CardProArr = { 35, 55, 10 };//普通 特殊 神话
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
        ///  通过随机到的品质从卡牌池组中随机抽取一张卡牌
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
        public List<int> NormalCardGroup;//普通卡牌池组
        public List<int> SpecialCardGroup;//特殊卡牌池组
        public List<int> MythCardGroup;//神话卡牌池组
    }

    [System.Serializable]
    public class CardBaseInfo
    {
        public string Name = ""; // 名字 
        public string Dec = "";  // 描述
        public int ID;
        public int Quality; //品质
        public int GetNum; //当前已收集数量
        public int Limit; //卡牌上限

        public static CardBaseInfo _instance = null;

        private void Awake()
        {
            _instance = this;

        }

        //创建相关卡牌JsON信息
        public static void CreateCardJson(DRPlayer[] PlayerArr)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            Data data = new Data();
            data.CardBaseInfo = new CardBaseInfo[PlayerArr.Length];
            Debug.Log(fileUrl + "  当前卡牌JSon路径！！！！");
            //卡牌池组
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
            //刷新文件列表
            AssetDatabase.Refresh();
#endif 
        }

        //根据卡牌品质获取卡牌池组
        public static List<int> GetCardGroup(int type)
        {
            //读取数据
            string jsonData = ReadCardData();
            //解析数据
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

        //根据ID获取卡牌数量
        public static int GetCardNumByID(int Id)
        {
            //读取数据
            string jsonData = ReadCardData();
            //解析数据
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

        //根据ID获取卡牌品质
        public static int GetCardQualityByID(int Id)
        {
            //读取数据
            string jsonData = ReadCardData();
            //解析数据
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

        //根据卡牌ID修改卡牌获得数量
        public static void AmendCardInfo(int Id, int GetNum)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //读取数据
            string jsonData = ReadCardData();
            //解析数据
            Data info = JsonUtility.FromJson<Data>(jsonData);
            //找到数据修改数据
            foreach (CardBaseInfo card in info.CardBaseInfo)
            {
                if (card.ID == Id)
                {
                    card.GetNum = card.GetNum + GetNum;
                }
            }
            //转成json，然后保存数据
            string json = JsonUtility.ToJson(info);
            AlterCardData(json);
        }

        //读取数据
        public static string ReadCardData()
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //string类型的数据常量
            string readData;

            //读取文件
            using (StreamReader sr = File.OpenText(fileUrl))
            {
                //数据保存
                readData = sr.ReadToEnd();
                sr.Close();
            }
            //返回数据
            return readData;
        }

        //保存修改后的数据
        public static void AlterCardData(string content)
        {
            string fileUrl = Application.dataPath + @"/GameMain/Configs/CardInfojson.json";
            //读取文件
            using (StreamWriter sr = new StreamWriter(fileUrl))
            {
                //数据保存
                sr.WriteLine(content);
                sr.Close();
            }
        }
    }

}
