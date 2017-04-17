using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Source.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using Engine.Source.Components;

namespace Engine.Source.Systems
{
    public class CameraSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {

            var cameraIds = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            if (cameraIds == null)
                return;

            foreach (int cameraId in cameraIds){

                var cameraComp = ComponentManager.GetEntityComponent<CameraComponent>(cameraId);

                cameraComp.ViewMatrix = Matrix.CreateLookAt(cameraComp.Position, cameraComp.LookAt, cameraComp.UpVector);
                cameraComp.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, cameraComp.AspectRatio, cameraComp.NearPlane, cameraComp.FarPlane);

                //var test = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, cameraComp.AspectRatio, cameraComp.NearPlane * 0.5f, cameraComp.FarPlane * 1.2f);
                cameraComp.CameraFrustrum = new BoundingFrustum(cameraComp.ViewMatrix * cameraComp.ProjectionMatrix);
            }
        }
    }
}
