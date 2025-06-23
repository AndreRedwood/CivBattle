using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Unit : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> models = new List<GameObject>();
	public int ModelCount { get { return models.Count; } }
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

	[SerializeField]
	private float rotationAngle = 0f;

	[SerializeField]
	private Vector2[] corners = new Vector2[4];

	public void Initialize(GameObject modelPrefab, int modelCount, int formationDepht)
	{
		//throw it out to separate class & function
		outline = new GameObject("Outline").AddComponent<LineRenderer>();
		outline.gameObject.layer = LayerMask.NameToLayer("Overlays");
		outline.transform.SetParent(transform);
		outline.gameObject.SetActive(false); //maybe just renderer off?
		//--

		//move it to better place
		formation = new FormationData(FormationData.CalculateRows(modelCount, formationDepht),
			formationDepht, new float[2] { 1.5f, 3 });

		unitHitBox = gameObject.AddComponent<BoxCollider>();
		CalculateHitBox();

		//make it first
		for (int i = 0; i < modelCount; i++)
		{
			GameObject model = GameObject.Instantiate(modelPrefab);
			model.transform.SetParent(transform);
			models.Add(model);
		}

		FormUp(ModelCount, Rows, Ranks);
	}

	private void CalculateHitBox()
	{
		unitHitBox.size = new Vector3(Ranks * Density[1], 2, Rows * Density[0]);
		unitHitBox.center = new Vector3(-0.5f * (Ranks - 1) * Density[1], 1, 0f);
		//separate this
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
		int fullRanks = modelCount % ranks == 0 ? ranks : ranks - 1;

		List<Vector2> result = new List<Vector2>();

		if (fullRanks > 0)
		{
			Vector2[,] positions = new Vector2[fullRanks, rows];

			positions[0, 0] = new Vector2((Frontage / 2) - (Density[0] / 2), 0);

			for (int currentRank = 0; currentRank < fullRanks; currentRank++)
			{
				for (int placeInRank = 0; placeInRank < rows; placeInRank++)
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

			for (int placeInRank = 0; placeInRank < lefoverModels; placeInRank++)
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

		if (models.Count != result.Count)
		{
			throw new ArgumentException("Number of models and positions doesn't match.");
		}

		for (int i = 0; i < models.Count; i++)
		{
			models[i].transform.position = new Vector3(result[i].x, 0, result[i].y);
		}
		//Generate Formation reworked here
	}
}
