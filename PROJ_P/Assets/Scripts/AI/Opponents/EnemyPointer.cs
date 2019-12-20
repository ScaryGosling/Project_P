using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPointer : MonoBehaviour
{
    GameObject arrow;
    private List<GameObject> arrowPool = new List<GameObject>();
    private int arrowPoolCursor = 0;
    [SerializeField]private Color32 arrowColor = new Color32(255, 00, 0, 100);
    [SerializeField] [Range(0,10)]private float scale = 5f;
    [SerializeField] [Range(0,1)]private float distance = 0.9f;
    private List<Unit> enemies = new List<Unit>();
    private Vector3 screenpos, screenCenter, screenBounds;
    private float angle, cos, sin, m;
    public static EnemyPointer instance;
    [SerializeField] private float seconds = 30;
    [SerializeField] private int enemiesToShow = 10;
    private float countDown;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        countDown = seconds;
    }
    void LateUpdate()
    {
        if (enemies.Count <= enemiesToShow && enemies.Count != 0)
        {
            if (countDown <= 0)
            {
                PaintArrow();

            }
            else
            {
                countDown -= Time.deltaTime;
            }
        }
        else
        {
            countDown = seconds;
        }
        CleanPool();
    }

    private void PaintArrow()
    {
        ResetPool();
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
    public void AddToList (Unit unit)
    {
        enemies.Add(unit);
    }
    public void RemoveFromList(Unit unit)
    {
        enemies.Remove(unit);
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
            output = BowoniaPool.instance.GetFromPool(PoolObject.ENEMY_POINTER);
            output.transform.SetParent(transform);
            output.transform.localScale  = Vector3.one * scale;
            arrowPool.Add(output);
        }
        arrowPoolCursor++;
        return output;
    }
    GameObject obj;
    private void CleanPool()
    {
        while (arrowPool.Count > arrowPoolCursor)
        {
            obj = arrowPool[arrowPool.Count - 1];
            arrowPool.Remove(obj);
            BowoniaPool.instance.AddToPool(PoolObject.ENEMY_POINTER, obj);
        }
    }
}
