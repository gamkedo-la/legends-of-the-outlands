using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ScalesTestAnimatorScript : MonoBehaviour
{
    private Animator m_anim;
    public Collider plate1Container, plate2Container;
    public float weight1, weight2;
    public ScaleState currentState = ScaleState.Level;

    public enum ScaleState{Plate1Down, Level, Plate2Down};

	void Start()
    {
        m_anim = GetComponent<Animator>();
    }


	void Update()
    {
        if (weight1 > weight2 + 0.1f && currentState != ScaleState.Plate1Down)
        {
            print("Plate 1 down");
            m_anim.SetTrigger("Plate 1 down");
            currentState = ScaleState.Plate1Down;
        }

        if (weight2 > weight1 + 0.1f && currentState != ScaleState.Plate2Down)
        {
            print("Plate 2 down");
            m_anim.SetTrigger("Plate 2 down");
            currentState = ScaleState.Plate2Down;
        }

        if (weight1 >= weight2 - 0.1f && weight2 >= weight1 - 0.1f && currentState != ScaleState.Level)
        {
            print("Plates level");
            m_anim.SetTrigger("Level");
            currentState = ScaleState.Level;
        }
    }

    public void changeWeight(bool isPlate1, float weight)
    {
        if(isPlate1){
            weight1 += weight;
        }
        else{
            weight2 += weight;
        }
    }
}
