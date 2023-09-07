using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextFunc
{
    /// <summary>
    /// 전체 텍스트 라인을 읽어온다. 
    /// </summary>
    /// <param name="TextFileName">읽어올 파일명. </param>
    /// <returns></returns>
    public string[] ReadTextAllLine(string TextFileName)
    {
        string[] temp = File.ReadAllLines($"{Application.dataPath}/Data/{TextFileName}.txt");
        return temp;
    }

    /// <summary>
    /// 특정 아이템 값을 읽어온다. 아이템과 값의 구분은 : 콜론으로 한다.
    /// </summary>
    /// <param name="TextFileName">읽어올 파일명</param>
    /// <param name="ItemName">아이템 네임</param>
    /// <returns></returns>
    public string ReadTextByItem(string TextFileName, string ItemName)
    {
        Dictionary<string, string> targetDic = ReadTextToDictionary(TextFileName);
        string temp = "";

        if (targetDic.ContainsKey(ItemName))
            temp = targetDic[ItemName];
        else
            LogDisplay.LogError($"{TextFileName} 구성에 {ItemName} 가 존재하지않습니다. 향후 동작에 이상이 있을 수 있습니다.");

        return temp;
    }

    public Dictionary<string, string> ReadTextToDictionary(string TextFileName)
    {
        string[] temp = ReadTextAllLine(TextFileName);
        Dictionary<string, string> targetDic = new Dictionary<string, string>();
        foreach (var item in temp)
        {
            string[] split = item.Split(':');
            if(split.Length != 2)
            {
                LogDisplay.LogWarning($"{TextFileName} 구성에 올바르지 않은 라인이 존재합니다.");
                continue;
            }
            targetDic.Add(split[0], split[1]);
        }
        return targetDic;
    }


    //public void GetJson(string Path)
    //{

    //}
}
