using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static Engine.Source.Structs;
using Engine.Source.Random_stuff;

namespace Engine.Source.Factories
{
    public class HeightMapFactory
    {
        private GraphicsDevice graphicsDevice;

        // heightMap
        private Texture2D heightMap;
        private Texture2D heightMapTexture;
        public VertexPositionNormalTexture[] verticesTexture { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        private int fractions_per_side;
        private int chunk_width;
        private int chunk_height;

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

        public HeightmapComponentTexture CreateTexturedHeightMap(Texture2D heightMap, Texture2D heightMapTexture, int fractions_per_side)
        {
            HeightmapComponentTexture heightMapComponent = new HeightmapComponentTexture();
            this.heightMap = heightMap;
            this.heightMapTexture = heightMapTexture;
            this.fractions_per_side = fractions_per_side;
            SetHeightMapData(ref heightMapComponent);
            heightMapComponent.Effect = Effect;
            heightMapComponent.IndexBuffer = IndexBuffer;
            heightMapComponent.Indices = Indices;
            heightMapComponent.VertexBuffer = VertexBuffer;
            return heightMapComponent;
        }
        

        private void SetHeightMapData(ref HeightmapComponentTexture comp)
        {
            width = heightMap.Width;
            height = heightMap.Height;
            chunk_height = height / fractions_per_side;
            chunk_width = width / fractions_per_side;
            SetHeights();
            SetVerticesTexture();
            SetIndices();
            CalculateNormals();
            comp.HeightMapData = heightMapData;

            SetUpHeightMapChunks(ref comp);
            //CopyToBuffers();
            //SetEffects();
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
                    heightMapData[x, y] = greyValues[x + y * width].R * 0.2f;
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

        private void SetUpHeightMapChunks(ref HeightmapComponentTexture heightMapComp)
        {
            for (int x = 0; x < width - chunk_width; x += chunk_width)
            { 
                for (int y = 0; y < height - chunk_height; y += chunk_height)
                {
                    
                    Rectangle clipRect = new Rectangle(x, y, chunk_width + 1, chunk_height + 1);
                    var offsetpos = new Vector3(x, 0, -y);

                    //HeightMapChunk chunk = CreateHeightMapChunk(heightMap, new Rectangle(x, y, chunk_width, chunk_height),
                    //new Vector3(x, 0, -y), GetVertexTextureNormals(new Rectangle(x, y, chunk_width, chunk_height)), heightMapTexture);

                    HeightMapChunk chunk = CreateHeightMapChunk(heightMap, clipRect, offsetpos, GetVertexTextureNormals(clipRect), heightMapTexture);
                    
                    heightMapComp.HeightMapChunks.Add(chunk);
                }
            }
        }

        private VertexPositionNormalTexture[] GetVertexTextureNormals(Rectangle rect)
        {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[rect.Width * rect.Height];

            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                {
                    terrainVerts[(x - rect.X) + (y - rect.Y) * rect.Height].Normal = verticesTexture[x + y * height].Normal;
                }
            }
            return terrainVerts;
        }


        private HeightMapChunk CreateHeightMapChunk(Texture2D terrainMap, Rectangle terrainRect, Vector3 offsetPosition, VertexPositionNormalTexture[] vertexNormals, Texture2D texture)
        {
            var chunk = new HeightMapChunk()
            {
                OffsetPosition = offsetPosition
            };

            var heightinfo = CreateHightmap(terrainMap, terrainRect);
            var chunkVertices = InitTerrainVertices(heightinfo, terrainRect);
            var boundingBox = CreateBoundingBox(chunkVertices);
            var sphere = CreateBoundingSphere(chunkVertices);

            var effect = new BasicEffect(graphicsDevice)
            {
                FogEnabled = true,
                FogStart = 10f,
                FogColor = Color.LightGray.ToVector3(),
                FogEnd = 400f,
                TextureEnabled = true,
                Texture = texture
            };

            var indices = InitIndices(terrainRect);
            chunk.indicesDiv3 = indices.Length / 3; // för att slipa göra den här divisionen flera gånger

            //copy the calculated normal values
            CopyNormals(vertexNormals, chunkVertices);

            PrepareBuffers(ref chunk, indices, chunkVertices);
            
            chunk.Effect = effect;
            chunk.BoundingBox = boundingBox;
            chunk.Vertices = chunkVertices;
            chunk.Rectangle = terrainRect;
            chunk.Width = terrainRect.Width;
            chunk.Height = terrainRect.Height;
            return chunk;
        }

        private void CopyNormals(VertexPositionNormalTexture[] vertexNormals, VertexPositionNormalTexture[] vertices)
        {
            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal = vertexNormals[i].Normal;
            }
        }

