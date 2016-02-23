using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class WindowTestAnimatorScript : MonoBehaviour
{
    private Animator m_anim;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_anim.SetTrigger("Open window");
            print("Open window");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            m_anim.SetTrigger("Close window");
            print("Close window");
        }
    }
}
