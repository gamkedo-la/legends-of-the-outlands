using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ScalesTestAnimatorScript : MonoBehaviour
{
    private Animator m_anim;


	void Start()
    {
        m_anim = GetComponent<Animator>();
	}


	void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            print("Plate 1 down");
            m_anim.SetTrigger("Plate 1 down");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Plate 2 down");
            m_anim.SetTrigger("Plate 2 down");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print("Plates level");
            m_anim.SetTrigger("Level");
        }
    }
}
