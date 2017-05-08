#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


matrix World;
matrix View;
matrix Projection;
matrix WorldInverseTranspose;

float4 AmbientColor;
float AmbientIntensity;

float3 DiffuseLightDirection;
float4 DiffuseColor;
float DiffuseIntensity;

float Shininess;
float4 SpecularColor;
float SpecularIntensity;

float3 ViewVector = float3(1, 0, 0);

///FOG
float FogStart = 40;
float FogEnd = 100;
float4 FogColor = float4(1, 0, 0, 1);
bool FogEnabled = 0;
float3 CameraPosition;

bool TextureEnabled = 1;
//texture ModelTexture;
texture Texture;

sampler2D textureSampler = sampler_state
{
    //Texture = (ModelTexture);
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

//texture Texture;
//sampler TextureSampler = sampler_state
//{
//    Texture = (Texture);
//};

float3 LightDirection;
float4x4 LightViewProj;
float DepthBias;
float ShadowStrenght;

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    Texture = <ShadowMap>;
};

struct DrawWithShadowMap_VSIn
{ // samma som testshader
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct DrawWithShadowMap_VSOut
{
    float4 Position : POSITION0;
    float3 Normal : TEXCOORD0;
    float2 TexCoord : TEXCOORD1;
    float4 WorldPos : TEXCOORD2;

    //från testshader som inte finns i shadowMap
    float4 Color : COLOR0;  // behövs color?
    float fogFactor : FOG;
};




struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth : TEXCOORD0;
};

// Transforms the model into light space an renders out the depth of the object
CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : POSITION)
{
    CreateShadowMap_VSOut Out;
    Out.Position = mul(Position, mul(World, LightViewProj));
    Out.Depth = Out.Position.z / Out.Position.w;
    return Out;
}

// Saves the depth value out to the 32bit floating point texture
float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
    return float4(input.Depth, 0, 0, 0);
}










float ComputeFogFactor(float d)
{
    //d is the distance to the geometry sampling from the camera
    //this simply returns a value that interpolates from 0 to 1 
    //with 0 starting at FogStart and 1 at FogEnd 
    return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled;
}




// Draws the model with shadows
DrawWithShadowMap_VSOut DrawWithShadowMap_VertexShader(DrawWithShadowMap_VSIn input)
{
    DrawWithShadowMap_VSOut Output;

    float4x4 WorldViewProj = mul(mul(World, View), Projection);
    
    // Transform the models verticies and normal
    Output.Position = mul(input.Position, WorldViewProj);
    Output.Normal = (float3) normalize(mul(input.Normal, (float3x4) World));
    Output.TexCoord = input.TexCoord;
    
    // Save the vertices postion in world space
    Output.WorldPos = mul(input.Position, World);
    
    return Output;


    // från testshader, todo 
    //VertexShaderOutput output;
    //float distance;

    //float4 worldPosition = mul(input.Position, World);
    //float4 viewPosition = mul(worldPosition, View);
    //float4 normal = mul(input.Normal, WorldInverseTranspose);
    //float lightIntensity = dot(normal.xyz, DiffuseLightDirection);

    //output.Position = mul(viewPosition, Projection);
    //output.TextureCoordinate = input.TextureCoordinate;
    //output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
    //output.Normal = normal;

    //distance = length(worldPosition.xyz - CameraPosition);
    //output.fogFactor = saturate(ComputeFogFactor(distance));

    //return output;

}

// Determines the depth of the pixel for the model and checks to see 
// if it is in shadow or not
float4 DrawWithShadowMap_PixelShader(DrawWithShadowMap_VSOut input) : COLOR
{
    // Color of the model
    float4 diffuseColor = tex2D(textureSampler, input.TexCoord);
    // Intensity based on the direction of the light
    float diffuseIntensity = saturate(dot(LightDirection, input.Normal));
    // Final diffuse color with ambient color added
    float4 diffuse = diffuseIntensity * diffuseColor + AmbientColor;
    



    // detta tror jag ska var kvar på något sätt

    // Find the position of this pixel in light space
    float4 lightingPosition = mul(input.WorldPos, LightViewProj);
    
    // Find the position in the shadow map for this pixel
    float2 ShadowTexCoord = 0.5 * lightingPosition.xy /
                            lightingPosition.w + float2(0.5, 0.5);
    ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

    // Get the current depth stored in the shadow map
    float shadowdepth = (float) tex2D(ShadowMapSampler, ShadowTexCoord).r;
    
    // Calculate the current pixel depth
    // The bias is used to prevent folating point errors that occur when
    // the pixel of the occluder is being drawn
    float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
    
    // Check to see if this pixel is in front or behind the value in the shadow map
    if (shadowdepth < ourdepth)
    {
        // Shadow the pixel by lowering the intensity
        ShadowStrenght = 1 - ShadowStrenght;
        diffuse *= float4(ShadowStrenght, ShadowStrenght, ShadowStrenght, 0);
    };
    
    return diffuse;
}

// Technique for creating the shadow map
technique CreateShadowMap
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL CreateShadowMap_VertexShader();
        PixelShader = compile PS_SHADERMODEL CreateShadowMap_PixelShader();
    }
}

// Technique for drawing with the shadow map
technique DrawWithShadowMap
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL DrawWithShadowMap_VertexShader();
        PixelShader = compile PS_SHADERMODEL DrawWithShadowMap_PixelShader();
    }
}