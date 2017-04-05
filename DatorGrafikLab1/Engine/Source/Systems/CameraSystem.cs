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
        public void Update(GameTime gameTime)
        {
            ComponentManager compMan = ComponentManager.Instance;

            var cameraIds = compMan.GetAllEntitiesWithComponentType<CameraComponent>();
            if (cameraIds == null)
                return;

            foreach (int cameraId in cameraIds){

                var cameraComp = compMan.GetEntityComponent<CameraComponent>(cameraId);

                //var ViewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                //var ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.6f, 1.0f, 1000.0f);

                cameraComp.ViewMatrix = Matrix.CreateLookAt(cameraComp.Position, cameraComp.LookAt, cameraComp.UpVector);
                cameraComp.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, cameraComp.AspectRatio, cameraComp.NearPlane, cameraComp.FarPlane);

            }
        }
    }
}
