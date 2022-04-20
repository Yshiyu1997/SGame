using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  �׿ǳ�
    /// </summary>
    public class Bettle : Creature
    {
        [Header("�ƶ��ٶ�")]
        public float moveSpeed = 3;
        [Header("ʩ�ӵ���")]
        public float force = 1;
        [Header("���ת���ʱ����")]
        public float randomTime = 4f;

        public bool isRight; // �������
        public bool isLeft;  // �������


        private Rigidbody m_RigiBody;
        private int randomRotateNum; // ���ת����ֵ
        private int[] directionArr = { 20, 30, 50, 70, 90, 130, 150, -20, -30, -50, -70, -90, -130, -150 };
        private float randomLimitTime = 0;
        private float randomRotateNum2;


        private void Awake()
        {
            m_RigiBody = this.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            RandomDirection();
            randomLimitTime = randomTime;
            StartCoroutine("Wait");
        }


        private void FixedUpdate()
        {
            EnemyMove();
        }

        /// <summary>
        ///  ���ת��
        /// </summary>
        private void RandomDirection()
        {

            randomRotateNum = UnityEngine.Random.Range(0, 14);
            if (randomRotateNum < directionArr.Length)
            {
                //transform.Rotate(new Vector3(0, -1, 0) * directionArr[randomRotateNum]);
                transform.DORotate(new Vector3(0, -1, 0) * directionArr[randomRotateNum], 0.5f);
            }

        }
        /// <summary>
        /// �ƶ�
        /// </summary>
        private void EnemyMove()
        {
            //characterController.Move(transform.forward * Time.deltaTime * moveSpeed);
            //body.AddForce(transform.forward * force * speed);
            body.MovePosition(transform.position+ transform.forward * Time.deltaTime* moveSpeed);
        }

        IEnumerator Wait()
        {
            while (true)
            {
                //��������һ��Timer����
                yield return new WaitForSeconds(1);
                Timer();
            }
        }

        private void Timer()
        {
            randomLimitTime--;
            if (randomLimitTime <= 0)
            {
                // �������
                RandomDirection();
                randomLimitTime = randomTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Wall")
            {
                // ����ǽ  ת��
                RandomDirection();
                randomLimitTime = randomTime;
            }
            else if (collision.transform.tag == "Creature")
            {
                // ������� 
                transform.Rotate(new Vector3(0, -1, 0) * 90);
            }
        }

    }
}
