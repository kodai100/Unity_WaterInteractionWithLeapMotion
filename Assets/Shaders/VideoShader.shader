
Shader "Custom/VideoShader" {
	Properties{
		_MainTex("Source", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		ZWrite Off
		Fog{ Mode Off }

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 3.0

#include "UnityCG.cginc"

		struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_img v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
		return o;
	}

	sampler2D _MainTex;
	sampler2D _VideoTex;

	fixed4 frag(v2f i) : SV_TARGET{
		float4 base = tex2D(_VideoTex, i.uv);
		

		return base;
	}
		ENDCG
	}
	}
		FallBack Off
}