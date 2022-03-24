using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveTimer : MonoBehaviour
{
    [SerializeField] private float removeTimer = 0.5f;
    private Image image;
    

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (image.enabled)
            StartCoroutine(Timer());

    }

    private IEnumerator Timer() 
    { 
        yield return new WaitForSeconds(removeTimer);
        image.enabled = false;
    }
}
