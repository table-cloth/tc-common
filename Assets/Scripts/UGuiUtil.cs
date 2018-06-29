using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public static class UGuiUtil
{
    public static Button CreateButton(string _name, Transform _parent)
    {
        GameObject buttonObject = new GameObject(_name);
        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();
        return button;
    }

    public static void SetPosition(RectTransform _rectTransform, Vector3 _pos)
    {
        _rectTransform.anchoredPosition = _pos;
    }

    public static void SetSize(RectTransform _rectTransform, Vector2 _size)
    {
        _rectTransform.sizeDelta = _size;
    }

    public static void SetPivot(RectTransform _rectTransform, Vector2 _pivot)
    {
        _rectTransform.pivot = _pivot;
    }

    public static void SetAnchor(RectTransform _rectTransform, Vector2 _minAnchor, Vector2 _maxAnchor)
    {
        _rectTransform.anchorMin = _minAnchor;
        _rectTransform.anchorMax = _maxAnchor;
    }
}
