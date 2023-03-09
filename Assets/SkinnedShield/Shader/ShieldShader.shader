Shader "DDGameStudio/Shield" {
    Properties {
        _Brightness ("Brightness", Float ) = 1
        _Intensity ("Intensity", Float ) = 1
        _Color ("Color", Color) = (0.1764706,0.5229208,1,1)
        _Texture ("Texture", 2D) = "white" {}
        _Texture_Decay ("Texture_Decay", 2D) = "white" {}
        _Decay ("Decay", Range(0.05, 0.95)) = 0.223077
        _Power ("Power", Float ) = 1
        _Speed ("Speed", Range(-15, 15)) = 0.5
		_Offset("Mesh Offset", Float) = 0.02
    }

    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Power;
            uniform sampler2D _Texture_Decay; uniform float4 _Texture_Decay_ST;
            uniform float4 _Color;
            uniform float _Decay;
            uniform float _Intensity;
            uniform float _Brightness;
            uniform float _Speed;
			uniform float _Offset;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };

            VertexOutput vert (VertexInput v) {
				v.vertex.xyz += v.normal * _Offset;
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;

                float4 node_2434 = _Time + _TimeEditor;
                float2 node_923 = (i.uv0+(node_2434.g*_Speed)*float2(0,0.1));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_923, _Texture));
                float node_893 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_Power);
                float2 node_1319 = float2((_Texture_var.r+node_893),((i.uv0.g*0.0)+_Decay));
                float4 _Texture_Decay_var = tex2D(_Texture_Decay,TRANSFORM_TEX(node_1319, _Texture_Decay));
                float3 emissive = ((_Color.rgb*clamp(((_Texture_Decay_var.r*node_893)*_Intensity),0.05,0.95))*_Brightness);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
