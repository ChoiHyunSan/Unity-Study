using UnityEngine;

public enum state
{
    dialog, use, wait, none
}

[CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class Guest_Info : ScriptableObject
{
    public string gName;
    public int[] gSeed;
    public int[] gEmotion = new int[5];

    public int gStatisfaction;
    public state gState;
    public Sprite img;


    string getName()
    {
        return gName;
    }
}
