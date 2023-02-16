using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EdgeDetectionPostEffect : BasePostEffect
{
    public Color _EdgeColor = Color.white;
    public Color _BackgroundColor = Color.white;

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
            this.material.SetColor("_EdgeColor", _EdgeColor);
            this.material.SetColor("_BackgroundColor", _BackgroundColor);

            Graphics.Blit(source, destination, this.material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
