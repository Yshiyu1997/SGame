//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-04-16 14:50:27.970
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
    /// 玩家生物表。
    /// </summary>
    public class DRPlayer : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取生物编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取最大生命。
        /// </summary>
        public int LifeValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否是AI。
        /// </summary>
        public bool IsAi
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取角速度。
        /// </summary>
        public float AngularSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击音效。
        /// </summary>
        public int AttackSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取受击音效。
        /// </summary>
        public int HitSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取大小。
        /// </summary>
        public float Scale
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级所需的DNA经验。
        /// </summary>
        public int RequireDNA
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
            LifeValue = int.Parse(columnStrings[index++]);
            IsAi = bool.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);
            AngularSpeed = float.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);
            AttackSound = int.Parse(columnStrings[index++]);
            HitSound = int.Parse(columnStrings[index++]);
            Scale = float.Parse(columnStrings[index++]);
            RequireDNA = int.Parse(columnStrings[index++]);

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
                    LifeValue = binaryReader.Read7BitEncodedInt32();
                    IsAi = binaryReader.ReadBoolean();
                    Speed = binaryReader.ReadSingle();
                    AngularSpeed = binaryReader.ReadSingle();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                    AttackSound = binaryReader.Read7BitEncodedInt32();
                    HitSound = binaryReader.Read7BitEncodedInt32();
                    Scale = binaryReader.ReadSingle();
                    RequireDNA = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
