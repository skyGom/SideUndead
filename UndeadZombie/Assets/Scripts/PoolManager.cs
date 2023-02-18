using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] Prefabs;

    private List<GameObject>[] poolList;

    private void Awake()
    {
        poolList = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < poolList.Length; i++)
        {
            poolList[i] = new List<GameObject>();
        }
    }

    public GameObject GetPool(int index)
    {
        GameObject select = null;

        foreach (GameObject item in poolList[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(Prefabs[index], transform);
            poolList[index].Add(select);
        }

        return select;
    }
}
