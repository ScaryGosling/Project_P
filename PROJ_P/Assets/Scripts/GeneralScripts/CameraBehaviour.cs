using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime;
    [SerializeField][Range(0,1)] private float fadeAmount;
    [SerializeField] private float fadeTime;
    private GameObject fadedObject;
    [SerializeField] private Shader transparentShader;
    private Renderer hitRenderer;

    private FadedHouse fadedHouse;



    public struct FadedHouse
    {

        public Renderer renderer;
        public Material[] oldMaterials;
        public Shader[] oldShader;
        public Shader transparentShader;
        private const string shaderAlpha = "Vector1_817719AB";
        public float fadeAmount;

        public Coroutine fadeCoroutine;
        public GameObject go;
        public float fadeTime;

        public IEnumerator FadeOut()
        {
            float fadeTime = this.fadeTime;

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].shader = transparentShader;
            }

            while (renderer.material.GetFloat(shaderAlpha) > fadeAmount)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i].SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) - fadeTime * Time.deltaTime);
                }
                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            float fadeTime = this.fadeTime;

            while (renderer.material.GetFloat(shaderAlpha) < 1)
            {

                for (int i = 0; i < renderer.materials.Length; i++)
                {

                    renderer.materials[i].SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) + fadeTime * Time.deltaTime);

                }
                yield return null;
            }

            ResetShaders();
            fadeCoroutine = null;

        }

        public void ResetShaders()
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i] = oldMaterials[i];
                renderer.materials[i].shader = oldShader[i];
            }

        }


    }




    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, smoothTime);
        CheckObstacles();
    }





    public void CheckObstacles()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity))
        {

            if (hit.collider.CompareTag("Building") && (hit.collider.gameObject != fadedHouse.go || fadedHouse.fadeCoroutine == null))
            {
                if(fadedHouse.renderer != null)
                    fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeIn());

                hitRenderer = hit.collider.GetComponent<Renderer>();
                fadedHouse = SetupFadedHouse(hitRenderer);
                fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeOut());

            } else if (hit.collider.CompareTag("Player") && fadedHouse.renderer != null)
            {
                fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeIn());
            }
        }


    }

    public FadedHouse SetupFadedHouse(Renderer renderer)
    {
        FadedHouse fadedHouse = new FadedHouse();

        fadedHouse.renderer = renderer;
        fadedHouse.oldMaterials = new Material[renderer.materials.Length];
        fadedHouse.oldShader = new Shader[renderer.materials.Length];
        fadedHouse.go = renderer.gameObject;
        fadedHouse.transparentShader = transparentShader;
        fadedHouse.fadeAmount = fadeAmount;
        fadedHouse.fadeTime = fadeTime;

        for (int i = 0; i < fadedHouse.oldMaterials.Length; i++){ fadedHouse.oldMaterials[i] = renderer.materials[i]; }
        for (int i = 0; i < fadedHouse.oldShader.Length; i++){ fadedHouse.oldShader[i] = renderer.materials[i].shader; }

        return fadedHouse;
    }




}
