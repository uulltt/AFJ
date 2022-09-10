using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class VendorMenu : MonoBehaviour
{

    public List<VendorItem> Items;

    public List<VendorItem> order;

    //    public Dialogue boughtsomething, boughtnothing, ordercheck;

    public void Update()
    {
        /*if (IsPointerOverUIElement())
        {

        }*/
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                Debug.Log(curRaysastResult.gameObject.name);
                return true;
            }
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    public void AddToOrder(VendorItem item)
    {
        order.Add(item);
        Debug.Log(string.Format("added {0} to order", item.name));
        UpdateQuantity();
    }

    public void RemoveFromOrder(VendorItem item)
    {
        if (order.Contains(item))
            order.Remove(item);
        UpdateQuantity();
    }

    public void UpdateQuantity()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            transform.GetChild(i).GetChild(2).GetComponent<TMP_InputField>().text = order.Where(x => x == Items[i]).Count().ToString();
        }
    }

    public void Checkout()
    {
        int orderCount = order.Count;
        foreach (VendorItem item in order)
        {
            //Player.Inventory.Items.Add(item.item);
            //Player.money -= item.price;
        }
        order.Clear();
        //dialogueObject.GetChild(2).gameObject.SetActive(false);
    }
}
