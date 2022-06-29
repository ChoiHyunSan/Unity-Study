using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    //public
    public GuestInfo[] mGuestInfos;             // Scriptable Objects���� ������ ��� �ִ� �迭

    //private
    private static Guest instance = null;       // �̱��� ����� ���� instance ����

    // Start is called before the first frame update
    private void Awake()
    {
        // �̱��� ��� ���
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
        // ������ ���� �޾��� ��쿡�� �ش� ��Ƽ�� ������ ��ȭ���� �ο���


        // ������ �̿��߿� �ð��� �귯 ���� �ٲ�� ���� ��ȭ�� ���� �����·� �����Ѵ�.


        // ���� ���� �̺�Ʈ �߻��ÿ� ���� ���ɿ� �����Ǿ� �ִ� ���鿡�� ���� ������ �޾Ҵٴ� �������� �ѱ��.


        // �̱��� ��� Ȯ���� ���� �׽�Ʈ�ڵ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveSceneToLivingRoom();
        }
        // ������ ��ȯ�� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetEmotion(0, 0, 5);
        }
        // ������ ������ ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // �����Ѽ� ħ�� Ȯ���� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfos[0].isDisSat = IsExcessLine(0);
            Debug.Log(mGuestInfos[0].isDisSat);
        }
    }

    public void MoveSceneToLivingRoom()
    {
        SceneManager.LoadScene("LivingRoom");
        Debug.Log("hello");
    }

    // ��Ƽ�� ���������� �޾ƿ��� API
    public string GetName(int gusetNum) 
    { 
        return mGuestInfos[gusetNum].mName; 
    }

    public int GetEmotion(int gusetNum, int emotionNum) 
    { 
        return mGuestInfos[gusetNum].mEmotion[emotionNum]; 
    }

    public Sprite GetImg(int guestNum, int imgNum) 
    { 
        return mGuestInfos[guestNum].sImg[imgNum]; 
    }

    public int GetSat(int guestNum) 
    { 
        return mGuestInfos[guestNum].mSatatisfaction; 
    }

    public bool IsDisSat(int guestNum) 
    { 
        return mGuestInfos[guestNum].isDisSat; 
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    // ���� ���� ���� (������ ȭ��󿡼� ��Ƽ���� �����Ͽ����� �۾��� �ش� �������� ����)
    // 1. ������ �����Ͽ� ��Ƽ���� ���� (��Ƽ�� �ɾ��ִ� ���°� �ƴ϶�� ���� �Ұ���)
    // 2. ������ �̿�ð���ŭ�� ��� (��⵵�� ���� �ٲ�� ���� ����)
    // 3. ������ ��������ŭ�� ��Ƽ�� ������ ���ϱ� 
    // 4. ������ �������� ��Ƽ�� ���������� Ȯ��. (�������� ���� �����Ѽ� ħ�� ����)
    // 5. ���� ���� �����Ѽ��� ħ������ ��� ��Ƽ�� �Ҹ� ��Ƽ�� ���� (�Ҹ� ��Ƽ�� ���� ������ ��ũ��Ʈ �߰� �ۼ�)
    // 6. ���� �������� ����Ǿ��� �ÿ� ������ �� ���� (�ش� ��Ƽ�� ��ǥ���� ����)

    // 7. �������� �ö��� ��� ���翡 �Ѹ� �� �ִ� ���ѿ� ���õ� ���� �޾Ƽ� �ɱ�
    // 8. ���� ������ ���� ����� ȭ�鿡 ����ְ� ��Ƽ�� ������ �������� ��������
    //------------------------------------------------------------------------------------------------------------------------------------------



    // �����Ѽ��� ħ���ϴ��� �Ǵ��Ͽ� �Ѿ�� ��� �Ҹ���Ƽ�� ��ȯ�Ѵ�. -> ���� ���� ���� 5������ ����
    public void CheckIsDisSat(int guestNum)
    {
        // ������ ���� ħ���� ��츦 Ȯ��
        if (IsExcessLine(guestNum)) 
        {
            mGuestInfos[guestNum].isDisSat = true; // �Ҹ� ��Ƽ�� ��ȯ
            mGuestInfos[guestNum].mSatatisfaction = 0;
        }
    }

    // ��Ƽ�� ������ ���濡 �ʿ��� API 
    // Event Handler�� �̿��Ͽ� ������ �����ȿ� ���� ���ϰų� ���� �����Ѽ��� ħ���Ͽ� �Ҹ� ��Ƽ�� �Ǵ°�� �̺�Ʈ�� �ߵ����� ����
    public void SetEmotion(int guestNum, int emotionNum, int value) { mGuestInfos[guestNum].mEmotion[emotionNum] += value; }

    public bool IsExcessLine(int guestNum) // ���� �����Ѽ��� ħ���ߴ��� Ȯ���ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (GetEmotion(guestNum, limitEmotion[i].upLimitEmotion) >= limitEmotion[i].upLimitEmotionValue 
                || GetEmotion(guestNum, limitEmotion[i].downLimitEmotion) <= limitEmotion[i].downLimitEmotionValue) // �����Ѽ��� ħ���� ���
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return false;
            }
        }

        // �����Ѽ� ��� ħ������ �ʴ� ���
        Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�");
        return true;
    }

    public void RenewakSat(int guestNum)     // �������� �����ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {
        int temp = 0;

        for(int i = 0; i< 5; i++)
        {
            // ������ ���� ���� ������ Ȯ��
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfos[guestNum].mSatEmotions[i].up &&
             mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfos[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // ���� �������� ���� ��Ƽ�� ������ ���ϴ� ��� ������ ������ �ʿ��ϴ�.
    // 1. ������ �����޾� �̿��ϴ� ���� ���� �ٲ�� ������ ���ư��� �ϴ� ��� ��ȿ
    // -> ���� �ð� (������ �����޾� �̿��ϴ� �ð�)���� ����ϴٰ� �ð��� ���� �� �������� �����ϴ� ��� ��� ����)
    //
    // 2. ������ �����޾� ��ȭ�� ������ ��Ƽ�� ���� ���Ѽ��� ���Ѽ��� ħ���ϸ� �ȵȴ�.
    // -> ������ �����ް� ���� ���� ���Ѽ��� ���Ѽ��� ħ�����ϴ� ��� �Ҹ� ��Ƽ�� ��ȯ�ȴ�.


    // �Ҹ� ��Ƽ�� �г�Ƽ
    // Cloud Factory �湮�� ����
    // ġ���� ��Ͽ� �Ҹ���Ƽ ǥ��
    // ������ 0���� ��ȯ




}


