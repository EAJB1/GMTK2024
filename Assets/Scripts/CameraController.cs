using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float inBoundsLerp, outBoundsLerp;

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
        Vector3 newPosition = Player.instance.transform.position;

        if (currentBounds != null)
        {
            newPosition += (Vector3)currentBounds.camOffset;

            if (currentBounds.lockY)
            {
                newPosition.y = currentBounds.transform.position.y;
            }

            if (currentBounds.lockX)
            {
                newPosition.x = currentBounds.transform.position.x;
            }

            cam.orthographicSize = currentBounds.cameraSize;

            Lerp(newPosition, currentBounds.lockX ? inBoundsLerp : outBoundsLerp, currentBounds.lockY ? inBoundsLerp : outBoundsLerp);
            return;
        }

        newPosition += cameraOffset;

        Lerp(newPosition, outBoundsLerp, outBoundsLerp);
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

    void Lerp(Vector3 pos, float lerpX, float lerpY)
    {
        float x = Mathf.Lerp(transform.position.x, pos.x, lerpX * Time.deltaTime);
        float y = Mathf.Lerp(transform.position.y, pos.y, lerpY * Time.deltaTime);

        transform.position = new Vector3(x, y, cameraOffset.z);
    }
}
