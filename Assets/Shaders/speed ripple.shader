Shader "Custom/speed ripple"
{
    Properties
    {
        _MainTex ("render texture", 2D) = "white"{}

       
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define MAX_OFFSET 0.15


            sampler2D _MainTex;


            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
               float3 color = 0;
                
               float2 uv = i.uv;

                color = tex2D(_MainTex, uv);

                
                float2 cp = -1.0 + 2.0 * i.vertex.xy / _ScreenParams.xy;
                float cl = length(cp);
                uv = (i.vertex.xy / _ScreenParams.xy) + (cp / cl) * cos(cl * 20.0 - _Time.z * 4.0) * 0.02 ;
                float3 color2 = tex2D(_MainTex, uv);
                

             //   uv = i.vertex.xy / _ScreenParams.xy;
	            //color = max(tex2D(_MainTex, uv), tex2D(_MainTex, uv + 2));

                return float4(lerp(color, color2, 0.5), 1.0);
 
            }
            ENDCG
        }
    }
}

