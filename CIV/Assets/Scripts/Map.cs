using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
	//for Future - throw it to singleton
	[SerializeField]
	Camera cam;

	//temporary UI links
	[SerializeField]
	private GameObject SelectionPanel;
	[SerializeField]
	private TextMeshProUGUI unitNameLabel;
	[SerializeField]
	private TextMeshProUGUI unitSkillLabel;
	[SerializeField]
	private TextMeshProUGUI unitMoraleLabel;

	[SerializeField]
	private GameObject unitPrefab;
	[SerializeField]
	private LayerMask clickable;
	[SerializeField] 
	private LayerMask ground;
	[SerializeField]
	private int mapSize = 300;
	public Unit formation;

	public UnitData templateUnit;

    void Start()
    {
		cam = Camera.main;

		GameObject unitGameObject = new GameObject("Formation");
		unitGameObject.AddComponent<Unit>();
		formation = unitGameObject.GetComponent<Unit>();
		formation.Initialize(unitPrefab, templateUnit, 60, 3);
    }

    void Update()
    {
		//separate this to control / ui manager in the future
		if(!EventSystem.current.IsPointerOverGameObject())
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
				{
					SelectUnit(hit.collider.gameObject.GetComponent<Unit>());
				}
			}
		}
    }

	public void SelectUnit(Unit unit)
	{
		SelectionPanel.SetActive(true);
		unitNameLabel.text = unit.UnitData.Name;
		unitSkillLabel.text = $"{unit.UnitData.Skill}";
		unitMoraleLabel.text = $"{unit.UnitData.Morale}\n{unit.UnitData.Nerve}";
	}

	public void DeselectUnit()
	{
		SelectionPanel.SetActive(false);
	}
}
