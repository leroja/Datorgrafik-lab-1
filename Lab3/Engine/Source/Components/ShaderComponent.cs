using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Source.Components
{
    public class ShaderComponent : IComponent
    {
        public Effect ShaderEffect { private set; get; }
        public Vector4 AmbientColor { set; get; }
        public float AmbientIntensity { set; get; }
        public Vector3 DiffuseLightDirection { get; set; }
        public Vector4 Diffusecolor { get; set; }
        public float DiffuseIntensity { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public Vector4 FogColor { get; set; }
        public bool FogEnabled { get; set; }


        public ShaderComponent(Effect effect)
        {
            ShaderEffect = effect;
            AmbientColor = Color.White.ToVector4();
            AmbientIntensity = 0.1f;
            Diffusecolor = Color.White.ToVector4();
            DiffuseLightDirection = new Vector3(0, 0.5f, 0);
            DiffuseIntensity = 0.1f;
            FogEnabled = false;
            FogStart = 0;
            FogEnd = 0;
            FogColor = Color.SeaShell.ToVector4();
            
        }

        public void AmbientLightSettings(Color AmbientColor, float AmbientIntensity)
        {
            this.AmbientColor = AmbientColor.ToVector4();
            this.AmbientIntensity = AmbientIntensity;
        }
        public void DirectionallightSettings(Vector3 DiffuseLightDirection, Color DiffuseColor, float DiffuseIntensity)
        {
            this.DiffuseLightDirection = DiffuseLightDirection;
            Diffusecolor = DiffuseColor.ToVector4();
            this.DiffuseIntensity = DiffuseIntensity;
        }
        public void ActivateAppropriateFogSettings()
        {
            FogEnabled = true;
            FogEnd = 400;
            FogStart = 50;
            FogColor = Color.LightGray.ToVector4();
        }
        public void AddCustomFog(float FogEnd, float FogStart, Color FogColor)
        {
            this.FogColor = FogColor.ToVector4();
            this.FogEnd = FogEnd;
            this.FogStart = FogStart;
            FogEnabled = true;
        }
    }
}
