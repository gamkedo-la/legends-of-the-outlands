using UnityEngine;
using System.Collections;

public enum ScaleState { Plate1Down, Level, Plate2Down };


[RequireComponent(typeof(Animator))]
public class ScalesAnimatorScript : MonoBehaviour
{
    private Animator m_anim;
    public Collider plate1Container, plate2Container;
    public float weight1, weight2;
    public ScaleState currentState = ScaleState.Level;


    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

	public void WeightDelta(float weightAmt, int forSide) {
		if(forSide==1) {
			weight1 += weightAmt;
		} else {
			weight2 += weightAmt;
		}
		UpdatePos();
	}


    void UpdatePos()
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
		Debug.Log("ignoring changeWeight, WeightDelta triggers account for stacked objects");
        /*if (isPlate1)
        {
            weight1 += weight;
        }
        else
        {
            weight2 += weight;
        }*/
    }
}
