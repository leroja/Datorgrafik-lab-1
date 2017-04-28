using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Humanoid
{
    public abstract class CuboidMesh : IGameObject
    {
        protected GraphicsDevice GraphicsDevice;
        protected VertexBuffer VertexBuffer;

        protected VertexPositionNormalTexture[] vertices;
        protected VertexBuffer vertexBuffer;

        protected short[] indices;
        protected IndexBuffer indexBuffer;
        
        protected Matrix World = Matrix.Identity;
        private float _sizeX;
        private float _sizeY;
        private float _sizeZ;

        // Normals
        private static readonly Vector3 RIGHT = new Vector3(1, 0, 0); // +X
        private static readonly Vector3 LEFT = new Vector3(-1, 0, 0); // -X
        private static readonly Vector3 UP = new Vector3(0, 1, 0); // +Y
        private static readonly Vector3 DOWN = new Vector3(0, -1, 0); // -Y
        private static readonly Vector3 FORWARD = new Vector3(0, 0, 1); // +Z
        private static readonly Vector3 BACKWARD = new Vector3(0, 0, -1); // -Z


        public CuboidMesh(GraphicsDevice graphics, float sizeX, float sizeY, float sizeZ)
        {
            GraphicsDevice = graphics;

            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;

            Initialize();
        }

        private void Initialize()
        {
            SetupIndices();
            SetupVertices();
            SetupVertexBuffer();
            SetupIndexBuffer();
        }


        private void SetupVertices()
        {
            List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>(36);
            
            float dX = _sizeX / 2;
            float dY = _sizeY / 2;
            float dZ = _sizeZ / 2;
            
            Vector3 FRONT_TOP_LEFT = new Vector3(-dX, dY, dZ);
            Vector3 FRONT_TOP_RIGHT = new Vector3(dX, dY, dZ);
            Vector3 FRONT_BOTTOM_LEFT = new Vector3(-dX, -dY, dZ);
            Vector3 FRONT_BOTTOM_RIGHT = new Vector3(dX, -dY, dZ);
            Vector3 BACK_TOP_LEFT = new Vector3(-dX, dY, -dZ);
            Vector3 BACK_TOP_RIGHT = new Vector3(dX, dY, -dZ);
            Vector3 BACK_BOTTOM_LEFT = new Vector3(-dX, -dY, -dZ);
            Vector3 BACK_BOTTOM_RIGHT = new Vector3(dX, -dY, -dZ);
            
            // Front face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, FORWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, FORWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, FORWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, FORWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, FORWARD, new Vector2(1, 0)));

            // Top face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, UP, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, UP, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, UP, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, UP, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, UP, new Vector2(1, 0)));

            // Right face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, RIGHT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, RIGHT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_RIGHT, RIGHT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, RIGHT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, RIGHT, new Vector2(1, 0)));

            // Bottom face
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, DOWN, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, DOWN, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, DOWN, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_RIGHT, DOWN, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, DOWN, new Vector2(1, 0)));

            // Left face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, LEFT, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, LEFT, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, LEFT, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_TOP_LEFT, LEFT, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(FRONT_BOTTOM_LEFT, LEFT, new Vector2(1, 0)));

            // Back face
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, BACKWARD, new Vector2(1, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_RIGHT, BACKWARD, new Vector2(0, 0)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_RIGHT, BACKWARD, new Vector2(0, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_TOP_LEFT, BACKWARD, new Vector2(1, 1)));
            vertexList.Add(new VertexPositionNormalTexture(BACK_BOTTOM_LEFT, BACKWARD, new Vector2(1, 0)));

            vertices = vertexList.ToArray();
        }

        private void SetupVertexBuffer()
        {
            if (vertexBuffer == null)
            {
                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
                vertexBuffer.SetData(vertices);
            }
        }

        private void SetupIndices()
        {
            List<short> indexList = new List<short>(36);

            for (short i = 0; i < 36; ++i)
                indexList.Add(i);

            indices = indexList.ToArray();
        }

        private void SetupIndexBuffer()
        {
            indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.None);
            indexBuffer.SetData(indices);
        }
        
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(BasicEffect effect, Matrix world) { }

    }
}
