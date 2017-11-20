using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField {
	// A flow field is a two dimensional array of PVectors
	private Vector3[,] field;
	private int cols, rows; // Columns and Rows
	private float resolution; // How large is each "cell" of the flow field

	public FlowField(float r) {
		resolution = r;
		float w = 20f;
		float h = 20f;
		// Determine the number of columns and rows based on sketch's width and height
		cols = (int)(w/resolution);
		rows = (int)(h/resolution);
		field = new Vector3[cols,rows];
		init();
	}

	public void init() {
		// Reseed noise so we get a new flow field every time
		//noiseSeed((int)random(10000));
		float xoff = 0f;
		for (int i = 0; i < cols; i++) {
			float yoff = 0f;
			for (int j = 0; j < rows; j++) {
				//Mathf.PerlinNoise
				float theta = Map(Mathf.PerlinNoise(xoff,yoff),0f,1f,0f,2f*Mathf.PI);
				// Polar to cartesian coordinate transformation to get x and y components of the vector
				field[i,j] = new Vector3(Mathf.Cos(theta),Mathf.Tan(theta)*0.01f,Mathf.Sin(theta));
				yoff += 0.1f;
			}
			xoff += 0.1f;
		}
	}

	public Vector3 lookup(Vector3 lookup) {
		int column = (int)constrain(lookup.x/resolution,0,cols-1);
		int row = (int)constrain(lookup.z/resolution,0,rows-1);

		return field[column,row];
	}

	public float Map(float x, float in_min, float in_max, float out_min, float out_max) {
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

	public int constrain(float val, int min, int max) {
		int res = Mathf.Clamp((int)val,min,max);
		return res;
	}
}
