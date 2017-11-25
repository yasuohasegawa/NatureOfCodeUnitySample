using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
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

		startX = Random.Range (5f, 15f);
		startY = Random.Range (-1f, 1f);
		startZ = Random.Range (5f, 15f);

		location = new Vector3(startX,startY,startZ);

		maxspeed = Random.Range (0.1f, 0.3f);
		maxforce = Random.Range (0.001f, 0.01f);

		this.gameObject.transform.localPosition = location;
	}

	// Update is called once per frame
	void Update () {

	}

	public void Reset() {
		startX = Random.Range (5f, 15f);
		startY = Random.Range (-1f, 1f);
		startZ = Random.Range (5f, 15f);

		location = new Vector3(startX,startY,startZ);

		this.gameObject.transform.localPosition = location;
	}

	// We accumulate a new acceleration each time based on three rules
	public void Flock(List<Boid> boids) {
		Vector3 sep = Separate(boids);   // Separation
		Vector3 ali = Align(boids);      // Alignment
		Vector3 coh = Cohesion(boids);   // Cohesion
		// Arbitrarily weight these forces
		sep *= 1.5f;
		ali *= 1.0f;
		coh *= 1.0f;
		// Add the force vectors to acceleration
		ApplyForce(sep);
		ApplyForce(ali);
		ApplyForce(coh);
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
		
	public Vector3 Separate (List<Boid> list) {
		float desiredseparation = 2f;
		Vector3 sum = Vector3.zero;
		int count = 0;
		// For every boid in the system, check if it's too close
		for (int i = 0; i<list.Count; i++) {
			Boid other = list[i];
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

		if (count > 0) {
			sum /= (float)count;
		}

		// Average -- divide by how many
		if (sum.magnitude > 0) {

			sum.Normalize ();
			sum *= maxspeed;
			// Implement Reynolds: Steering = Desired - Velocity
			sum = sum-velocity;
			sum = Vector3.ClampMagnitude(sum, maxforce);
		}
		return sum;
	}


	// Alignment
	// For every nearby boid in the system, calculate the average velocity
	public Vector3 Align (List<Boid> list) {
		float neighbordist = 5f;
		Vector3 sum = Vector3.zero;
		int count = 0;
		for (int i = 0; i<list.Count; i++) {
			Boid other = list[i];
			float d = Vector3.Distance (this.gameObject.transform.localPosition, other.transform.localPosition);
			if ((d > 0) && (d < neighbordist)) {
				sum += other.velocity;
				count++;
			}
		}
		if (count > 0) {
			sum /= (float)count;
			sum.Normalize ();
			sum *= maxspeed;
			Vector3 steer = sum-velocity;
			steer = Vector3.ClampMagnitude(steer, maxforce);
			return steer;
		}

		return Vector3.zero;
	}


	// Cohesion
	// For the average position (i.e. center) of all nearby boids, calculate steering vector towards that position
	public Vector3 Cohesion (List<Boid> list) {
		float neighbordist = 5f;
		Vector3 sum = Vector3.zero;
		int count = 0;
		for (int i = 0; i<list.Count; i++) {
			Boid other = list[i];
			float d = Vector3.Distance (this.gameObject.transform.localPosition, other.transform.localPosition);
			if ((d > 0) && (d < neighbordist)) {
				sum += other.location; // Add position
				count++;
			}
		}
		if (count > 0) {
			sum /= (float)count;
			return Seek(sum);  // Steer towards the position
		}

		return Vector3.zero;
	}

	public void Borders() {
		float dist = 20f;
		float r = 2f;
		if (location.x < -r) {
			Reset ();
		}
		if (location.y < -r) {
			Reset ();
		}
		if (location.z < -r) {
			Reset ();
		}
		if (location.x > dist) {
			Reset ();
		}
		if (location.y > r) {
			Reset ();
		}
		if (location.z > dist) {
			Reset ();
		}
	}
}
