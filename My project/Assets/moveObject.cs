using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObject : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveObjectFunc();
        rotateMoveFunc();
    }


    private float speed_move = 3.0f;  // �̵� �ӵ�
    private float speed_rota = 15.0f; // ȸ�� �ӵ�

    void moveObjectFunc()
    {
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * speed_move * Time.deltaTime;
        keyV = keyV * speed_move * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.forward * keyV);
    }
    
    void rotateMoveFunc()
    {
        if(Input.GetKey(KeyCode.Q)) // ���� ȸ�� 
        {
            transform.Rotate(Vector3.up * Time.deltaTime * (-1) * speed_rota);
        }
        else if(Input.GetKey(KeyCode.E)) //������ ȸ��
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed_rota);
        }
        else if (Input.GetKey(KeyCode.R)) // ���� ȸ��
        {
            transform.Rotate(Vector3.right * Time.deltaTime * (-1) * speed_rota);
        }
        else if (Input.GetKey(KeyCode.F)) // �Ʒ��� ȸ��
        {
            transform.Rotate(Vector3.right * Time.deltaTime * speed_rota);
        }
    }
}