﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Inventory;
using UnityEngine;

public class Chest : Interactable
{
    public bool ContainsKey;
    public bool OpenClose;
    public bool IsTriggered;
    public int counter;

    private Color startcolor;

    // Use this for initialization
    void Start()
    {
        IsTriggered = false;
        ContainsKey = false;
        counter = 0;
    }

    void OnMouseEnter()
    {
        startcolor = transform.parent.GetComponent<Renderer>().material.color;
        transform.parent.GetComponent<Renderer>().material.color = Color.magenta;
        this.GetComponent<Renderer>().material.color = Color.magenta;
    }

    void OnMouseExit()
    {
        transform.parent.GetComponent<Renderer>().material.color = startcolor;
        this.GetComponent<Renderer>().material.color = startcolor;
    }


    public bool CheckForKey()
    {
        var inv = GameObject.Find("Inventory").GetComponent<Inventory>();

        if (inv != null)
        {
            foreach (var item in inv.items)
            {
                if (gameObject.name == "Chest_Lid1" && item.ID == 3)
                {
                    ContainsKey = true;
                }

                if (gameObject.name == "Chest_Lid2" && item.ID == 10)
                {
                    ContainsKey = true;
                }
            }
        }
        return ContainsKey;
    }

    public override void Interact()
    {
        Debug.Log("Chest was clicked");
        IsTriggered = true;

        if (CheckForKey())
        {
            if (IsTriggered)
            {
                if (ContainsKey)
                {
                    var inv = GameObject.Find("Inventory").GetComponent<Inventory>();
                    if (OpenClose && counter != 0)
                    {
                        Debug.Log("Chest Open: " + OpenClose);

                        transform.Rotate(60, 0, 0, Space.Self);
                        OpenClose = !OpenClose;
                        if (gameObject.name == "Chest_Lid1")
                        {
                            if (!inv.items.Exists(f => f.ID == 4))
                            {
                                inv.AddItem(4);
                            }
                        }
                        else if (gameObject.name == "Chest_Lid2")
                        {
                            if (!inv.items.Exists(f => f.ID == 13))
                                inv.AddItem(13);
                        }
                    }
                    else if (!OpenClose)
                    {
                        Debug.Log("Chest Closed: " + OpenClose);
                        transform.Rotate(-60, 00, 0, Space.Self);
                        OpenClose = !OpenClose;
                        IsTriggered = false;
                        if (gameObject.name == "Chest_Lid1")
                        {
                            if (!inv.items.Exists(f => f.ID == 4))
                                inv.AddItem(4);
                        }
                        else if (gameObject.name == "Chest_Lid2")
                        {
                            if (!inv.items.Exists(f => f.ID == 13))
                                inv.AddItem(13);
                        }
                    }
                    counter++;
                }
            }
        }
    }
}