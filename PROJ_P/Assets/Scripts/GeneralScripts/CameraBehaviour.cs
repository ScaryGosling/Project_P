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
    [SerializeField] private float movementSensitity;
    [SerializeField][Range(0,1)] private float fadeAmount;
    [SerializeField] private float fadeTime;
    private GameObject fadedObject;
    [SerializeField] private Shader transparentShader;
    [SerializeField] private Shader opaqueShader;
    private Renderer hitRenderer;
    private FadedHouse fadedHouse;
    private Vector3 mousePosition;



    public struct FadedHouse
    {

        public bool isFaded;
        public Renderer renderer;
        public Material[] oldMaterials;
        public Shader opaqueShader;
        public Shader transparentShader;
        private const string shaderAlpha = "_Alpha";
        public float fadeAmount;

        public Coroutine fadeCoroutine;
        public GameObject go;
        public float fadeTime;

        public IEnumerator FadeOut()
        {
            isFaded = true;
            float fadeTime = this.fadeTime;
            Debug.Log("Fade out");

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].shader = transparentShader;
            }

            while (renderer.material.GetFloat(shaderAlpha) > fadeAmount)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    if(renderer.materials[i].HasProperty(shaderAlpha))
                        renderer.materials[i].SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) - fadeTime * Time.deltaTime);
                }
                yield return null;
            }
            
        }

        public IEnumerator FadeIn()
        {
            isFaded = false;
            float fadeTime = this.fadeTime;


            while (renderer.material.GetFloat(shaderAlpha) < 1)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    if (renderer.materials[i].HasProperty(shaderAlpha))
                        renderer.materials[i].SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) + fadeTime * Time.deltaTime);
                }
                yield return null;
            }
            
            
            ResetShaders();

        }

        public void ResetShaders()
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i] = oldMaterials[i];
                renderer.materials[i].shader = opaqueShader;
            }

        }


    }




    void Update()
    {
        
        mousePosition = new Vector3(Input.mousePosition.x / Screen.width * 2 - 1, 0, Input.mousePosition.y / Screen.height * 2 - 1) * movementSensitity;
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset + mousePosition, ref velocity, smoothTime);
        CheckObstacles();

        Debug.Log(mousePosition);
    }





    public void CheckObstacles()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity))
        {

            if (hit.collider.CompareTag("Building"))
            {
                hitRenderer = hit.collider.GetComponent<Renderer>();


                //If no faded house has been added
                if(fadedHouse.go == null)
                {
                    fadedHouse = SetupFadedHouse(hitRenderer);
                    fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeOut());
                    return;
                }

                //If entered house is same as faded hose, but it is not faded
                if(hit.collider.gameObject == fadedHouse.go && fadedHouse.renderer.material.GetFloat("_Alpha") >= 1)
                {
                    Debug.Log("Entered same house again");
                    if(fadedHouse.fadeCoroutine != null)
                        StopCoroutine(fadedHouse.fadeCoroutine);
                    fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeOut());
                }

                //If entering another faded house
                if(hit.collider.gameObject != fadedHouse.go)
                {
                    if(fadedHouse.fadeCoroutine != null && fadedHouse.renderer.material.HasProperty("_Alpha"))
                    {
                        StopCoroutine(fadedHouse.fadeCoroutine);
                        fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeIn());
                    }
                    fadedHouse = SetupFadedHouse(hitRenderer);
                    fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeOut());
                    return;
                }


            }

            if (hit.collider.CompareTag("Player"))
            {
                if (fadedHouse.fadeCoroutine != null && fadedHouse.renderer.material.HasProperty("_Alpha"))
                {
                    StopCoroutine(fadedHouse.fadeCoroutine);
                    fadedHouse.fadeCoroutine = StartCoroutine(fadedHouse.FadeIn());
                }

            }
            

        }


    }

    public FadedHouse SetupFadedHouse(Renderer renderer)
    {
        FadedHouse fadedHouse = new FadedHouse();

        fadedHouse.renderer = renderer;
        fadedHouse.oldMaterials = new Material[renderer.materials.Length];
        fadedHouse.go = renderer.gameObject;
        fadedHouse.transparentShader = transparentShader;
        fadedHouse.fadeAmount = fadeAmount;
        fadedHouse.fadeTime = fadeTime;

        for (int i = 0; i < fadedHouse.oldMaterials.Length; i++){ fadedHouse.oldMaterials[i] = renderer.materials[i]; }
        //for (int i = 0; i < fadedHouse.oldShader.Length; i++){ fadedHouse.oldShader[i] = opa; }
        fadedHouse.opaqueShader = opaqueShader;

        return fadedHouse;
    }




}
