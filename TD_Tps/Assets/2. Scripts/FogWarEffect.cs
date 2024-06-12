using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FogWarEffect : MonoBehaviour
{
    public Material fogMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (fogMaterial != null)
        {
            Graphics.Blit(src, dest, fogMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
