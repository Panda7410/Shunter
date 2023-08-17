using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class swapCam : MonoBehaviour
{
    public Transform tr;
    public List<Transform> transforms = new List<Transform>();
    public Ease ease = Ease.InSine;
    public float MTime = 1f;
    Tween tween;
    
    public void MoveTr(int index)
    {
        tween?.Kill();
        tween = tr.DOMove(transforms[index].position, MTime).SetEase(ease);
    }
}
