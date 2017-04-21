using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanoidTest
{
    public class Leg : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();
        private const float MAXROTATION = MathHelper.PiOver4;
        //private float speed = 0, rotationSpeed = 0.003f;

        //private Vector3 _rotation = Vector3.Zero;
        private Vector3 _rotation = new Vector3(0, -9, 0);
        private Vector3 _position = new Vector3(0, 1.5f, 0);
        //private Vector3 _jointPos = new Vector3(2, -4, 0);
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
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    _rotation = new Vector3(_rotation.X, _rotation.Y + 0.01f, _rotation.Z);

            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //    _rotation = new Vector3(_rotation.X, _rotation.Y - 0.01f, _rotation.Z);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                //speed = 0.1f * (float)gameTime.ElapsedGameTime.Milliseconds;

                if (_rotation.Y < MAXROTATION)
                {
                    if (direction)
                        //modelRotation += speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        _rotation = new Vector3(_rotation.X, _rotation.Y + 0.01f, _rotation.Z);
                    else
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.01f, _rotation.Z);
                        //modelRotation -= speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    if (!direction)
                        //modelRotation -= speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        _rotation = new Vector3(_rotation.X, _rotation.Y - 0.01f, _rotation.Z);
                    else
                        //modelRotation += speed * rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        _rotation = new Vector3(_rotation.X, _rotation.Y + 0.01f, _rotation.Z);
                }

                if (_rotation.Y > MAXROTATION || _rotation.Y < -MAXROTATION)
                    direction = !direction;

                //model.Bones["RightArm"].Transform = TransformPart(rightArmMatrix, modelRotation);
                //model.Bones["LeftArm"].Transform = TransformPart(leftArmMatrix, -modelRotation);
                //model.Bones["RightLeg"].Transform = TransformPart(rightLegMatrix, -modelRotation);
                //model.Bones["LeftLeg"].Transform = TransformPart(leftLegMatrix, modelRotation);
            }

            World = Matrix.Identity *
                Matrix.CreateTranslation(_position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_jointPos);

            foreach (IGameObject go in _children)
                go.Update(gameTime);
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = World * world;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

            foreach (IGameObject go in _children)
                go.Draw(effect, World * world);
        }
    }
}
