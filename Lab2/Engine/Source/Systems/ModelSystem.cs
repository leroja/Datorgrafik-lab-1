using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Source.Components;
using Engine.Source.Managers;

namespace Engine.Source.Systems
{
    public class ModelSystem : IRender
    {
        CameraComponent defaultCam;
        public override void Draw(GameTime gameTime)
        {
            //Check for all entities with a camera
            List<int> entitiesWithCamera = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            //pick one
            defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entitiesWithCamera.First());

            Dictionary<int,IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            foreach(var entity in mc)
            {
               
                if (ComponentManager.Instance.CheckIfEntityHasComponent<TransformComponent>(entity.Key))
                {
                    //Hämta ut Modelcomponenten
                    ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
                    //Hämta ut Transformcomponenten
                    TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);
                    //Check if model has it's own camera, if not use default
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<CameraComponent>(entity.Key))
                        defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);

                    if (mcp.MeshWorldMatrices != null)
                    {
                        for (int index = 0; index < mcp.Model.Meshes.Count; index++)
                        {
                            ModelMesh mesh = mcp.Model.Meshes[index];
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.EnableDefaultLighting();
                                effect.PreferPerPixelLighting = true;


                                effect.World = mesh.ParentBone.Transform * mcp.MeshWorldMatrices[index] * tfc.ObjectMatrix;
                                effect.View = defaultCam.ViewMatrix;
                                effect.Projection = defaultCam.ProjectionMatrix;
                            }
                            mesh.Draw();
                        }
                    }
                    else
                    {
                        foreach (ModelMesh modelMesh in mcp.Model.Meshes)
                        {
                            //Check if model has it's own camera, if not use default
                            if (ComponentManager.Instance.CheckIfEntityHasComponent<CameraComponent>(entity.Key))
                                defaultCam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);

                            foreach (BasicEffect effect in modelMesh.Effects)
                            {

                                Matrix objectWorld = tfc.ObjectMatrix;
                                effect.World = modelMesh.ParentBone.Transform * objectWorld; //* mcp.WorldMatrix;
                            
                                effect.View = defaultCam.ViewMatrix;
                                effect.Projection = defaultCam.ProjectionMatrix;

                                effect.EnableDefaultLighting();
                                effect.LightingEnabled = true;

                                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                                {
                                    pass.Apply();
                                    modelMesh.Draw();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
