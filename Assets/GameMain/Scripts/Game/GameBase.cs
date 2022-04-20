//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------


using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }

        protected ScrollableBackground SceneBackground
        {
            get;
            private set;
        }

        public bool GameOver
        {
            get;
            protected set;
        }

        private float m_DelayCreateEnemy = 0;// 延迟生成敌人

        private Vector3 playerPos = Vector3.zero;// 玩家当前的位置
        private int createAreaIndex = 0;// 敌人生成区域 （1,2,3,4） 分四个区域 均匀分布 
        private float enemyPosY = 58.6f;// 敌人Y坐标
        private float limitDiatance = 5f; // 限制玩家周围生成敌人的距离大小
        public int distanceWithPlayer_Inside = 10;//敌人生物圈内圆半径
        public int distanceWithPlayer_Outside = 15;//敌人生物圈外圆半径
        public int maxEnemyLiveNum = 200;//当前场景存活的最大敌人数量

        public virtual void Initialize()
        {
            GameEntry.Event.Sbscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Sbscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);


            // 进化成功事件的监听
            GameEntry.Event.Sbscribe(EvolveSusEventArgs.EventId, OnEvolveSus);
            //SceneBackground = Object.FindObjectOfType<ScrollableBackground>();
            //if (SceneBackground == null)
            //{
            //    Log.Warning("Can not find scene background.");
            //    return;
            //}

            //SceneBackground.VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
            //GameEntry.Entity.ShowMyAircraft(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
            //{
            //    Name = "My Aircraft",
            //    Position = Vector3.zero,
            //});

            OnEnterGame();
            GameOver = false;
            //m_MyAircraft = null;
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            // 取消进化成功事件的监听
            GameEntry.Event.Unsubscribe(EvolveSusEventArgs.EventId, OnEvolveSus);

        }

        public virtual void OnEnterGame()
        {
            Log.Debug("开始生物的产生");
            Manager._instance.isFirstGame = true;
            // 记录一个DNA节点
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
            //if (GameEntry.Setting.HasSetting("Level"))
            //{
            //    Log.Debug("当前关卡"+GameEntry.Setting.GetInt("Level"));
            //    //Manager._instance.level = GameEntry.Setting.GetInt("Level");
            //Manager._instance.level = 1;
            //}
            //else
            //{
            //    GameEntry.Setting.SetInt("Level",Manager._instance.level);
            //    GameEntry.Setting.Save();
            //}
            // 生成玩家  可以根据关卡进行生成
            //GameEntry.Entity.ShowTyrannosaurusRex(new TyrannosaurusRexData(GameEntry.Entity.GenerateSerialId(), 10000, false, 3f, Vector3.zero));
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), GameUtil._instance.beginPlayerTypeId, Vector3.zero,
                Manager._instance.playerSkinIndexArr[Manager._instance.level - 1]));
            Manager._instance.isCanMove = true;
           
            //CreateAi();

        }

        private void OnEvolveSus(object sender, GameEventArgs e)
        {
            OnEnterGame();
            GameOver = false;
            GameEntry.HPBar.m_AllGetValue = 0;
            // 记录新的节点
            GameEntry.DataNode.GetOrAddNode("DNAGet").SetData<VarDouble>(GameEntry.HPBar.m_AllGetValue);
            Manager._instance.DNAValue = 0;
            Manager._instance.level = 1;
            GameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            //if (m_MyAircraft != null && m_MyAircraft.IsDead)
            //{
            //    GameOver = true;
            //    return;
            //}
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            //if (ne.EntityLogicType == typeof(MyAircraft))
            //{
            //    m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            //}
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        /// <summary>
        ///  生成AI
        /// </summary>
        private void CreateAi()
        {

            //生成对应的猎物
            // 迅猛龙
            for (int i = 0; i <= 30; i++)
            {
                //GameEntry.Entity.ShowVelociraptor(new VelociraptorData(GameEntry.Entity.GenerateSerialId(), 10003, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),10003, DistributeEnemy_In(distanceWithPlayer_Inside)));
            }
            // 三角龙
            for (int i = 0; i <= 5; i++)
            {
                //GameEntry.Entity.ShowTriceratops(new TriceratopsData(GameEntry.Entity.GenerateSerialId(), 10004, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 10004, DistributeEnemy_In(distanceWithPlayer_Inside)));
            }

            // 腕龙
            for (int i = 0; i <= 3; i++)
            {
                //GameEntry.Entity.ShowBrachiosauru(new BrachiosauruData(GameEntry.Entity.GenerateSerialId(), 10005, true, 1f, DistributeEnemy_In(distanceWithPlayer_Inside)));
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 10005, DistributeEnemy_In(distanceWithPlayer_Inside)));
            }
        }

        /*
        * 在场景中分配敌人  内圆
        */
        private Vector3 DistributeEnemy_In(int circleRadius)
        {
            createAreaIndex++;
            playerPos = Manager._instance.playerPos;
            Debug.Log("玩家的位置"+playerPos);
            Vector3 pos = Vector3.zero;
            Quaternion roa = Quaternion.identity;
            Vector2 point = Random.insideUnitCircle * circleRadius;
            //if (Vector2.Distance(point, Vector2.zero) < limitDiatance)
            //{
            //    // 在内圈  不生成
            //    return Vector3.zero;
            //}
            switch (createAreaIndex)
            {
                case 1:
                    // 右上
                    return pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
                case 2:
                    // 右下
                    return pos = new Vector3(playerPos.x + Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                case 3:
                    // 左下
                    return pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z - Mathf.Abs(point.y));
                case 4:
                    // 左上
                    createAreaIndex = 0;// 还原
                    return pos = new Vector3(playerPos.x - Mathf.Abs(point.x), enemyPosY, playerPos.z + Mathf.Abs(point.y));
            }

            return Vector3.zero;
       
        }
    }
}
