using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class HeightmapComponentTexture : IComponent
    {

        private GraphicsDevice graphicsDevice;

        // heightMap
        private Texture2D heightMap;
        private Texture2D heightMapTexture;
        private VertexPositionTexture[] vertices;
        private int width;
        private int height;

        public BasicEffect Effect { get; set; }
        public int[] Indices { get; set; }

        // array to read heightMap data
        float[,] heightMapData;

        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }

        public HeightmapComponentTexture(GraphicsDevice graphicsDevice, Texture2D heightMap, Texture2D heightMapTexture)
        {
            this.graphicsDevice = graphicsDevice;
            this.heightMap = heightMap;
            this.heightMapTexture = heightMapTexture;
            SetHeightMapData();
        }

        public void SetHeightMapData()
        {
            width = heightMap.Width;
            height = heightMap.Height;
            SetHeights();
            SetVertices();
            SetIndices();
            SetEffects();
        }

        public void SetHeights()
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

        public void SetIndices()
        {
            // amount of triangles
            Indices = new int[6 * (width - 1) * (height - 1)];
            int number = 0;
            // collect data for corners
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // create double triangles
                    Indices[number] = x + (y + 1) * width;      // up left
                    Indices[number + 1] = x + y * width + 1;        // down right
                    Indices[number + 2] = x + y * width;            // down left
                    Indices[number + 3] = x + (y + 1) * width;      // up left
                    Indices[number + 4] = x + (y + 1) * width + 1;  // up right
                    Indices[number + 5] = x + y * width + 1;        // down right
                    number += 6;
                }
            }
            IndexBuffer = new IndexBuffer(graphicsDevice, typeof(int), Indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(Indices);
        }

        public void SetVertices()
        {
            vertices = new VertexPositionTexture[width * height];
            Vector2 texturePosition;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);
                    vertices[x + y * width] = new VertexPositionTexture(new Vector3(x, heightMapData[x, y], -y), texturePosition);
                }
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);
        }



        public void SetEffects()
        {
            Effect = new BasicEffect(graphicsDevice)
            {
                
                Texture = heightMapTexture,
                TextureEnabled = true,
                // lite fog stuff
                FogEnabled = true,
                FogStart = 50,
                FogEnd = 400,
                FogColor = Color.Aquamarine.ToVector3()
        };
        }

    }
}
