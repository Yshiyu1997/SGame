using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpeciesGame
{
    public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public static Joystick instance;
        //ͼ���ƶ����뾶
        public float maxRadius = 100;
        //��ʼ������ͼ��λ��
        private Vector2 moveBackPos;

        private Vector2 pointDownPos;

        public GameObject rockerBg;

        public GameObject rocker;

        public bool isCanControl = false;// �ܹ��������

        //hor,ver�����Է�����
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
            //��ʼ������ͼ��λ��
            moveBackPos = rockerBg.transform.position;
        }



        private void FixedUpdate()
        {
            // ��� ����ģ���ƶ�

        }



        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("�������" + eventData.position);
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
        /// ����꿪ʼ��קʱ
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPos;

            //��ק��ʵ��
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rockerBg.transform as RectTransform, //��ȡң�п����ƶ���Transform
                eventData.position, // ��Ļ����ϵ�´����ĵ�
                eventData.pressEventCamera,//�����¼������
                out localPos//��ñ�������ϵ�ĵ�
                );

            //��������˰뾶��λ�þ�Ҫ��ң�в���ȥ
            //magnitude��ȡ�������ĳ���
            if (localPos.magnitude > maxRadius)
            {
                //��׼������
                localPos = localPos.normalized * maxRadius;
            }

            //�޸�λ��
            rocker.transform.localPosition = localPos;
        }


        /// <summary>
        /// �����ֹͣ��קʱ
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

