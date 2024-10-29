using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Skills
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

public enum AdventurerState
{
    Waiting,
    Dispatching,
    Ready,
    Fighting,
    Returning
}

[System.Serializable]
public class Adventurer
{
    public Skills skills;
    public AdventurerInfo info;
    public AdventurerState state;
    public string name;

    public Adventurer(Skills skills, AdventurerInfo info, string name)
    {
        this.skills = skills;
        this.info = info;
        this.state = AdventurerState.Waiting;
        this.name = name;
    }
}