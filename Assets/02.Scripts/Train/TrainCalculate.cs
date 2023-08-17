using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TrainCalculate
{
    public static float GetBackLenth(List<TrainModule> BackTrainModules, TrainModule MainTrainModule)
    {
        float totalLenth = 0;
        //몸통포함.
        totalLenth
                += MainTrainModule.GetTrainSpec.HeadFromBogie
                + MainTrainModule.GetTrainSpec.LengthBetweenBogies
                + MainTrainModule.GetTrainSpec.TailFromBogie;

        for (int i = 0; i < BackTrainModules.Count; i++)
        {
            totalLenth
                += BackTrainModules[i].GetTrainSpec.HeadFromBogie
                + BackTrainModules[i].GetTrainSpec.LengthBetweenBogies
                + BackTrainModules[i].GetTrainSpec.TailFromBogie;
        }

        //정방향이면 메인차량의 머리를 빼고 역방향이면 꼬리를 뺀다. 
        if (!MainTrainModule.TrainDirect) // False 정방향.
            totalLenth -= MainTrainModule.GetTrainSpec.HeadFromBogie;
        else
            totalLenth -= MainTrainModule.GetTrainSpec.TailFromBogie;
        //마지막 차량은 반대. 
        if (BackTrainModules.Count > 0)
        {
            if (!BackTrainModules[BackTrainModules.Count - 1].TrainDirect)
                totalLenth -= BackTrainModules[BackTrainModules.Count - 1].GetTrainSpec.TailFromBogie;
            else
                totalLenth -= BackTrainModules[BackTrainModules.Count - 1].GetTrainSpec.HeadFromBogie;
        }
        else // 카운트가 0일경우. 메인차량이 마지막차량. 
        {
            if (!MainTrainModule.TrainDirect) // False 정방향.
                totalLenth -= MainTrainModule.GetTrainSpec.TailFromBogie;
            else
                totalLenth -= MainTrainModule.GetTrainSpec.HeadFromBogie;
        }

        return totalLenth;
    }

    public static float GetFrontLenth(List<TrainModule> FrontTrainModules, TrainModule MainTrainModule)
    {
        float totalLenth = 0;
        //프론트에서는 길이 더하지 않음.
        try
        {

            for (int i = 0; i < FrontTrainModules.Count; i++)
            {
                totalLenth
                    += FrontTrainModules[i].GetTrainSpec.HeadFromBogie
                    + FrontTrainModules[i].GetTrainSpec.LengthBetweenBogies
                    + FrontTrainModules[i].GetTrainSpec.TailFromBogie;
            }
            //메인차량의 연결부위 덧셈
            if (!MainTrainModule.TrainDirect) // False 정방향.
                totalLenth += MainTrainModule.GetTrainSpec.HeadFromBogie;
            else
                totalLenth += MainTrainModule.GetTrainSpec.TailFromBogie;

            //마지막 차량은 반대. 뺄셈. 
            if (FrontTrainModules.Count > 0)
            {
                if (!FrontTrainModules[FrontTrainModules.Count - 1].TrainDirect)
                    totalLenth -= FrontTrainModules[FrontTrainModules.Count - 1].GetTrainSpec.HeadFromBogie;
                else
                    totalLenth -= FrontTrainModules[FrontTrainModules.Count - 1].GetTrainSpec.TailFromBogie;
            }

        }
        catch (Exception)
        {

            Debug.LogError("아잇.!");
        }
        return totalLenth;
    }

    public static float GetNextDist(float CurrentDist, float Speed)
    {
        float AddDist = Speed * Time.deltaTime * 0.2778f;
        return CurrentDist + AddDist;
    }

}


