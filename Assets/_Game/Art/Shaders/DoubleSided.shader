Shader "Custom/DoubleSided"
{
    Properties
    {
        _Texture("Texture", 2D) = "black" {}
        _ClipThreshold("Clip Threshold", Range(0,1)) = 1
    }
    
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct MeshData
            {
                float3 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _Texture;
            float _ClipThreshold;
            
            Interpolators vert (MeshData v)
            {
                Interpolators i;
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = v.uv0;
                return i;
            }
            
            fixed4 frag (Interpolators i) : SV_Target
            {
                float4 baseColor = tex2D(_Texture, i.uv);
                clip(baseColor.a - 0.0001 - _ClipThreshold);
                return baseColor;
            }
            ENDCG
        }
    }
}