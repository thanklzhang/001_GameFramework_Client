using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public TestUI1 scroll;
    public Button btn;

    public float high;
    public Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(()=>
        {
            this.scroll.SetContentPos(pos);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
