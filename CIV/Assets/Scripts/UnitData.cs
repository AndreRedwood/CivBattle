using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitData
{
	public string Name;

	public int Speed;
	public int Armor;
	public int Skill;

	public int Morale;
	public int Nerve;

	public UnitData(string name, int speed, int armor, int skill, int morale, int nerve)
	{
		Name = name;
		Speed = speed;
		Armor = armor;
		Skill = skill;
		Morale = morale;
		Nerve = nerve;
	}

	public UnitData(UnitData template)
	{
		Name = template.Name;
		Speed = template.Speed;
		Armor = template.Armor;
		Skill = template.Skill;
		Morale = template.Morale;
		Nerve = template.Nerve;
	}
}
