using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] Camera cam;
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
            if (currentBounds.followX)
            {
                newPosition.y = currentBounds.transform.position.y;
            }

            if (currentBounds.followY)
            {
                newPosition.x = currentBounds.transform.position.x;
            }

            cam.orthographicSize = currentBounds.cameraSize;
        }

        Lerp(newPosition);
    }

    bool InBounds(Bounds currentBounds)
    {
        Vector3 boundsPosition = currentBounds.transform.localScale;

        Vector2 distance = currentBounds.transform.position - Player.instance.transform.position;

        if (distance.x <= boundsPosition.x / 2f &&
            distance.x >= -boundsPosition.x / 2f &&
            distance.y <= boundsPosition.y / 2f &&
            distance.y >= -boundsPosition.y / 2f)
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
}
