﻿using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Interactables.Safe
{
    public class Safe : Interactable
    {
        public GameObject SafePinNumberObj;
        public InteractableManager InteractableManager;
        public SafePinNumber SafePinNumber;
        public List<Transform> PinNumbersObjects;
        public bool IsSafeOpen = false;
        public Transform SafeDoors;
        public TMP_Text SafeText;
        private float timeStamp;
        private bool IsCoolingDown = false;
        private const int coolDownPeriodInSeconds = 3;
        private Color startcolor;


        void Start()
        {
            PinNumbersObjects = new List<Transform>();
        }

        void OnMouseEnter()
        {

            startcolor = this.GetComponent<Renderer>().material.color;
            this.GetComponent<Renderer>().material.color = Color.magenta;
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.magenta;
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.color = Color.magenta;


        }
        void OnMouseExit()
        {
            this.GetComponent<Renderer>().material.color = startcolor;
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = startcolor;
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.color = startcolor;
        }

        public override void Interact()
        {
            if (!IsSafeOpen && !GameplayChecker.SafePuzzleSolved)
            {
                InteractableManager = GameObject.Find("Interaction").GetComponent<InteractableManager>();
                InteractableManager.safeInteraction = this.gameObject;
                if (InteractableManager == null) return;
                playerAgent = GameObject.Find("Character(Clone)").GetComponent<NavMeshAgent>();

                if(GameObject.Find("Safe"))
                InteractableManager.Activate("Safe");
                SafePinNumber = GameObject.Find("Code").GetComponent<SafePinNumber>();
                SafeDoors = GameObject.Find("Safe").transform.GetChild(0).GetComponent<Transform>();
                SafePinNumberObj = GameObject.Find("Code");

                
                if (SafePinNumber == null) return;
                if (SafeDoors == null) return;
                if (SafePinNumberObj == null) return;


                if (PinNumbersObjects.Count > 1)
                {
                    PinNumbersObjects = new List<Transform>();
                }
                if (PinNumbersObjects != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        PinNumbersObjects.Add(SafePinNumberObj.transform.GetChild(i));
                    }
                }
                
            }
            else
            {
                if (IsSafeOpen)
                {
                    SafeDoors.Rotate(new Vector3(0f, 0f, 99.192f));
                    IsSafeOpen = false;
                }
                
                else
                {
                    SafeDoors.Rotate(new Vector3(0f, 0f, -99.192f));
                    IsSafeOpen = true;
                }
            }
           

        }

        public void DisplayPinNumbers(int number)
        {
            foreach (var pinNumbersObject in PinNumbersObjects)
            {

                var pinNumberText = pinNumbersObject.transform.GetChild(0).GetComponent<TMP_Text>();
                if (pinNumberText.text.Equals(string.Empty))
                {
                    var safeText = GameObject.Find("SafeText").gameObject.GetComponent<TMP_Text>();
                    pinNumberText.text = number.ToString();
                    if (SafePinNumber.UpdatePinNumber(number))
                    {
                        foreach (var pinNumberObject in PinNumbersObjects)
                        {
                            pinNumberObject.transform.GetComponent<Image>().color = Color.green;
                        }
                        var doors = GameObject.Find("Safe").transform.GetChild(0).GetComponent<Transform>();
                        doors.Rotate(new Vector3(0f, 0f, -99.192f));
                        timeStamp = Time.time + coolDownPeriodInSeconds;
                        IsSafeOpen = true;
                        IsCoolingDown = true;
                        safeText.text = "You have successfully solved the puzzle. The window will close in 3 seconds.";
                        
                        var inv = GameObject.Find("Inventory").GetComponent<Inventory>();
                        if (!inv.items.Exists(f => f.ID == 2))
                        {
                            inv.AddItem(2);
                        }
                        Debug.Log("Correct Pin");
                        GameplayChecker.SafePuzzleSolved = true;

                    }
                    else if (SafePinNumber.LengthOfCurrentString() == 4)
                    {
                        if (!SafePinNumber.SafeCode.Contains("0"))
                        {
                            foreach (var pinNumberObject in PinNumbersObjects)
                            {
                                pinNumberObject.transform.GetComponent<Image>().color = Color.red;
                            }
                           
                            
                            timeStamp = Time.time + coolDownPeriodInSeconds;
                            IsCoolingDown = true;

                            safeText.text = "Pin number is incorrect. The puzzle will restart in 3 seconds.";
                        }
                    }
                    
                    break;
                }
            }
        }

        void Update()
        {
            if (IsCoolingDown)
            {
                if (timeStamp <= Time.time)
                {
                    if (GameplayChecker.SafePuzzleSolved)
                    {
                        Clean();
                        Destroy(GameObject.Find("Safe_Interaction_Panel(Clone)"));
                        GameObject.Find("Interaction").GetComponent<InteractableManager>().interactionWindow
                            .SetActive(false);
                        IsCoolingDown = false;

                    }
                    else
                    {
                        foreach (var pinNumberObject in PinNumbersObjects)
                        {
                            pinNumberObject.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Empty;
                        }
                        var safeText = GameObject.Find("SafeText").gameObject.GetComponent<TMP_Text>();
                        safeText.text = "Decode safe with 4 different numbers. Take a look around have you seen anything suspicious?";
                        Clean();
                        IsCoolingDown = false;
                    }

                    
                    
                }
            }
        }

        public void Clean()
        {
            if (PinNumbersObjects != null)
            {
                for (int i = 0; i < PinNumbersObjects.Count; i++)
                {
                   PinNumbersObjects[i].transform.GetComponent<Image>().color = Color.white;

                    this.SafePinNumber.PinNumbers[i] = 0;
                }

                if (IsSafeOpen)
                {
                    Destroy(GameObject.Find("Safe_Interaction_Panel(Clone)"));
                    GameObject.Find("Interaction").GetComponent<InteractableManager>().interactionWindow.SetActive(false);
                }
            }
           
        }

        public void CoolingDown()
        {
            
        }
        
    }
}
