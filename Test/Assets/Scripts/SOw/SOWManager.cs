using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    public Queue<int>               mWaitGuestList;         // �����ǿ��� ������ �ް� �Ѿ�� �մԵ��� ����Ʈ
    public Queue<int>               mUsingGuestList;        // ������ �������� �ڸ��� �ɾ� ������ �������� �غ� �� �մԵ��� ����Ʈ
    int                             mMaxNumOfUsingGuest;    // mUsingGuestList�� ���� �� �ִ� �ִ��� ũ��

    private Guest                   mGuestManager;          // GuestManager�� �����´�.
    private static  SOWManager      instance = null;
    
    void Start()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mWaitGuestList = new Queue<int>();
        mUsingGuestList = new Queue<int> ();
    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    // �Ϸ簡 ������ ������ ������ �ʱ�ȭ�Ѵ�.
    private void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();


    }

    // ��� ����Ʈ�� �մ��� �߰������ִ� �Լ�
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);

        Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ�� �߰��Ǿ����ϴ�.");
    }


    // ��� ����Ʈ���� �մ��� ���� �޴� ����Ʈ�� �߰������ִ� �Լ�
    private void MoveToUsingList()
    {
        int guestNum = mWaitGuestList.Dequeue();
        mUsingGuestList.Enqueue(guestNum);
        Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ���� ����� ����Ʈ�� �̵��Ͽ����ϴ�.");
    }

    // �Ϸ簡 ���� �� Queue�� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� ������ش�.
    private void MakeGuestDisSat()
    {
        for(int i = 0; i< mWaitGuestList.Count; i++)
        {
            mGuestManager.mGuestInfos[mWaitGuestList.Dequeue()].isDisSat = true;
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            mGuestManager.mGuestInfos[mUsingGuestList.Dequeue()].isDisSat = true;
        }
    }
}
