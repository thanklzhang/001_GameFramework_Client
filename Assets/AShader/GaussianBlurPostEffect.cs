using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GaussianBlurPostEffect : BasePostEffect
{
    [Range(0, 4)]
    public int iterations = 3;
    [Range(0.2f, 3.0f)]
    public float blurSpread = 0.6f;
    [Range(1, 8)]
    public int downSample = 2;

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
            int width = source.width / downSample;
            int height = source.height / downSample;

            RenderTexture buffer0 = RenderTexture.GetTemporary(width, height, 0);
            buffer0.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, buffer0);

            for (int i = 0; i < iterations; i++)
            {
                //Vertical Pass
                this.material.SetFloat("_BlurSize", blurSpread);

                RenderTexture buffer1 = RenderTexture.GetTemporary(width, height, 0);
                Graphics.Blit(buffer0, buffer1, this.material, 0);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;

                //Horizontal Pass
                buffer1 = RenderTexture.GetTemporary(width, height, 0);
                Graphics.Blit(buffer0, buffer1, this.material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }


            Graphics.Blit(buffer0, destination);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
