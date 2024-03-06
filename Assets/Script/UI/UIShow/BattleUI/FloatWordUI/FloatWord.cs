using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatWord
{
    public GameObject gameObject;
    Transform transform;
    public Text valueText;
    public GameObject followGo;
    RectTransform parentRoot;
    bool isUsing;

    public bool IsUsing { get => isUsing; }

    float timer;

    Animator ani;
    private float aniLength;

    public void Init(GameObject go, Transform parentRoot)
    {
        this.parentRoot = parentRoot.GetComponent<RectTransform>();
        this.gameObject = go;
        this.transform = this.gameObject.transform;

        this.valueText = transform.Find("root/text/value_text").GetComponent<Text>();

        ani = transform.GetComponentInChildren<Animator>();
    }

    internal void Start(string word, GameObject followGo, int floatStyle, Color color)
    {
        this.isUsing = true;
        this.followGo = followGo;
        SetShowValue(word);

        this.gameObject.SetActive(true);

        AnimationClip clip = null;
        string stateName = "";
        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
        if (0 == floatStyle)
        {
            //向左
            stateName = "float_word_left";
        }
        else
        {
            stateName = "float_word_right";
        }

        clip = FindClip(stateName);

        this.aniLength = clip.length;

        ani.Play(stateName);

        this.transform.SetAsLastSibling();

        this.valueText.color = color;
    }

    public AnimationClip FindClip(string stateName)
    {
        var clips = ani.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            if (clip.name.Contains(stateName))
            {
                return clip;
            }
        }
        return null;
    }

    public void SetShowValue(string word)
    {
        valueText.text = word;
    }

    public void Update(float deltaTime)
    {
        if (null == this.followGo)
        {
            //这里应该和 entity 同步
            return;
        }
        var entityObj = this.followGo;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRoot, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 80;






        timer += deltaTime;
        if (timer >= this.aniLength)
        {
            this.Finish();
        }

    }

    public void Finish()
    {
        timer = 0;
        this.gameObject.SetActive(false);
        this.isUsing = false;
    }

    public void Release()
    {
        Finish();
    }

}

