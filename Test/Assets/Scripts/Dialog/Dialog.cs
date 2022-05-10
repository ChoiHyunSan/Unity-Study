using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_info
{
    public string guestName; // 손님 이름
    public Dictionary<int, string[]> Data; // <만족도, 만족도에 해당하는 대화지문>
}

public class Dialog : MonoBehaviour
{
    private dialog_info guestInfo;

    string test_char = "짱구"; // 테스트를 위한 임시 이름 ( 손님의 이름을 가져왔다고 가정)
    string test_dialog = "천방지축 얼렁뚱땅 빙글빙글 돌아가는 짱구네 하루";
    public GameObject textPanel; // 대화 창
    public GameObject takeGuestPanel; // 손님 받기/ 거절 버튼
    public Text GuestText; // 대화가 진행 될 텍스트

    private Dictionary<int, string[]> dialogData;

    private int DialogIndex; // 해당 만족도에 속하는 지문의 인덱스

    private string DialogText; // 실제로 화면에 출력시킬 내용
    private int DialogCharIndex; // 실제로 화면에 출력시키는 내용의 인덱스
    private bool isReading; // 현재 대화창에서 대화를 출력하는 중인가?
    private bool isLastDialog; // 마지막 대화를 불러왔는가?

    private void Awake()
    {
        isReading = false;
        DialogCharIndex = 0;
        DialogIndex = 0;
        DialogCharIndex = 0;
        GuestText.text = "";

        dialogData = new Dictionary<int,string[]>();
        guestInfo.Data = new Dictionary<int,string[]>();
        loadDialog();

    }
    private void initDialog()
    {
        DialogCharIndex = 0;
        GuestText.text = "";
    }

    // 손님의 이름을 읽어온다.
    private void loadGuest()
    {
          
    }

    // 손님의 이름을 이용하여 그에 해당하는 텍스트를 파일에서 불러온다.
    private void loadDialog()
    {
        //만족도에 따라 다른 대화창을 가져온다.
        dialogData.Add(1, new string[]  {"나는 짱구야", test_dialog, "만나서 반가워", "End"}); // 불러왔다고 가정하고 테스트 문장 삽입
    }
    
    public string GetDialog(int sat, int dialogindex) // 만족도 , 대화 내용 순번
    {
        return dialogData[sat][dialogindex];
    }
    private void readDialogAtAll()
    {
        GuestText.text += GetDialog(1, DialogIndex);
        isReading = false;
    }
    private void readDialogAtOne()
    {
        isReading = true;
        if (GuestText.text == GetDialog(1, DialogIndex)) {
            // 텍스트가 모두 출력이 된 경우에 클릭 시, 다음 문장이 출력된다.
            DialogIndex++;
            isReading = false;
            return; 
        }
        GuestText.text += GetDialog(1, DialogIndex)[DialogCharIndex];
        DialogCharIndex++;
        Debug.Log(GuestText.text);
        Invoke("readDialogAtOne",0.05f);
    }  
    
    // 대화창에서 대화 내용을 출력한다.
    public void readDialog()
    {
        initDialog();

        if (isLastDialog == false)
        {
            // 마지막 End 문자열이 나오는 경우 ( 대화를 모두 불러온 경우)
            if (GetDialog(1, DialogIndex) == "End") 
            {
                isLastDialog = true; 
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
            Debug.Log(GuestText.text);
            return;
        }
        else
        {
            // 대화 내용을 모두 출력하고 나면 손님 응대에 관한 여부를 플레이어에게 묻는다. (받는다/ 받지 않는다)
            takeGuest();
        }

        // 어느 문장까지 출력하였는지 저장한다.
        // DialogIndex 를 초기화 하지 않는 이상, 대화는 이전 혹은 이후로 넘어가지 않기 때문에 우선은 보류하는 것으로 생각 중.
    }

    private void takeGuest()
    {
        takeGuestPanel.SetActive(true);
    }
    

}
