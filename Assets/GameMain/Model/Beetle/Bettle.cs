using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeciesGame
{
    /// <summary>
    ///  甲壳虫
    /// </summary>
    public class Bettle : Creature
    {
        [Header("移动速度")]
        public float moveSpeed = 3;
        [Header("施加的力")]
        public float force = 1;
        [Header("随机转向的时间间隔")]
        public float randomTime = 4f;

        public bool isRight; // 随机向右
        public bool isLeft;  // 随机向左


        private Rigidbody m_RigiBody;
        private int randomRotateNum; // 随机转向数值
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
        ///  随机转向
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
        /// 移动
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
                //两秒运行一次Timer函数
                yield return new WaitForSeconds(1);
                Timer();
            }
        }

        private void Timer()
        {
            randomLimitTime--;
            if (randomLimitTime <= 0)
            {
                // 重新随机
                RandomDirection();
                randomLimitTime = randomTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Wall")
            {
                // 碰到墙  转向
                RandomDirection();
                randomLimitTime = randomTime;
            }
            else if (collision.transform.tag == "Creature")
            {
                // 碰到玩家 
                transform.Rotate(new Vector3(0, -1, 0) * 90);
            }
        }

    }
}
