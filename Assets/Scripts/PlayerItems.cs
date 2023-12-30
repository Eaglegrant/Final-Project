using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [Header("Items (ELEMENT 1 = START ITEM)")]
    public GameObject[] itemsInOrder;
    [Header("Keybinds")]
    public KeyCode itemRight = KeyCode.E;
    public KeyCode itemLeft = KeyCode.Q;
    [Header("References")]
    
    public GameObject PlayerModel;
    Rigidbody rb;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeItem(1); 
    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();

    }
    private void InputHandle()
    {
        if (Input.GetKeyDown(itemRight) || (Input.GetAxis("Mouse ScrollWheel") > 0f))
        {
            ChangeItem(1);
        }
        else if (Input.GetKeyDown(itemLeft) || (Input.GetAxis("Mouse ScrollWheel") < 0f))
        {
            ChangeItem(-1);
        }
    }

    private void ChangeItem(int change)
    {
        bool activiy = false;
        itemsInOrder[currentIndex].SetActive(false);
        UpdateScripts(activiy);
        activiy = true;
        currentIndex += change;
        if (currentIndex < 0)
        {
            currentIndex = itemsInOrder.Length-1;
        }else if(currentIndex > itemsInOrder.Length-1)
        {
            currentIndex = 0;
        }
        Debug.Log(currentIndex);
        itemsInOrder[currentIndex].SetActive(true);
        UpdateScripts(activiy);
    }
    private void UpdateScripts(bool state)
    {
        if (itemsInOrder[currentIndex].name == "Grappling Gun")
        {
            PlayerModel.GetComponent<Grappling>().enabled = state;
        }
    }
}
