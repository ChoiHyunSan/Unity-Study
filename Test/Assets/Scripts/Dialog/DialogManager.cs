using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    // �ҷ��� ���� ����
    public int                      guestNum;           // �մ��� ��ȣ�� �Ѱܹ޴´�.
    private int                     guestSat;           // �մ��� ���� ������
    string                          testName;           // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)

    [SerializeField]
    private DialogDB                dialogDB;           // ��ȭ ������ ������ ���� DB
    private string[]                textList;           // ��ȭ ������ �ҷ��ͼ� �����ص� ����Ʈ
    private int[]                   guestImageList;     // ��ȭ ���뿡 �´� ǥ���� �����ص� ����Ʈ

    // �� ȭ�鿡 ���� �ؽ�Ʈ�� �� ���� 
    private string                  DialogGuestName;    // ȭ�鿡 ��½�ų �մ� �̸�
    private string                  DialogText;         // ������ ȭ�鿡 ��½�ų ����

    // �� ȭ�鿡 ���� �ؽ�Ʈ ������Ʈ ����
    public GameObject               textPanel;          // ��ȭ â
    public GameObject               takeGuestPanel;     // �մ� �ޱ�/ ���� ��ư
    public Text                     GuestText;          // ��ȭ�� ���� �� �ؽ�Ʈ
    public Text                     GuestName;          // ��ȭ���� �մ��� �̸��� ǥ�õ� �ؽ�Ʈ

    // �մ��� �̹����� ���µ� �ʿ��� ������ ����
    public Sprite[]                 guestImageArr;      // �̹��� �ε�����
    public GameObject               guestSprite;        // ���� ȭ�鿡 ��µǴ� �̹��� ������Ʈ
    private SpriteRenderer          guestSpriteRender;  // ������Ʈ�� Sprite ������Ʈ�� �о�� SpriteRenderer

    // ��ȭ ������ �ʿ��� ������ ����
    private int                     DialogIndex;        // �ش� �������� ���ϴ� ������ �ε���s
    private int                     DialogCharIndex;    // ������ ȭ�鿡 ��½�Ű�� ������ �ε���
    private int                     DialogImageIndex;   //  ������ ȭ�鿡 ��½�Ű�� �̹����� �ε���
    private bool                    isReading;          // ���� ��ȭâ���� ��ȭ�� ����ϴ� ���ΰ�?
    private bool                    isLastDialog;       // ������ ��ȭ�� �ҷ��Դ°�?

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

    // �ش� �մԿ� ���� ��ȭ�� ������ �ҷ����� �Լ�
    private void loadDialogInfo()
    {
        // ���� ���� GameManager �Ѱ��� �����ϰ�, �� ������ �Ϸ縶�� 5���� �մ��� �����Ͽ� �����ǿ� �÷��̾ ���� �ð��� ���Ͽ� �����ϰ� �Ѹ� �湮��Ų��.
        // GameManager���� ������ �մ��� ��ȣ�� �޾ƿ���, �մ��� ��ȣ�� �´� �մ��� ������ �����´�.

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
    public string GetDialog( int dialogindex) // ������ , ��ȭ ���� ����
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
            // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.
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


    // �մ԰��� ��ȭ�� ��������ִ� �Լ�
    public void readDialog()
    {
        initDialog();
 
        // ������ End ���ڿ��� ������ ��� ( ��ȭ�� ��� �ҷ��� ���)
        if (GetDialog(DialogIndex) == "End")
        {
            isLastDialog = true;
            // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
            takeGuest();
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
        return;
  

        // ��� ������� ����Ͽ����� �����Ѵ�.
        // DialogIndex �� �ʱ�ȭ ���� �ʴ� �̻�, ��ȭ�� ���� Ȥ�� ���ķ� �Ѿ�� �ʱ� ������ �켱�� �����ϴ� ������ ���� ��.
    }

    private void takeGuest()
    {
        takeGuestPanel.SetActive(true);
    }
}
