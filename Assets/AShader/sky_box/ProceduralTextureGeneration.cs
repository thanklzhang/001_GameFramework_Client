using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTextureGeneration : MonoBehaviour
{
    //材质
    public Material material = null;
    //生成的程序纹理
    private Texture2D m_generatedTexture = null;
    //设置各种变量，并且设置set和get函数，在set函数中调用_UpdateMaterial()方法，从而实时更新程序纹理
    #region Material properties
    [SerializeField]
    private int m_textureWidth = 512;
    public int textureWidth222
    {
        get
        {
            return m_textureWidth;
        }
        set
        {
            m_textureWidth = value;
            Debug.Log("zxy test :::");
            _UpdateMaterial();
        }
    }

    [SerializeField]
    private Color m_backgroundColor = Color.white;
    public Color backgroundColor
    {
        get
        {
            return m_backgroundColor;
        }
        set
        {
            m_backgroundColor = value;
            _UpdateMaterial();
        }
    }

    [SerializeField]
    private Color m_circleColor = Color.yellow;
    public Color circleColor
    {
        get
        {
            return m_circleColor;
        }
        set
        {
            m_circleColor = value;
            _UpdateMaterial();
        }
    }
    [SerializeField]
    private float m_blurFactor = 2.0f;
    public float blurFactor
    {
        get
        {
            return m_blurFactor;
        }
        set
        {
            m_blurFactor = value;
            _UpdateMaterial();
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (material == null)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.Log("Cannot find a renderer");
            }
            //从脚本所在的物体上获得材质
            material = renderer.sharedMaterial;
        }
        _UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //为材质生成程序纹理
    public void _UpdateMaterial()
    {
        if (material != null)
        {
            m_generatedTexture = _GenerateProceduralTexture();
            material.SetTexture("_MainTex", m_generatedTexture);
        }
    }

    //生成程序纹理
    private Texture2D _GenerateProceduralTexture()
    {
        //新建一张纹理
        Texture2D proceduralTexture = new Texture2D(textureWidth222, textureWidth222);
        //圆与圆之间的距离
        float circleInterval = textureWidth222 / 4.0f;
        float radius = textureWidth222 / 10.0f;
        //模糊系数
        float edgeBlur = 1.0f / m_blurFactor;

        //为纹理中的每个像素计算颜色
        for (int w = 0; w < textureWidth222; w++)
        {
            for (int h = 0; h < textureWidth222; h++)
            {
                Color pixel = backgroundColor;
                //通过与九个圆心的距离进行计算来求得这个像素的颜色信息
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //获得圆形的中心坐标
                        Vector2 circleCenter = new Vector2(circleInterval * (i + 1), circleInterval * (j + 1));
                        //计算该像素到每个中心坐标的距离与半径做比较，如果小于0，则是圆形内的像素，反之则不是
                        float dist = Vector2.Distance(new Vector2(w, h), circleCenter) - radius;
                        //Mathf.SmoothStep平滑差值，根据离圆的距离和模糊系数计算得到颜色的混合差值，计算得到最后的颜色
                        //如果dist * edgeBlur的结果小于0，则背景颜色的系数为0，全部为圆形的颜色，因为这时候在圆形内。
                        //如果dist * edgeBlur的结果大于1，则背景颜色的系数为1，全部为背景的颜色，因为这时候在圆形外。
                        //如果dist * edgeBlur的结果在[0,1]之间，则说明在边缘处，所以为两个颜色的混合。
                        Color color = _MixColor(circleColor, new Color(pixel.r, pixel.g, pixel.b, 0.0f), Mathf.SmoothStep(0f, 1.0f, dist * edgeBlur));
                        //与背景颜色混合
                        pixel = _MixColor(pixel, color, color.a);
                    }
                }
                //计算好的颜色写入纹理中
                proceduralTexture.SetPixel(w, h, pixel);
            }
        }
        //应用纹理颜色的更改设置
        proceduralTexture.Apply();
        //返回纹理
        return proceduralTexture;
    }

    private Color _MixColor(Color color0, Color color1, float mixFactor)
    {
        Color mixColor = Color.white;
        //取的第一个颜色的值和第二个颜色的值之间的一个值，由第三个参数影响
        mixColor.r = Mathf.Lerp(color0.r, color1.r, mixFactor);
        mixColor.g = Mathf.Lerp(color0.g, color1.g, mixFactor);
        mixColor.b = Mathf.Lerp(color0.b, color1.b, mixFactor);
        mixColor.a = Mathf.Lerp(color0.a, color1.a, mixFactor);
        return mixColor;
    }
}