        private float[,] CreateHightmap(Texture2D terrainMap, Rectangle terrainRect)
        {
            var width = terrainMap.Width;
            var height = terrainMap.Height;

            //get the pixels from the terrain map
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);

            //copy the desired portion of the map
            var heightInfo = new float[terrainRect.Width, terrainRect.Height];
            for (int x = terrainRect.X; x < terrainRect.X + terrainRect.Width; ++x)
            {
                for (int y = terrainRect.Y; y < terrainRect.Y + terrainRect.Height; ++y)
                {
                    heightInfo[x - terrainRect.X, y - terrainRect.Y] = colors[x + y * width].R * 0.2f;
                }
            }
            return heightInfo;
        }

        private int[] InitIndices(Rectangle terrainRect)
        {
            var width = terrainRect.Width;
            var height = terrainRect.Height;
            var indices = new int[(width - 1) * (height - 1) * 6];
            int indicesCount = 0; ;

            for (int y = 0; y < height - 1; ++y)
            {
                for (int x = 0; x < width - 1; ++x)
                {
                    int botLeft = x + y * width;
                    int botRight = (x + 1) + y * width;
                    int topLeft = x + (y + 1) * width;
                    int topRight = (x + 1) + (y + 1) * width;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = botRight;
                    indices[indicesCount++] = botLeft;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = topRight;
                    indices[indicesCount++] = botRight;
                }
            }
            return indices;
        }
        private void InitNormals(int[] indices, VertexPositionNormalTexture[] vertices)
        {
            int indicesLen = indices.Length / 3;
            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal = new Vector3(0f, 0f, 0f);
            }

            for (int i = 0; i < indicesLen; ++i)
            {
                //get indices indexes
                int i1 = indices[i * 3];
                int i2 = indices[i * 3 + 1];
                int i3 = indices[i * 3 + 2];

                //get the two faces
                Vector3 face1 = vertices[i1].Position - vertices[i3].Position;
                Vector3 face2 = vertices[i1].Position - vertices[i2].Position;

                //get the cross product between them
                Vector3 normal = Vector3.Cross(face1, face2);

                //update the normal
                vertices[i1].Normal += normal;
                vertices[i2].Normal += normal;
                vertices[i3].Normal += normal;
            }
        }

        private VertexPositionNormalTexture[] InitTerrainVertices(float[,] heightInfo, Rectangle terrainRect)
        {
            var width = terrainRect.Width;
            var height = terrainRect.Height;
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    terrainVerts[x + y * height].Position = new Vector3(x, heightInfo[x, y], -y);
                    terrainVerts[x + y * height].TextureCoordinate.X = (float)x / (width - 1.0f);
                    terrainVerts[x + y * height].TextureCoordinate.Y = (float)y / (height - 1.0f);
                }
            }
            return terrainVerts;
        }

        private void PrepareBuffers(ref HeightMapChunk chunk, int[] indices, VertexPositionNormalTexture[] vertices)
        {
            chunk.IndexBuffer = new IndexBuffer(graphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            chunk.IndexBuffer.SetData(indices);

            chunk.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            chunk.VertexBuffer.SetData(vertices);
        }

        private BoundingBox CreateBoundingBox(VertexPositionNormalTexture[] vertexArray)
        {
            List<Vector3> points = new List<Vector3>();

            foreach (VertexPositionNormalTexture v in vertexArray)
            {
                points.Add(v.Position);
            }

            BoundingBox b = BoundingBox.CreateFromPoints(points);
            return b;
        }

        private BoundingSphere CreateBoundingSphere(VertexPositionNormalTexture[] vertexArray)
        {
            var first = vertexArray.First();
            var last = vertexArray.Last();
            return BoundingSphere.CreateFromPoints(new List<Vector3> { first.Position, last.Position });
        }

        
    }
}
