using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    public Queue<int>               mWaitGuestList;         // 응접실에서 수락을 받고 넘어온 손님들의 리스트
    public Queue<int>               mUsingGuestList;        // 날씨의 공간에서 자리에 앉아 구름을 제공받을 준비가 된 손님들의 리스트
    int                             mMaxNumOfUsingGuest;    // mUsingGuestList가 가질 수 있는 최대의 크기

    private Guest                   mGuestManager;          // GuestManager를 가져온다.
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

    // 하루가 지나면 날씨의 공간을 초기화한다.
    private void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();


    }

    // 대기 리스트에 손님을 추가시켜주는 함수
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);

        Debug.Log(guestNum + "번 손님이 대기 리스트에 추가되었습니다.");
    }


    // 대기 리스트에서 손님을 제공 받는 리스트로 추가시켜주는 함수
    private void MoveToUsingList()
    {
        int guestNum = mWaitGuestList.Dequeue();
        mUsingGuestList.Enqueue(guestNum);
        Debug.Log(guestNum + "번 손님이 대기 리스트에서 사용자 리스트로 이동하였습니다.");
    }

    // 하루가 끝날 때 Queue에 남아있는 뭉티들을 불만 뭉티로 만들어준다.
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
