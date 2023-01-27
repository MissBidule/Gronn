using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Nature
{
    public string name;

    public int natureID;

    public int carbonStocks;

    public int carbonAbsorption;

    public int yearsOfGrowth;

    public bool canEvolve;
    
    public GameObject natureModel;
}
