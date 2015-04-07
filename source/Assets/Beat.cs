using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Beat : MonoBehaviour {

	private GameObject previousTarget;
	public GameObject target;

	public float Speed;

	public int group = -1;

	void Start () {
		//transform.position = StartTone.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy(this.gameObject);
			return;
		}

		var dist = target.transform.position - transform.position;
		if (dist.magnitude < 0.1f) {
			transform.position = target.transform.position;

			var targetTone = target.GetComponent<Tone>();			

			targetTone.Play(group);

			GameObject newTarget = null;

			foreach (var con in targetTone.LocalConnections) {
				if (con.From == target.gameObject && (targetTone.LocalConnections.Count == 1 || con.To != previousTarget)) {
					newTarget = con.To;
				}
			}

			previousTarget = target;			
			target = newTarget;
		} else {
			transform.position += dist.normalized * Speed * Time.deltaTime;
		}
	}

	GameObject GetNextTarget(GameObject currentTarget, GameObject previousTarget) {
		var radius = 5;
		var step = 1;

		var origin = currentTarget.transform.position;
		var target = previousTarget.transform.position;

		for (int i = 0; i < 360; i += step) {
			var hits = Physics.RaycastAll(origin, target - origin, radius);
			var _hits = from hit in hits
						where hit.collider.gameObject != currentTarget && hit.collider.gameObject != previousTarget
						select hit;

			if (_hits.Count() > 0) {
				return _hits.First().collider.gameObject;
			} else {
				target -= new Vector3(Mathf.Cos(i) * radius, Mathf.Sin(i) * radius);
			}						
		}

		return null;
	}
}
