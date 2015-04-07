using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Connection {
	public GameObject From;
	public GameObject To;

	public Vector3 FromPos;
	public Vector3 ToPos;

	public Vector3 Direction;

	public LineRenderer lr;

	public Connection(GameObject from, GameObject to, LineRenderer lr) {
		Direction = (to.transform.position - from.transform.position).normalized;

		From = from;
		To = to;

		FromPos = From.transform.position + Direction * 0.1f;
		ToPos = To.transform.position - Direction * 0.1f;

		this.lr = lr;
	}

	public void Update() {
		FromPos = From.transform.position + Direction * 0.1f;
		ToPos = To.transform.position - Direction * 0.1f;
		
		lr.SetPosition(0, FromPos);
		lr.SetPosition(1, ToPos);
	}
}

public class Connections : MonoBehaviour {

	public List<Connection> Cons;

	public GameObject ConnectionRenderer;

	// Use this for initialization
	void Start () {
		Cons = new List<Connection>();
	}
	
	// Update is called once per frame
	void Update () {
		Cons.RemoveAll(x => x.To == null || x.From == null);
		
		//var spheres = from s in GameObject.FindGameObjectsWithTag("Sphere")
		//			  orderby s.transform.position.x ascending
		//			  select s;

		//var connected = new List<GameObject>();

		//foreach (var s in spheres) {

		//	if (connected.Contains(s)) {
		//		continue;
		//	}


		//	var far = GameObject.Find("FarAway");
		//	GameObject closest = far;
		//	GameObject secondClosest = far;
		//	foreach (var s2 in spheres) {
		//		if (s == s2) continue;

		//		if (connected.Contains(s2)) {
		//			continue;
		//		}

		//		var cDist = (s.transform.position - closest.transform.position).magnitude;
		//		var scDist = (s.transform.position - secondClosest.transform.position).magnitude;
		//		var newDist = (s.transform.position - s2.transform.position).magnitude;

		//		if (newDist < cDist) {
		//			secondClosest = closest;
		//			closest = s2;
		//		} else if (newDist < scDist) {
		//			secondClosest = s2;
		//		}
		//	}

		//	if (closest != far && secondClosest != far) {
		//		AddConnection(s, closest);
		//		AddConnection(s, secondClosest);

		//		AddConnection(closest, secondClosest);

		//		connected.Add(s);
		//		connected.Add(closest);
		//		connected.Add(secondClosest);
		//	}

		//}	

		//var toRemove = new List<Connection>();

		//foreach (var con in Cons) {
		//	foreach (var con2 in Cons) {

		//		var li = LineIntersectionPoint(con.FromPos, con.ToPos,
		//										con2.FromPos, con2.ToPos);				

		//		if (li != Vector3.zero) {
					

		//			if (LineSegmentCheck(li, con.FromPos, con.ToPos) && LineSegmentCheck(li, con2.FromPos, con2.ToPos)) {
		//				//Debug.DrawLine(con.FromPos, con.ToPos, Color.red);
		//				//Debug.DrawLine(con2.FromPos, con2.ToPos, Color.red);
		//				//Debug.DrawLine(li, li + Vector3.up / 3, Color.green);

		//				toRemove.Add(con);
		//				toRemove.Add(con2);
		//			}
		//		}
		//	}
		//}

		//foreach (var con in toRemove) {
		//	Cons.Remove(con);

		//	con.From.GetComponent<Tone>().LocalConnections.Remove(con);
		//}

		foreach (var con in Cons) {
			con.Update();

			//con.From.GetComponent<Tone>().Next = con.To.GetComponent<Tone>();
			//con.To.GetComponent<Tone>().Next = con.From.GetComponent<Tone>();

			Debug.DrawLine(con.FromPos, con.ToPos);
		}

		//Cons.Clear();
	}

	void LateUpdate() {

	}

	public Connection AddConnection(GameObject a, GameObject b) {

		var addFirst = true;
		var addSecond = true;
		foreach (var con in Cons) {
			if ((con.From == a && con.To == b)) {
				addFirst = false;
			}

			if ((con.From == b && con.To == a)) {
				addSecond = false;
			}
		}

		if (addFirst) {
			var lr = (Instantiate(ConnectionRenderer, a.transform.position, Quaternion.identity) as GameObject).GetComponent<LineRenderer>();

			var lrComp = lr.GetComponent<ConnetionTrail>();
			lrComp.From = a;
			lrComp.To = b;

			var newCon = new Connection(a, b, lr);
			Cons.Add(newCon);

			a.GetComponent<Tone>().LocalConnections.Add(newCon);
		}


		if (addSecond) {
			var lr = (Instantiate(ConnectionRenderer, b.transform.position, Quaternion.identity) as GameObject).GetComponent<LineRenderer>();

			var lrComp = lr.GetComponent<ConnetionTrail>();
			lrComp.From = b;
			lrComp.To = a;

			var newCon2 = new Connection(b, a, lr);
			Cons.Add(newCon2);
			
			b.GetComponent<Tone>().LocalConnections.Add(newCon2);
		}

		return null;
	}

	bool LineSegmentCheck(Vector3 intersection, Vector3 point1, Vector3 point2) {
		var result = false;
		if (Mathf.Min (point1.x, point2.x) <= intersection.x && Mathf.Max (point1.x, point2.x) >=  intersection.x && 
			Mathf.Min (point1.y, point2.y) <= intersection.y && Mathf.Max (point1.y, point2.y) >= intersection.y)
		{
			result = true;
		}
 
		return result;
	}

	Vector3 LineIntersectionPoint(Vector3 startA, Vector3 endA, Vector3 startB, Vector3 endB) {
		// Get A,B,C of first line - points : ps1 to pe1
		float A1 = endA.y - startA.y;
		float B1 = startA.x - endA.x;
		float C1 = A1 * startA.x + B1 * startA.y;

		// Get A,B,C of second line - points : ps2 to pe2
		float A2 = endB.y - startB.y;
		float B2 = startB.x - endB.x;
		float C2 = A2 * startB.x + B2 * startB.y;

		// Get delta and check if the lines are parallel
		float delta = A1 * B2 - A2 * B1;
		if (delta == 0)
			return Vector3.zero;

		// now return the Vector2 intersection point
		return new Vector3(
			(B2 * C1 - B1 * C2) / delta,
			(A1 * C2 - A2 * C1) / delta
		);
	}
}
