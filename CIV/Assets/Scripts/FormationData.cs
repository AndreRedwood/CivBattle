using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FormationData
{
	public int Rows;
	public int Ranks;
	public float[] Density = new float[2];
	public float Frontage { get { return Rows * Density[0]; } }
	public float Depth { get { return Ranks * Density[1]; } }

	public FormationData(int rows, int ranks, float[] density)
	{
		Rows = rows;
		Ranks = ranks;
		Density[0] = density[0];
		Density[1] = density[1];
	}
}
