using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EdgeDetectionPost_New_Effect : BasePostEffect
{
    public Color _EdgeColor = Color.white;
    public Color _BackgroundColor = Color.white;

    //采样像素距离
    public float sampleDistance = 1;

    public float normalSensitivity = 1;
    public float depthSensitivity = 1;

    public float edgeOnly = 0;

    public Shader shader;
    private bool isSupport = true;
    private Material material;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        camera = this.GetComponent<Camera>();
        camera.depthTextureMode |= DepthTextureMode.DepthNormals;
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

            this.material.SetFloat("_SampleDistance", sampleDistance);
            this.material.SetFloat("_NormalSensitivity", normalSensitivity);
            this.material.SetFloat("_DepthSensitivity", depthSensitivity);

            this.material.SetFloat("_EdgeOnly", edgeOnly);

            Graphics.Blit(source, destination, this.material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
