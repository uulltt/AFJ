using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using static UnityEngine.UI.Toggle;

public class Requirements : MonoBehaviour
{
    [System.Serializable]
    public class BuildingNumber
    {
        public Construct building;
        public string name;
        public int quantity;
        public int needed;

        public override string ToString()
        {
            return name + ": " + quantity.ToString() + "/" + needed.ToString();
        }
    }
    public List<BuildingNumber> RequireBuildings;
    public TMP_Text info;

    public bool RequirementsMet => RequireBuildings.All(b => b.quantity >= b.needed);

    public ToggleEvent enableBegin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        info.text = string.Join("\n", RequireBuildings.Select(s => s.ToString()).ToList()).Trim();
        enableBegin?.Invoke(RequirementsMet);
    }
}
