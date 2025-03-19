using UnityEditor;
using UnityEngine;

namespace UpsideDown.UI
{
    public class CoolShaderScript : MonoBehaviour
    {
        private static readonly int VolumeTex = Shader.PropertyToID("_VolumeTex");
        [SerializeField] private int textureSize = 32;
        [SerializeField] private Material noiseMaterial;
        [SerializeField] private float noiseScale = 0.1f;
        [SerializeField] private float noiseStrength = 1.0f;

        void Start()
        {
            GenerateNoiseVolume();
        }

        void GenerateNoiseVolume()
        {
            Texture3D noiseTexture = new(textureSize, textureSize, textureSize, TextureFormat.RGBA32, false);
            Color[] colors = new Color[textureSize * textureSize * textureSize];

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    for (int z = 0; z < textureSize; z++)
                    {
                        float nx = x / (float)textureSize;
                        float ny = y / (float)textureSize;
                        float nz = z / (float)textureSize;

                        // Perlin noise across all axes
                        float noiseValue = Mathf.PerlinNoise(nx * noiseScale, ny * noiseScale) * 
                                           Mathf.PerlinNoise(ny * noiseScale, nz * noiseScale) * 
                                           Mathf.PerlinNoise(nz * noiseScale, nx * noiseScale);

                        noiseValue = Mathf.Clamp01(noiseValue * noiseStrength);

                        // Store in RGBA format (adjust colors if needed)
                        colors[x + y * textureSize + z * textureSize * textureSize] = new Color(noiseValue, noiseValue, noiseValue, 1.0f);
                    }
                }
            }

            noiseTexture.SetPixels(colors);
            noiseTexture.Apply();

            // Assign to material
            if (noiseMaterial != null)
            {
                noiseMaterial.SetTexture(VolumeTex, noiseTexture);
            }
        }
    }
}
