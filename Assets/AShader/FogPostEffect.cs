using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FogPostEffect : BasePostEffect
{
    [Range(0, 1)]
    public float fogDensity = 0.9f;

    public Color fogColor = Color.white;

    public float fogStart = 0;

    public float fogEnd = 2;

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
        camera.depthTextureMode |= DepthTextureMode.Depth;

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
            var cameraTran = camera.transform;
            float near = camera.nearClipPlane;
            float aspect = camera.aspect;
            float fov = camera.fieldOfView;

            float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            Vector3 up = cameraTran.up * halfHeight;
            Vector3 right = cameraTran.right * halfHeight * aspect;

            // /near 是因为根据相似三角形进行处理 在 shader 中直接 * depth 即可
            Vector3 topLeft = (cameraTran.forward * near + up + -right) / near;
            Vector3 topRight = (cameraTran.forward * near + up + right) / near;
            Vector3 bottomLeft = (cameraTran.forward * near - up + -right) / near;
            Vector3 bottomRight = (cameraTran.forward * near - up + right) / near;


            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetRow(0, bottomLeft);
            matrix.SetRow(1, bottomRight);
            matrix.SetRow(2, topRight);
            matrix.SetRow(3, topLeft);

            this.material.SetMatrix("_Matrix", matrix);
            this.material.SetFloat("_FogDensity", fogDensity);
            this.material.SetColor("_FogColor", fogColor);
            this.material.SetFloat("_FogStart", fogStart);
            this.material.SetFloat("_FogEnd", fogEnd);
            this.material.SetFloat("_FarDepth", camera.farClipPlane);

            Graphics.Blit(source, destination, this.material);

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
