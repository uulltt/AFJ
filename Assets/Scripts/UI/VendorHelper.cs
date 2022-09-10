using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This is a class intended to help populate any inventory menu to show the current contents of whatever inventory it's referencing
/// </summary>
public class VendorHelper : MonoBehaviour
{
    public VendorObject vendor;
    public GameObject vendorButton, checkoutButton, YesNoPrompt;

    /// <summary>
    /// this is to be used for the inventory screen used for vendors, as ideally we only want one vendor menu, but multiple inventories for different vendors
    /// </summary>
    /// <param name="currentHolder"></param>
    public void SetItemHolder(VendorObject currentHolder)
    {
        vendor = currentHolder;
    }

    public void OnEnable()
    {
        RefreshOrder();
    }

    public void RefreshOrder(bool initial = false)
    {
        StartCoroutine(PopulateVendorMenu(initial));
    }
    /// <summary>
    /// populates the player's inventory menu with their current inventory
    /// </summary>
    /// <param name="ate">whether the player has just consumed something or no, which will determine a refresh. </param>
    /// <returns></returns>
    public IEnumerator PopulateVendorMenu(bool initial = false)
    {
        try
        {
            if (initial)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    //cleans out inventory menu before re-adding all of the items.
                    GameObject g = transform.GetChild(0).gameObject;
                    transform.GetChild(0).SetParent(null, false);
                    Destroy(g);
                }
            }
            for (int i = 0; i < vendor.Items.Count; i++)
            {
                VendorItem currentItem = vendor.Items[i];
                if (initial)
                {
                    GameObject ItemObject = (GameObject)Instantiate(vendorButton, Vector3.zero, Quaternion.identity); //create new button for the specified item
                    ItemObject.transform.SetParent(transform, false);
                    ItemObject.name = currentItem.name;

                    ItemObject.transform.GetChild(0).GetComponent<Text>().text = currentItem.name;
                    //ItemObject.transform.GetChild(1).GetComponent<Image>().sprite = currentItem.sprite;
                    //ItemObject.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(currentItem.sprite.rect.width / (currentItem.sprite.rect.height / 30f), 30f);
                    Button removeItem = ItemObject.transform.GetChild(1).GetComponent<Button>();
                    Button addItem = ItemObject.transform.GetChild(3).GetComponent<Button>();
                    VendorItem currentIndex = vendor.Items[i];
                    addItem.onClick.AddListener(() => vendor.AddToOrder(currentIndex));
                    removeItem.onClick.AddListener(() => vendor.RemoveFromOrder(currentIndex));
                    ItemObject.transform.GetChild(2).GetComponent<Text>().text = vendor.order.Where(x => x == currentItem).Count().ToString(); //quantity of item in order
                    ItemObject.transform.GetChild(4).GetComponent<Text>().text = "$" + currentIndex.price.ToString();
                }
                else
                {
                    transform.GetChild(i).transform.GetChild(4).GetComponent<Text>().text = vendor.order.Where(x => x == currentItem).Count().ToString(); //quantity of item in order
                }
            }
            if (initial)
            {
                Button checkOut = ((GameObject)Instantiate(checkoutButton, Vector3.zero, Quaternion.identity)).GetComponent<Button>(); //create new button for the specified item
                checkOut.transform.SetParent(transform, false);
                checkOut.GetComponentInChildren<Text>().text = "CheckOut";
                checkOut.onClick.AddListener(YesNoCheck);
                YesNoPrompt.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                YesNoPrompt.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                YesNoPrompt.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(CheckOut);
                YesNoPrompt.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(CancelOrder);
            }
        }
        catch (System.Exception)
        {

        }

        yield return new WaitForChangedResult();
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        /*if (!transform.IsChildOf(eventSystem.currentSelectedGameObject.transform))
        {
            FindObjectOfType<EventSystem>().firstSelectedGameObject = transform.GetChild(0).gameObject;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(transform.GetChild(0).gameObject);
        }*/
    }

    private void CheckOut()
    {
        YesNoPrompt.SetActive(false);
        vendor.Checkout();
    }

    private void CancelOrder()
    {
        YesNoPrompt.SetActive(false);
        //vendor.Choose(vendor.ordercheck.nextEvent as Dialogue);
    }

    private void YesNoCheck()
    {
        YesNoPrompt.SetActive(true);
        //vendor.Choose(vendor.ordercheck);
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        //if (!transform.IsChildOf(eventSystem.currentSelectedGameObject.transform))
        //{
        eventSystem.firstSelectedGameObject = YesNoPrompt.transform.GetChild(0).gameObject;
        eventSystem.SetSelectedGameObject(YesNoPrompt.transform.GetChild(0).gameObject);
        // }
    }
}
