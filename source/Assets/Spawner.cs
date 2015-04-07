using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public GameObject SpherePrefab;
	public Dictionary<int, GameObject> Spheres;
	void Start () {
		Spheres = new Dictionary<int, GameObject>();
	}

	

	// Update is called once per frame
	void Update () {
		//if (Input.GetMouseButtonDown(0)) {
		//	var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	pos.z = 0;

		//	var sp = Instantiate(SpherePrefab, pos, Quaternion.identity) as GameObject;
		//}

		foreach (var touch in Input.touches) {
			var pos = Camera.main.ScreenToWorldPoint(touch.position);
			pos.z = 0;
			if (touch.phase == TouchPhase.Began) {
				var sp = Instantiate(SpherePrefab, pos, Quaternion.identity) as GameObject;
				if (Spheres.ContainsKey(touch.fingerId)) {
					Spheres.Remove(touch.fingerId);
				}

				Spheres.Add(touch.fingerId, sp);
			} else if (touch.phase == TouchPhase.Moved) {
				Spheres[touch.fingerId].transform.position = pos;
			} else if (touch.phase == TouchPhase.Ended) {
				Destroy(Spheres[touch.fingerId]);
				Spheres.Remove(touch.fingerId);
			}
		}
	}
}
