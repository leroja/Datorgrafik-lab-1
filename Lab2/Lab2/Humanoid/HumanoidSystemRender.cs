using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Components;

namespace Lab2.Humanoid
{
    public class HumanoidSystemRender : IRender
    {
        public override void Draw(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<HumanoidComponent>();
            var CameraId = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>()[0];
            var camera = ComponentManager.GetEntityComponent<CameraComponent>(CameraId);
            foreach (var id in ids)
            {
                var humanoidComp = ComponentManager.GetEntityComponent<HumanoidComponent>(id);
                humanoidComp.Effect.Projection = camera.ProjectionMatrix;
                humanoidComp.Effect.View = camera.ViewMatrix;

                humanoidComp.Humanoid.Draw(humanoidComp.Effect, Matrix.Identity);
            }
        }
    }
}
