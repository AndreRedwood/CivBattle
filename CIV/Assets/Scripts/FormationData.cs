using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
		Density[0] = density[0]; //gap between units
		Density[1] = density[1]; //gap between ranks
	}

	public static int CalculateRows(int modelCount, int ranks)
	{
		int result = 0;
		if (modelCount % ranks == 0)
		{
			result = modelCount / ranks;
		}
		else
		{
			int i = 0;
			do
			{
				i++;
			} while (i * ranks < modelCount);
			result = i;
		}
		return result;
	}
}
