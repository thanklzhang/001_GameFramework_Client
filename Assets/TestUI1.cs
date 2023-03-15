using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI1 : ScrollRect
{
    public void SetContentPos(Vector2 pos)
    {
        this.SetContentAnchoredPosition(pos);
    }
}
