using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public Dictionary<int, SelectionComponent> selectedTable = new();

    public void EnableSelected(SelectionComponent so)
    {
        int objectId = so.GetInstanceID();

        if (!(selectedTable.ContainsKey(objectId)))
        {
            selectedTable.Add(objectId, so);
            so.enabled = true;
            Debug.Log("Added " + so.name + " to selectedDictionary");
        }
    }

    public void Deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<SelectionComponent>());

        selectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach (KeyValuePair<int, SelectionComponent> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                pair.Value.enabled = false;
            }
        }
    }
}