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

        // ������ ���� �ް� �̿��� ��ģ ��쿡�� �ش� ��Ƽ�� ������ ��ȭ���� �ο���

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            NewChoiceGuest();
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

    public int CheckDisSat(int[] guestList, int Index)
    {
        int result = 0;

        for(int i = 0; i<= Index; i++)
        {
            if (mGuestInfos[i].isDisSat == true || mGuestInfos[i].mNotVisitCount != 0)
            {
                result++;
            }
        }

        return result;
    }

    public int[] NewChoiceGuest()
    {
        int[]       guestList           = new int[5];
        int         totalGuestNum       = 20;
        int         posssibleGuestNum   = 0;
        List<int>   VisitedGuestNum     = new List<int>();
        List<int>   NotVisitedGuestNum  = new List<int>();

        // �湮 Ƚ���� ���� ��Ƽ�� �������� 5�� �� ��Ƽ�� ���ܵǾ�� �ϹǷ� ���� ����Ʈ���� ������.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if(mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                if (mGuestInfos[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
            }
            if(mGuestInfos[i].isDisSat == false && mGuestInfos[i].mNotVisitCount == 0 && mGuestInfos[i].mVisitCount != 10 && mGuestInfos[i].isCure == false)
            {
                posssibleGuestNum++;
            }
        }
        Debug.Log(posssibleGuestNum);

        int     GuestIndex = 0;
        bool    isOverLap = true;
        int     checkDisSat = 0;

        // �湮 �̷��� �ִ� ��Ƽ�� 4�� �̻��� ���� ���
        // ��� �湮 �̷��� �ִ� ��Ƽ�� �̰� �������� �湮 �̷��� ���� ��Ƽ�� ä���.
        if (VisitedGuestNum.Count < 4)
        {
            Debug.Log("�湮 �̷��� �ִ� ��Ƽ�� 4���̻��� ���� �ʽ��ϴ�");
            for (int i = 0; i < VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }

                        // �湮 �Ұ� ��Ƽ + �Ҹ� ��Ƽ > 3�� ��� �ٽ� �̰Բ� �Ѵ�.
                        if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                            && CheckDisSat(guestList, GuestIndex) == 3)
                        {
                            if (posssibleGuestNum >= 2)
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                Debug.Log(guestList[GuestIndex] + "���� �߰��Ǿ����ϴ�.");
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 5 - VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                        if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                            && CheckDisSat(guestList, GuestIndex) == 3)
                        {
                            if (posssibleGuestNum >= 2)
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];
                Debug.Log(guestList[GuestIndex] + "���� �߰��Ǿ����ϴ�.");
                GuestIndex++;
                isOverLap = true;

            }
        }
        // �湮 �̷��� ���� ��Ƽ�� ���� ���
        // ��� ��Ƽ�� �湮 �̷��� �ִ� ��Ƽ�߿��� �̴´�.
        else if (NotVisitedGuestNum.Count == 0)
        {
            Debug.Log("�湮 �̷��� ���� ��Ƽ�� �����ϴ�");
            for (int i = 0; i < 5; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                            Debug.Log("�� �ߺ�.");
                        }
                        int rejectCount = 0;
                        // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                        for (int k = 0; i < GuestIndex; k++)
                        {
                            if (mGuestInfos[guestList[GuestIndex]].isDisSat == true || mGuestInfos[guestList[GuestIndex]].mNotVisitCount != 0)
                            {
                                rejectCount++;
                                Debug.Log(rejectCount);
                            }
                        }
                        Debug.Log("reject Count : " + rejectCount);
                        if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                            && rejectCount == 3)
                        {
                            if (posssibleGuestNum >= 2)
                            {
                                count++;
                                Debug.Log(mGuestInfos[VisitedGuestNum[temp]]);
                                Debug.Log("�Ҹ� ��Ƽ�� �ʹ� �����ϴ�.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];

                Debug.Log(guestList[GuestIndex] + "���� �߰��Ǿ����ϴ�.");
                GuestIndex++;
                isOverLap = true;

            }
        }
        // �� ���� ��쿡�� �湮 �̷��� �ִ� ��Ƽ 4��, �湮 �̷��� ���� ��Ƽ 1���� �̴´�.
        else
        {
            Debug.Log("�湮�̷� ��Ƽ 4��, �湮 �̷��� ���� ��Ƽ 1���� �̽��ϴ�.");
            for (int i = 0; i < 4; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }

                        int rejectCount = 0;
                        // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                        for(int k = 0; i< GuestIndex; k++)
                        {
                            if (mGuestInfos[guestList[GuestIndex]].isDisSat == true || mGuestInfos[guestList[GuestIndex]].mNotVisitCount != 0)
                            {
                                rejectCount++;
                            }
                        }
                        if ((mGuestInfos[VisitedGuestNum[temp]].isDisSat == true || mGuestInfos[VisitedGuestNum[temp]].mNotVisitCount != 0)
                            && rejectCount == 3)
                        {
                            if (posssibleGuestNum >= 2)
                            {
                                count++; 
                                Debug.Log(mGuestInfos[NotVisitedGuestNum[temp]]);
                                Debug.Log("�Ҹ� ��Ƽ�� �ʹ� �����ϴ�.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                Debug.Log(guestList[GuestIndex] + "���� �߰��Ǿ����ϴ�.");
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                        int rejectCount = 0;
                        // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                        for (int k = 0; i < GuestIndex; k++)
                        {
                            if (mGuestInfos[guestList[GuestIndex]].isDisSat == true || mGuestInfos[guestList[GuestIndex]].mNotVisitCount != 0)
                            {
                                rejectCount++;
                            }
                        }
                        if ((mGuestInfos[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfos[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                            && rejectCount == 3)
                        {
                            if (posssibleGuestNum >= 2)
                            {
                                count++;
                                Debug.Log(mGuestInfos[NotVisitedGuestNum[temp]]);
                                Debug.Log("�Ҹ� ��Ƽ�� �ʹ� �����ϴ�.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];

                Debug.Log(guestList[GuestIndex] + "���� �߰��Ǿ����ϴ�.");
                GuestIndex++;
                isOverLap = true;
            }
        }
        // �Ҹ� ��Ƽ��� ��������.

        Debug.Log(checkDisSat);

        return guestList;
    }

    // �Ϸ簡 �����ϸ鼭 ���� �� �湮�� ��Ƽ �̱�
    public int[] ChoiceGuest() // ������
    {
        int[]   guestList = new int[5];     // ��ȯ�� ��Ƽ ����Ʈ
        int     VisitedGuestNum = 0;        // �湮 �̷��� �ִ� ��Ƽ ��
        int     NotVisitedGuestNum = 0;     // �湮 �̷��� ���� ��Ƽ ��
        bool    chooseRight;

        // ���� ���� ��Ƽ�� ���� ������ Ȯ���Ѵ�. (���� ��Ƽ = ��ü - ġ���Ϸ� - �湮 �Ұ�)
        int     countGuest = 20;
        for(int i = 0; i< 20; i++)
        {
            if(mGuestInfos[i].isCure == true || mGuestInfos[i].mNotVisitCount != 0)
            {
                countGuest--;
            }
        }

        // ���� ��Ƽ�� ���� 5���� �̻��̶�� �Ϲ����� ������� ��Ƽ ����Ʈ�� �̴´�.
        if (countGuest > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                guestList[i] = -1;
                chooseRight = false;
                while (chooseRight == false)    // ��Ƽ�� �ùٸ��� ����� �� ����
                {
                    int temp = Random.Range(0, 20);

                    // �湮�� Ƚ���� 10ȸ�̰ų� ������ 5�� ä�� ġ���� �Ϸ��� ��Ƽ�� ��� �ٽ� �̴´�.
                    if (mGuestInfos[temp].mVisitCount == 10 || mGuestInfos[temp].isCure == true)
                    {
                        break;
                    }
                    // �̹� ���� ��Ƽ�� ��쿡�� �ٽ� �̴´�.
                    if(mGuestInfos[temp].isChosen == true)
                    {
                        break;
                    }

                    if (mGuestInfos[temp].mVisitCount == 0) // �ش� ��Ƽ�� �湮 �̷��� ���� ���
                    {
                        NotVisitedGuestNum++;
                    }
                    {
                        VisitedGuestNum++;
                    }

                    guestList[i] = temp;
                    mGuestInfos[temp].isChosen = true;
                    chooseRight = true;
                }
            }

            // ����Ʈ�� �ִ� ��Ƽ�� ������ �湮 �̷��� �ִ� ��Ƽ 4��, �湮 �̷��� ���� ��Ƽ 1���� �������� Ȯ���Ѵ�.


        }
        // 5���� ������ ��� �� �� �մ� ��Ƽ�� ��� ����Ʈ�� ��´�.
        else
        {
            int j = 0;
            for(int i = 0; i< 20; i++)
            {
                if(mGuestInfos[i].isCure == false || mGuestInfos[i].mNotVisitCount == 0)
                {
                    guestList[j] = i;
                    j++;
                }
            }
        }

        // �Ҹ� ��Ƽ�� �湮 �Ұ� ������ ��Ƽ���� ����Ʈ���� �����Ѵ�.


        Debug.Log("���� �湮�� ��Ƽ ����Ʈ�� �ʱ�ȭ �Ǿ����ϴ�");

        for(int i = 0; i< 20; i++)
        {
            mGuestInfos[i].isChosen = false;
        }

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
        InitGuestTime();

        // ä�������� �ٽ� ���ŵȴ�.

    }

}


