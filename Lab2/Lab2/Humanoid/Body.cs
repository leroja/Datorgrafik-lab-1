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
    public class Body : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();

        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _position = Vector3.Zero;

        public Body(GraphicsDevice graphics)
            : base(graphics, 4, 8, 4)
        {

            _children.Add(new Leg(graphics, 1f, 4f, 1f, new Vector3(2, -4, 0), new Vector3(0, 0, MathHelper.Pi), true));
            _children.Add(new Leg(graphics, 1f, 4f, 1f, new Vector3(-2, -4, 0), new Vector3(0, 0f, MathHelper.Pi), false));
            _children.Add(new Head(graphics, new Vector3(0, 3.5f, 0)));
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _position.X += 0.01f;
                _position.Z -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _rotation = new Vector3(_rotation.X + 0.01f, _rotation.Y, _rotation.Z);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _rotation = new Vector3(_rotation.X - 0.01f, _rotation.Y, _rotation.Z);

            World = Matrix.Identity *
                Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z)) *
                Matrix.CreateTranslation(_position);

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
