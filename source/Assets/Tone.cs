using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tone : MonoBehaviour {

	public GameObject BeatPrefab;

	public AudioClip Drum;
	public AudioClip[] Source1;
	public AudioClip[] Source2;
	public AudioClip[] Source3;
	public AudioClip[] Source4;

	public AudioSource source;

	private Connections connections;
	public List<Connection> LocalConnections = new List<Connection>();

	void Start () {
		source = GetComponent<AudioSource>();

		connections = GameObject.Find("Connections").GetComponent<Connections>();

		var spheres = from s in GameObject.FindGameObjectsWithTag("Sphere")
					  orderby (s.transform.position - this.transform.position).sqrMagnitude ascending
					  select s;

		foreach (var tone in spheres) {
			if (tone == this.gameObject) continue;
			
			var dist = (transform.position - tone.transform.position).magnitude;
			if (dist < 10) {
				var toneComp = tone.GetComponent<Tone>();
				var count = toneComp.LocalConnections.Count;
				if (toneComp.source.clip == Drum) {
					if (count == 0) {
						var con = connections.AddConnection(this.gameObject, tone);
					}					
				} else {
					if (count < 2) {
						var con = connections.AddConnection(this.gameObject, tone);
					}				
				}			
			}			
		}

		if (LocalConnections.Count > 0) {
			var beats = GameObject.FindGameObjectsWithTag("Beat");

			var weHaveABeat = false;
			foreach (var beat in beats) {				
				foreach (var con in LocalConnections) {
					var bComp = beat.GetComponent<Beat>();
					if (bComp.target == con.From || bComp.target == con.To) {
						weHaveABeat = true;
						
						source.clip = GetClipFromGroup(bComp.group);

						break;
					}
				}
			}

			if (!weHaveABeat) {
				var newBeat = Instantiate(BeatPrefab, transform.position, Quaternion.identity) as GameObject;

				var newBeatComp = newBeat.GetComponent<Beat>();

				if (spheres.Count() <= 2) {
					newBeatComp.group = -1;					
				} else {
					newBeatComp.group = Random.Range(0, 4);
				}

				source.clip = GetClipFromGroup(newBeatComp.group);

				newBeatComp.target = this.gameObject;
			}
		}
	}

	public AudioClip GetClipFromGroup(int group) {
		if (group == 0)
			return Source1[Random.Range(0, Source1.Length)];

		if (group == 1)
			return Source2[Random.Range(0, Source2.Length)];

		if (group == 2)
			return Source3[Random.Range(0, Source3.Length)];

		if (group == 3)
			return Source4[Random.Range(0, Source4.Length)];

		return Drum;
	}

	// Update is called once per frame
	void Update () {
		LocalConnections.RemoveAll(x => x.To == null || x.From == null);
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

	public void Delete() {
		Destroy(this.gameObject);
	}

	public void Play(int group) {
		if (source.clip == null) {
			source.clip = GetClipFromGroup(group);
		}

		source.PlayOneShot(source.clip);


		StartCoroutine(bump());
		//iTween.ScaleBy(this.gameObject, iTween.Hash(
		//	"amount", new Vector3(1.1f, 1.1f, 1.1f), 
		//	"time", 0.1f, 
		//	"looptype", iTween.LoopType.pingPong,
		//	"oncomplete", "OnComplete"
		//	));		
	}

	private IEnumerator bump() {

		var amount = 1.3f;
		var time = 0.3f;

		var originalScale = transform.localScale;
		iTween.ScaleBy(gameObject,
			iTween.Hash("amount", new Vector3(amount, amount, amount), "time", time, "easetype", iTween.EaseType.easeOutBack));

		yield return new WaitForSeconds(time);

		var backAmount = 1.0f - (amount - 1.0f);

		iTween.ScaleTo(gameObject,
			iTween.Hash("scale", originalScale, "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));

	}

	void OnComplete() {
		iTween.Stop(this.gameObject);
	}
}
