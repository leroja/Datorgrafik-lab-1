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
        public void Draw(GameTime gameTime)
        {

            Dictionary<int,IComponent> mc = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            foreach(var entity in mc)
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<TransformComponent>(entity.Key))
                {
                    //Hämta ut Modelcomponenten
                    ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
                    //Hämta ut Transformcomponenten
                    TransformComponent tfc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity.Key);

                    foreach (ModelMesh modelMesh in mcp.model.Meshes)
                    {
                        Vector3 scale = tfc.Scale;
                        Vector3 position = tfc.Position;

                        //System.Console.WriteLine(modelMesh.Name);
                        foreach (BasicEffect effect in modelMesh.Effects)
                        {
                            //In my understanding this calculation should already be done in the transform system, so therefore it should 
                            //just be assigning object world to the value of for tfc.objectworld or mcp.objectworld depending on where the data
                            //should be kept. 
                            //TODO: se the above comment
                            Matrix objectWorld = Matrix.CreateScale(tfc.Scale) * tfc.Rotation * Matrix.CreateTranslation(tfc.Position);
                            effect.World = modelMesh.ParentBone.Transform * objectWorld * mcp.worldMatrix;

                            // need to get the camera here?
                            CameraComponent cmp = ComponentManager.Instance.GetEntityComponent<CameraComponent>(entity.Key);
                            effect.View = cmp.ViewMatrix;
                            effect.Projection = cmp.ProjectionMatrix;

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

            //Working code 
            //foreach (ModelMesh modelMesh in plane.Meshes)
            //{
            //    Vector3 scale = new Vector3(5, 5, 5);
            //    Vector3 position = new Vector3(0, 0, -50);

            //    //System.Console.WriteLine(modelMesh.Name);
            //    foreach (BasicEffect effect in modelMesh.Effects)
            //    {
            //        Matrix objectWorld = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(position);
            //        effect.World = modelMesh.ParentBone.Transform * objectWorld * world;
            //        effect.View = view;
            //        effect.Projection = projection;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;

            //        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            //        {
            //            pass.Apply();
            //            modelMesh.Draw();
            //        }
            //    }
            //}
        }
    }
}
