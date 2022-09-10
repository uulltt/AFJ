using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VendorObject : MonoBehaviour
{

    public List<VendorItem> Items;
    
    public List<VendorItem> order;

//    public Dialogue boughtsomething, boughtnothing, ordercheck;


    public void AddToOrder(VendorItem item)
    {
        order.Add(item);
        Debug.Log(string.Format("added {0} to order", item.name));
        FindObjectOfType<VendorHelper>().RefreshOrder();
    }

    public void RemoveFromOrder(VendorItem item)
    {
        order.Remove(item);
        FindObjectOfType<VendorHelper>().RefreshOrder();
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
