using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanoidTest
{
    public abstract class CuboidMesh : IGameObject
    {
        protected GraphicsDevice GraphicsDevice;
        protected VertexBuffer VertexBuffer;
        public const short TEXTURE_WRAP_N = 1;

        protected Matrix World = Matrix.Identity;
        private float _sizeX = 1f;
        private float _sizeY = 1f;
        private float _sizeZ = 1f;

        public CuboidMesh(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;

            Initialize();
        }

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
            VertexPositionNormalTexture[] nonIndexedCube = new VertexPositionNormalTexture[36];

            float dX = _sizeX / 2;
            float dY = _sizeY / 2;
            float dZ = _sizeZ / 2;

            Vector3 topLeftFront = new Vector3(-dX, dY, dZ);
            Vector3 bottomLeftFront = new Vector3(-dX, -dY, dZ);
            Vector3 topRightFront = new Vector3(dX, dY, dZ);
            Vector3 bottomRightFront = new Vector3(dX, -dY, dZ);
            Vector3 topLeftBack = new Vector3(-dX, dY, -dZ);
            Vector3 topRightBack = new Vector3(dX, dY, -dZ);
            Vector3 bottomLeftBack = new Vector3(-dX, -dY, -dZ);
            Vector3 bottomRightBack = new Vector3(dX, -dY, -dZ);

            // Front face
            nonIndexedCube[0] =
                    new VertexPositionNormalTexture(topLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[1] =
                    new VertexPositionNormalTexture(bottomLeftFront, new Vector3(), new Vector2(TEXTURE_WRAP_N,0));
            nonIndexedCube[2] =
                    new VertexPositionNormalTexture(topRightFront, new Vector3(), new Vector2(0,0));
            nonIndexedCube[3] =
                    new VertexPositionNormalTexture(bottomLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[4] =
                    new VertexPositionNormalTexture(bottomRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[5] =
                    new VertexPositionNormalTexture(topRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            // Back face 
            nonIndexedCube[6] =
                    new VertexPositionNormalTexture(topLeftBack, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[7] =
                    new VertexPositionNormalTexture(topRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));
            nonIndexedCube[8] =
                    new VertexPositionNormalTexture(bottomLeftBack, new Vector3(), new Vector2(0,0));
            nonIndexedCube[9] =
                    new VertexPositionNormalTexture(bottomLeftBack, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[10] =
                    new VertexPositionNormalTexture(topRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[11] =
                    new VertexPositionNormalTexture(bottomRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            // Top face
            nonIndexedCube[12] =
                    new VertexPositionNormalTexture(topLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[13] =
                    new VertexPositionNormalTexture(topRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));
            nonIndexedCube[14] =
                    new VertexPositionNormalTexture(topLeftBack, new Vector3(), new Vector2(0, 0));
            nonIndexedCube[15] =
                    new VertexPositionNormalTexture(topLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[16] =
                    new VertexPositionNormalTexture(topRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[17] =
                    new VertexPositionNormalTexture(topRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            // Bottom face 
            nonIndexedCube[18] =
                    new VertexPositionNormalTexture(bottomLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[19] =
                    new VertexPositionNormalTexture(bottomLeftBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));
            nonIndexedCube[20] =
                    new VertexPositionNormalTexture(bottomRightBack, new Vector3(), new Vector2(0, 0));
            nonIndexedCube[21] =
                    new VertexPositionNormalTexture(bottomLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[22] =
                    new VertexPositionNormalTexture(bottomRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[23] =
                    new VertexPositionNormalTexture(bottomRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            // Left face
            nonIndexedCube[24] =
                    new VertexPositionNormalTexture(topLeftFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[25] =
                    new VertexPositionNormalTexture(bottomLeftBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));
            nonIndexedCube[26] =
                    new VertexPositionNormalTexture(bottomLeftFront, new Vector3(), new Vector2(0, 0));
            nonIndexedCube[27] =
                    new VertexPositionNormalTexture(topLeftBack, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[28] =
                    new VertexPositionNormalTexture(bottomLeftBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[29] =
                    new VertexPositionNormalTexture(topLeftFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            // Right face 
            nonIndexedCube[30] =
                    new VertexPositionNormalTexture(topRightFront, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[31] =
                    new VertexPositionNormalTexture(bottomRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));
            nonIndexedCube[32] =
                    new VertexPositionNormalTexture(bottomRightBack, new Vector3(), new Vector2(0, 0));
            nonIndexedCube[33] =
                    new VertexPositionNormalTexture(topRightBack, new Vector3(), new Vector2(0, TEXTURE_WRAP_N));
            nonIndexedCube[34] =
                    new VertexPositionNormalTexture(topRightFront, new Vector3(), new Vector2(TEXTURE_WRAP_N, TEXTURE_WRAP_N));
            nonIndexedCube[35] =
                    new VertexPositionNormalTexture(bottomRightBack, new Vector3(), new Vector2(TEXTURE_WRAP_N, 0));

            VertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, 36, BufferUsage.WriteOnly);
            VertexBuffer.SetData(nonIndexedCube);

        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(BasicEffect effect, Matrix world) { }

    }
}
