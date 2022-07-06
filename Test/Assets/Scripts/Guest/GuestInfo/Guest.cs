using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    //public
    public GuestInfo[]      mGuestInfos;                        // Scriptable Objects들의 정보를 담고 있는 배열

    public float            mGuestTime;                         // 뭉티의 방문 주기
    private bool            isTimeToTakeGuest;                  // 뭉티 방문주기가 지났는지 확인

    public int[]            mTodayGuestList = new int[5];       // 오늘 방문 예정인 뭉티 목록
    public int              mGuestIndex;                        // 이번에 방문할 뭉티의 번호

    //private
    private static Guest    instance = null;                    // 싱글톤 기법을 위함 instance 생성

    private void Start()
    {
        mGuestTime = 0;
        mGuestIndex = 0;
        isTimeToTakeGuest = false;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        // 싱글톤 기법 사용
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
        // 뭉티의 방문주기를 돌린다.
        if (mGuestTime < 5.0f)
        {
            mGuestTime += Time.deltaTime;
        }
        else if(mGuestTime >= 5.0f && isTimeToTakeGuest == false)
        {
            Debug.Log("뭉티 방문시간이 되었습니다");
            isTimeToTakeGuest = true;

            // 응접실 이동하는 버튼들에 대한 상호작용

        }

        // 구름을 수령 받고 이용을 마친 경우에만 해당 뭉티의 감정의 변화값을 부여함

        // 싱글톤 기법 확인을 위한 테스트코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveSceneToLivingRoom();
        }
        // 감정값 변환을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetEmotion(0, 0, 1, 5, 10);
        }
        // 만족도 갱신을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // 상하한선 침범 확인을 위한 함수 테스트 (성공)
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

    // 뭉티의 정보값들을 받아오는 API
    public string GetName(int gusetNum) 
    { 
        return mGuestInfos[gusetNum].mName; 
    }

    //------------------------------------------------------------------------------------------------------------------------------------------
    // 구름 제공 순서 (구름이 화면상에서 뭉티까지 도착하여지는 작업은 해당 순서에서 생략)
    // 1. 구름을 선택하여 뭉티에게 제공 (뭉티가 앉아있는 상태가 아니라면 제공 불가능) 
    // 2. 구름의 이용시간만큼을 대기 (대기도중 날이 바뀌면 제공 실패)
    // 3. 구름의 감정값만큼을 뭉티의 감정에 더하기 - 함수 생성
    // 4. 구름을 제공받은 뭉티의 감정선들을 확인. (만족도와 감정 상하한선 침범 여부) - 함수 생성
    // 5. 만약 감정 상하한선을 침범했을 경우 뭉티를 불만 뭉티로 설정 (불만 뭉티에 대한 관리는 스크립트 추가 작성) - 함수 생성
    // 6. 만약 만족도가 변경되었을 시에 만족도 값 갱신 (해당 뭉티의 대표감정 갱신) - 함수 생성

    // 7. 만족도가 올라갔을 경우 마당에 뿌릴 수 있는 씨앗(재료)에 관련된 값을 받아서 심기
    // 8. 구름 제공에 관한 결과를 화면에 띄워주고 뭉티를 날씨의 공간에서 내보내기
    //------------------------------------------------------------------------------------------------------------------------------------------
    
    // 상하한선을 침범하는지 판단하여 넘어가는 경우 불만뭉티로 변환한다. -> 구름 제공 순서 5번에서 진행
    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // 침범하는 경우에 감정값을 임의로 저장할 변수

        // 상하한 선을 침범한 경우를 확인
        if (temp != -1) 
        {
            mGuestInfos[guestNum].isDisSat = true;              // 불만 뭉티로 변환
            mGuestInfos[guestNum].mSatatisfaction = 0;          // 만족도 0 으로 갱신
            mGuestInfos[guestNum].mVisitCount = 0;              // 남은 방문횟수 0으로 갱신
            
            // 치유의 기록으로 불만 뭉티가 된 상태와 손님 번호, 어떤 감정 변화로 인한 것인지 전달해주기


            return true;
        }
        return false;
    }

    // 뭉티의 정보값 변경에 필요한 API 
    // Event Handler를 이용하여 만족도 범위안에 들지 못하거나 감정 상하한선을 침범하여 불만 뭉티가 되는경우 이벤트를 발동시켜 관리
    public void SetEmotion(int guestNum, int emotionNum0, int emotionNum1, int value0, int value1) 
    { 
        mGuestInfos[guestNum].mEmotion[emotionNum0] += value0; 
        mGuestInfos[guestNum].mEmotion[emotionNum1] += value1; 
    }

    public int IsExcessLine(int guestNum) // 감정 상하한선을 침범했는지 확인하는 함수. -> 구름 제공 순서 4번에서 진행
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // 상하한선을 침범한 경우
            {
                Debug.Log("상하한선을 침범하였습니다");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfos[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
                Debug.Log("상하한선을 침범하였습니다");
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // 상하한선 모두 침범하지 않는 경우
        Debug.Log("상하한선을 침범하지 않았습니다");
        return -1;
    }

    public void RenewakSat(int guestNum)     // 만족도를 갱신하는 함수. -> 구름 제공 순서 4번에서 진행
    {
        int temp = 0;

        for(int i = 0; i< 5; i++)
        {
            // 만족도 범위 내에 들어가는지 확인
            if(mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfos[guestNum].mSatEmotions[i].up &&
             mGuestInfos[guestNum].mEmotion[mGuestInfos[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfos[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfos[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // 구름 제공으로 인해 뭉티의 감정이 변하는 경우 일정한 조건이 필요하다.
    // 1. 구름을 제공받아 이용하는 동안 날이 바뀌어 집으로 돌아가야 하는 경우 무효
    // -> 일정 시간 (구름을 제공받아 이용하는 시간)동안 대기하다가 시간이 지난 후 감정값을 변경하는 방법 사용 예정)
    //
    // 2. 구름을 제공받아 변화된 감정이 뭉티의 감정 상한선과 하한선을 침범하면 안된다.
    // -> 구름을 제공받고 나서 감정 상한선과 하한선이 침범당하는 경우 불만 뭉티로 변환된다.

    // 불만 뭉티의 패널티
    // Cloud Factory 방문에 제한
    // 치유의 기록에 불만뭉티 표시
    // 만족도 0으로 변환

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

        // 방문 횟수가 끝난 뭉티와 만족도가 5가 된 뭉티는 제외되어야 하므로 먼저 리스트에서 빼낸다.
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

        // 방문 이력이 있는 뭉티가 4명 이상이 없는 경우
        // 모든 방문 이력이 있는 뭉티를 뽑고 나머지를 방문 이력이 없는 뭉티로 채운다.
        if (VisitedGuestNum.Count < 4)
        {
            Debug.Log("방문 이력이 있는 뭉티가 4명이상이 되지 않습니다");
            for (int i = 0; i < VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }

                        // 방문 불가 뭉티 + 불만 뭉티 > 3인 경우 다시 뽑게끔 한다.
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
                Debug.Log(guestList[GuestIndex] + "값이 추가되었습니다.");
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 5 - VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
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
                Debug.Log(guestList[GuestIndex] + "값이 추가되었습니다.");
                GuestIndex++;
                isOverLap = true;

            }
        }
        // 방문 이력이 없는 뭉티가 없는 경우
        // 모든 뭉티를 방문 이력이 있는 뭉티중에서 뽑는다.
        else if (NotVisitedGuestNum.Count == 0)
        {
            Debug.Log("방문 이력이 없는 뭉티가 없습니다");
            for (int i = 0; i < 5; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                            Debug.Log("값 중복.");
                        }
                        int rejectCount = 0;
                        // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
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
                                Debug.Log("불만 뭉티가 너무 많습니다.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];

                Debug.Log(guestList[GuestIndex] + "값이 추가되었습니다.");
                GuestIndex++;
                isOverLap = true;

            }
        }
        // 그 외의 경우에는 방문 이력이 있는 뭉티 4명, 방문 이력이 없는 뭉티 1명은 뽑는다.
        else
        {
            Debug.Log("방문이력 뭉티 4명, 방문 이력이 없는 뭉티 1명을 뽑습니다.");
            for (int i = 0; i < 4; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }

                        int rejectCount = 0;
                        // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
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
                                Debug.Log("불만 뭉티가 너무 많습니다.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                Debug.Log(guestList[GuestIndex] + "값이 추가되었습니다.");
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // 난수 생성
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // 이미 값이 들어있어 중복되는 경우
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                        int rejectCount = 0;
                        // 불만 뭉티이거나 방문 불가 상태 뭉티의 수를 구해야 한다.
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
                                Debug.Log("불만 뭉티가 너무 많습니다.");
                            }
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];

                Debug.Log(guestList[GuestIndex] + "값이 추가되었습니다.");
                GuestIndex++;
                isOverLap = true;
            }
        }
        // 불만 뭉티라면 빼버린다.

        Debug.Log(checkDisSat);

        return guestList;
    }

    // 하루가 시작하면서 당일 날 방문할 뭉티 뽑기
    public int[] ChoiceGuest() // 수정중
    {
        int[]   guestList = new int[5];     // 반환할 뭉티 리스트
        int     VisitedGuestNum = 0;        // 방문 이력이 있는 뭉티 수
        int     NotVisitedGuestNum = 0;     // 방문 이력이 없는 뭉티 수
        bool    chooseRight;

        // 먼저 남은 뭉티의 수가 얼마인지 확인한다. (남은 뭉티 = 전체 - 치유완료 - 방문 불가)
        int     countGuest = 20;
        for(int i = 0; i< 20; i++)
        {
            if(mGuestInfos[i].isCure == true || mGuestInfos[i].mNotVisitCount != 0)
            {
                countGuest--;
            }
        }

        // 남은 뭉티의 수가 5마리 이상이라면 일반적인 방법으로 뭉티 리스트를 뽑는다.
        if (countGuest > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                guestList[i] = -1;
                chooseRight = false;
                while (chooseRight == false)    // 뭉티가 올바르게 골라질 때 까지
                {
                    int temp = Random.Range(0, 20);

                    // 방문한 횟수가 10회이거나 만족도 5를 채워 치유를 완료한 뭉티인 경우 다시 뽑는다.
                    if (mGuestInfos[temp].mVisitCount == 10 || mGuestInfos[temp].isCure == true)
                    {
                        break;
                    }
                    // 이미 뽑힌 뭉티인 경우에도 다시 뽑는다.
                    if(mGuestInfos[temp].isChosen == true)
                    {
                        break;
                    }

                    if (mGuestInfos[temp].mVisitCount == 0) // 해당 뭉티가 방문 이력이 없는 경우
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

            // 리스트에 있는 뭉티의 조합이 방문 이력이 있는 뭉티 4명, 방문 이력이 없는 뭉티 1명의 구성인지 확인한다.


        }
        // 5마리 이하인 경우 고를 수 잇는 뭉티를 모두 리스트에 담는다.
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

        // 불만 뭉티와 방문 불가 상태의 뭉티들을 리스트에서 제외한다.


        Debug.Log("오늘 방문할 뭉티 리스트가 초기화 되었습니다");

        for(int i = 0; i< 20; i++)
        {
            mGuestInfos[i].isChosen = false;
        }

        return guestList;
    }

    // 해당 뭉티를 초기화 시켜주는 함수
    public void InitGuestData() // 추후에 개발
    {

    }

    // 방문주기를 초기화 해주는 함수
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        Debug.Log("방문주기 초기화");
    }
    
    // 하루가 지나면서 초기화가 필요한 정보들을 변환해준다.
    public void InitDay() 
    {
        // 날씨의 공간에 아직 남아있는 뭉티들을 불만 뭉티로 만든다.


        // 새로운 방문 뭉티 리스트를 뽑는다.


        // 방문 주기를 초기화한다.
        InitGuestTime();

        // 채집물들이 다시 갱신된다.

    }

}


