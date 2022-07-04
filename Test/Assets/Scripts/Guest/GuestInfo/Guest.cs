using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    //public
    public GuestInfo[]      mGuestInfos;                        // Scriptable Objects���� ������ ��� �ִ� �迭

    public float            mGuestTime;                         // ��Ƽ�� �湮 �ֱ�
    private bool            isTimeToTakeGuest;                  // ��Ƽ �湮�ֱⰡ �������� Ȯ��

    public int[]            mTodayGuestList = new int[5];       // ���� �湮 ������ ��Ƽ ���
    public int              mGuestIndex;                        // �̹��� �湮�� ��Ƽ�� ��ȣ

    //private
    private static Guest    instance = null;                    // �̱��� ����� ���� instance ����

    private void Start()
    {
        mGuestTime = 0;
        mGuestIndex = 0;
        isTimeToTakeGuest = false;
    }

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
        // ��Ƽ�� �湮�ֱ⸦ ������.
        if (mGuestTime < 5.0f)
        {
            mGuestTime += Time.deltaTime;
        }
        else if(mGuestTime >= 5.0f && isTimeToTakeGuest == false)
        {
            Debug.Log("��Ƽ �湮�ð��� �Ǿ����ϴ�");
            isTimeToTakeGuest = true;

            // ������ �̵��ϴ� ��ư�鿡 ���� ��ȣ�ۿ�

        }
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
            SetEmotion(0, 0, 1, 5, 10);
        }
        // ������ ������ ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // �����Ѽ� ħ�� Ȯ���� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfos[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfos[0].isDisSat);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InitGuestTime();
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

    //------------------------------------------------------------------------------------------------------------------------------------------
    // ���� ���� ���� (������ ȭ��󿡼� ��Ƽ���� �����Ͽ����� �۾��� �ش� �������� ����)
    // 1. ������ �����Ͽ� ��Ƽ���� ���� (��Ƽ�� �ɾ��ִ� ���°� �ƴ϶�� ���� �Ұ���) 
    // 2. ������ �̿�ð���ŭ�� ��� (��⵵�� ���� �ٲ�� ���� ����)
    // 3. ������ ��������ŭ�� ��Ƽ�� ������ ���ϱ� - �Լ� ����
    // 4. ������ �������� ��Ƽ�� ���������� Ȯ��. (�������� ���� �����Ѽ� ħ�� ����) - �Լ� ����
    // 5. ���� ���� �����Ѽ��� ħ������ ��� ��Ƽ�� �Ҹ� ��Ƽ�� ���� (�Ҹ� ��Ƽ�� ���� ������ ��ũ��Ʈ �߰� �ۼ�) - �Լ� ����
    // 6. ���� �������� ����Ǿ��� �ÿ� ������ �� ���� (�ش� ��Ƽ�� ��ǥ���� ����) - �Լ� ����

    // 7. �������� �ö��� ��� ���翡 �Ѹ� �� �ִ� ����(���)�� ���õ� ���� �޾Ƽ� �ɱ�
    // 8. ���� ������ ���� ����� ȭ�鿡 ����ְ� ��Ƽ�� ������ �������� ��������
    //------------------------------------------------------------------------------------------------------------------------------------------
    
    // �����Ѽ��� ħ���ϴ��� �Ǵ��Ͽ� �Ѿ�� ��� �Ҹ���Ƽ�� ��ȯ�Ѵ�. -> ���� ���� ���� 5������ ����
    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // ħ���ϴ� ��쿡 �������� ���Ƿ� ������ ����

        // ������ ���� ħ���� ��츦 Ȯ��
        if (temp != -1) 
        {
            mGuestInfos[guestNum].isDisSat = true;              // �Ҹ� ��Ƽ�� ��ȯ
            mGuestInfos[guestNum].mSatatisfaction = 0;          // ������ 0 ���� ����
            mGuestInfos[guestNum].mVisitCount = 0;              // ���� �湮Ƚ�� 0���� ����
            
            // ġ���� ������� �Ҹ� ��Ƽ�� �� ���¿� �մ� ��ȣ, � ���� ��ȭ�� ���� ������ �������ֱ�


            return true;
        }
        return false;
    }

    // ��Ƽ�� ������ ���濡 �ʿ��� API 
    // Event Handler�� �̿��Ͽ� ������ �����ȿ� ���� ���ϰų� ���� �����Ѽ��� ħ���Ͽ� �Ҹ� ��Ƽ�� �Ǵ°�� �̺�Ʈ�� �ߵ����� ����
    public void SetEmotion(int guestNum, int emotionNum0, int emotionNum1, int value0, int value1) 
    { 
        mGuestInfos[guestNum].mEmotion[emotionNum0] += value0; 
        mGuestInfos[guestNum].mEmotion[emotionNum1] += value1; 
    }

    public int IsExcessLine(int guestNum) // ���� �����Ѽ��� ħ���ߴ��� Ȯ���ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // �����Ѽ��� ħ���� ���
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // �����Ѽ� ��� ħ������ �ʴ� ���
        Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�");
        return -1;
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


    // �Ϸ簡 �����ϸ鼭 ���� �� �湮�� ��Ƽ �̱�
    public int[] ChoiceGuest() // ������
    {
        int[] guestList = new int[5];

        for(int i = 0; i< 5; i++)
        {
            int temp = Random.Range(0, 20);

            guestList[i] = temp;
        }
        Debug.Log("���� �湮�� ��Ƽ ����Ʈ�� �ʱ�ȭ �Ǿ����ϴ�");
        return guestList;
    }

    // �ش� ��Ƽ�� �ʱ�ȭ �����ִ� �Լ�
    public void InitGuestData() // ���Ŀ� ����
    {

    }

    // �湮�ֱ⸦ �ʱ�ȭ ���ִ� �Լ�
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        Debug.Log("�湮�ֱ� �ʱ�ȭ");
    }
    
    // �Ϸ簡 �����鼭 �ʱ�ȭ�� �ʿ��� �������� ��ȯ���ش�.
    public void InitDay() 
    {
        // ������ ������ ���� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� �����.

        // ���ο� �湮 ��Ƽ ����Ʈ�� �̴´�.

        // �湮 �ֱ⸦ �ʱ�ȭ�Ѵ�.

        // ä�������� �ٽ� ���ŵȴ�.

    }

}


