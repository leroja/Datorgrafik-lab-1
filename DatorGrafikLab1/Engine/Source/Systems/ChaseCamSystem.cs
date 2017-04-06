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

            var chaseCameras = ComponentManager.Instance.GetAllEntitiesWithComponentType<ChaseCamComponent>();

            if (chaseCameras == null)
                return;

            foreach (var cameraId in chaseCameras)
            {
                var chaseCam = ComponentManager.Instance.GetEntityComponent<ChaseCamComponent>(cameraId);
                var baseCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraId);
                var transform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(cameraId);

                var rotation = Matrix.CreateFromQuaternion(transform.QuaternionRotation);

                if (chaseCam.Tripy)
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
