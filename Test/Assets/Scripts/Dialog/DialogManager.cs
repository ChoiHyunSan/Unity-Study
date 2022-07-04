using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    // 불러올 값들 선언
    private Guest                   mGuestManager;

    public int                      mGuestNum;           // 손님의 번호를 넘겨받는다.
    private int                     mGuestSat;           // 손님의 현재 만족도
    string                          mTestName;           // 테스트를 위한 임시 이름 ( 손님의 이름을 가져왔다고 가정)

    [SerializeField]
    private DialogDB                mDialogDB;           // 대화 내용을 저장해 놓은 DB
    private string[]                mTextList;           // 대화 내용을 불러와서 저장해둘 리스트
    private int[]                   mGuestImageList;     // 대화 내용에 맞는 표정을 저장해둘 리스트

    // 씬 화면에 나올 텍스트에 들어갈 내용 
    private string                  mDialogGuestName;    // 화면에 출력시킬 손님 이름
    private string                  mDialogText;         // 실제로 화면에 출력시킬 내용

    // 씬 화면에 들어가는 텍스트 오브젝트 선언
    public GameObject               gTextPanel;          // 대화 창
    public GameObject               gTakeGuestPanel;     // 손님 받기/ 거절 버튼
    public Text                     tGuestText;          // 대화가 진행 될 텍스트
    public Text                     tGuestName;          // 대화중이 손님의 이름이 표시될 텍스트

    // 손님의 이미지를 띄우는데 필요한 변수들 선언
    public Sprite[]                 sGuestImageArr;      // 이미지 인덱스들
    public GameObject               gGuestSprite;        // 실제 화면에 출력되는 이미지 오브젝트
    private SpriteRenderer          sGuestSpriteRender;  // 오브젝트의 Sprite 컴포넌트를 읽어올 SpriteRenderer

    // 대화 구현에 필요한 변수값 선언
    private int                     mDialogIndex;        // 해당 만족도에 속하는 지문의 인덱스s
    private int                     mDialogCharIndex;    // 실제로 화면에 출력시키는 내용의 인덱스
    private int                     mDialogImageIndex;   // 실제로 화면에 출력시키는 이미지의 인덱스
    private bool                    isReading;           // 현재 대화창에서 대화를 출력하는 중인가?
    private bool                    isLastDialog;        // 마지막 대화를 불러왔는가?

    // Start is called before the first frame update
    private void Start()
    {
        mDialogIndex = 0;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tGuestText.text = "";
        tGuestName.text = "실연";

        sGuestSpriteRender = gGuestSprite.GetComponent<SpriteRenderer>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
    }

    // 테스트 함수
    // 대화창에서 다른 캐릭터 혹은 다른 만족도의 텍스트를 받아오는 경우 오류가 있는지 확인하기 위한 함수
    void A() 
    {
        tGuestName.text = mGuestManager.GetName(0);  // 날씨의 공간에서 응접실로 갈때 변수를 받아서 넘어가게끔 설정하면 될 것 같다. 
        mGuestSat =  mGuestManager.mGuestInfos[0].mSatatisfaction;

        mDialogIndex = 0;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tGuestText.text = "";

        LoadDialogInfo();
    }

    void Awake()
    {
        mGuestNum = 1;
        mGuestSat = 1;

        mGuestImageList = new int[20];
        mTextList = new string[20];

        isReading = false;

        LoadDialogInfo();
        ReadDialog();
    }

    // Update is called once per frame
    void Update()
    {
        // 싱글톤 기법 확인을 위한 테스트코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveScenetoWeatherSpace();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            A();
        }
    }

    public void MoveScenetoWeatherSpace()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("hello");
    }

    // 해당 손님에 대한 대화값 정보를 불러오는 함수
    private void LoadDialogInfo()
    {
        // 게임 내에 GameManager 한개를 생성하고, 그 곳에서 하루마다 5명의 손님을 지정하여 응접실에 플레이어가 없는 시간에 한하여 랜덤하게 한명씩 방문시킨다.
        // GameManager에서 지정한 손님의 번호를 받아오고, 손님의 번호에 맞는 손님의 정보를 가져온다.

        int i;
        int j = 0;

        Debug.Log(mDialogDB.DialogText.Count);
        for (i = 0; i < mDialogDB.DialogText.Count; ++i)
        {
            if (mDialogDB.DialogText[i].GuestID == mGuestNum)
            {
                if (mDialogDB.DialogText[i].Sat == mGuestSat) 
                {
                    mTextList[j] = mDialogDB.DialogText[i].Text;
                    mGuestImageList[j] = mDialogDB.DialogText[i].DialogImageNumber;
                    j++;
                }
            }
        }
        mTextList[j] = "End";
    }

    private void InitDialog()
    {
        mDialogCharIndex = 0;
        tGuestText.text = "";
    }
    public string GetDialog(int dialogindex) // 만족도 , 대화 내용 순번
    {
        return mTextList[dialogindex];
    }

    private void ReadDialogAtAll()
    {
        tGuestText.text += GetDialog(mDialogIndex);
        isReading = false;
    }

    private void ReadDialogAtOne()
    {
        isReading = true;
        //GuestName.text = testName;
        if (tGuestText.text == GetDialog(mDialogIndex))
        {
            // 텍스트가 모두 출력이 된 경우에 클릭 시, 다음 문장이 출력된다.
            mDialogIndex++;
            mDialogImageIndex++;
            sGuestSpriteRender.sprite = sGuestImageArr[mDialogImageIndex];
            isReading = false;
            return;
        }
        tGuestText.text += GetDialog(mDialogIndex)[mDialogCharIndex];
        mDialogCharIndex++;

        Invoke("ReadDialogAtOne", 0.05f);
    }


    // 손님과의 대화를 실행시켜주는 함수
    public void ReadDialog()
    {
        InitDialog();
 
        // 마지막 End 문자열이 나오는 경우 ( 대화를 모두 불러온 경우)
        if (GetDialog(mDialogIndex) == "End")
        {
            isLastDialog = true;
            // 대화 내용을 모두 출력하고 나면 손님 응대에 관한 여부를 플레이어에게 묻는다. (받는다/ 받지 않는다)
            TakeGuest();
            return;
        }
        gTextPanel.SetActive(true);
        // 대화가 출력중인 도중에 클릭한 경우, 문장이 한번에 출력이 된다.
        if (isReading == true)
        {
            ReadDialogAtAll();
            return;
        }
        // 기본적으로 빈 텍스트에서 대화 내용을 한 글자씩 추가하여 출력하고 딜레이 하기를 반복한다.
        ReadDialogAtOne();
        return;
  
        // 어느 문장까지 출력하였는지 저장한다.
        // DialogIndex 를 초기화 하지 않는 이상, 대화는 이전 혹은 이후로 넘어가지 않기 때문에 우선은 보류하는 것으로 생각 중.
    }

    private void TakeGuest()
    {
        gTakeGuestPanel.SetActive(true);
    }

    // 손님 수락하기
    public void AcceptGuest()
    { 
       mGuestManager.InitGuestTime();
        // 손님이 이동했으므로 응접실에 있는 것들을 초기화 시켜준다.
        ClearGuest();
        MoveScenetoWeatherSpace();
    }

    // 손님 거절하기
    public void RejectGuest()
    {
        // 방문하지 않는 횟수를 3으로 지정한다. (3일간 방문 X)
        mGuestManager.mGuestInfos[mGuestNum].mNotVisitCount = 3;
        mGuestManager.InitGuestTime();

        // 손님이 이동했으므로 응접실에 있는 것들을 초기화 시켜준다.
        ClearGuest();
        MoveScenetoWeatherSpace();
    }

    // 응접실을 초기화 시켜준다.
    private void ClearGuest()
    {

    }
}
// 추가할 기능 구현목록

// 대화가 끝난 뭉티를 거절하면 해당 뭉티는 3일간 방문하지 않는다.
// -> 거절버튼에 대한 상호작용 만들어야 함 (방문금지 3일 + 되돌아가기)

// 대화 도중이나 대화 시작 전에 하루가 종료되는 경우 해당 뭉티의 방문 이력은 없는 것으로 처리
// -> 수락, 거절 버튼 누를때 방문한 것으로 추가시키는 방식으로 진행 

// 수락, 거절 버튼을 누를때는 배경화면이 페이드아웃 되면서 버튼만 하이라이트 되어야 함.