using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  进化树枚举
    /// </summary>
    public enum SpeciesTree
    {
        None = 0,
        Cell,       // 真核细胞
        AncientFish,  // 远古鱼类
        Lizard,       // 蜥蜴
        Duckmole,     // 鸭嘴兽
        Squirrel,     // 松鼠
        Ape,          // 猿猴
        Human,        // 人类
        TerrifyingApe, // 恐怖直立猿

        TropicalFish,     // 热带鱼
        BonyFish,           // 硬骨鱼类
        CartilaginousFish,  // 软骨鱼类
        SeaTurtle,          // 海龟    
        Tortoise,           // 玄武
        GuardingQuartet,    // 镇守四方
        MarineMammals,      // 海洋哺乳类
        LegendaryFish,      // 鲲
        GobbleWorld,         // 吞噬天下

        Eoraptor,         // 始盗龙
        PlatEatDinao,       // 草食龙
        MeatEatDinao,       // 棘龙
        Tyrannosaurus,      // 暴龙
        Triceratops,        // 三角龙
        Brachiosaurus,      // 腕龙
        Godzilla,           // 哥斯拉
        MonsterKing         // 怪物之王
    }

    /// <summary>
    ///  卡牌品质
    /// </summary>
    public enum Quality
    {
        Common,
        High,
        Myth
    }



    /// <summary>
    ///  生物信息（更新填充）
    /// </summary>
    public class SpeciesInfo : MonoBehaviour
    {

        public static SpeciesInfo _instance = null;

        /// <summary>
        ///  物种卡牌品质框  普通  稀有  神话
        /// </summary>
        public Sprite[] m_QualitySprArr;

        /// <summary>
        ///  物种卡牌 图片精灵  
        /// </summary>
        public Sprite[] m_SpeciesCardArr;

        private void Awake()
        {
            if (!_instance) _instance = this;

        }

        /// <summary>
        ///  判断物种的分支和种类 根据不同枚举进行归类
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
        ///  获得物种的基本信息 名字、描述
        /// </summary>
        /// <returns></returns>
        public SpeciesBaseInfo GetSpeciesName(SpeciesTree species)
        {
            SpeciesBaseInfo speciesBaseInfo = null;
            switch (species)
            {
                case SpeciesTree.Eoraptor:
                    speciesBaseInfo = new SpeciesBaseInfo("始盗龙","远古的一种恐龙，以偷食龙蛋和一些植物为生。",Quality.Common,0);
                    return speciesBaseInfo;
                case SpeciesTree.PlatEatDinao:
                    speciesBaseInfo = new SpeciesBaseInfo("食草龙", "远古的一种恐龙，以植物为生。",Quality.Common,1);
                    return speciesBaseInfo;
                case SpeciesTree.MeatEatDinao:
                    speciesBaseInfo = new SpeciesBaseInfo("棘龙", "远古的一种恐龙，以捕猎比自己小型的恐龙作为食物。",Quality.High,2);
                    return speciesBaseInfo;
                case SpeciesTree.Tyrannosaurus:
                    speciesBaseInfo = new SpeciesBaseInfo("暴龙", "远古的一种恐龙，可以说是陆地恐龙中的金字塔顶尖捕猎者。",Quality.Common,3);
                    return speciesBaseInfo;
                case SpeciesTree.Triceratops:
                    speciesBaseInfo = new SpeciesBaseInfo("三角龙", "远古的一种恐龙，以植物为食，性格温顺。",Quality.Common,4);
                    return speciesBaseInfo;
                case SpeciesTree.Brachiosaurus:
                    speciesBaseInfo = new SpeciesBaseInfo("腕龙", "远古的一种恐龙，以植物为食，体格在陆地恐龙中名列前茅。",Quality.Common,5);
                    return speciesBaseInfo;
            }
            return null;
        }

        /// <summary>
        ///  得到质量描述
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        public string GetQualityDec(Quality quality)
        {
            switch (quality)
            {
                case Quality.Common:
                    return "普通";
                case Quality.High:
                    return "稀有";
                case Quality.Myth:
                    return "神话";
            }
            return "普通";
        }

    }
    /// <summary>
    ///  物种卡牌的信息 包括 名字、描述 
    /// </summary>
    public class SpeciesBaseInfo
    {
        public string Name = ""; // 名字 
        public string Dec = "";  // 描述
        public Quality CQuality=Quality.Common; // 质量
        public int SkinIndex = 0;// 物种卡牌皮肤

        public SpeciesBaseInfo(string name,string dec,Quality cQuality,int skinIndex)
        {
            Name = name;
            Dec = dec;
            CQuality = cQuality;
            SkinIndex = skinIndex;
        }
    }


   
}
