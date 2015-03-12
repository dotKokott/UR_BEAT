using UnityEngine;
using System.Collections;

public class Tone : MonoBehaviour {

	public Tone Next;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
