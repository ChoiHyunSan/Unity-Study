using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public event EventHandler OnStatsChanged;

    // 감정값의 최소- 최대 값 지정
    public static int STAT_MIN = 0;
    public static int STAT_MAX = 100;

    // 열거형 감성 타입 선언
    public enum Type {
        Joy,
        Sad,
        Fear,
        Calm,
        Anger,
    }

    // private 형태의 SingleStat 자료형 감정들을 선언
    private SingleStat JoyStat;
    private SingleStat SadStat;
    private SingleStat FearStat;
    private SingleStat CalmStat;
    private SingleStat AngerStat;

    // Stats을 생성하고 매개 변수를 받아서 멤버 변수들을 초기화 할 수 있도록 함수 생성
    public Stats(int JoyStatAmount, int SadStatAmount, int FearStatAmount, int CalmStatAmount, int AngerStatAmount) {
        JoyStat = new SingleStat(JoyStatAmount);
        SadStat = new SingleStat(SadStatAmount);
        FearStat = new SingleStat(FearStatAmount);
        CalmStat = new SingleStat(CalmStatAmount);
        AngerStat = new SingleStat(AngerStatAmount);
    }

    public void Start()
    {
    }

    // Type형의 값이 들어왔을 때 switch 문을 통해 해당 Type에 맞는 스탯을 반환해준다.
    private SingleStat GetSingleStat(Type statType) {
        switch (statType) {
        default:
        case Type.Joy:       return JoyStat;
        case Type.Sad:      return SadStat;
        case Type.Fear:        return FearStat;
        case Type.Calm:         return CalmStat;
        case Type.Anger:       return AngerStat;
        }
    }
    
    public void SetStatAmount(Type statType, int statAmount) {
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    public void IncreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType) + 1);
    }

    public void DecreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType) - 1);
    }

    public int GetStatAmount(Type statType) {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(Type statType) {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }


    private class SingleStat {

        private int stat;

        public SingleStat(int statAmount) {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(int statAmount) {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
        }

        public int GetStatAmount() {
            return stat;
        }

        public float GetStatAmountNormalized() {
            return (float)stat / STAT_MAX;
        }
    }
}
