using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zxyTestMatrix : MonoBehaviour
{
    public float xAngle;
    public float yAngle;
    public float zAngle;

    public Vector4 curr;

    // Start is called before the first frame update
    void Start()
    {


        //var s = GetX() * GetY() * this.transform.forward;

    }
    
    // Update is called once per frame
    void Update()
    {
        return;
        float xRad = xAngle * Mathf.Deg2Rad;
        float yRad = yAngle * Mathf.Deg2Rad;
        float zRad = zAngle * Mathf.Deg2Rad;


        Matrix4x4 rotateX = new Matrix4x4(
           new Vector4(1, 0, 0, 0),
           new Vector4(0, Mathf.Cos(xRad), -Mathf.Sin(xRad), 0),
           new Vector4(0, Mathf.Sin(xRad), Mathf.Cos(xRad), 0),
           new Vector4(0, 0, 0, 1)).transpose;

        Matrix4x4 rotateY = new Matrix4x4(
           new Vector4(Mathf.Cos(yRad), 0, Mathf.Sin(yRad), 0),
           new Vector4(0, 1, 0, 0),
           new Vector4(-Mathf.Sin(yRad), 0, Mathf.Cos(yRad), 0),
           new Vector4(0, 0, 0, 1)).transpose;

        Matrix4x4 rotateZ = new Matrix4x4(
           new Vector4(Mathf.Cos(zRad), -Mathf.Sin(zRad), 0, 0),
           new Vector4(Mathf.Sin(zRad), Mathf.Cos(zRad), 0, 0),
           new Vector4(0, 0, 1, 0),
           new Vector4(0, 0, 0, 1)).transpose;
        Debug.Log(rotateZ);
        Debug.Log(rotateZ * rotateX);
        Debug.Log(rotateZ * rotateX * rotateY);
        var result = rotateY * rotateX * rotateZ * curr;

        Debug.Log("" + result);
        
    }

    public Matrix4x4 GetX()
    {
        Debug.Log("X");
        return new Matrix4x4();
    }

    public Matrix4x4 GetY()
    {
        Debug.Log("Y");
        return new Matrix4x4();
    }
}
