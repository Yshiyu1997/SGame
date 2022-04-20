using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    public static CamerFollow instance = null;
    [Header("摄像机和玩家的距离")]
    public float distance = 10; // 摄像机和玩家的距离  
    public float rot = 0; // 两者之间的横向(水平) 弧度
    [Header("摄像机和玩家的垂直角度")]
    public float roll = 30;//  两者之间的竖直方向的 角度  （弧度) 
    public float speed = 2;// 速度
    public bool isRealTime;// 是否实时
    [Header("目标节点")]
    public GameObject playerTarget;// 玩家目标节点

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        roll = roll * Mathf.PI * 2 / 360;
    }



    private void LateUpdate()
    {
        if (playerTarget == null) return;
        if (Camera.main == null) return;
        // 目标节点的位置
        Vector3 playerPos = playerTarget.transform.position;
        Vector3 cameraPos = Vector3.zero;
        float d = distance * Mathf.Cos(roll);  // 水平距离
        float height = distance * Mathf.Sin(roll); // 竖直距离
        cameraPos.x = playerPos.x + d * Mathf.Cos(rot);
        cameraPos.z = playerPos.z + d * Mathf.Sin(rot);
        cameraPos.y = playerPos.y + height;
        if (isRealTime)
        {
            Camera.main.transform.position = cameraPos;  // 实时跟随
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos, Time.deltaTime * speed);  // 摄像机滑动跟随
        }
        // 朝向目标
        Camera.main.transform.LookAt(playerTarget.transform);
    }
}
