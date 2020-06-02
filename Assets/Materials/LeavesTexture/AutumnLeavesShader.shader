// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/AutumnLeavesShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaTex("Alpha mask", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Shininess("Shininess", Range(0.0, 0.8)) = 0.7

    }
    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent" 
        }
        LOD 100


        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };


            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 lightDir : TEXCOORD1;
                half3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            half _Shininess;
            float4 _LightColor0;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                TANGENT_SPACE_ROTATION;
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_AlphaTex, i.uv);


                i.lightDir = normalize(i.lightDir);
                i.viewDir = normalize(i.viewDir);
                half3 halfDir = normalize(i.lightDir + i.viewDir);

                //fixed4 tex = tex2D(_MainTex, i.uv);
                // ノーマルマップから法線を取得
                half3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));
                // ノーマルマップの法線が確実に正規化されているならなくてもいい
                //normal = normalize(normal);

                half3 diffuse = max(_Shininess, dot(normal, i.lightDir)) * col.rgb;
                half3 specular = pow(max(_Shininess, dot(normal, halfDir)), 0.078125 * 128.0) * col.rgb;


                return fixed4(col.r * diffuse.r + specular.r, col.g * diffuse.g + specular.g, col.b * diffuse.b + specular.b, col2.r);
            }
            ENDCG
        }

    }
    FallBack "Diffuse"
}
