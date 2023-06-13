using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float delay = 5f;
    bool iDown;
    public GameObject[] items;
    public bool[] hasItems;
    public int[] eviActive= new int[5];
    public NPCFrompot npcA;
    public NPCFrompot npcB;

    public NPCFrompot npcC;

    public NPCFrompot npcD;
     public TMP_Text mesh;

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
       for (int i = 0; i < eviActive.Length; i++)
        {
            eviActive[i] = 0;
        }   
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
            
            if (nearObject.CompareTag("Items"))
            {
                 Item item = nearObject.GetComponent<Item>();
                 if(item.value==0){
                    if(mesh.enabled==false) mesh.enabled=true;
                    npcC.npcprompot[3]=npcC.npcwithevidence[2];
                    eviActive[0]=1;
                    mesh.text=npcC.gotoNpc[2];
                    Invoke("HideText", delay);
                    }
                 else if(item.value==1){
                    hasItems[1]=true;

                 }


                int itemIndex = item.value;
                hasItems[itemIndex] = true;
                Debug.Log("checked");
                Destroy(nearObject);

            }
        }
    }
    private void HideText()
    {
       mesh.enabled = false; // Text 요소를 비활성화하여 숨김
    }
}
