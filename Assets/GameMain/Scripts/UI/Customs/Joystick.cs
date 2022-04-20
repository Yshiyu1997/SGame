using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpeciesGame
{
    public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public static Joystick instance;
        //图标移动最大半径
        public float maxRadius = 100;
        //初始化背景图标位置
        private Vector2 moveBackPos;

        private Vector2 pointDownPos;

        public GameObject rockerBg;

        public GameObject rocker;

        public bool isCanControl = false;// 能够控制玩家

        //hor,ver的属性访问器
        private float horizontal = 0;
        private float vertical = 0;

        //private PlayerController controller;

        public float Horizontal
        {
            get { return horizontal; }
        }

        public float Vertical
        {
            get { return vertical; }
        }

        private void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //初始化背景图标位置
            moveBackPos = rockerBg.transform.position;
        }



        private void FixedUpdate()
        {
            // 添加 控制模型移动

        }



        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("点击文字" + eventData.position);
            //transform.parent.transform.position = eventData.position;
            if (!Manager._instance.isCanMove) return;
            isCanControl = true;
            //rockerBg.transform.position = eventData.position;
            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent.parent.transform, eventData.position,
                eventData.pressEventCamera, out position))
            {
                rockerBg.GetComponent<RectTransform>().localPosition = position;
            }

            pointDownPos = eventData.position;
        }

        /// <summary>
        /// 当鼠标开始拖拽时
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPos;

            //拖拽的实现
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rockerBg.transform as RectTransform, //获取遥感可以移动的Transform
                eventData.position, // 屏幕坐标系下触摸的点
                eventData.pressEventCamera,//触发事件的相机
                out localPos//获得本地坐标系的点
                );

            //如果超过了半径的位置就要让遥感不出去
            //magnitude获取该向量的长度
            if (localPos.magnitude > maxRadius)
            {
                //标准化向量
                localPos = localPos.normalized * maxRadius;
            }

            //修改位置
            rocker.transform.localPosition = localPos;
        }


        /// <summary>
        /// 当鼠标停止拖拽时
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            isCanControl = false;
            rockerBg.transform.position = moveBackPos;
            rocker.transform.localPosition = Vector3.zero;
        }
    }
}

