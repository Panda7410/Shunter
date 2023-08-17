using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiFuncTween : MonoBehaviour
{
    [Header("조절할UI")]
    public RectTransform TargetRect;
    [Header("시간")]
    public float ControllTime = 0.7f;
    public Ease UiEase = Ease.Linear;
    [Space]
    [Header("===================")]
    [Header("델타 X")]
    public bool SizeDeltaXControll = false;
    public float SizeDeltaX;
    [Header("델타 Y")]
    public bool SizeDeltaYControll = false;
    public float SizeDeltaY;
    [Header("===================")]
    [Header("시작 사이즈 여부")]
    public bool SizeDeltaXStartSize = false;
    public float StartSizeDeltaX;
    public bool SizeDeltaYStartSize = false;
    public float StartSizeDeltaY;
    [Header("======================================")]
    [Header("======================================")]

    [Space]
    [Header("스케일 X")]
    public bool SizeScaleXControll = false;
    public float SizeScaleX;

    [Header("스케일 Y")]
    public bool SizeScaleYControll = false;
    public float SizeScaleY;
    [Header("===================")]
    [Header("시작 스케일 여부")]
    public bool SizeScaleXStartSize = false;
    public float StartScaleX = 1;
    public bool SizeScaleYStartSize = false;
    public float StartScaleY = 1;

    [Header("======================================")]
    [Header("======================================")]

    [Space]
    [Header("Positon")]
    public bool PosX;
    public float PosXValue;
    public bool PosY;
    public float PosYValue;
    public bool PosZ;
    public float PosZValue;
    [Header("======================================")]
    [Header("시작포지션여부")]
    public bool PosStartX;
    public float PosStartXValue;
    public bool PosStartY;
    public float PosStartYValue;
    public bool PosStartZ;
    public float PosStartZValue;



    [Header("함께 컨트롤하는 트윈들.")]
    [Tooltip("같은 오브젝트 하위에 없으면 수동으로 추가할것.")]
    public List<UiFuncTween> funcTweens = new List<UiFuncTween>();


    Sequence UiSequence;// = DOTween.Sequence();

    

    // Start is called before the first frame update
    void Start()
    {
        if (TargetRect == null)
            TargetRect = GetComponent<RectTransform>();
        if (TargetRect == null)
            Debug.LogError("RectTransform이 할당되지 않았습니다.");

        UiFuncTween[] twns = transform.parent.GetComponentsInChildren<UiFuncTween>();

        for (int i = 0; i < twns.Length; i++)
        {
            if (funcTweens.Contains(twns[i]) || twns[i] == this)
                continue;
            if (twns[i].TargetRect == TargetRect)
                funcTweens.Add(twns[i]);
        }
    }

    [ContextMenu("실행")]
    public void Call()
    {
        if (TargetRect == null)
            TargetRect = GetComponent<RectTransform>();
        if (TargetRect == null)
        {
            Debug.LogError("RectTransform이 할당되지 않았습니다.");
            return;
        }

        if (UiSequence != null)
            UiSequence.Kill();
        for (int i = 0; i < funcTweens.Count; i++)
        {
            if (funcTweens[i].UiSequence != null)
                funcTweens[i].UiSequence.Kill();
        }
        //시작사이즈 변경여부.
        if (SizeDeltaXStartSize) TargetRect.sizeDelta = new Vector2(StartSizeDeltaX, TargetRect.sizeDelta.y);
        if (SizeDeltaYStartSize) TargetRect.sizeDelta = new Vector2(TargetRect.sizeDelta.x, StartSizeDeltaY);
        //시작스케일 변경여부
        if (SizeScaleXStartSize) TargetRect.localScale = new Vector3(StartScaleX, TargetRect.localScale.y, 1);
        if (SizeScaleYStartSize) TargetRect.localScale = new Vector3(TargetRect.localScale.x, StartScaleY, 1);
        //포지션여부
        if (PosStartX) TargetRect.localPosition = new Vector3(PosStartXValue , TargetRect.localPosition.y, TargetRect.localPosition.z);
        if (PosStartY) TargetRect.localPosition = new Vector3(TargetRect.localPosition.x, PosStartYValue, TargetRect.localPosition.z);
        if (PosStartZ) TargetRect.localPosition = new Vector3(TargetRect.localPosition.x, TargetRect.localPosition.y, PosStartZValue);



        UiSequence = DOTween.Sequence();
        //XY 델타값 모두 컨트롤 
        if(SizeDeltaXControll && SizeDeltaYControll)
            UiSequence.Append(TargetRect.DOSizeDelta(new Vector2(SizeDeltaX, SizeDeltaY), ControllTime).SetEase(UiEase));
        else if(SizeDeltaXControll)//x값만 컨트롤. 
            UiSequence.Append(TargetRect.DOSizeDelta(new Vector2(SizeDeltaX, TargetRect.sizeDelta.y), ControllTime).SetEase(UiEase));
        else if (SizeDeltaYControll)//y값만 컨트롤. 
            UiSequence.Append(TargetRect.DOSizeDelta(new Vector2(TargetRect.sizeDelta.x, SizeDeltaY), ControllTime).SetEase(UiEase));

        //XY 스케일 모두 컨트롤
        if (SizeScaleXControll && SizeScaleYControll)
            UiSequence.Join(TargetRect.DOScale(new Vector3(SizeScaleX, SizeScaleY, 1), ControllTime).SetEase(UiEase));
        else if (SizeScaleXControll)//x값만 컨트롤. 
            UiSequence.Join(TargetRect.DOScale(new Vector3(SizeScaleX, TargetRect.localScale.y,1), ControllTime).SetEase(UiEase));
        else if (SizeScaleYControll)//y값만 컨트롤. 
            UiSequence.Join(TargetRect.DOScale(new Vector3(TargetRect.localScale.x, SizeScaleY,1), ControllTime).SetEase(UiEase));

        //XY 모든 포지션조절
        if (PosX && PosY && PosZ)
            UiSequence.Join(TargetRect.DOLocalMove(new Vector3(PosXValue, PosYValue, PosZValue), ControllTime));
        else if(PosX && PosY)
            UiSequence.Join(TargetRect.DOLocalMove(new Vector3(PosXValue, PosYValue, TargetRect.localPosition.z), ControllTime));
        else if (PosX && PosZ)
            UiSequence.Join(TargetRect.DOLocalMove(new Vector3(PosXValue, TargetRect.localPosition.y, PosZValue), ControllTime));
        else if (PosY && PosZ)
            UiSequence.Join(TargetRect.DOLocalMove(new Vector3(TargetRect.localPosition.x, PosYValue, PosZValue), ControllTime));
        else if (PosX)
            UiSequence.Join(TargetRect.DOLocalMoveX(PosXValue, ControllTime));
        else if (PosY)
            UiSequence.Join(TargetRect.DOLocalMoveY(PosYValue, ControllTime));
        else if (PosZ)
            UiSequence.Join(TargetRect.DOLocalMoveZ(PosZValue, ControllTime));



        UiSequence.Play();
    }
}
