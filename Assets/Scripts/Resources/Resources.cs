using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Resources
{
    //values
    public float Magic;
    public float Wood;
    public float Stone;
    public float Gold;

    //constructor
    public Resources(float magic, float wood, float stone, float gold)
    {
        Magic = magic;
        Wood = wood;
        Stone = stone;
        Gold = gold;
    }   

    //override < and > operations
    public static bool operator <(Resources lh, Resources rh)
    {
        return
            lh.Magic < rh.Magic &&
            lh.Wood < rh.Wood &&
            lh.Stone < rh.Stone &&
            lh.Gold < rh.Gold;
    }
    public static bool operator >(Resources lh, Resources rh)
    {
        return rh < lh;
    }

    //override <= and >= operators
    public static bool operator <=(Resources lh, Resources rh)
    {
        return
            lh.Magic <= rh.Magic &&
            lh.Wood <= rh.Wood &&
            lh.Stone <= rh.Stone &&
            lh.Gold <= rh.Gold;
    }
    public static bool operator >=(Resources lh, Resources rh)
    {
        return rh <= lh;
    }

    //override == and != operator
    public static bool operator ==(Resources lh, Resources rh)
    {
        return
            lh.Magic == rh.Magic &&
            lh.Wood == rh.Wood &&
            lh.Stone == rh.Stone &&
            lh.Gold == rh.Gold;
    }
    public static bool operator !=(Resources lh, Resources rh)
    {
        return
            lh.Magic != rh.Magic ||
            lh.Wood != rh.Wood ||
            lh.Stone != rh.Stone ||
            lh.Gold != rh.Gold;
    }

    //override + and - operations
    public static Resources operator -(Resources lh, Resources rh)
    {
        return new Resources(lh.Magic - rh.Magic, lh.Wood - rh.Wood, lh.Stone - rh.Stone, lh.Gold - rh.Gold);
    }
    public static Resources operator +(Resources lh, Resources rh)
    {
        return new Resources(lh.Magic + rh.Magic, lh.Wood + rh.Wood, lh.Stone + rh.Stone, lh.Gold + rh.Gold);
    }

    //override * and / operator
    public static Resources operator *(Resources lh, float rh)
    {
        return new Resources(lh.Magic * rh, lh.Wood * rh, lh.Stone * rh, lh.Gold * rh);
    }
    public static Resources operator *(float lh, Resources rh)
    {
        return new Resources(rh.Magic * lh, rh.Wood * lh, rh.Stone * lh, rh.Gold * lh);
    }
    public static Resources operator /(Resources lh, float rh)
    {
        return new Resources(lh.Magic / rh, lh.Wood / rh, lh.Stone / rh, lh.Gold / rh);
    }

    //Override Equals object equality
    public override bool Equals(object obj)
    {
        if (obj is Resources other)
        {
            return this == other;
        }
        return false;
    }

    //override GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Magic, Wood, Stone, Gold);
    }

    // Optional: Override ToString for better output formatting
    public override string ToString()
    {
        return
            "Magic: " + Magic.ToString() + "\n" +
            "Wood: " + Wood.ToString() + "\n" +
            "Stone: " + Stone.ToString() + "\n" +
            "Gold: " + Gold.ToString();
    }
}
