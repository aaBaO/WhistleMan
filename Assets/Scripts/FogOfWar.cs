using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    public Camera fogOfWarCamera;
    public RawImage fogOfWarMainImage;
    RenderTexture m_fogOfWarRt;

    public int rtWidth = 1024;

    void Start()
    {
        fogOfWarCamera = GetComponent<Camera>();
        float ratio = Screen.width * 1.0f / Screen.height; 
        int width = rtWidth;
        int height = (int)(rtWidth / ratio);

        RenderTextureDescriptor rtd = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32);
        rtd.depthBufferBits = 0;
        rtd.useMipMap = false;
        RenderTexture rt = new RenderTexture(rtd);
        rt.name = "FogOfWarMain";
        rt.filterMode = FilterMode.Point;
        fogOfWarCamera.targetTexture = rt;

        fogOfWarMainImage.texture = rt;
    }

}
