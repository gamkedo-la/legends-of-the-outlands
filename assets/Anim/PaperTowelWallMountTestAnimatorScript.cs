using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PaperTowelWallMountTestAnimatorScript : MonoBehaviour
{
    private Animator m_anim;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }


    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.T))
        {
            m_anim.SetTrigger("Open wall mount");
            print("Release paper towel holder");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            m_anim.SetTrigger("Reset wall mount");
            print("Reset paper towel holder");
        }
    }
}
