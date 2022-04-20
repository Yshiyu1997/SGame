//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace SpeciesGame
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        /// <summary>
        ///  显示玩家实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowPlayer(this EntityComponent entityComponent, PlayerData data)
        {
            entityComponent.ShowEntity(typeof(Player), "Player", Constant.AssetPriority.PlayerAsset, data);
        }
        /// <summary>
        ///  显示霸王龙一级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowTyrannosaurusRex(this EntityComponent entityComponent,TyrannosaurusRexData data)
        {
            entityComponent.ShowEntity(typeof(TyrannosaurusRex),"TyrannosaurusRex", Constant.AssetPriority.TyrannosaurusRexAsset,data);
        }

        /// <summary>
        ///  显示霸王龙二级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowBaryonyx(this EntityComponent entityComponent, BaryonyxData data)
        {
            entityComponent.ShowEntity(typeof(Baryonyx), "Baryonyx", Constant.AssetPriority.BaryonyxAsset, data);
        }

        /// <summary>
        ///  显示霸王龙三级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowCarnotaurus(this EntityComponent entityComponent, CarnotaurusData data)
        {
            entityComponent.ShowEntity(typeof(Carnotaurus), "Carnotaurus", Constant.AssetPriority.CarnotaurusAsset, data);
        }

        /// <summary>
        ///  显示迅猛龙一级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowVelociraptor(this EntityComponent entityComponent, VelociraptorData data)
        {
            entityComponent.ShowEntity(typeof(Velociraptor), "Velociraptor", Constant.AssetPriority.VelociraptorAsset, data);
        }

        /// <summary>
        ///  显示三角龙一级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowTriceratops(this EntityComponent entityComponent, TriceratopsData data)
        {
            entityComponent.ShowEntity(typeof(Triceratops), "Triceratops", Constant.AssetPriority.TriceratopsAsset, data);
        }

        /// <summary>
        ///  显示腕龙一级实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowBrachiosauru(this EntityComponent entityComponent, BrachiosauruData data)
        {
            entityComponent.ShowEntity(typeof(Brachiosauru), "Brachiosauru", Constant.AssetPriority.BrachiosauruAsset, data);
        }

        /// <summary>
        ///  显示敌人生物实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowEnemy(this EntityComponent entityComponent, EnemyData data)
        {
            entityComponent.ShowEntity(typeof(Enemy), "Enemy", Constant.AssetPriority.EnemyAsset, data);
        }

        /// <summary>
        ///  显示肉块实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowRou(this EntityComponent entityComponent, RouData data)
        {
            entityComponent.ShowEntity(typeof(Rou), "Rou", Constant.AssetPriority.RouAsset, data);
        }

        /// <summary>
        ///  显示果实实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowGuoShi(this EntityComponent entityComponent, GuoShiData data)
        {
            entityComponent.ShowEntity(typeof(GuoShi), "GuoShi", Constant.AssetPriority.RouAsset, data);
        }

        /// <summary>
        ///  显示陆地生物死亡特效实体
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowDeath_Particle(this EntityComponent entityComponent, Death_ParticleData data)
        {
            entityComponent.ShowEntity(typeof(Death_Particle), "Death_Particle", Constant.AssetPriority.Death_ParticleAsset, data);
        }

        /// <summary>
        ///  显示升级特效实体一
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowLevelUp_1_Particle(this EntityComponent entityComponent, LevelUp_1Data data)
        {
            entityComponent.ShowEntity(typeof(LevelUp_1), "LevelUp_1", Constant.AssetPriority.LevelUp_1Asset, data);
        }

        /// <summary>
        ///  显示升级特效实体二
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowLevelUp_2_Particle(this EntityComponent entityComponent, LevelUp_2Data data)
        {
            entityComponent.ShowEntity(typeof(LevelUp_2), "LevelUp_2", Constant.AssetPriority.LevelUp_2Asset, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
