using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlurMotionPost_New_Effect : BasePostEffect
{
    [Range(0, 1)]
    public float blurAmount = 0.9f;
    [Range(1, 10)]
    public int _Iteration = 1;


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

    private void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Matrix4x4 preToViewProjection_Matrix;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isSupport && material != null)
        {

            this.material.SetFloat("_BlurAmount", blurAmount);
            this.material.SetFloat("_Iteration", _Iteration);

            this.material.SetMatrix("_PreToViewProjection_Matrix", preToViewProjection_Matrix);
            var currToViewProjectionMatrix = camera.projectionMatrix * camera.worldToCameraMatrix;
            Matrix4x4 currToViewProjection_Inverse_Matrix = currToViewProjectionMatrix.inverse;
            this.material.SetMatrix("_CurrToViewProjection_Inverse_Matrix", currToViewProjection_Inverse_Matrix);
            Graphics.Blit(source, destination, this.material);

            preToViewProjection_Matrix = currToViewProjectionMatrix;

        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnDisable()
    {

    }
}
