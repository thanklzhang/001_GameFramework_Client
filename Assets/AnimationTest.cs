using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public Animator ani;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ss()
    {
        yield return null;
        // yield return new WaitForSeconds(0.1f);
        ani.SetTrigger("walk");
        yield return null;
        // yield return new WaitForSeconds(0.1f);
        ani.SetTrigger("idle");
        yield return null;
        // yield return new WaitForSeconds(0.1f);
        ani.SetTrigger("attack");
        
        
        
        // ani.SetInteger("Action",1);
        // ani.SetInteger("Action",2);
        // ani.SetInteger("Action",3);
        
        //ani.SetBool("walk_bool",true);
        // ani.SetInteger("Action",2);
        // ani.SetInteger("Action",3);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ss());
           
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ani.SetTrigger("skill_1");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ani.SetTrigger("die");
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            ani.SetTrigger("idle");
        }

        ani.speed = speed;
    }
}