//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------


using GameFramework.Event;
using System.Diagnostics;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Localization;

namespace SpeciesGame
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        private MenuForm m_MenuForm = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Sbscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            GameEntry.Setting.SetString(Constant.Setting.Language, Language.ChineseSimplified.ToString());
            GameEntry.Setting.Save();
            m_StartGame = false;
            Log.Debug("进入Menu场景");
            //Debug.Log("进入Menu场景++");
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);

            SetSaveData();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_MenuForm != null)
            {
                m_MenuForm.Close(isShutdown);
                m_MenuForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                procedureOwner.SetData<Float>("NextSceneId", GameEntry.Config.GetInt("Scene.Main"));
                procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Survival);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
        
        /// <summary>
        ///  设置要保存的数据的初始值
        /// </summary>
        private void SetSaveData()
        {
            if (!GameEntry.Setting.HasSetting("EvolveLevel"))
            {
                // 存储等级（主线）
                GameEntry.Setting.SetInt("EvolveLevel", 1);
            }

            if (!GameEntry.Setting.HasSetting("SoftwareLevel"))
            {
                // 存储等级（软体）
                GameEntry.Setting.SetInt("SoftwareLevel", 0);
            }

            if (!GameEntry.Setting.HasSetting("SeaLevel"))
            {
                // 存储等级（现代海洋）
                GameEntry.Setting.SetInt("SeaLevel", 0);
            }

            if (!GameEntry.Setting.HasSetting("DinosaurLevel"))
            {
                // 存储等级（恐龙）
                GameEntry.Setting.SetInt("DinosaurLevel", 0);
            }

            if (!GameEntry.Setting.HasSetting("Coin"))
            {
                // 存储金币
                GameEntry.Setting.SetInt("Coin",0);
            }

            if (!GameEntry.Setting.HasSetting("CompeleteEnd"))
            {
                // 完成的结局
                GameEntry.Setting.SetInt("CompeleteEnd", 0);
            }

            if (!GameEntry.Setting.HasSetting("MainTreeUnLockState"))
            {
                // 主线的解锁情况
                GameEntry.Setting.SetString("MainTreeUnLockState","1000000");
            }

            if (!GameEntry.Setting.HasSetting("DinosaurUnLockState"))
            {
                // 恐龙支线的解锁情况
                GameEntry.Setting.SetString("DinosaurUnLockState", "0000000");
            }

            if (!GameEntry.Setting.HasSetting("SeaUnLockState"))
            {
                // 海洋鱼支线的解锁情况
                GameEntry.Setting.SetString("SeaUnLockState", "00000000");
            }

            if (!GameEntry.Setting.HasSetting("BeginPlayerTypeId"))
            {
                // 保存上次退出游戏时候的玩物种 (只记录每个关卡开始的生物编号)
                GameEntry.Setting.SetInt("BeginPlayerTypeId", 10000);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_MenuForm = (MenuForm)ne.UIForm.Logic;
        }
    }
}
