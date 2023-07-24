using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using Rect = Battle.Rect;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class zxyTest : MonoBehaviour
{
    public float width;
    public float height;

    public Vector2 widthDir;
    public Vector2 heightDir;

    public Vector2 rect_center;

    public Vector2 circle_center;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        m_Color = new Color(1, 1, 1, 0);
    }

    public bool isCollision;


    void OnDrawGizmos()
    { 
        DrawRect();
        DrawCircle();
        //DrawTest();
    }

    public void DrawTest()
    {
        Vector2 pos0 = new Vector2(0,0);
        Vector2 pos1 = new Vector2(1,1);
        Debug.DrawLine(pos0,pos1);

        var s = Vector3.Cross(pos1,new Vector3(0,0,1));
        
        Debug.DrawLine(pos0,s);
    }

    public UnityEngine.Vector2 ToVector2(Battle.Vector2 v)
    {
        return new UnityEngine.Vector2(v.x, v.y);
    }

    // Update is called once per frame
    void DrawRect()
    {
        Rect rect = new Rect()
        {
            center = new Battle.Vector2(rect_center.x, rect_center.y),
            height = height,
            heightDir = new Battle.Vector2(heightDir.x, heightDir.y),
            width = width,
            widthDir = new Battle.Vector2(widthDir.x, widthDir.y)
        };
        rect.heightDir = rect.heightDir.normalized;
        rect.widthDir = rect.widthDir.normalized;

        Circle circle = new Circle()
        {
            radius = radius,
            center = new Battle.Vector2(circle_center.x, circle_center.y)
        };


        isCollision = CollisionTool.CheckBoxAndCircle(rect, circle);

        var left_down =
            ToVector2(rect.center + -rect.widthDir * rect.width / 2.0f + -rect.heightDir * rect.height / 2.0f);
        var right_down =
            ToVector2(rect.center + rect.widthDir * rect.width / 2.0f + -rect.heightDir * rect.height / 2.0f);
        var right_up = ToVector2(rect.center + rect.widthDir * rect.width / 2.0f + rect.heightDir * rect.height / 2.0f);
        var left_up = ToVector2(rect.center + -rect.widthDir * rect.width / 2.0f + rect.heightDir * rect.height / 2.0f);


        Debug.DrawLine(left_down, right_down, Color.green);
        Debug.DrawLine(right_down, right_up, Color.green);
        Debug.DrawLine(right_up, left_up, Color.green);
        Debug.DrawLine(left_up, left_down, Color.green);
    }

    public Transform m_Transform;

    //public float m_Radius = 1; // 圆环的半径
    public float m_Theta = 0.1f; // 值越低圆环越平滑
    public Color m_Color = Color.green; // 线框颜色


    void DrawCircle()
    {
        if (m_Transform == null) return;
        if (m_Theta < 0.0001f) m_Theta = 0.0001f;

        // 设置矩阵
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = m_Transform.localToWorldMatrix;

        // 设置颜色
        Color defaultColor = Gizmos.color;
        Gizmos.color = m_Color;

        // 绘制圆环
        Vector2 beginPoint = Vector2.zero;
        Vector2 firstPoint = Vector2.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 endPoint = new Vector2(x, y);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint + circle_center, endPoint + circle_center);
            }

            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint + circle_center, beginPoint + circle_center);

        // 恢复默认颜色
        Gizmos.color = defaultColor;

        // 恢复默认矩阵
        Gizmos.matrix = defaultMatrix;
    }
}