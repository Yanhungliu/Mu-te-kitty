using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpControll : MonoBehaviour
{

    private bool isHurt;
    private Animator anim;
    // Start is called before the first frame update


    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy Attack"))
        {
            isHurt = true;
            anim.SetBool("isHurt", isHurt);
            
        }
        else
        {
            isHurt = false;
            anim.SetBool("isHurt", isHurt);
        }
    }
}
