using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoopFunc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Client.Instance.OnConnect += () => 
        {
            if (LoopTimer != null)
                StopCoroutine(LoopTimer);
            LoopTimer = _LoopTimer();
            StartCoroutine(LoopTimer);
        };
        Client.Instance.OnDisConnect += () => 
        {
            if (LoopTimer != null)
                StopCoroutine(LoopTimer);
        };
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    IEnumerator LoopTimer;
    IEnumerator _LoopTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        DataModel dataModel = new DataModel()
        {
            Order = OrderKind.GET,
        };
        dataModel.Items.Add(new Item() { Name = ItemList.훈련제어 });
        dataModel.Items.Add(new Item() { Name = ItemList.시스템제어 });
        dataModel.Items.Add(new Item() { Name = ItemList.자기진단실시 });

        while (true)
        {
            yield return wait;

            Client.Instance.SendData(dataModel);
        }
    }
}
