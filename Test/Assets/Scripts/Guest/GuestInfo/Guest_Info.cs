using UnityEngine;

// ���������� �մ��� ���� �ൿ ���¸� ǥ�� ( �� 4����)
public enum state
{
    dialog, use, wait, none // ������� (��ȭ��, ���� �̿� ��, ���� �̿� �����, ���� ��)
}

[CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class Guest_Info : ScriptableObject
{
    public string gName; // �մ��� �̸�
    public int[] gSeed; // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int[] gEmotion = new int[5]; // �մ��� ���� ��

    public int gStatisfaction; // �մ��� ������
    public state gState; // �մ��� ���� �ൿ ����
    public Sprite img; // �մ��� �̹���

    // �մ��� �̸��� ��ȯ���ִ� �Լ� - �׽�Ʈ �Լ�
    string getName()
    {
        return gName;
    }
}
