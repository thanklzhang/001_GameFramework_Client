using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BSCPostEffect : BasePostEffect
{
    [Range(0,3)]
    public float bright = 1;
    [Range(0, 3)]
    public float saturation = 1;
    [Range(0, 3)]
    public float contrast = 1;

    public Shader shader;
    private bool isSupport = true;
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isSupport && material != null)
        {
            this.material.SetFloat("_Bright", bright);
            this.material.SetFloat("_Saturation", saturation);
            this.material.SetFloat("_Contrast", contrast);
            Graphics.Blit(source, destination, this.material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
