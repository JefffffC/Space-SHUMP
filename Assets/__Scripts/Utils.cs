using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // returns a list of all Materials on this GameObject and its children
    static public Material[] GetAllMaterials (GameObject go) // since static public, can be called anywhere in project using Utils.GetAllMaterials()
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>(); // a method which generates an array of renderers for gameobject and its children
        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends) // iterate through each renderer in the array
        {
            mats.Add(rend.material); // add the material of the renderer to the list
        }
        return (mats.ToArray()); // turn the list to an array and return it as an array of materials
    }
}
