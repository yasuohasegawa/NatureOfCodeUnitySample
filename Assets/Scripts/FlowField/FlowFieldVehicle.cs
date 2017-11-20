using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldVehicle : MonoBehaviour {
	private Vector3 location;
	private Vector3 velocity;
	private Vector3 acceleration;

	private float maxforce;
	private float maxspeed;

	private float startX = -3f;
	private float startY = -3f;
	private float startZ = -3f;

	private float nowDeg = 0f;

	// Use this for initialization
	void Start () {
		acceleration = new Vector3(0f,0f,0f);
		velocity = new Vector3(0f,0f,0f);

		startX = Random.Range (10f, 19f);
		startY = Random.Range (-1f, 1f);
		startZ = Random.Range (2f, 18f);

		location = new Vector3(startX,startY,startZ);

		maxspeed = Random.Range (0.1f, 0.3f);
		maxforce = Random.Range (0.001f, 0.01f);

		this.gameObject.transform.localPosition = location;
	}

	// Update is called once per frame
	void Update () {

	}

	// Implementing Reynolds' flow field following algorithm
	// http://www.red3d.com/cwr/steer/FlowFollow.html
	public void follow(FlowField flow) {
		// What is the vector at that spot in the flow field?
		Vector3 desired = flow.lookup(this.gameObject.transform.localPosition);
		// Scale it up by maxspeed
		desired *= maxspeed;
		// Steering is desired minus velocity
		Vector3 steer = desired-velocity;
		steer = Vector3.ClampMagnitude(steer, maxforce);
		ApplyForce(steer);
	}

	public void UpdatePosition() {
		velocity += acceleration;

		velocity = Vector3.ClampMagnitude(velocity, maxspeed);

		location += velocity;

		float heading = Mathf.Atan2(velocity.x, velocity.z);
		float theta = heading + Mathf.PI/2f;

		nowDeg += ((Mathf.Rad2Deg * theta) - nowDeg) * 0.01f;

		this.gameObject.transform.eulerAngles = new Vector3 (nowDeg, nowDeg, nowDeg);

		this.gameObject.transform.localPosition = location;

		acceleration *= 0f;
	}

	public void ApplyForce(Vector3 force) {
		acceleration += force;
	}

	public Vector3 Seek(Vector3 target) {
		Vector3 desired = target - location;
		desired.Normalize ();
		desired *= maxspeed;

		Vector3 steer = desired-velocity;

		steer = Vector3.ClampMagnitude(steer, maxforce);

		return steer;
	}

	public void borders() {
		float dist = 20f;
		if (location.x < 0f) {
			location.x = dist;
		}
		if (location.z < 0f) {
			location.z = dist-2f;
		}
		if (location.x > dist) {
			location.x = -dist;
		}
		if (location.z > dist) {
			location.z = -dist+2f;
		}

	}

}
