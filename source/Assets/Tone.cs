using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tone : MonoBehaviour {

	public Tone Next;
	private AudioSource source;

	private Connections connections;
	public List<Connection> LocalConnections = new List<Connection>();

	void Start () {
		source = GetComponent<AudioSource>();

		connections = GameObject.Find("Connections").GetComponent<Connections>();
	}
	
	// Update is called once per frame
	void Update () {
	
		//foreach (var tone in GameObject.FindGameObjectsWithTag("Sphere")) {
		//	if (tone != this.gameObject) {
		//		var dist = (transform.position - tone.transform.position).magnitude;
		//		if (dist < 5) {
		//			var con = connections.AddConnection(this.gameObject, tone);	
		//		}
				
		//	}
		//}
	}

	void LateUpdate() {
		//LocalConnections.Clear();
	}

	public void Play() {
		source.PlayOneShot(source.clip);
		//iTween.ScaleBy(this.gameObject, iTween.Hash(
		//	"amount", new Vector3(1.1f, 1.1f, 1.1f), 
		//	"time", 0.1f, 
		//	"looptype", iTween.LoopType.pingPong,
		//	"oncomplete", "OnComplete"
		//	));		
	}

	void OnComplete() {
		iTween.Stop(this.gameObject);
	}
}
