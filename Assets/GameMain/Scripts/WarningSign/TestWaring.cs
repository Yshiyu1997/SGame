using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    public class TestWaring : MonoBehaviour
    {
        /// <summary>
        ///  画布
        /// </summary>
        public Canvas m_Canvas;


        public RectTransform m_RectTransform = null;


        void Start()
        {
            m_Canvas.renderMode = RenderMode.WorldSpace;
            m_Canvas.worldCamera = GameEntry.Scene.MainCamera;
        }

        // Update is called once per frame
        void Update()
        {
            if (IsInView(transform.position)&&PlayerWeapon._instance.JudgeIsCanAttack(this.gameObject.GetComponent<Enemy>().m_EnemyData.TypeId)==0)
            {
                m_Canvas.gameObject.SetActive(true);
                m_RectTransform.LookAt(GameEntry.Scene.MainCamera.transform);
            }
            else
            {
                m_Canvas.gameObject.SetActive(false);
            }
        }


       public bool IsInView(Vector3 worldPos)
       {
          Transform camTransform = GameEntry.Scene.MainCamera.transform;
          Vector2 viewPos = GameEntry.Scene.MainCamera.WorldToViewportPoint(worldPos);
          Vector3 dir = (worldPos - camTransform.position).normalized;
          float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面
  
          if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
              return true;
          else
             return false;
       }

       

    }
}
