using UnityEngine;
using System.Collections;

public class ConnetionTrail : MonoBehaviour {

	public GameObject From;
	public GameObject To;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (From == null || To == null) {
			Destroy(this.gameObject);
		}
	}
}
