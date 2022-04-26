using UnityEngine;

// 열거형으로 손님의 현재 행동 상태를 표현 ( 총 4가지)
public enum state
{
    dialog, use, wait, none // 순서대로 (대화중, 구름 이용 중, 구름 이용 대기중, 가게 밖)
}

[CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class Guest_Info : ScriptableObject
{
    public string gName; // 손님의 이름
    public int[] gSeed; // 손님이 심고 갈 수 있는 재료의 인덱스 값
    public int[] gEmotion = new int[5]; // 손님의 감정 값

    public int gStatisfaction; // 손님의 만족도
    public state gState; // 손님의 현재 행동 상태
    public Sprite img; // 손님의 이미지

    // 손님의 이름을 반환해주는 함수 - 테스트 함수
    string getName()
    {
        return gName;
    }
}
