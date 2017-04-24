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
    public class Body : CuboidMesh
    {
        private List<IGameObject> _children = new List<IGameObject>();
        private int entityId;

        public Body(GraphicsDevice graphics, int entityId)
            : base(graphics, 4, 8, 4)
        {
            this.entityId = entityId;
            _children.Add(new Leg(graphics, 1f, 4f, 1f, new Vector3(2, -4, 0), new Vector3(0, 0, MathHelper.Pi), true));
            _children.Add(new Leg(graphics, 1f, 4f, 1f, new Vector3(-2, -4, 0), new Vector3(0, 0f, MathHelper.Pi), false));
            _children.Add(new Head(graphics, new Vector3(0, 3.5f, 0)));
        }

        public override void Update(GameTime gameTime)
        {

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);
            var heightId = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>()[0];
            var heightMap = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(heightId);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                transformComp.Position += transformComp.Forward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                transformComp.Position -= transformComp.Forward;
            }

            var rot = transformComp.Rotation;
            rot.X = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                rot.X += 0.02f;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                rot.X -= 0.02f;
            transformComp.Rotation = rot;
            try
            {
                float terrainHeight = heightMap.HeightMapData[(int)transformComp.Position.X, Math.Abs((int)transformComp.Position.Z)];

                terrainHeight += 8;

                transformComp.Position = new Vector3(transformComp.Position.X, terrainHeight, transformComp.Position.Z);
            }
            catch (Exception)
            {
            }
            
            World = transformComp.ObjectMatrix;

            foreach (IGameObject go in _children)
                go.Update(gameTime);
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

            foreach (IGameObject go in _children)
                go.Draw(effect, World * world);
        }
    }
}
