
using UnityEngine;

public class NoiseGenerator 
{
    public float[,] GenerateNoise(int width, int height, float scale)
    {
        float[,] noise = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = j / scale;
                float y = i / scale;

                noise[i, j] = Mathf.PerlinNoise(y, x);
            }
        }

        return noise;
    }
}
