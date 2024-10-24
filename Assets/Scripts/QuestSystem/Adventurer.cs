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


public enum ClassType
{
    Warrior,
    Archer,
    Mage
}

public class Adventurer
{
    public Skills skills;
    public ClassType classType;
    public Sprite headSprite;
    public string name;

    public Adventurer(Skills skills, ClassType classType, Sprite headSprite, string name)
    {
        this.skills = skills;
        this.classType = classType;
        this.headSprite = headSprite;
        this.name = name;
    }
}
