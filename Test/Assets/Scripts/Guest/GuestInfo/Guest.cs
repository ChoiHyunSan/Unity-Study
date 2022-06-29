using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    //public
    public GuestInfo[] mGuestInfos;             // Scriptable Objects들의 정보를 담고 있는 배열

    //private
    private static Guest instance = null;       // 싱글톤 기법을 위함 instance 생성

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
        // 구름을 수령 받았을 경우에만 해당 뭉티의 감정의 변화값을 부여함


        // 구름을 이용중에 시간이 흘러 날이 바뀌면 값이 변화된 것을 원상태로 복구한다.


        // 구름 수령 이벤트 발생시에 구름 수령에 연관되어 있는 값들에게 구름 수령을 받았다는 정보만을 넘긴다.


        // 싱글톤 기법 확인을 위한 테스트코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveSceneToLivingRoom();
        }
        // 감정값 변환을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetEmotion(0, 0, 5);
        }
        // 만족도 갱신을 위한 함수 테스트 (성공)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // 상하한선 침범 확인을 위한 함수 테스트 (성공)
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

    // 뭉티의 정보값들을 받아오는 API
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
    // 구름 제공 순서 (구름이 화면상에서 뭉티까지 도착하여지는 작업은 해당 순서에서 생략)
    // 1. 구름을 선택하여 뭉티에게 제공 (뭉티가 앉아있는 상태가 아니라면 제공 불가능)
    // 2. 구름의 이용시간만큼을 대기 (대기도중 날이 바뀌면 제공 실패)
    // 3. 구름의 감정값만큼을 뭉티의 감정에 더하기 
    // 4. 구름을 제공받은 뭉티의 감정선들을 확인. (만족도와 감정 상하한선 침범 여부)
    // 5. 만약 감정 상하한선을 침범했을 경우 뭉티를 불만 뭉티로 설정 (불만 뭉티에 대한 관리는 스크립트 추가 작성)
    // 6. 만약 만족도가 변경되었을 시에 만족도 값 갱신 (해당 뭉티의 대표감정 갱신)

    // 7. 만족도가 올라갔을 경우 마당에 뿌릴 수 있는 씨앗에 관련된 값을 받아서 심기
    // 8. 구름 제공에 관한 결과를 화면에 띄워주고 뭉티를 날씨의 공간에서 내보내기
    //------------------------------------------------------------------------------------------------------------------------------------------



    // 상하한선을 침범하는지 판단하여 넘어가는 경우 불만뭉티로 변환한다. -> 구름 제공 순서 5번에서 진행
    public void CheckIsDisSat(int guestNum)
    {
        // 상하한 선을 침범한 경우를 확인
        if (IsExcessLine(guestNum)) 
        {
            mGuestInfos[guestNum].isDisSat = true; // 불만 뭉티로 변환
            mGuestInfos[guestNum].mSatatisfaction = 0;
        }
    }

    // 뭉티의 정보값 변경에 필요한 API 
    // Event Handler를 이용하여 만족도 범위안에 들지 못하거나 감정 상하한선을 침범하여 불만 뭉티가 되는경우 이벤트를 발동시켜 관리
    public void SetEmotion(int guestNum, int emotionNum, int value) { mGuestInfos[guestNum].mEmotion[emotionNum] += value; }

    public bool IsExcessLine(int guestNum) // 감정 상하한선을 침범했는지 확인하는 함수. -> 구름 제공 순서 4번에서 진행
    {

        SLimitEmotion[] limitEmotion = mGuestInfos[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (GetEmotion(guestNum, limitEmotion[i].upLimitEmotion) >= limitEmotion[i].upLimitEmotionValue 
                || GetEmotion(guestNum, limitEmotion[i].downLimitEmotion) <= limitEmotion[i].downLimitEmotionValue) // 상하한선을 침범한 경우
            {
                Debug.Log("상하한선을 침범하였습니다");
                return false;
            }
        }

        // 상하한선 모두 침범하지 않는 경우
        Debug.Log("상하한선을 침범하지 않았습니다");
        return true;
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




}


