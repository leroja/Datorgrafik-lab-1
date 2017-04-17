using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Source.Managers;
using Engine.Source.Components;

namespace Engine.Source.Systems
{
    public class HeightmapSystemColour : IRender
    {
        private GraphicsDevice device;

        public HeightmapSystemColour(GraphicsDevice device)
        {
            this.device = device;
        }

        public override void Draw(GameTime gameTime)
        {
            var ents = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentColour>();

            // Todo fix with camera
            var cameraIds = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            var cameraComp = ComponentManager.GetEntityComponent<CameraComponent>(cameraIds[0]);
            if (ents != null)
            {
                foreach(int heightMapId in ents)
                {
                    var heightMap = ComponentManager.GetEntityComponent<HeightmapComponentColour>(heightMapId);
                    var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightMapId);

                    heightMap.Effect.View = cameraComp.ViewMatrix;
                    heightMap.Effect.Projection = cameraComp.ProjectionMatrix;
                    // Todo fix with world matrix
                    heightMap.Effect.World = transformComp.ObjectMatrix;
                    

                    foreach (var pass in heightMap.Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        device.Indices = heightMap.IndexBuffer;
                        device.SetVertexBuffer(heightMap.VertexBuffer);
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, heightMap.Indices.Length / 3);
                    }
                }
            }
        }
    }
}
