using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
	[SerializeField]
	private GameObject unitPrefab;

    // Start is called before the first frame update
    void Start()
    {
		List<Vector2> list = GenerateFormation(60, 0f, 20);
		foreach (Vector2 position in list)
		{
			GameObject unit = Instantiate(unitPrefab);
			unit.transform.position = new Vector3(position.x, 0.1f, position.y);
			unit.transform.eulerAngles = new Vector3(0, 0, 0);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public List<Vector2> GenerateFormation(int unitCount, float rotation, int rankWidth = -1)
	{
		if (rankWidth < 0)
		{
			throw new NotImplementedException();
			//rankWidth = GetDefultRankWidth(unitCount);
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
				(ranks) * -rankGap * (float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
				(ranks) * -rankGap * (float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
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
		if (lastRankPositions != null)
		{
			foreach (Vector2 position in lastRankPositions)
			{
				Vector2 positionPass = position;
				result.Add(positionPass);
			}
		}
		return result;
	}
}
