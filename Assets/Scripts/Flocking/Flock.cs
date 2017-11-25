using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock {

	public List<Boid> boids;

	public Flock() {
		boids = new List<Boid> ();
	}

	public void Run() {
		for(int i = 0; i<boids.Count; i++) {
			boids [i].Flock (boids);
			boids [i].UpdatePosition ();
			boids [i].Borders ();
		}
	}

	public void AddBoid(GameObject target) {
		Boid b = target.AddComponent<Boid> ();
		boids.Add (b);
	}

}
