using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  ������ö��
    /// </summary>
    public enum SpeciesTree
    {
        None = 0,
        Cell,       // ���ϸ��
        AncientFish,  // Զ������
        Lizard,       // ����
        Duckmole,     // Ѽ����
        Squirrel,     // ����
        Ape,          // Գ��
        Human,        // ����
        TerrifyingApe, // �ֲ�ֱ��Գ

        TropicalFish,     // �ȴ���
        BonyFish,           // Ӳ������
        CartilaginousFish,  // �������
        SeaTurtle,          // ����    
        Tortoise,           // ����
        GuardingQuartet,    // �����ķ�
        MarineMammals,      // ��������
        LegendaryFish,      // ��
        GobbleWorld,         // ��������

        Eoraptor,         // ʼ����
        PlatEatDinao,       // ��ʳ��
        MeatEatDinao,       // ����
        Tyrannosaurus,      // ����
        Triceratops,        // ������
        Brachiosaurus,      // ����
        Godzilla,           // ��˹��
        MonsterKing         // ����֮��
    }

    /// <summary>
    ///  ����Ʒ��
    /// </summary>
    public enum Quality
    {
        Common,
        High,
        Myth
    }



    /// <summary>
    ///  ������Ϣ��������䣩
    /// </summary>
    public class SpeciesInfo : MonoBehaviour
    {

        public static SpeciesInfo _instance = null;

        /// <summary>
        ///  ���ֿ���Ʒ�ʿ�  ��ͨ  ϡ��  ��
        /// </summary>
        public Sprite[] m_QualitySprArr;

        /// <summary>
        ///  ���ֿ��� ͼƬ����  
        /// </summary>
        public Sprite[] m_SpeciesCardArr;

        private void Awake()
        {
            if (!_instance) _instance = this;

        }

        /// <summary>
        ///  �ж����ֵķ�֧������ ���ݲ�ͬö�ٽ��й���
        /// </summary>
        public SpeciesTree JudgeSpeciesType(int typeId)
        {
            switch (typeId)
            {
                case 10000:
                    return SpeciesTree.Eoraptor;
                case 10003:
                    return SpeciesTree.PlatEatDinao;
                case 10006:
                    return SpeciesTree.MeatEatDinao;
                case 10009:
                    return SpeciesTree.Tyrannosaurus;
                case 10012:
                    return SpeciesTree.Triceratops;
                case 10015:
                    return SpeciesTree.Brachiosaurus;
            }

            return SpeciesTree.None;
        }

        /// <summary>
        ///  ������ֵĻ�����Ϣ ���֡�����
        /// </summary>
        /// <returns></returns>
        public SpeciesBaseInfo GetSpeciesName(SpeciesTree species)
        {
            SpeciesBaseInfo speciesBaseInfo = null;
            switch (species)
            {
                case SpeciesTree.Eoraptor:
                    speciesBaseInfo = new SpeciesBaseInfo("ʼ����","Զ�ŵ�һ�ֿ�������͵ʳ������һЩֲ��Ϊ����",Quality.Common,0);
                    return speciesBaseInfo;
                case SpeciesTree.PlatEatDinao:
                    speciesBaseInfo = new SpeciesBaseInfo("ʳ����", "Զ�ŵ�һ�ֿ�������ֲ��Ϊ����",Quality.Common,1);
                    return speciesBaseInfo;
                case SpeciesTree.MeatEatDinao:
                    speciesBaseInfo = new SpeciesBaseInfo("����", "Զ�ŵ�һ�ֿ������Բ��Ա��Լ�С�͵Ŀ�����Ϊʳ�",Quality.High,2);
                    return speciesBaseInfo;
                case SpeciesTree.Tyrannosaurus:
                    speciesBaseInfo = new SpeciesBaseInfo("����", "Զ�ŵ�һ�ֿ���������˵��½�ؿ����еĽ��������Ⲷ���ߡ�",Quality.Common,3);
                    return speciesBaseInfo;
                case SpeciesTree.Triceratops:
                    speciesBaseInfo = new SpeciesBaseInfo("������", "Զ�ŵ�һ�ֿ�������ֲ��Ϊʳ���Ը���˳��",Quality.Common,4);
                    return speciesBaseInfo;
                case SpeciesTree.Brachiosaurus:
                    speciesBaseInfo = new SpeciesBaseInfo("����", "Զ�ŵ�һ�ֿ�������ֲ��Ϊʳ�������½�ؿ���������ǰé��",Quality.Common,5);
                    return speciesBaseInfo;
            }
            return null;
        }

        /// <summary>
        ///  �õ���������
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        public string GetQualityDec(Quality quality)
        {
            switch (quality)
            {
                case Quality.Common:
                    return "��ͨ";
                case Quality.High:
                    return "ϡ��";
                case Quality.Myth:
                    return "��";
            }
            return "��ͨ";
        }

    }
    /// <summary>
    ///  ���ֿ��Ƶ���Ϣ ���� ���֡����� 
    /// </summary>
    public class SpeciesBaseInfo
    {
        public string Name = ""; // ���� 
        public string Dec = "";  // ����
        public Quality CQuality=Quality.Common; // ����
        public int SkinIndex = 0;// ���ֿ���Ƥ��

        public SpeciesBaseInfo(string name,string dec,Quality cQuality,int skinIndex)
        {
            Name = name;
            Dec = dec;
            CQuality = cQuality;
            SkinIndex = skinIndex;
        }
    }


   
}
