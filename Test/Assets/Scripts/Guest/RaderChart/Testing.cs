using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    int[] emotion = new int[5];
    Stats stats;

    private void Start() {
        GameObject GuestManager = GameObject.Find("GuestManager");


        for (int i = 0; i < 5; i++)
        {
            emotion[i] = GuestManager.GetComponent<Guest>().Guest_Infos[0].gEmotion[i];
        }


        //if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
        Debug.Log("감정 변환 완료");

        stats = new Stats(emotion[0], emotion[1], emotion[2], emotion[3], emotion[4]);

        uiStatsRadarChart.SetStats(stats);
    }
    private void Update()
    {
        uiStatsRadarChart.SetStats(stats);
    }

}
