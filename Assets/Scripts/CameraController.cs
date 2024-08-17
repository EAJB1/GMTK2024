using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float lerp;
    [SerializeField] Transform target;
    [SerializeField] Transform[] allBounds;
    
    float[] distance;

    void Start()
    {
        distance = new float[allBounds.Length];
    }

    void Update()
    {
        FindClosestBounds();
    }

    void LateUpdate()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        Vector3 newPosition;

        newPosition = cameraOffset + Player.instance.transform.position;

        if (InBounds())
        {
            Lerp(target.position);
            return;
        }

        Lerp(newPosition);
    }

    bool InBounds()
    {
        Vector3 bounds = target.localScale;

        Vector2 distance = target.position - Player.instance.transform.position;

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

    void FindClosestBounds()
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
    }
}
