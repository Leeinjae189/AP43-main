using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool iDown;
    public GameObject[] items;
    public bool[] hasItems;
  
    public NPCFrompot npcA;
    public NPCFrompot npcB;

    public NPCFrompot npcC;

    public NPCFrompot npcD;

    void GetInput()
    {
        iDown = Input.GetButtonDown("Interaction");
    }

    GameObject nearObject;
    void OnTriggerStay(Collider other)
    {
       if (other.CompareTag("Items"))
            nearObject = other.gameObject;
       
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Items"))
            nearObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

    void Interaction()
    {
        if(Input.GetKeyDown(KeyCode.E) && nearObject != null)
        {
            Debug.Log("checked");
            if (nearObject.CompareTag("Items"))
            {
                 Item item = nearObject.GetComponent<Item>();
                 if(item.value==2){
                    npcA.npcprompot[1]=npcA.npcwithevidence[0];
                 }

                int itemIndex = item.value;
                hasItems[itemIndex] = true;
                Debug.Log("checked");
                Destroy(nearObject);

            }
        }
    }
}
