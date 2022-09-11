using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnMenu : MonoBehaviour
{
    public List<VendorItem> Items;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public void UpdateQuantity()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<TMPro.TMP_Text>().text = Resources.Instance.Inventory.Where(x => x == Items[i]).Count().ToString();
        }
    }

    void Update()
    {
    }
}
