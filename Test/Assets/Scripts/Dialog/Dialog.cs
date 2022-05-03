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
    string test_char = "¯��"; // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)
    string test_dialog = "õ������ �󷷶׶� ���ۺ��� ���ư��� ¯���� �Ϸ�";

    public GameObject textPanel; // ��ȭ â
    public Text GuestText; // ��ȭ�� ���� �� �ؽ�Ʈ
    private Dictionary<int, string[]> dialogData;
    public bool isReading;
    private int DialogIndex;

    private void Awake()
    {
        DialogIndex = 0;
        dialogData = new Dictionary<int,string[]>();
        loadDialog();
    }

    // �մ��� �̸��� �о�´�.
    private void loadGuest()
    {
          
    }

    // �մ��� �̸��� �̿��Ͽ� �׿� �ش��ϴ� �ؽ�Ʈ�� ���Ͽ��� �ҷ��´�.
    private void loadDialog()
    {
        //�������� ���� �ٸ� ��ȭâ�� �����´�.
        dialogData.Add(1, new string[] { "�ȳ�", "���� ¯����", "������ �ݰ���" }); // �ҷ��Դٰ� �����ϰ� �׽�Ʈ ���� ����
    }
    
    public string GetDialog(int sat, int dialogindex)
    {
        return dialogData[sat][dialogindex];
    }

    IEnumerator WaitForIt(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    // ��ȭâ���� ��ȭ ������ ����Ѵ�.

    public void readDialog()
    {
        textPanel.SetActive(true);
        // �⺻������ �� �ؽ�Ʈ���� ��ȭ ������ �� ���ھ� �߰��Ͽ� ����ϰ� ������ �ϱ⸦ �ݺ��Ѵ�.
        GuestText.text = GetDialog(1, DialogIndex);
        DialogIndex++;
        Debug.Log(GuestText.text);
        StartCoroutine(WaitForIt(1000.0f));


        // �ؽ�Ʈ â�� Ŭ���� ���, ������ �ѹ��� ����� �ȴ�.


        // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.


        // ��� ������� ����Ͽ����� �����Ѵ�.


        // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
        takeGuest();
    }

    private void takeGuest()
    {

    }
    

}
