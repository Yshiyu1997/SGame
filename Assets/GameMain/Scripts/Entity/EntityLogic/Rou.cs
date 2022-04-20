using DG.Tweening;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class Rou : Entity
    {

        /// <summary>
        ///  肉块的实体数据
        /// </summary>
        private RouData m_RouData = null;

        /// <summary>
        ///  肉块能够消除的开关
        /// </summary>
        private bool isCanHideRou = false;

        /// <summary>
        ///  刚体
        /// </summary>
        private Rigidbody m_Rigidbody=null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_RouData = (RouData)userData;

            m_Rigidbody = this.GetComponent<Rigidbody>();

            transform.position = m_RouData.PosValue;

            transform.localScale = new Vector3(2.5f,2.5f,2.5f);

            // 抛出
            DOTween.To(setter: value =>
            {
                //Debug.Log(value);
                transform.position = Parabola(transform.position, transform.position + Vector3.forward*0.015f,0.08f, value);
            }, startValue: 0, endValue: 1, duration: 1).SetEase(Ease.Linear);

            Invoke("ChangeRouHideState", 0.3f);

        }

        /// <summary>
        ///  改变肉块能够消失的状态
        /// </summary>
        private void ChangeRouHideState()
        {
            isCanHideRou = true;
        }


        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log("碰撞的东西"+collision.gameObject.name);
        //    if (collision.gameObject.name == Manager._instance.playerObj.name&&isCanHideRou)
        //    {
        //        // 玩家吃到了肉块获得了经验  肉块消失
        //        GameEntry.Entity.HideEntity(this);
        //        isCanHideRou = false;

        //        // 派发肉块消失的事件
        //        GameEntry.Event.Fire(this,ReferencePool.Acquire<RouHideEventArgs>());
        //    }
        //}

        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => 4 * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("碰撞的东西" + other.gameObject.layer);
            if (other.gameObject.layer ==LayerMask.NameToLayer("Weapon") && isCanHideRou)
            {
                GameEntry.Sound.PlaySound(30000);
                GameUtil.PhoneVibrate(80);
                // 玩家吃到了肉块获得了经验  肉块消失
                GameEntry.Entity.HideEntity(this);
                isCanHideRou = false;

                // 派发肉块消失的事件
                GameEntry.Event.Fire(this, ReferencePool.Acquire<RouHideEventArgs>());
            }
        }
    }
}
