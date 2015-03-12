using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {

	public Tone StartTone;

	private Tone target;

	public float Speed;

	void Start () {
		transform.position = StartTone.transform.position;
		target = StartTone.Next;
	}
	
	// Update is called once per frame
	void Update () {
		var dist = target.transform.position - transform.position;
		if (dist.magnitude < 0.1f) {
			transform.position = target.transform.position;
			target.Play();

			target = target.Next;
		} else {
			transform.position += dist.normalized * Speed * Time.deltaTime;
		}
	}
}
