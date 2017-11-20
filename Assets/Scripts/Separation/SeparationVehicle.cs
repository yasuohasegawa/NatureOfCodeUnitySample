using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based from http://natureofcode.com/book/chapter-6-autonomous-agents/
public class SeparationVehicle : MonoBehaviour {
	private Vector3 location;
	private Vector3 velocity;
	private Vector3 acceleration;

	private float r;
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

		startX = Random.Range (-5f, 5f);
		startY = Random.Range (-5f, 5f);
		startZ = Random.Range (-5f, 5f);
		location = new Vector3(startX,startY,startZ);

		r = 1.2f;

		maxspeed = 0.3f;
		maxforce = 0.005f;

		this.gameObject.transform.localPosition = location;
	}

	public Vector3 Separate (List<SeparationVehicle> list) {
		float desiredseparation = r*2;
		Vector3 sum = Vector3.zero;
		int count = 0;
		// For every boid in the system, check if it's too close
		for (int i = 0; i<list.Count; i++) {
			SeparationVehicle other = list[i];
			float d = Vector3.Distance (this.gameObject.transform.localPosition, other.transform.localPosition);
			// If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
			if ((d > 0) && (d < desiredseparation)) {
				// Calculate vector pointing away from neighbor
				Vector3 diff = this.gameObject.transform.localPosition - other.transform.localPosition;
				diff.Normalize();
				diff /= d;        // Weight by distance
				sum += diff;
				count++;            // Keep track of how many
			}
		}
		// Average -- divide by how many
		if (count > 0) {
			// Our desired vector is moving away maximum speed
			//Vector3.Magnitude
			sum /= count;

			sum.Normalize ();
			sum *= maxspeed;
			// Implement Reynolds: Steering = Desired - Velocity
			sum = sum-velocity;
			sum = Vector3.ClampMagnitude(sum, maxforce);
		}
		return sum;
	}

	// Update is called once per frame
	void Update () {

	}

	public void applyBehaviors(List<SeparationVehicle> list) {
		Vector3 separateForce = Separate(list);
		Vector3 seekForce = Seek(new Vector3(0f,0f,0f));
		separateForce *= 2f;
		seekForce *= 1f;
		ApplyForce(separateForce);
		ApplyForce(seekForce); 
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
}
