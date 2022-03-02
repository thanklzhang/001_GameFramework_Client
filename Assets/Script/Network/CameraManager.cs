using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class CameraObject
{
    public Camera camera;
    Transform root;
    public void Init(Transform root)
    {
        this.root = root;
        camera = root.GetComponent<Camera>();
    }

    public void SetPosition(Vector3 pos)
    {
        root.position = pos;
    }

    public void SetRotation(Quaternion quaternion)
    {
        root.rotation = quaternion;
    }

    internal object GetPosition()
    {
        return this.root.position;
    }
}

//网络消息管理器 可以在这里收敛所有协议消息
public class CameraManager : Singleton<CameraManager>
{
    CameraObject cameraObj3D;
    CameraObject cameraObjUI;

    Transform cameraRoot;
    public void Init(Transform cameraRoot)
    {
        this.cameraRoot = cameraRoot;

        cameraObj3D = new CameraObject();
        var camera3DTran = cameraRoot.Find("Camera_3D");
        cameraObj3D.Init(camera3DTran);

        cameraObjUI = new CameraObject();
        var cameraUITran = cameraRoot.Find("Camera_UI");
        cameraObjUI.Init(cameraUITran);
    }

    public CameraObject GetCamera3D()
    {
        return cameraObj3D;
    }

    public CameraObject GetCameraUI()
    {
        return cameraObjUI;
    }

}
