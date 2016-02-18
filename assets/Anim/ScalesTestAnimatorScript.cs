using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ScalesTestAnimatorScript : MonoBehaviour
{
    private Animator m_anim;
    public Collider plate1Container, plate2Container;
    public float weight1, weight2;

	void Start()
    {
        m_anim = GetComponent<Animator>();
	}


	void Update()
    {
        if (weight1 > weight2 + 0.1f)
        {
            print("Plate 1 down");
            m_anim.SetTrigger("Plate 1 down");
        }

        if (weight2 > weight1 + 0.1f)
        {
            print("Plate 2 down");
            m_anim.SetTrigger("Plate 2 down");
        }

        if (weight1 >= weight2 - 0.1f && weight2 >= weight1 - 0.1f)
        {
            print("Plates level");
            m_anim.SetTrigger("Level");
        }
    }
}
