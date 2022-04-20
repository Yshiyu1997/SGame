//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-04-16 14:50:27.989
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    /// <summary>
    /// 进化等级表。
    /// </summary>
    public class DRLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取等级编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取敌人物种一。
        /// </summary>
        public int Enemy_1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物种最大数量。
        /// </summary>
        public int EnemyNum_1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敌人物种二。
        /// </summary>
        public int Enemy_2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物种最大数目。
        /// </summary>
        public int EnemyNum_2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敌人物种三。
        /// </summary>
        public int Enemy_3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物种最大数目。
        /// </summary>
        public int EnemyNum_3
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Enemy_1 = int.Parse(columnStrings[index++]);
            EnemyNum_1 = int.Parse(columnStrings[index++]);
            Enemy_2 = int.Parse(columnStrings[index++]);
            EnemyNum_2 = int.Parse(columnStrings[index++]);
            Enemy_3 = int.Parse(columnStrings[index++]);
            EnemyNum_3 = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Enemy_1 = binaryReader.Read7BitEncodedInt32();
                    EnemyNum_1 = binaryReader.Read7BitEncodedInt32();
                    Enemy_2 = binaryReader.Read7BitEncodedInt32();
                    EnemyNum_2 = binaryReader.Read7BitEncodedInt32();
                    Enemy_3 = binaryReader.Read7BitEncodedInt32();
                    EnemyNum_3 = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_Enemy_ = null;

        public int Enemy_Count
        {
            get
            {
                return m_Enemy_.Length;
            }
        }

        public int GetEnemy_(int id)
        {
            foreach (KeyValuePair<int, int> i in m_Enemy_)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetEnemy_ with invalid id '{0}'.", id));
        }

        public int GetEnemy_At(int index)
        {
            if (index < 0 || index >= m_Enemy_.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetEnemy_At with invalid index '{0}'.", index));
            }

            return m_Enemy_[index].Value;
        }

        private KeyValuePair<int, int>[] m_EnemyNum_ = null;

        public int EnemyNum_Count
        {
            get
            {
                return m_EnemyNum_.Length;
            }
        }

        public int GetEnemyNum_(int id)
        {
            foreach (KeyValuePair<int, int> i in m_EnemyNum_)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetEnemyNum_ with invalid id '{0}'.", id));
        }

        public int GetEnemyNum_At(int index)
        {
            if (index < 0 || index >= m_EnemyNum_.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetEnemyNum_At with invalid index '{0}'.", index));
            }

            return m_EnemyNum_[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_Enemy_ = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, Enemy_1),
                new KeyValuePair<int, int>(2, Enemy_2),
                new KeyValuePair<int, int>(3, Enemy_3),
            };

            m_EnemyNum_ = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, EnemyNum_1),
                new KeyValuePair<int, int>(2, EnemyNum_2),
                new KeyValuePair<int, int>(3, EnemyNum_3),
            };
        }
    }
}
