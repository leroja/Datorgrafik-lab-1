using Engine.Source.Components;
using Engine.Source.Managers;
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
    public class Head : CuboidMesh
    {
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _position = new Vector3(0, 1f, 0);
        private Vector3 _jointPos;
        
        public Head(GraphicsDevice graphics, Vector3 jointPos)
            : base(graphics, 1.5f, 1.5f, 1.5f)
        {
            _jointPos = jointPos;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                _rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                _rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);

            World = Matrix.Identity *
                Matrix.CreateTranslation(_position) *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_jointPos);
        }

        public override void Draw(BasicEffect effect, Matrix world)
        {
            effect.World = World * world;
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);
        }
    }
}
