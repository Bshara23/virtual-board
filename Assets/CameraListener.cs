using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraListener : MonoBehaviour
{
    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture();
        var renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webCamTexture;
        webCamTexture.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
