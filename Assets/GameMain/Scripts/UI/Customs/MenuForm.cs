//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using DG.Tweening;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class MenuForm : UGuiForm
    {
        [SerializeField]
        private GameObject m_StartButton = null;

        private ProcedureMenu m_ProcedureMenu = null;

        public void OnStartButtonClick()
        {
            Test();
            // 此处开始激活只有 主线的生物线
            //GameUtil._instance.beginPlayerTypeId = GameEntry.Setting.GetInt("BeginPlayerTypeId");
            //GameUtil._instance.beginEvolveLevel = GameEntry.Setting.GetInt("EvolveLevel");

            m_ProcedureMenu.StartGame();
        }
        /// <summary>
        ///  测试
        /// </summary>
        private void Test()
        {
            GameUtil._instance.beginPlayerTypeId = 10000;
            GameUtil._instance.beginEvolveLevel = 1;
        }

        public void OnSettingButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnAboutButtonClick()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
        }

        public void OnQuitButtonClick()
        {
            //GameEntry.UI.OpenDialog(new DialogParams()
            //{
            //    Mode = 2,
            //    Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
            //    Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
            //    OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
            //});
        }
        /// <summary>
        ///  显示进化树界面
        /// </summary>
        public void OnShowEvoluTree()
        {
            GameEntry.UI.OpenUIForm(UIFormId.EvoluTreeForm);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            m_StartButton.GetComponent<RectTransform>().DOScale(new Vector2(1.1f, 1.1f), 1f).OnComplete(() =>
            {
                m_StartButton.GetComponent<RectTransform>().DOScale(new Vector2(1f, 1f), 1f);
            }).SetLoops(-1,LoopType.Yoyo);


            //m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            m_ProcedureMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}
