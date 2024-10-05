using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPP : MonoBehaviour
{

    [SerializeField] Material mat;

    void OnEnable()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
