using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    int[] emotion = new int[5];
    Stats stats;
    private GameObject GuestManager;

    private void Start() {
        // 처음 시작 시, GuestManager를 찾고 
        GuestManager = GameObject.Find("GuestManager");

        for (int i = 0; i < 5; i++)
        {
            emotion[i] = GuestManager.GetComponent<Guest>().mGuestInfos[0].mEmotion[i];
        }

        //if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
        Debug.Log("감정 변환 완료");

        stats = new Stats(emotion[0], emotion[1], emotion[2], emotion[3], emotion[4]);
        uiStatsRadarChart.SetStats(stats);
    }
    private void Update()
    { 
        // 어떤 손님의 감정값이며 값의 변화가 있었는지 Update하여 체크
        UpdateCharacter();
    }
    
    public void ChangeCharacter(int Guest_number)
    {
        // 매개변수로 넘겨받은 손님의 고유번호에 따라 해당 손님의 감정 값을 불러오고 방사형 그래프의 값을 갱신한다.
        for (int i = 0; i < 5; i++)
        {
            emotion[i] = GuestManager.GetComponent<Guest>().mGuestInfos[Guest_number].mEmotion[i];
        }
        Debug.Log("감정 변환 완료");

        // 넘겨받은 값들을 stats으로 넘겨준다.
        stats = new Stats(emotion[0], emotion[1], emotion[2], emotion[3], emotion[4]);
        uiStatsRadarChart.SetStats(stats);
    }
    
    // 입력값에 따라서 캐릭터 정보가 변하게끔 함수 제작 - 테스트 함수
    private void UpdateCharacter()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeCharacter(2);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeCharacter(3);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeCharacter(4);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            // 기쁨 스탯을 1씩 증가시키는 테스트 코드 생성
            stats.IncreaseStatAmount(0);
        }
    }
}
