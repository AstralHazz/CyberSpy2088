﻿Shader "TunnelEffect/TunnelCap" {
	Properties {
		_BackgroundColor ("Transition Color", Color) = (1,1,1)
        _CurveParams("Curve Params", Vector) = (0.02, 15.0, 0.01)
        _Params1 ("Params 1", Vector) = (1.5, 0.5, 0.1, 0.12)
	}
   	SubShader {
       Tags {
	       "Queue"="Geometry+100"
	       "RenderType"="Opaque"
       }
       Cull Off

       Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"
        
		half4  _BackgroundColor;
        float3 _CurveParams;
        float4 _Params1; // x = travel speed, y = rotation speed, z = twist, w = brightness
		
		struct appdata {
			float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(appdata v) {
			v2f o;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float distToCam = distance(wpos, _WorldSpaceCameraPos);
//            o.pos = UnityObjectToClipPos(v.vertex);
//            o.pos.xy += sin(distToCam * _CurveParams.z + _Params1.x) * distToCam * _CurveParams.xy;

            v.vertex.xy += sin(distToCam * _CurveParams.z + _Params1.x) * distToCam * _CurveParams.xy;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
    	
		fixed4 frag(v2f i) : SV_Target {
            UNITY_SETUP_INSTANCE_ID(i);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			return _BackgroundColor;
		}
			
		ENDCG
    }
  }  
}