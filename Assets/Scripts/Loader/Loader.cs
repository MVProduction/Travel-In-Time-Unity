﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Loader
{
    public class Loader : MonoBehaviour
    {
        public Slider slider;
        public GameObject progressText;
        public GameObject Spiral;
        public float cooldown = 4;
        private float timeStamp;

        void Start()
        {
            gameObject.SetActive(true);
        }

        public void Loading()
        {
            StartCoroutine(Rotate(10));
            StartCoroutine(LoadAsynchronously());
        }

        IEnumerator LoadAsynchronously()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            DontDestroyOnLoad(GameObject.Find("Audio(Clone)"));

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                slider.value = progress;
                progressText.transform.GetComponent<TextMeshProUGUI>().text = $"{progress * 100f}%";
                
                yield return null;
            }

            
        }

        IEnumerator Rotate(float duration)
        {
            float startRotation = transform.eulerAngles.z;
            float endRotation = startRotation + 360.0f;
            float t = 0.0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
                Spiral.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.z,zRotation);
                yield return null;
            }
        }
    }
}
