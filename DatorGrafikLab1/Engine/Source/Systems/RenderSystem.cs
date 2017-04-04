using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using Engine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Source.Systems
{
    public class RenderSystem : IRender
    {
        ///Might be a test class, if we dont need it then it cant be removed, used it to write some triangles and such
        public void draw(GameTime gameTime)
        {
            ///device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColor.VertexDeclaration);

            Dictionary<int, IComponent> componentDict = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();

            foreach(var entity in componentDict)
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(entity.Key))
                {
                    
                    
                    ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity.Key);
                    GraphicsDevice device = SystemManager.Instance.device;



                    //Effect basicEffect = new BasicEffect(device);


                    //basicEffect.CurrentTechnique = basicEffect.Techniques[2];

                    //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                    //{
                    //    
                    //    pass.Apply();
                    //    device.DrawUserPrimitives(PrimitiveType.TriangleList, mcp.vertices, 0, 1, VertexPositionColor.VertexDeclaration);

                    //}

                    SpriteBatch sp = SystemManager.Instance.spriteBatch;

                    Texture2D SimpleTexture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);

                    Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
                    SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

                    // Paint a 100x1 line starting at 20, 50
                    sp.Draw(SimpleTexture, new Rectangle(20, 50, 100, 1), Color.Blue);

                } 
                
            }
        }

       
    }
}
