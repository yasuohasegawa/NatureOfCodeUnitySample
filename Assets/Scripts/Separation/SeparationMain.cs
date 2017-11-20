using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based from http://natureofcode.com/book/chapter-6-autonomous-agents/
public class SeparationMain : MonoBehaviour {
	public GameObject cube;

	private List<SeparationVehicle> vehicles = new List<SeparationVehicle> ();

	// Use this for initialization
	void Start () {
		int max = 25;
		for (int i = 0; i < max; i++) {
			GameObject obj = null;
			obj = Instantiate (cube);

			SeparationVehicle c = obj.AddComponent<SeparationVehicle> ();
			vehicles.Add (c);
			obj.transform.parent = this.gameObject.transform;
		}
		cube.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		for(int i = vehicles.Count-1; i>=0; i--){
			vehicles[i].applyBehaviors(vehicles);
			vehicles[i].UpdatePosition ();
		}
	}

}
