using System;
using System.IO;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    GameObject redButton;

    public bool pressed = false;

    public void InitButton(GameObject button_On)
    {
        redButton = button_On;
        redButton.GetComponent<MeshRenderer>().enabled = false;
        
    }
	void OnTriggerEnter(Collider collider)
    {
        redButton.GetComponent<MeshRenderer>().enabled = true;
        
        pressed = true;
    }

    void OnTriggerExit(Collider collider)
    {
        redButton.GetComponent<MeshRenderer>().enabled = false;
        
        pressed = false;
    }

    static public void AssignButton(GameObject button, GameObject button_on)
    {
        button.layer = 18; //button layer
        button_on.transform.SetParent(button.transform, true);
        button.AddComponent<ButtonTrigger>();
        button.GetComponent<ButtonTrigger>().InitButton(button_on);
    }

}
