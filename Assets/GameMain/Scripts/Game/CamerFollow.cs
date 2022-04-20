using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    public static CamerFollow instance = null;
    [Header("���������ҵľ���")]
    public float distance = 10; // ���������ҵľ���  
    public float rot = 0; // ����֮��ĺ���(ˮƽ) ����
    [Header("���������ҵĴ�ֱ�Ƕ�")]
    public float roll = 30;//  ����֮�����ֱ����� �Ƕ�  ������) 
    public float speed = 2;// �ٶ�
    public bool isRealTime;// �Ƿ�ʵʱ
    [Header("Ŀ��ڵ�")]
    public GameObject playerTarget;// ���Ŀ��ڵ�

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
        // Ŀ��ڵ��λ��
        Vector3 playerPos = playerTarget.transform.position;
        Vector3 cameraPos = Vector3.zero;
        float d = distance * Mathf.Cos(roll);  // ˮƽ����
        float height = distance * Mathf.Sin(roll); // ��ֱ����
        cameraPos.x = playerPos.x + d * Mathf.Cos(rot);
        cameraPos.z = playerPos.z + d * Mathf.Sin(rot);
        cameraPos.y = playerPos.y + height;
        if (isRealTime)
        {
            Camera.main.transform.position = cameraPos;  // ʵʱ����
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos, Time.deltaTime * speed);  // �������������
        }
        // ����Ŀ��
        Camera.main.transform.LookAt(playerTarget.transform);
    }
}
