using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZxyVectorTest : MonoBehaviour
{
    public GameObject obj;
    public Vector3 ee;
    // Start is called before the first frame update
    void Start()
    {
        //var qua = Quaternion.identity;
        //qua.SetFromToRotation(Vector3.right,Vector3.up);
        //obj.transform.rotation = qua;

        //obj.transform.rotation.SetFromToRotation(Vector3.right, Vector3.up);
        //obj.transform.position.Set(1,1,1);


        //var rotation = obj.transform.rotation;
        //Quaternion qr = Quaternion.identity;
        //qr.SetFromToRotation(Vector3.forward, -Vector3.forward);
        //obj.transform.rotation *= qr;

        //Quaternion qr = Quaternion.identity;
        //qr.SetLookRotation(new Vector3(1, 3, 1));
        
        //obj.transform.rotation = qr;
    }

    // Update is called once per frame
    void Update()
    {





        //Vector3 di = new Vector3(0, 0, 1);

        //var dir = rotation * di;
        //Debug.Log(dir);

        //var eulerAngles = obj.transform.rotation.eulerAngles;
        //Debug.Log("zxy : rotation : " + eulerAngles);
        obj.transform.rotation = Quaternion.Euler(ee);
        var eulerAngles = obj.transform.rotation.eulerAngles;
        Debug.Log(eulerAngles);



    }
}
