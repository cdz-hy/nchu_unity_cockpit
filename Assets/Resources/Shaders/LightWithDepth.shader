Shader "Custom/LightWithDepth"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0,10)) = 1
        _Falloff ("Falloff", Range(0.1,5)) = 1
        [Toggle] _UseDepthWrite ("Use Depth Write", Float) = 1
        _DepthBias ("Depth Bias", Range(0,0.1)) = 0.01
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        
        // 第一个Pass：写入深度缓冲
        Pass
        {
            ZWrite On
            ZTest LEqual
            ColorMask 0 // 不写入颜色，只写入深度
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            
            float _UseDepthWrite;
            float _DepthBias;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 应用深度偏移，确保光源显示在物体前面
                o.vertex.z -= _DepthBias * o.vertex.w;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 只有在启用深度写入时才执行
                if (_UseDepthWrite < 0.5)
                    discard;
                    
                return fixed4(0,0,0,0); // 不写入颜色
            }
            ENDCG
        }
        
        // 第二个Pass：渲染光晕效果
        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Intensity;
            float _Falloff;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 采样纹理
                fixed4 col = tex2D(_MainTex, i.uv) * _Color * i.color;
                
                // 创建一个径向渐变效果
                float dist = distance(i.uv, float2(0.5, 0.5));
                float falloff = pow(1.0 - saturate(dist * 2.0), _Falloff);
                
                // 应用强度和衰减
                col.rgb *= _Intensity;
                col.a *= falloff;
                
                // 应用雾效果
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
} 