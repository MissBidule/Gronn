using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureDatabase : MonoBehaviour
{
    public List<Nature> natureDatabase = new List<Nature>();
    //Instance
    public static NatureDatabase Instance;
    private void Awake(){Instance=this;}
}
