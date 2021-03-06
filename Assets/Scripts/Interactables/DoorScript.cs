﻿using Assets.Scripts;
using Assets.Scripts.Inventory;
using UnityEngine;

namespace Interactables
{
    public class DoorScript : Interactable
    {

        public bool ContainsKey;
        public bool OpenClose;
        public bool IsTriggered;
        public int counter;
        private Color startcolor;


        // Use this for initialization
        void Start ()
        {
            Debug.Log("Hello I am here");
            IsTriggered = false;
            ContainsKey = false;
            counter = 0;

            if (!GameplayChecker.AreDoorsOpen && GameplayChecker.SafePuzzleSolved && GameplayChecker.FirstTimeOpened)
            {
                //if (transform.rotation.y > -90)
                //    CloseDoors();
                ContainsKey = true;
                OpenClose = false;
                counter++;
            }
            else if (GameplayChecker.AreDoorsOpen && GameplayChecker.SafePuzzleSolved)
            {
                OpenDoors();
                OpenClose = true;
                counter++;
            }
               
        }

        void OnMouseEnter()
        {
            startcolor = this.GetComponent<Renderer>().material.color;
            this.GetComponent<Renderer>().material.color = Color.magenta;
            
        }
        void OnMouseExit()
        {
            this.GetComponent<Renderer>().material.color = startcolor;
        }

        public bool CheckForKey()
        {
            var inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            foreach (var item in inv.items)
            {
                if (item.ID == 2)
                {
                    ContainsKey = true;
                    return ContainsKey;
                }
            }
            return ContainsKey;
        }

        void Update()
        {
           
        }
        

        public override void Interact()
        {
            Debug.Log("Doors were clicked");
            IsTriggered = true;

            if (CheckForKey())
            {
                if (IsTriggered)
                {
                    if (ContainsKey)
                    {
                        if (GameplayChecker.AreDoorsOpen && counter != 0)
                        {
                            Debug.Log("Doors Open: " + OpenClose);
                            CloseDoors();

                        }
                        else if (!GameplayChecker.AreDoorsOpen)
                        {
                            Debug.Log("Doors Closed: " + OpenClose);
                            OpenDoors();

                        }
                        counter++;
                    }
                }

            }
        }

        private void OpenDoors()
        {
            transform.Rotate(0, 90, 0, Space.Self);
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            OpenClose = !OpenClose;
            IsTriggered = false;
            GameplayChecker.FirstTimeOpened = true;
            GameplayChecker.AreDoorsOpen = true;
            
        }

        private void CloseDoors()
        {
            transform.Rotate(0, -90, 0, Space.Self);
            transform.localScale = new Vector3(0.7f, transform.localScale.y, transform.localScale.z);
            OpenClose = !OpenClose;
            IsTriggered = false;
            GameplayChecker.AreDoorsOpen = false;
        }
    }
           
    }

