using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingMain : MonoBehaviour {
	public GameObject cube;

	private Flock flock;

	// Use this for initialization
	void Start () {
		int max = 30;
		flock = new Flock();
		for (int i = 0; i < max; i++) {
			GameObject obj = null;
			obj = Instantiate (cube);
			flock.AddBoid (obj);
			obj.transform.parent = this.gameObject.transform;
		}
		cube.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		flock.Run ();
	}
}
