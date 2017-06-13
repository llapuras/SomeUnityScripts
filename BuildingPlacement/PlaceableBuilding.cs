using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableBuilding : MonoBehaviour {
    
    [HideInInspector]
    public List<Collider> colliders = new List<Collider>();
    private bool isSelected;
	// Use this for initialization
	void OnGUI() {
        if (isSelected) {
            GUI.TextArea(new Rect(100, 200, 100, 30), name);
        }
	}

     void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            colliders.Add(other);
        }
    }

     void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
        {
            colliders.Remove(other);
        }
    }
    
    public void SetSelected(bool s)
    {
        isSelected = s;
    }

}
