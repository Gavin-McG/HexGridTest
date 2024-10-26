using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
    public float skill;
    public float strength;
    public float teamwork;

    public Skills (float skill, float strength, float teamwork)
    {
        this.skill = skill;
        this.strength = strength;
        this.teamwork = teamwork;
    }
}


[System.Serializable]
public enum ClassType
{
    Warrior,
    Archer,
    Mage
}

[System.Serializable]
public class Adventurer
{
    public Skills skills;
    public AdventurerAsset character;
    public string name;

    public Adventurer(Skills skills, AdventurerAsset character, string name)
    {
        this.skills = skills;
        this.character = character;
        this.name = name;
    }
}
