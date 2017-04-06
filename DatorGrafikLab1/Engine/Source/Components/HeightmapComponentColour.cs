using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Source.Structs;

namespace Engine.Source.Components
{
    public class HeightmapComponentColour : IComponent
    {
        private int TerrainHeight { get; set; }
        private int TerrainWidth { get; set; }

        public Texture2D HeightMap { get; set; }
        private GraphicsDevice Device;

        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexPositionColorNormal[] Vertices { get; set; }
        public float[,] HeightData { get; set; }
        public int[] Indices { get; set; }
        public BasicEffect Effect { get; set; }

        public HeightmapComponentColour(Texture2D heightMap, GraphicsDevice device)
        {
            this.HeightMap = heightMap;
            this.Device = device;
            LoadHeightData(heightMap);
            SetUpVertices();
            SetUpIndices();
            CalculateNormals();

            CopyToBuffers();
            SetUpEffect();
        }

        private void SetUpEffect()
        {
            Effect = new BasicEffect(Device);
            Effect.EnableDefaultLighting();

            // färg
            Effect.VertexColorEnabled = true;
            // lite fog stuff
            Effect.FogEnabled = true;
            Effect.FogStart = 50;
            Effect.FogEnd = 400;
            Effect.FogColor = Color.SeaShell.ToVector3();

        }

        private void LoadHeightData(Texture2D heightMap)
        {
            TerrainWidth = heightMap.Width;
            TerrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[TerrainWidth * TerrainHeight];
            heightMap.GetData(heightMapColors);

            HeightData = new float[TerrainWidth, TerrainHeight];
            for (int x = 0; x < TerrainWidth; x++)
                for (int y = 0; y < TerrainHeight; y++)
                    HeightData[x, y] = heightMapColors[x + y * TerrainWidth].R;
        }

        private void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < TerrainWidth; x++)
            {
                for (int y = 0; y < TerrainHeight; y++)
                {
                    if (HeightData[x, y] < minHeight)
                        minHeight = HeightData[x, y];
                    if (HeightData[x, y] > maxHeight)
                        maxHeight = HeightData[x, y];
                }
            }

            Vertices = new VertexPositionColorNormal[TerrainWidth * TerrainHeight];
            for (int x = 0; x < TerrainWidth; x++)
            {
                for (int y = 0; y < TerrainHeight; y++)
                {
                    Vertices[x + y * TerrainWidth].Position = new Vector3(x, HeightData[x, y], -y);

                    if (HeightData[x, y] < minHeight + (maxHeight - minHeight) * 0.25)
                        Vertices[x + y * TerrainWidth].Color = Color.Blue;
                    else if (HeightData[x, y] < minHeight + (maxHeight - minHeight) * 0.5)
                        Vertices[x + y * TerrainWidth].Color = Color.Green;
                    else if (HeightData[x, y] < minHeight + (maxHeight - minHeight) * 0.75)
                        Vertices[x + y * TerrainWidth].Color = Color.Brown;
                    else
                        Vertices[x + y * TerrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices()
        {
            Indices = new int[(TerrainWidth - 1) * (TerrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < TerrainHeight - 1; y++)
            {
                for (int x = 0; x < TerrainWidth - 1; x++)
                {
                    int lowerLeft = (x + y * TerrainWidth);
                    int lowerRight = ((x + 1) + y * TerrainWidth);
                    int topLeft = (x + (y + 1) * TerrainWidth);
                    int topRight = ((x + 1) + (y + 1) * TerrainWidth);

                    Indices[counter++] = topLeft;
                    Indices[counter++] = lowerRight;
                    Indices[counter++] = lowerLeft;

                    Indices[counter++] = topLeft;
                    Indices[counter++] = topRight;
                    Indices[counter++] = lowerRight;
                }
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < Indices.Length / 3; i++)
            {
                int index1 = Indices[i * 3];
                int index2 = Indices[i * 3 + 1];
                int index3 = Indices[i * 3 + 2];

                Vector3 side1 = Vertices[index1].Position - Vertices[index3].Position;
                Vector3 side2 = Vertices[index1].Position - Vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                Vertices[index1].Normal += normal;
                Vertices[index2].Normal += normal;
                Vertices[index3].Normal += normal;
            }

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i].Normal.Normalize();
        }

        private void CopyToBuffers()
        {
            VertexBuffer = new VertexBuffer(Device, VertexPositionColorNormal.VertexDeclaration, Vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Vertices);

            IndexBuffer = new IndexBuffer(Device, typeof(int), Indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(Indices);
        }

    }
}
