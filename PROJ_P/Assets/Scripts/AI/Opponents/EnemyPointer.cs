using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPointer : MonoBehaviour
{
    [SerializeField] private GameObject enemyIndicatorPrefab;
    GameObject arrow;

    private List<GameObject> arrowPool = new List<GameObject>();
    private int arrowPoolCursor = 0;
    [SerializeField] private GameObject parent;
    [SerializeField]private Color32 arrowColor = new Color32(255, 00, 0, 100);
    [SerializeField] [Range(0,1)]private float scale = 0.5f;
    [SerializeField] [Range(0,1)]private float distance = 0.9f;
    private void Start()
    {
 
    }
    void Update()
    {
        PaintArrow();
    }
    Unit[] enemies;
    Vector3 screenpos, screenCenter, screenBounds;
    float angle, cos, sin, m;
    private void PaintArrow()
    {
        ResetPool();
        enemies = FindObjectsOfType(typeof (Unit)) as Unit[];
        foreach (Unit enemy in enemies)
        {
            screenpos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            if (IsOnScreen())
            {
                //if enemy visible
            }
            else
            {
                if (screenpos.z < 0)
                {
                    screenpos *= -1;
                }
                screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
                screenpos -= screenCenter;


                angle = Mathf.Atan2(screenpos.y, screenpos.x);
                angle -= 90 * Mathf.Deg2Rad;

                cos = Mathf.Cos(angle);
                sin = -Mathf.Sin(angle);
                screenpos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

                m = cos / sin;

                screenBounds = screenCenter * distance;


                if (cos > 0)
                {
                    screenpos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                }
                else
                {
                    screenpos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
                }

                if (screenpos.x > screenBounds.x)
                {
                    screenpos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
                }
                else if (screenpos.x < -screenBounds.x)
                {
                    screenpos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
                }

                screenpos += screenCenter;
                screenpos -= transform.position;
                screenpos /= transform.localScale.x;

                arrow = GetArrow();
                arrow.GetComponent<Image>().color = arrowColor; 
                arrow.transform.localPosition = screenpos;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
        }
        CleanPool();
    }

    private bool IsOnScreen()
    {
        return screenpos.z > 0 &&
                        screenpos.x > 0 && screenpos.x < Screen.width &&
                        screenpos.y > 0 && screenpos.y < Screen.height;
    }

    private void ResetPool()
    {
        arrowPoolCursor = 0;
    }

        GameObject output;
    private GameObject GetArrow()
    {
        if (arrowPoolCursor < arrowPool.Count)
        {
            output = arrowPool[arrowPoolCursor];
        }
        else
        {
            output = Instantiate(enemyIndicatorPrefab, parent.transform);
            output.transform.localScale  *= scale;
            arrowPool.Add(output);
        }
        arrowPoolCursor++;
        return output;
    }

    private void CleanPool()
    {
        while (arrowPool.Count > arrowPoolCursor)
        {
            GameObject obj = arrowPool[arrowPool.Count-1];
            arrowPool.Remove(obj);
            Destroy(obj);
        }
    }
}
