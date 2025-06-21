using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Unit
{
	[SerializeField]
	private Vector2 position = new Vector2(0, 0);
	public Vector2 Position { get { return position; } }
	[SerializeField]
	private float rotationAngle = 0f;

	[SerializeField]
	private FormationData formation;

	//public int UnitCount { get { return units.Count; } }
	public int UnitCount { get { return 60; } } //temporaty workaround

	[SerializeField]
	private int ranks;

	[SerializeField]
	private UnitManager mainPivot;
	[SerializeField]
	private List<GameObject> units;
	[SerializeField]
	Vector2[] corners;

	public Unit(GameObject unitPrefab, int unitCount, int formationDepth)
	{
		//temporary solution!
		GameObject mainPivotObject = new GameObject("Formation");
		//later rework into Soldiers instead of GameObjects
		units = new List<GameObject>();
		//for (int i = 0; i < unitCount; i++)
		//{
		//	GameObject unit = GameObject.Instantiate(unitPrefab);
		//	unit.transform.SetParent(mainPivotObject.transform);
		//	units.Add(unit);
		//}

		ranks = formationDepth;
		formation = new FormationData(CalculateRows(), ranks, new float[2] { 1.5f, 3 });

		mainPivot = mainPivotObject.AddComponent<UnitManager>();
		mainPivot.Initialize(unitPrefab, UnitCount, formation);

		corners = new Vector2[4];
		
		//List<Vector2> list = GenerateFormation(unitCount, 0f, 3);
		//for(int i = 0; i < unitCount; i++)
		//{
		//	units[i].transform.position = new Vector3(list[i].x, 0, list[i].y);
		//	units[i].transform.eulerAngles = new Vector3(0, 0, 0);
		//}
		mainPivot.SetPosition(new Vector3(position.x, 0, position.y), rotationAngle);
		//caltulating from variables!
	}

	private int CalculateRows()
	{
		int result = 0;
		if (UnitCount % ranks == 0)
		{
			result = UnitCount / ranks;
		}
		else
		{
			int i = 0;
			do
			{
				i++;
			} while (i * ranks < UnitCount);
			result = i;
		}
		return result;
	}

	public void MoveFormation(Vector2 newPosition)
	{
		position = newPosition;
		mainPivot.SetPosition(new Vector3(position.x, 0, position.y));
	}

	public List<Vector2> GenerateFormation(int unitCount, float rotation, int rankDepth)
	{
		int rankWidth = 0;
		if (unitCount % rankDepth == 0)
		{
			rankWidth = unitCount / rankDepth;
		}
		else
		{
			int i = 0;
			do
			{
				i++;
			} while (i * rankDepth < unitCount);
			rankWidth = i;
		}

		float unitGap = 1.5f;
		float rankGap = 3f;

		int ranks = (unitCount / rankWidth);
		float rankLength = unitGap * rankWidth;
		List<Vector2> result = new List<Vector2>();

		if (ranks > 0)
		{
			Vector2[,] positions = new Vector2[ranks, rankWidth];

			positions[0, 0] = new Vector2(
				((rankLength / 2) - (unitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), 5),
				((rankLength / 2) - (unitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), 5)
				);

			for (int i = 1; i < rankWidth; i++)
			{
				positions[0, i] = new Vector2(
				positions[0, i - 1].x + unitGap *
				(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), 5),
				positions[0, i - 1].y + unitGap *
				(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), 5)
				);
			}

			for (int r = 1; r < ranks; r++)
			{
				for (int i = 0; i < rankWidth; i++)
				{
					positions[r, i] = new Vector2(
					positions[r - 1, i].x + rankGap *
					(float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
					positions[r - 1, i].y + rankGap *
					(float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
					);
				}
			}

			foreach (Vector2 position in positions)
			{
				Vector2 positionPass = position;
				result.Add(positionPass);
			}
		}
		Vector2[] lastRankPositions = null;
		if (unitCount % rankWidth != 0)
		{
			lastRankPositions = new Vector2[unitCount % rankWidth];
			Vector2 lastRankStart = new Vector2(
				(ranks) * rankGap * (float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
				(ranks) * rankGap * (float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
				);
			float lastRankLength = lastRankPositions.Length * unitGap;
			lastRankStart += new Vector2(
				((lastRankLength / 2) - (unitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), 5),
				((lastRankLength / 2) - (unitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), 5)
				);
			lastRankPositions[0] = new Vector2(lastRankStart.x, lastRankStart.y);
			for (int i = 1; i < lastRankPositions.Length; i++)
			{
				lastRankPositions[i] = new Vector2(
				lastRankPositions[i - 1].x + unitGap *
				(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), 5),
				lastRankPositions[i - 1].y + unitGap *
				(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), 5)
				);
			}
		}

		corners[0] = result[0];
		corners[1] = result[rankWidth - 1];

		if (lastRankPositions != null)
		{
			corners[2] = new Vector2(
					corners[1].x + (rankDepth - 1) * rankGap *
					(float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
					corners[1].y + (rankDepth - 1) * rankGap *
					(float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
					);
			corners[3] = new Vector2(
					corners[0].x + (rankDepth - 1) * rankGap *
					(float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
					corners[0].y + (rankDepth - 1) * rankGap *
					(float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
					);

			foreach (Vector2 position in lastRankPositions)
			{
				Vector2 positionPass = position;
				result.Add(positionPass);
			}
		}
		else
		{
			Debug.Log(result[result.Count - 1]);
			corners[2] = result[result.Count - 1];
			corners[3] = result[result.Count - rankWidth];
		}



		return result;
	}
}
