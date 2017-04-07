using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using Engine.Source.Components;

namespace Engine.Source.Systems
{
    public class ChaseCamSystem : IUpdate
    {
        public void Update(GameTime gameTime)
        {
            var chaseCameraIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<ChaseCamComponent>();

            if (chaseCameraIds == null)
                return;

            foreach (var cameraId in chaseCameraIds)
            {
                var chaseCam = ComponentManager.Instance.GetEntityComponent<ChaseCamComponent>(cameraId);
                var baseCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraId);
                var transform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(cameraId);

                var rotation = Matrix.CreateFromQuaternion(transform.QuaternionRotation);

                if (chaseCam.IsDrunk)
                {
                    var camPosition = chaseCam.OffSet;
                    camPosition += transform.Position;
                    
                    baseCam.Position = camPosition;
                }
                else
                {
                    var camPosition = Vector3.Transform(chaseCam.OffSet, rotation);
                    camPosition += transform.Position;
                    
                    baseCam.Position = camPosition;
                }

                baseCam.UpVector = Vector3.Transform(Vector3.Up, rotation);
                baseCam.LookAt = transform.Position;
            }
        }
    }
}
