using FixedPointy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject :Singleton<GlobalObject>
{
    public Transform UIRoot { get; set; }
    public GameObject gameStartupPrefab { get; set; }
}
