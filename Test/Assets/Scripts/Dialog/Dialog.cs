using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_info
{
    public string guestName;
    Dictionary<int, string[]> Data; // <������, �������� �ش��ϴ� ��ȭ����>
}

public class Dialog : MonoBehaviour
{
    string test_char = "¯��"; // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)
    string test_dialog = "õ������ �󷷶׶� ���ۺ��� ���ư��� ¯���� �Ϸ�";

    public GameObject textPanel; // ��ȭ â
    public Text GuestText; // ��ȭ�� ���� �� �ؽ�Ʈ


    private Dictionary<int, string[]> dialogData;



    private int DialogIndex; // �ش� �������� ���ϴ� ������ �ε���

    private string DialogText; // ������ ȭ�鿡 ��½�ų ����
    private int DialogCharIndex; // ������ ȭ�鿡 ��½�Ű�� ������ �ε���
    private bool isReading; // ���� ��ȭâ���� ��ȭ�� ����ϴ� ���ΰ�?


    private void Awake()
    {
        isReading = false;
        DialogCharIndex = 0;
        DialogIndex = 0;
        DialogCharIndex = 0;
        GuestText.text = "";
        dialogData = new Dictionary<int,string[]>();
        loadDialog();

    }
    private void initDialog()
    {
        DialogCharIndex = 0;
        GuestText.text = "";
    }

    // �մ��� �̸��� �о�´�.
    private void loadGuest()
    {
          
    }
    // �մ��� �̸��� �̿��Ͽ� �׿� �ش��ϴ� �ؽ�Ʈ�� ���Ͽ��� �ҷ��´�.
    private void loadDialog()
    {
        //�������� ���� �ٸ� ��ȭâ�� �����´�.
        dialogData.Add(1, new string[]  {"���� ¯����", test_dialog, "������ �ݰ���" }); // �ҷ��Դٰ� �����ϰ� �׽�Ʈ ���� ����
    }
    
    public string GetDialog(int sat, int dialogindex)
    {
        return dialogData[sat][dialogindex];
    }
    private void readDialogAtAll()
    {
        //initDialog();
        GuestText.text += GetDialog(1, DialogIndex);
        isReading = false;
    }
    private void readDialogAtOne()
    {
        isReading = true;
        if (GuestText.text == GetDialog(1, DialogIndex)) {
            //initDialog();
            DialogIndex++;
            isReading = false;
            return; 
        }
        GuestText.text += GetDialog(1, DialogIndex)[DialogCharIndex];
        DialogCharIndex++;
        Debug.Log(GuestText.text);
        Invoke("readDialogAtOne",0.2f);
    }  
    // ��ȭâ���� ��ȭ ������ ����Ѵ�.

    public void readDialog()
    {
        initDialog();
        textPanel.SetActive(true);
        // ��ȭ�� ������� ���߿� Ŭ���� ���, ������ �ѹ��� ����� �ȴ�.
        if (isReading == true) {
            readDialogAtAll();
            return; 
        }
        // �⺻������ �� �ؽ�Ʈ���� ��ȭ ������ �� ���ھ� �߰��Ͽ� ����ϰ� ������ �ϱ⸦ �ݺ��Ѵ�.
        readDialogAtOne();
        Debug.Log(GuestText.text);


        // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.


        // ��� ������� ����Ͽ����� �����Ѵ�.


        // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
        takeGuest();
    }

    private void takeGuest()
    {

    }
    

}
