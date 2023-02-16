using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlurMotionPostEffect : BasePostEffect
{
    [Range(0, 1)]
    public float blurAmount = 0.9f;

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

    private RenderTexture renderTexCache;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isSupport && material != null)
        {
           

            if (null == renderTexCache || source.width != renderTexCache.width ||
                source.height != renderTexCache.height)
            {
                renderTexCache = new RenderTexture(source.width, source.height, 0);
                renderTexCache.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, renderTexCache);
            }

            renderTexCache.MarkRestoreExpected();

            this.material.SetFloat("_BlurAmount", blurAmount);

            Graphics.Blit(source, renderTexCache, this.material);
            Graphics.Blit(renderTexCache, destination);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnDisable()
    {
        if (renderTexCache != null)
        {
            DestroyImmediate(renderTexCache);
        }

    }
}
