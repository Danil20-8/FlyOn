using UnityEngine;
using System.Collections;
using System;

public class FloatUI : MonoBehaviour
{

    [SerializeField]
    float time;
    [SerializeField]
    bool rotate = true;
    [SerializeField]
    bool move = true;
    [SerializeField]
    bool scale = false;
    [SerializeField]
    bool disable = false;
    [SerializeField]
    Transform HideTransform;
    Quaternion startRotation;
    Vector3 startPosition;
    Vector3 startScale;
    float progress = 1;
    bool hide { get { if (_first) { return disable ? !gameObject.activeSelf : _hide; } return _hide; } set { if (_first) _first = false; _hide = value; } }
    bool _hide;
    bool _first = true;
    Action state;

    void Start()
    {
        if (rotate)
            startRotation = transform.rotation;
        if (move)
            startPosition = transform.localPosition;
        if (scale)
            startScale = transform.localScale;
        if (state == null)
            state = ShowUpdate;
        Update();
    }
    public void Show()
    {
        if (hide)
        {
            progress = 1 - progress;
            hide = false;

            state = ShowUpdate;
            ShowActivate();
        }
    }
    public void Hide()
    {
        if (!hide)
        {
            progress = 1 - progress;
            hide = true;

            state = HideUpdate;
        }
    }
    public void HideShow()
    {
        if (hide)
            Show();
        else
            Hide();
    }
    void Update()
    {
        if (progress < 1f)
            progress += Time.deltaTime / time;
        else
            progress = 1f;

        state();
    }

    public void Reset()
    {
        hide = false;
        progress = 1;
        Update();
    }

    public void HideUpdate()
    {
        if (rotate)
        {
            if (progress >= 1)
                transform.rotation = HideTransform.rotation;
            else
                transform.rotation = Quaternion.Slerp(startRotation, HideTransform.rotation, progress);
        }

        if (move)
        {
            if (progress >= 1)
                transform.localPosition = HideTransform.localPosition;
            else
                transform.localPosition = Vector3.Lerp(startPosition, HideTransform.localPosition, progress);
        }

        if (scale)
        {
            if (progress >= 1)
                transform.localScale = HideTransform.localScale;
            else
                transform.localScale = Vector3.Lerp(startScale, HideTransform.localScale, progress);
        }

        if (progress >= 1)
            if (disable)
                gameObject.SetActive(false);
    }


    public void ShowActivate()
    {
        if (disable)
            gameObject.SetActive(true);
    }

    public void ShowUpdate()
    {
        if (rotate)
        {
            if (progress >= 1)
                transform.rotation = startRotation;
            else
                transform.rotation = Quaternion.Slerp(HideTransform.rotation, startRotation, progress);
        }

        if (move)
        {
            if (progress >= 1)
                transform.localPosition = startPosition;
            else
                transform.localPosition = Vector3.Lerp(HideTransform.localPosition, startPosition, progress);
        }

        if (scale)
        {
            if (progress >= 1)
                transform.localScale = startScale;
            else
                transform.localScale = Vector3.Lerp(HideTransform.localScale, startScale, progress);
        }
    }
}
