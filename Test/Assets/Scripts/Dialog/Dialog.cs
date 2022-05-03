using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_info
{
    public string guestName;
    Dictionary<int, string[]> Data;
}

public class Dialog : MonoBehaviour
{
    string test_char = "짱구"; // 테스트를 위한 임시 이름 ( 손님의 이름을 가져왔다고 가정)
    string test_dialog = "천방지축 얼렁뚱땅 빙글빙글 돌아가는 짱구네 하루";

    public GameObject textPanel; // 대화 창
    public Text GuestText; // 대화가 진행 될 텍스트
    private Dictionary<int, string[]> dialogData;
    public bool isReading;
    private int DialogIndex;

    private void Awake()
    {
        DialogIndex = 0;
        dialogData = new Dictionary<int,string[]>();
        loadDialog();
    }

    // 손님의 이름을 읽어온다.
    private void loadGuest()
    {
          
    }

    // 손님의 이름을 이용하여 그에 해당하는 텍스트를 파일에서 불러온다.
    private void loadDialog()
    {
        //만족도에 따라 다른 대화창을 가져온다.
        dialogData.Add(1, new string[] { "안녕", "나는 짱구야", "만나서 반가워" }); // 불러왔다고 가정하고 테스트 문장 삽입
    }
    
    public string GetDialog(int sat, int dialogindex)
    {
        return dialogData[sat][dialogindex];
    }

    IEnumerator WaitForIt(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    // 대화창에서 대화 내용을 출력한다.

    public void readDialog()
    {
        textPanel.SetActive(true);
        // 기본적으로 빈 텍스트에서 대화 내용을 한 글자씩 추가하여 출력하고 딜레이 하기를 반복한다.
        GuestText.text = GetDialog(1, DialogIndex);
        DialogIndex++;
        Debug.Log(GuestText.text);
        StartCoroutine(WaitForIt(1000.0f));


        // 텍스트 창을 클릭한 경우, 문장이 한번에 출력이 된다.


        // 텍스트가 모두 출력이 된 경우에 클릭 시, 다음 문장이 출력된다.


        // 어느 문장까지 출력하였는지 저장한다.


        // 대화 내용을 모두 출력하고 나면 손님 응대에 관한 여부를 플레이어에게 묻는다. (받는다/ 받지 않는다)
        takeGuest();
    }

    private void takeGuest()
    {

    }
    

}
