using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        string[] names = new string[5];
        int man;
        GameObject GuestManager = GameObject.Find("GuestManager");

        for (int i =0; i<5; i++)
        {
            names[i] = GuestManager.GetComponent<Guest>().Guest_Infos[i].gName;
            Debug.Log(names[i]);
        }

        GuestManager.GetComponent<Guest>().Guest_Infos[0].gName = "액션가면";

        for (int i = 0; i < 5; i++)
        {
            names[i] = GuestManager.GetComponent<Guest>().Guest_Infos[i].gName;
            Debug.Log(names[i]);
        }

        man = GuestManager.GetComponent<Guest>().Guest_Infos[0].gStatisfaction;
        Debug.Log(man);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    
}
