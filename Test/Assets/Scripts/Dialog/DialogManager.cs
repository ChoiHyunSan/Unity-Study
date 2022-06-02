using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    // 불러올 값들 선언
    public int                      guestNum;           // 손님의 번호를 넘겨받는다.
    private int                     guestSat;           // 손님의 현재 만족도
    string                          testName;           // 테스트를 위한 임시 이름 ( 손님의 이름을 가져왔다고 가정)

    [SerializeField]
    private DialogDB                dialogDB;           // 대화 내용을 저장해 놓은 DB
    private string[]                textList;           // 대화 내용을 불러와서 저장해둘 리스트
    private int[]                   guestImageList;     // 대화 내용에 맞는 표정을 저장해둘 리스트

    // 씬 화면에 나올 텍스트에 들어갈 내용 
    private string                  DialogGuestName;    // 화면에 출력시킬 손님 이름
    private string                  DialogText;         // 실제로 화면에 출력시킬 내용

    // 씬 화면에 들어가는 텍스트 오브젝트 선언
    public GameObject               textPanel;          // 대화 창
    public GameObject               takeGuestPanel;     // 손님 받기/ 거절 버튼
    public Text                     GuestText;          // 대화가 진행 될 텍스트
    public Text                     GuestName;          // 대화중이 손님의 이름이 표시될 텍스트

    // 손님의 이미지를 띄우는데 필요한 변수들 선언
    public Sprite[]                 guestImageArr;      // 이미지 인덱스들
    public GameObject               guestSprite;        // 실제 화면에 출력되는 이미지 오브젝트
    private SpriteRenderer          guestSpriteRender;  // 오브젝트의 Sprite 컴포넌트를 읽어올 SpriteRenderer

    // 대화 구현에 필요한 변수값 선언
    private int                     DialogIndex;        // 해당 만족도에 속하는 지문의 인덱스s
    private int                     DialogCharIndex;    // 실제로 화면에 출력시키는 내용의 인덱스
    private int                     DialogImageIndex;   //  실제로 화면에 출력시키는 이미지의 인덱스
    private bool                    isReading;          // 현재 대화창에서 대화를 출력하는 중인가?
    private bool                    isLastDialog;       // 마지막 대화를 불러왔는가?

    // Start is called before the first frame update
    private void Start()
    {
        DialogIndex = 0;
        DialogCharIndex = 0;
        DialogImageIndex = 0;
        GuestText.text = "";
        GuestName.text = "";
        guestSpriteRender = guestSprite.GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        guestImageList = new int[10];
        textList = new string[10];

        isReading = false;
        loadDialogInfo();
        readDialog();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 해당 손님에 대한 대화값 정보를 불러오는 함수
    private void loadDialogInfo()
    {
        // 게임 내에 GameManager 한개를 생성하고, 그 곳에서 하루마다 5명의 손님을 지정하여 응접실에 플레이어가 없는 시간에 한하여 랜덤하게 한명씩 방문시킨다.
        // GameManager에서 지정한 손님의 번호를 받아오고, 손님의 번호에 맞는 손님의 정보를 가져온다.

        guestNum = 1;
        guestSat = 1;

        int i;
        for (i = 0; i < dialogDB.DialogText.Count; ++i)
        {
            if (dialogDB.DialogText[i].GuestID == guestNum)
            {
                if (dialogDB.DialogText[i].Sat == guestSat) 
                {
                    textList[i] = dialogDB.DialogText[i].Text;
                    guestImageList[i] = dialogDB.DialogText[i].DialogImageNumber; 
                }
            }
        }
        textList[i] = "End";
    }

    private void initDialog()
    {
        DialogCharIndex = 0;
        GuestText.text = "";
    }
    public string GetDialog( int dialogindex) // 만족도 , 대화 내용 순번
    {
        return textList[dialogindex];
    }

    private void readDialogAtAll()
    {
        GuestText.text += GetDialog(DialogIndex);
        isReading = false;
    }

    private void readDialogAtOne()
    {
        isReading = true;
        GuestName.text = testName;
        if (GuestText.text == GetDialog(DialogIndex))
        {
            // 텍스트가 모두 출력이 된 경우에 클릭 시, 다음 문장이 출력된다.
            DialogIndex++;
            DialogImageIndex++;
            guestSpriteRender.sprite = guestImageArr[DialogImageIndex];
            isReading = false;
            return;
        }
        GuestText.text += GetDialog(DialogIndex)[DialogCharIndex];
        DialogCharIndex++;
        Invoke("readDialogAtOne", 0.05f);
    }


    // 손님과의 대화를 실행시켜주는 함수
    public void readDialog()
    {
        initDialog();
 
        // 마지막 End 문자열이 나오는 경우 ( 대화를 모두 불러온 경우)
        if (GetDialog(DialogIndex) == "End")
        {
            isLastDialog = true;
            // 대화 내용을 모두 출력하고 나면 손님 응대에 관한 여부를 플레이어에게 묻는다. (받는다/ 받지 않는다)
            takeGuest();
            return;
        }
        textPanel.SetActive(true);
        // 대화가 출력중인 도중에 클릭한 경우, 문장이 한번에 출력이 된다.
        if (isReading == true)
        {
            readDialogAtAll();
            return;
        }
        // 기본적으로 빈 텍스트에서 대화 내용을 한 글자씩 추가하여 출력하고 딜레이 하기를 반복한다.
        readDialogAtOne();
        return;
  

        // 어느 문장까지 출력하였는지 저장한다.
        // DialogIndex 를 초기화 하지 않는 이상, 대화는 이전 혹은 이후로 넘어가지 않기 때문에 우선은 보류하는 것으로 생각 중.
    }

    private void takeGuest()
    {
        takeGuestPanel.SetActive(true);
    }
}
