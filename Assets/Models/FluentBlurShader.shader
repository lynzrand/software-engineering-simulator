// Unused
Shader "Hidden/FluentBlurShader"
{
    Properties
    {
        _BlurRadius ("Blur Radius", uint) = 24
        _BlurIntensity ("Blur Intensity", 2D) = "gray" {} // defaults as a box blur
        _NoistTex ("Noise Texture", 2D) = "black" {}
        _NoiseIntensity ("Noise Intensity", Range(0.0, 1.0)) = 0.1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { 
            "Queue"="Transparent"
            "RenderType"="Opaque" 
        }

        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            half _BlurRadius;
            float _NoiseIntensity;

            Sampler2D _GrabTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                for()
                
                return col;
            }
            ENDCG
        }
    }
}
