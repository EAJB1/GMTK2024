using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float inBoundsLerp, outBoundsLerp, camSizeLerp;

    Bounds[] allBounds;
    Bounds currentBounds;
    float camSize = 5f;

    private void Awake()
    {
        allBounds = FindObjectsByType<Bounds>(FindObjectsSortMode.None);

        float r = (float)Screen.width / (float)Screen.height;
        float defaultR = 16f / 9f;

        if (r < defaultR)
        {
            cam.orthographicSize = camSize * (defaultR / r);
        }
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

        float targetSize = 5f;

        if (currentBounds != null)
        {
            if (currentBounds.lockY)
            {
                newPosition.y = currentBounds.transform.position.y;
            }

            if (currentBounds.lockX)
            {
                newPosition.x = currentBounds.transform.position.x;
            }

            newPosition += (Vector3)currentBounds.camOffset;

            targetSize = currentBounds.cameraSize;

            Lerp(newPosition, currentBounds.lockX ? inBoundsLerp : outBoundsLerp, currentBounds.lockY ? inBoundsLerp : outBoundsLerp);
        }
        else
        {
            newPosition += cameraOffset;

            Lerp(newPosition, outBoundsLerp, outBoundsLerp);
        }

        if (Mathf.Abs(camSize - targetSize) <= 0.05f)
        {
            camSize = targetSize;
        }
        else
        {
            camSize = Mathf.Lerp(camSize, targetSize, camSizeLerp * Time.deltaTime);
        }

        float r = (float)Screen.width / (float)Screen.height;
        float defaultR = 16f / 9f;

        if (r < defaultR)
        {
            cam.orthographicSize = camSize * (defaultR / r);
        }
        else
        {
            cam.orthographicSize = camSize; 
        }
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
