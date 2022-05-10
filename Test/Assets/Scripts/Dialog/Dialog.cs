using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_info
{
    public string guestName; // �մ� �̸�
    public Dictionary<int, string[]> Data; // <������, �������� �ش��ϴ� ��ȭ����>
}

public class Dialog : MonoBehaviour
{
    private dialog_info guestInfo;

    string test_char = "¯��"; // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)
    string test_dialog = "õ������ �󷷶׶� ���ۺ��� ���ư��� ¯���� �Ϸ�";
    public GameObject textPanel; // ��ȭ â
    public GameObject takeGuestPanel; // �մ� �ޱ�/ ���� ��ư
    public Text GuestText; // ��ȭ�� ���� �� �ؽ�Ʈ

    private Dictionary<int, string[]> dialogData;

    private int DialogIndex; // �ش� �������� ���ϴ� ������ �ε���

    private string DialogText; // ������ ȭ�鿡 ��½�ų ����
    private int DialogCharIndex; // ������ ȭ�鿡 ��½�Ű�� ������ �ε���
    private bool isReading; // ���� ��ȭâ���� ��ȭ�� ����ϴ� ���ΰ�?
    private bool isLastDialog; // ������ ��ȭ�� �ҷ��Դ°�?

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

    // �մ��� �̸��� �о�´�.
    private void loadGuest()
    {
          
    }

    // �մ��� �̸��� �̿��Ͽ� �׿� �ش��ϴ� �ؽ�Ʈ�� ���Ͽ��� �ҷ��´�.
    private void loadDialog()
    {
        //�������� ���� �ٸ� ��ȭâ�� �����´�.
        dialogData.Add(1, new string[]  {"���� ¯����", test_dialog, "������ �ݰ���", "End"}); // �ҷ��Դٰ� �����ϰ� �׽�Ʈ ���� ����
    }
    
    public string GetDialog(int sat, int dialogindex) // ������ , ��ȭ ���� ����
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
            // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.
            DialogIndex++;
            isReading = false;
            return; 
        }
        GuestText.text += GetDialog(1, DialogIndex)[DialogCharIndex];
        DialogCharIndex++;
        Debug.Log(GuestText.text);
        Invoke("readDialogAtOne",0.05f);
    }  
    
    // ��ȭâ���� ��ȭ ������ ����Ѵ�.
    public void readDialog()
    {
        initDialog();

        if (isLastDialog == false)
        {
            // ������ End ���ڿ��� ������ ��� ( ��ȭ�� ��� �ҷ��� ���)
            if (GetDialog(1, DialogIndex) == "End") 
            {
                isLastDialog = true; 
                return;
            }
            textPanel.SetActive(true);
            // ��ȭ�� ������� ���߿� Ŭ���� ���, ������ �ѹ��� ����� �ȴ�.
            if (isReading == true)
            {
                readDialogAtAll();
                return;
            }
            // �⺻������ �� �ؽ�Ʈ���� ��ȭ ������ �� ���ھ� �߰��Ͽ� ����ϰ� ������ �ϱ⸦ �ݺ��Ѵ�.
            readDialogAtOne();
            Debug.Log(GuestText.text);
            return;
        }
        else
        {
            // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
            takeGuest();
        }

        // ��� ������� ����Ͽ����� �����Ѵ�.
        // DialogIndex �� �ʱ�ȭ ���� �ʴ� �̻�, ��ȭ�� ���� Ȥ�� ���ķ� �Ѿ�� �ʱ� ������ �켱�� �����ϴ� ������ ���� ��.
    }

    private void takeGuest()
    {
        takeGuestPanel.SetActive(true);
    }
    

}
