using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Humanoid
{
    public class Leg : CuboidMesh
    {
        private const float MAXROTATION = MathHelper.PiOver4;

        private Vector3 _rotation = new Vector3(0, -9, 0);
        private Vector3 _position = new Vector3(0, 1.5f, 0);
        private Vector3 _jointPos;
        private bool direction;

        public Leg(GraphicsDevice graphics, float sizeX, float sizeY, float sizeZ, Vector3 jointPos, Vector3 rot, bool dir) : base(graphics, sizeX, sizeY, sizeZ)
        {
            _jointPos = jointPos;
            _rotation = rot;
            direction = dir;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (_rotation.Y < MAXROTATION)
                {
                    if (direction)
                        _rotation = new Vector3(_rotation.X, _rotation.Y + 0.02f, _rotation.Z);
                    else
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.02f, _rotation.Z);
                }
                else
                {
                    if (!direction)
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.02f, _rotation.Z);
                    else
                        _rotation = new Vector3(_rotation.X, _rotation.Y + 0.02f, _rotation.Z);
                }

                if (_rotation.Y > MAXROTATION || _rotation.Y < -MAXROTATION)
                    direction = !direction;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (_rotation.Y < MAXROTATION)
                {
                    if (direction)
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.03f, _rotation.Z);
                    else
                        _rotation = new Vector3(_rotation.X, _rotation.Y + 0.03f, _rotation.Z);
                }
                else
                {
                    if (!direction)
                        _rotation = new Vector3(_rotation.X, _rotation.Y -+ 0.03f, _rotation.Z);
                    else
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.03f, _rotation.Z);
                }

                if (_rotation.Y > MAXROTATION || _rotation.Y < -MAXROTATION)
                    direction = !direction;
            }

            World = Matrix.Identity *
                Matrix.CreateTranslation(_position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_jointPos);
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            effect.World = World * world;
            foreach (EffectPass ep in effect.CurrentTechnique.Passes)
            {
                ep.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
    }
}
