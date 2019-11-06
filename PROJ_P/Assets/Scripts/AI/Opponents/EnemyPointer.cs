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

    private void Start()
    {
 
    }
    void Update()
    {
        PaintArrow();
    }

    private void PaintArrow()
    {
        ResetPool();
        Unit[] enemies = GameObject.FindObjectsOfType(typeof (Unit)) as Unit[];
        foreach (Unit enemy in enemies)
        {
            Vector3 screenpos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            if (screenpos.z > 0 &&
                screenpos.x > 0 && screenpos.x < Screen.width &&
                screenpos.y > 0 && screenpos.y < Screen.height)
            {
                //if enemy visible
            }
            else
            {
                screenpos.x -= transform.position.x;
                screenpos.y -= transform.position.y;
                if (screenpos.z < 0)
                {
                    screenpos *= -1;
                }
                Vector3 screenCenter = new Vector3(Screen.width - 2 * transform.position.x, Screen.height - 2 * transform.position.y, 0) / 2;
                screenpos -= screenCenter;

                float angle = Mathf.Atan2(screenpos.y, screenpos.x);
                angle -= 90 * Mathf.Deg2Rad;

                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);
                Debug.Log(screenpos);
                screenpos = screenCenter + new Vector3(sin*150, cos*150, 0);

                float m = cos / sin;

                Vector3 screenBounds = screenCenter * 0.9f;

                if (cos > 0)
                {
                    screenpos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                }
                else
                {
                    screenpos = new Vector3(screenBounds.y / m, -screenBounds.y, 0);
                }

                if (screenpos.x > screenBounds.x)
                {
                    screenpos = new Vector3(screenBounds.x, screenBounds.x*m, 0);
                }
                else
                {
                    screenpos = new Vector3(screenBounds.x, -screenBounds.x*m, 0);
                }

                screenpos += screenCenter;


                arrow = GetArrow();
                arrow.GetComponent<Image>().color = new Color32(255,00,0, 100); ;
                arrow.transform.localPosition = screenpos;
                arrow.transform.localRotation = Quaternion.Euler(0,0, angle * Mathf.Rad2Deg);
            }
        }
        CleanPool();
    }

    private void ResetPool()
    {
        arrowPoolCursor = 0;
    }

    private GameObject GetArrow()
    {
        GameObject output;
        if (arrowPoolCursor < arrowPool.Count)
        {
            output = arrowPool[arrowPoolCursor];
        }
        else
        {
            output = Instantiate(enemyIndicatorPrefab, GameObject.Find("Canvas").transform);
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
