using System;
using System.IO;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    static public GameObject button;

    public bool pressed = false;

    static public void AssignButton(GameObject obj)
    {
        button = obj;
        button.layer = 18; //button layer
        button.AddComponent<ButtonTrigger>();
        var box = button.AddComponent<BoxCollider>();
        box.isTrigger = true;

    }

    void OnTriggerEnter(Collider collider)
    {
        pressed = true;


    }

    void OnTriggerExit(Collider collider)
    { 
        pressed = false;
    }

    
}
