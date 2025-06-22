using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> models = new List<GameObject>();
	[SerializeField]
	private BoxCollider unitHitBox;
	[SerializeField]
	private LineRenderer outline;

	[SerializeField]
	private FormationData formation;

	private int Ranks { get { return formation.Ranks; } }
	private int Rows { get { return formation.Rows; } }
	private float Frontage { get { return formation.Frontage; } }
	private float[] Density { get { return formation.Density; } }

	public void Initialize(GameObject modelPrefab, int modelCount, FormationData formation)
	{
		//here creating models and placing repositioning them
		//density[0] - row gap, density[1] - rank gap
		unitHitBox = this.AddComponent<BoxCollider>();
		outline = new GameObject("Outline").AddComponent<LineRenderer>();
		outline.gameObject.layer = LayerMask.NameToLayer("Overlays");
		outline.transform.SetParent(transform);
		this.formation = formation;
		CalculateHitBox();
		outline.gameObject.SetActive(false);

		for (int i = 0; i < modelCount; i++)
		{
			GameObject model = GameObject.Instantiate(modelPrefab);
			model.transform.SetParent(transform);
			models.Add(model);
		}

		FormUp(models.Count, Rows, Ranks);
	}

	private void CalculateHitBox()
	{
		unitHitBox.size = new Vector3(Ranks * Density[1], 2, Rows * Density[0]);
		unitHitBox.center = new Vector3(-0.5f * (Ranks - 1) * Density[1], 1, 0f);

		CalculateOutline();
	}

	private void CalculateOutline()
	{
		outline.transform.position = new Vector3(Density[1] * 0.5f, 0.1f, 0);
		outline.transform.eulerAngles = new Vector3(90, 0, 0);
		outline.useWorldSpace = false;
		outline.loop = true;
		outline.alignment = LineAlignment.TransformZ;
		Vector3[] corners = new Vector3[4] {
			new Vector3(0,(Rows * Density[0]) * 0.5f,0),
			new Vector3(0,(Rows * Density[0]) * -0.5f,0),
			new Vector3(-Ranks * Density[1],(Rows * Density[0]) * -0.5f,0),
			new Vector3(-Ranks * Density[1],(Rows * Density[0]) * 0.5f,0)
		};
		outline.widthMultiplier = 0.4f;
		outline.positionCount = 4;
		outline.SetPositions(corners);
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

	public void SetPosition(Vector3 position, float rotation)
	{
		transform.position = position;
		transform.Rotate(new Vector3(0, rotation, 0));
	}

	private void FormUp(int modelCount, int rows, int ranks)
	{
		int fullRanks = modelCount % ranks == 0 ? fullRanks = ranks : fullRanks = ranks - 1;

		List<Vector2> result = new List<Vector2>();

		if (fullRanks > 0)
		{
			Vector2[,] positions = new Vector2[fullRanks, rows];

			positions[0, 0] = new Vector2((Frontage / 2) - (Density[0] / 2),0);

			for(int currentRank = 0; currentRank < fullRanks; currentRank++)
			{
				for(int placeInRank = 0; placeInRank < rows;  placeInRank++) 
				{
					positions[currentRank, placeInRank] = new Vector2(
						-currentRank * Density[1],
						(placeInRank * Density[0]) - (Frontage * 0.5f) + (Density[0] * 0.5f));
				}
			}
			foreach (Vector2 position in positions)
			{
				result.Add(position);
			}
		}
		Vector2[] lastRankPositions = null;
		if (modelCount % rows != 0)
		{
			int lefoverModels = modelCount % rows;
			float lastRankFrontage = lefoverModels * Density[0];
			lastRankPositions = new Vector2[lefoverModels];

			for(int placeInRank = 0; placeInRank < lefoverModels; placeInRank++)
			{
				lastRankPositions[placeInRank] = new Vector2(
					-fullRanks * Density[1],
					(placeInRank * Density[0]) - (lastRankFrontage * 0.5f) + (Density[0] * 0.5f));
			}
			foreach (Vector2 position in lastRankPositions) 
			{
				result.Add(position);
			}
		}

		if(models.Count != result.Count)
		{
			throw new ArgumentException("Number of models and positions doesn't match.");
		}
		
		for(int i  = 0; i < models.Count; i++) 
		{
			models[i].transform.position = new Vector3(result[i].x, 0, result[i].y);
		}
		//Generate Formation reworked here
	}
}
