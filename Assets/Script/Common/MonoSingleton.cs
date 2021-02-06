using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour
    where T: MonoSingleton<T>
{

    protected static T s_instance = null;
    public static T Instance
    {
        get
        {
            return s_instance;
        }
    }
    protected virtual void Awake()
    {
      s_instance=(T)this;
    }
    protected virtual void Start()
    {

    }
    protected virtual void OnDestroy()
    {
        
    }
   
}
