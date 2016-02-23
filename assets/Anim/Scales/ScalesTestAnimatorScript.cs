using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ScalesTestAnimatorScript : MonoBehaviour
{
    public ScaleState startState = ScaleState.Level;

    private Animator m_anim;


    void Start()
    {
        m_anim = GetComponent<Animator>();

        switch (startState)
        {
            case (ScaleState.Plate1Down):
                m_anim.SetTrigger("Plate 1 down");
                break;

            case (ScaleState.Plate2Down):
                m_anim.SetTrigger("Plate 2 down");
                break;

            case (ScaleState.Level):
                m_anim.SetTrigger("Level");
                break;
        }
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
