using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public class TyrannosaurusRex :Entity
    {
        /// <summary>
        ///  基础霸王龙的实体数据
        /// </summary>
        private TyrannosaurusRexData m_TyrannosaurusRexData = null;

        /// <summary>
        ///  身上的操作脚本
        /// </summary>
        private Creature m_Creature = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TyrannosaurusRexData = (TyrannosaurusRexData)userData;
            m_Creature = this.GetComponent<Creature>();
            Log.Debug("显示霸王龙"+ this.gameObject.name);
            //Invoke("DealTyrannosaurusRex",3f);
            DealTyrannosaurusRex();

            // 初始化所有的值
            GameEntry.HPBar.ShowHPBar(this, 50, 0f);
            GameEntry.HPBar.m_AllGetValue = 0;
           

            // 显示DNA条 的 监听事件
            GameEntry.Event.Sbscribe(RouHideEventArgs.EventId,OnShowDNAItem);
            // 升级事件的监听
            GameEntry.Event.Sbscribe(LevelUpEventArgs.EventId, OnLevelUp);
        }

        /// <summary>
        ///  添加对应的霸王龙 （一级）
        /// </summary>
        private void DealTyrannosaurusRex()
        {
            //GameEntry.DataNode.GetOrAddNode("Level").SetData<VarInt32>();
            Manager._instance.isFirstGame = true;
            Manager._instance.OnAddSpecies(this.gameObject,m_TyrannosaurusRexData.IsAI,m_TyrannosaurusRexData.PosValue,0);
            SetScale(0.05f);
        }

        /// <summary>
        ///  设置模型大小
        /// </summary>
        private void SetScale(float setSize)
        {
            m_Creature.SetScale(setSize);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            // 取消监听肉块消失
            GameEntry.Event.Unsubscribe(RouHideEventArgs.EventId, OnShowDNAItem);
            // 取消升级事件的监听
            GameEntry.Event.Unsubscribe(LevelUpEventArgs.EventId, OnLevelUp);
        }

        /// <summary>
        ///  显示DNA经验条
        /// </summary>
        private void OnShowDNAItem(object sender,GameEventArgs e)
        {
            GameEntry.DNA.ShowDNA(this);

            // 更新DNA值
            GameEntry.HPBar.UpdateDNAValue(10);
        }
        /// <summary>
        ///  升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLevelUp(object sender, GameEventArgs e)
        {
            // 生成更高等级生物
            GameEntry.Entity.ShowBaryonyx(new BaryonyxData(GameEntry.Entity.GenerateSerialId(), 10001, false, 3f, this.transform.position));
            // 清除生物列表对应生物
            Manager._instance.creaturesList.Remove(this.gameObject);
            if (Manager._instance.selected != 0)
                Manager._instance.selected--;
            GameEntry.Entity.HideEntity(this);
        }

    }
}
