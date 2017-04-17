using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static Engine.Source.Structs;

namespace Engine.Source.Factories
{
    public class HeightMapFactory
    {
        private GraphicsDevice graphicsDevice;

        // heightMap
        private Texture2D heightMap;
        private Texture2D heightMapTexture;
        private VertexPositionNormalTexture[] verticesTexture;
        private int width;
        private int height;

        private BasicEffect Effect;
        private int[] Indices;

        // array to read heightMap data
        private float[,] heightMapData;

        private VertexBuffer VertexBuffer;
        private IndexBuffer IndexBuffer;
        

        public HeightMapFactory(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

        }

        public HeightmapComponentTexture CreateTexturedHeightMap(Texture2D heightMap, Texture2D heightMapTexture, int fractions)
        {
            HeightmapComponentTexture heightMapComponent = new HeightmapComponentTexture();
            this.heightMap = heightMap;
            this.heightMapTexture = heightMapTexture;
            SetHeightMapData();
            heightMapComponent.Effect = Effect;
            heightMapComponent.IndexBuffer = IndexBuffer;
            heightMapComponent.Indices = Indices;
            heightMapComponent.VertexBuffer = VertexBuffer;
            return heightMapComponent;
        }
        

        private void SetHeightMapData()
        {
            width = heightMap.Width;
            height = heightMap.Height;
            SetHeights();
            SetVerticesTexture();
            SetIndices();
            CalculateNormals();
            CopyToBuffers();
            SetEffects();
        }

        private void SetHeights()
        {
            Color[] greyValues = new Color[width * height];
            heightMap.GetData(greyValues);
            heightMapData = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = greyValues[x + y * width].R;
                }
            }
        }

        private void SetIndices()
        {
            Indices = new int[(width - 1) * (height - 1) * 6];
            int counter = 0;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int lowerLeft = (x + y * width);
                    int lowerRight = ((x + 1) + y * width);
                    int topLeft = (x + (y + 1) * width);
                    int topRight = ((x + 1) + (y + 1) * width);

                    Indices[counter++] = topLeft;
                    Indices[counter++] = lowerRight;
                    Indices[counter++] = lowerLeft;

                    Indices[counter++] = topLeft;
                    Indices[counter++] = topRight;
                    Indices[counter++] = lowerRight;
                }
            }
        }
                
        private void SetVerticesTexture()
        {
            verticesTexture = new VertexPositionNormalTexture[width * height];
            Vector2 texturePosition;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texturePosition = new Vector2((float)x / width, (float)y / height);
                    verticesTexture[x + y * width] = new VertexPositionNormalTexture(new Vector3(x, heightMapData[x, y], -y), Vector3.One, texturePosition);
                }
            }
        }


        private void CalculateNormals()
        {
            for (int i = 0; i < verticesTexture.Length; i++)
                verticesTexture[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < Indices.Length / 3; i++)
            {
                int index1 = Indices[i * 3];
                int index2 = Indices[i * 3 + 1];
                int index3 = Indices[i * 3 + 2];

                Vector3 side1 = verticesTexture[index1].Position - verticesTexture[index3].Position;
                Vector3 side2 = verticesTexture[index1].Position - verticesTexture[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                verticesTexture[index1].Normal += normal;
                verticesTexture[index2].Normal += normal;
                verticesTexture[index3].Normal += normal;
            }
            for (int i = 0; i < verticesTexture.Length; i++)
                verticesTexture[i].Normal.Normalize();
        }

        private void CopyToBuffers()
        {
            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, verticesTexture.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(verticesTexture);
            
            IndexBuffer = new IndexBuffer(graphicsDevice, typeof(int), Indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(Indices);
        }

        private void SetEffects()
        {
            Effect = new BasicEffect(graphicsDevice)
            {
                //LightingEnabled = true,

                FogEnabled = true,
                FogStart = 75,
                FogEnd = 400,
                FogColor = Color.SeaShell.ToVector3(),
                TextureEnabled = true,
                Texture = heightMapTexture
            };
        }
    }
}
