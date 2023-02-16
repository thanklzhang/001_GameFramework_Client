using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BloomPostEffect : BasePostEffect
{
    [Range(0, 4)]
    public int iterations = 3;
    [Range(0.2f, 3.0f)]
    public float blurSpread = 0.6f;
    [Range(1, 8)]
    public int downSample = 2;
    [Range(0.0f,4.0f)]
    public float luminanceThreshold = 0.6f;

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
            this.material.SetFloat("_LuminanceThreshord", luminanceThreshold);

            int width = source.width / downSample;
            int height = source.height / downSample;

            RenderTexture buffer0 = RenderTexture.GetTemporary(width, height, 0);
            buffer0.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, buffer0,this.material,0);

            for (int i = 0; i < iterations; i++)
            {
                //Vertical Pass
                this.material.SetFloat("_BlurSize", 1 + i * blurSpread);

                RenderTexture buffer1 = RenderTexture.GetTemporary(width, height, 0);
                Graphics.Blit(buffer0, buffer1, this.material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;

                //Horizontal Pass
                buffer1 = RenderTexture.GetTemporary(width, height, 2);
                Graphics.Blit(buffer0, buffer1, this.material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }

            this.material.SetTexture("_BloomTex",buffer0);
            Graphics.Blit(buffer0, destination, this.material,3);

        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
