using UnityEngine;
using System.Collections;

public class RawlAbilityBehavior : MonoBehaviour {

	private Material[] startMat;
	public GameObject RawlAbilityObject;
	public float changeInterval = 0.33F;
	public Renderer rend;

	void Start () {
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		RawlAbilityObject = GameObject.FindWithTag("RawlAbilityObject");
	}

	void Update () {
		if (startMat.Length == 0)
			return;
		int index = Mathf.FloorToInt (Time.time / changeInterval);

		index = index % startMat.Length;

		if(Input.GetKeyDown(KeyCode.Q))
		{
			Material startMat = Resources.Load("HighlightMat",typeof(Material)) as Material;
		}
		if(Input.GetKeyUp(KeyCode.Q))
		{
			rend.startMat = startMat [index];	
	}
}
}
