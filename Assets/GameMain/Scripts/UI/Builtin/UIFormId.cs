//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace SpeciesGame
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 主菜单。
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingForm = 101,

        /// <summary>
        /// 游戏界面
        /// </summary>
        GameForm = 102,

        /// <summary>
        ///  失败界面
        /// </summary>
        FailedForm=103,

        /// <summary>
        ///  失败界面
        /// </summary>
        EvoluTreeForm = 104,
        /// <summary>
        ///  宝箱界面
        /// </summary>
        BoxForm = 105,

        /// <summary>
        ///  提示界面
        /// </summary>
        TipsForm = 106,

        /// <summary>
        ///  开箱界面
        /// </summary>
        UnpackForm = 107,

        /// <summary>
        ///  奖励界面
        /// </summary>
        AwardForm = 108,

        /// <summary>
        ///  进化选择界面
        /// </summary>
        ChoiceForm = 109,

        /// <summary>
        ///  进化场景界面
        /// </summary>
        EvoLuScForm = 110,


    }
}
