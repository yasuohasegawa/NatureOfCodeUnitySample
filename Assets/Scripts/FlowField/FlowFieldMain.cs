using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldMain : MonoBehaviour {
	public GameObject cube;

	private List<FlowFieldVehicle> vehicles = new List<FlowFieldVehicle> ();

	private FlowField flowfield;

	// Use this for initialization
	void Start () {
		int max = 25;
		flowfield = new FlowField(1f);
		for (int i = 0; i < max; i++) {
			GameObject obj = null;
			obj = Instantiate (cube);

			FlowFieldVehicle c = obj.AddComponent<FlowFieldVehicle> ();
			vehicles.Add (c);
			obj.transform.parent = this.gameObject.transform;
		}
		cube.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		for(int i = vehicles.Count-1; i>=0; i--){
			vehicles[i].follow(flowfield);
			vehicles[i].UpdatePosition ();
			vehicles [i].borders ();
		}

		if (Input.GetMouseButtonDown(0)){
			flowfield.init ();
			Debug.Log ("down");
		}
	}
}
