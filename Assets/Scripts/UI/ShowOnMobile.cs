using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Platform: " + Application.platform);
        gameObject.SetActive(Application.isMobilePlatform);
    }
}
