using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBuilderCamControl : MonoBehaviour
{



    float LineDist = 1;
    public float JumpDist;

    [Range(-200f, 300f)]
    public float speed;

    Animation LineAnime;



    LineBuilderCore core;

    public void SetCamControl(Animation LineAnime, GameObject MainCar)
    {

        try
        {
            core = FindObjectOfType<LineBuilderCore>();
        }
        catch
        {
            Debug.LogError("잘못된 접근. LineBuilderCore 개체가 존재하지 않습니다.");
            return;
        }

        this.LineAnime = LineAnime;

        StartCoroutine(CamLoop(this.LineAnime, MainCar));

    }

    public void Jump()
    {

    }

    IEnumerator CamLoop(Animation animation ,GameObject Car)
    {
        yield return null;// 야야 잘못만들었다. 복사해서 다시 짜라 이거 ㅇㅇ

        GameObject CamMothoer = new GameObject(animation.name + "Cam");
        GameObject Cam = new GameObject("Cam");
        Cam.transform.SetParent(CamMothoer.transform);
        Cam.AddComponent<Camera>();
        CamMothoer.transform.position = Car.transform.position;
        CamMothoer.transform.rotation = Car.transform.rotation;
        Cam.transform.localPosition = new Vector3(0, 2.7f, 0);

        while (true)
        {
            animation["Take 001"].speed = core.MeterToLenght(speed);
            yield return null;

            CamMothoer.transform.position = Vector3.Lerp(CamMothoer.transform.position, Car.transform.position, 0.4f);
            CamMothoer.transform.rotation = Quaternion.Slerp(CamMothoer.transform.rotation, Car.transform.rotation, 0.4f);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
