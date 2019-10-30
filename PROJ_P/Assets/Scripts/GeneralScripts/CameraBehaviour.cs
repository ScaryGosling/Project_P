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
    private string shaderAlpha = "Vector1_817719AB";
    private Coroutine fade;
    private GameObject fadedObject;
    [SerializeField] private Shader opaqueShader;
    [SerializeField] private Shader transparentShader;


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
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, Time.deltaTime);


            if (!hit.collider.CompareTag("Player") && hit.collider.GetComponent<Renderer>() && hit.collider.GetComponent<Renderer>().material.HasProperty(shaderAlpha) && hit.collider.GetComponent<Renderer>().material.GetFloat(shaderAlpha) >= 1)
            {
                if (fadedObject != null)
                    StartCoroutine(FadeInObject(fadedObject.GetComponent<Renderer>()));

                fadedObject = hit.collider.gameObject;
                if (fade != null)
                    StopCoroutine(fade);
                fade = StartCoroutine(FadeOutObject(renderer));

            }
            else if (hit.collider.CompareTag("Player"))
            {

                if (fadedObject != null && fade != null)
                {
                    StopCoroutine(fade);
                    fade = StartCoroutine(FadeInObject(fadedObject.GetComponent<Renderer>()));
                    fadedObject = null;

                }
            }



        }
    }


    public IEnumerator FadeOutObject(Renderer renderer)
    {
        
        float fadeTime = this.fadeTime;
        renderer.material.shader = transparentShader;
        while (renderer.material.GetFloat(shaderAlpha) > fadeAmount)
        {
            renderer.material.SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) - fadeTime * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FadeInObject(Renderer renderer)
    {
        float fadeTime = this.fadeTime;
        
        while (renderer.material.GetFloat(shaderAlpha) < 1)
        {
            renderer.material.SetFloat(shaderAlpha, renderer.material.GetFloat(shaderAlpha) + fadeTime * Time.deltaTime);
            yield return null;
        }
        renderer.material.shader = opaqueShader;
    }
}
