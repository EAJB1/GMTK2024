using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float lerp;
    Bounds[] allBounds;

    Bounds currentBounds;

    private void Awake()
    {
        allBounds = FindObjectsByType<Bounds>(FindObjectsSortMode.None);
    }

    void Update()
    {
        currentBounds = null;

        foreach (Bounds b in allBounds)
        {
            if (InBounds(b))
            {
                currentBounds = b;
            }
        }
    }

    void LateUpdate()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        Vector3 newPosition;

        newPosition = cameraOffset + Player.instance.transform.position;

        if (currentBounds != null)
        {
            Lerp(currentBounds.transform.position);
            return;
        }

        Lerp(newPosition);
    }

    bool InBounds(Bounds currentBounds)
    {
        Vector3 bounds = currentBounds.transform.localScale;

        Vector2 distance = currentBounds.transform.position - Player.instance.transform.position;

        if (distance.x <= bounds.x / 2f &&
            distance.x >= -bounds.x / 2f &&
            distance.y <= bounds.y / 2f &&
            distance.y >= -bounds.y / 2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Lerp(Vector3 pos)
    {
        pos.z = cameraOffset.z;
        transform.position = Vector3.Lerp(transform.position, pos, lerp * Time.deltaTime);
    }

    /*void FindClosestBounds()
    {
        float smallest = distance[0];
        Transform closest = allBounds[0];

        for (int i = 0; i < allBounds.Length; i++)
        {
            distance[i] = (allBounds[i].position - Player.instance.transform.position).magnitude;

            if (distance[i] < smallest)
            {
                smallest = distance[i];
                closest = allBounds[i];
            }
        }

        target = closest;
    }*/
}
