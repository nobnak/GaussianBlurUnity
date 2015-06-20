Shader "Custom/Gaussian" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
	}
	SubShader {
		ZTest Always ZWrite Off Cull Off Fog { Mode Off }
		
			CGINCLUDE
			const static float WEIGHTS[8] = {  0.013,  0.067,  0.194,  0.226, 0.226, 0.194, 0.067, 0.013 };
			const static float OFFSETS[8] = { -6.264, -4.329, -2.403, -0.649, 0.649, 2.403, 4.329, 6.264 };
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			struct vsin {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vs2psDown {
				float4 vertex : POSITION;
				float2 uv[4] : TEXCOORD0;
			};
			struct vs2psBlur {
				float4 vertex : POSITION;
				float2 uv[8] : TEXCOORD0;
			};
			
			vs2psDown vertDownsample(vsin IN) {
				vs2psDown OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.uv[0] = IN.uv;
				OUT.uv[1] = IN.uv + float2(-0.5, -0.5) * _MainTex_TexelSize.xy;
				OUT.uv[2] = IN.uv + float2( 0.5, -0.5) * _MainTex_TexelSize.xy;
				OUT.uv[3] = IN.uv + float2(-0.5,  0.5) * _MainTex_TexelSize.xy;
				return OUT;
			}
			float4 fragDownsample(vs2psDown IN) : COLOR {
				float4 c = 0;
				for (uint i = 0; i < 4; i++)
					c += tex2D(_MainTex, IN.uv[i]) * 0.25;
				return c;
			}
			
			vs2psBlur vertBlurH(vsin IN) {
				vs2psBlur OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				for (uint i = 0; i < 8; i++)
					OUT.uv[i] = IN.uv + float2(OFFSETS[i], 0) * _MainTex_TexelSize.xy;
				return OUT;
			}
			vs2psBlur vertBlurV(vsin IN) {
				vs2psBlur OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				for (uint i = 0; i < 8; i++)
					OUT.uv[i] = IN.uv + float2(0, OFFSETS[i]) * _MainTex_TexelSize.xy;
				return OUT;
			}
			float4 fragBlur(vs2psBlur IN) : COLOR {
				float4 c = 0;
				for (uint i = 0; i < 8; i++)
					c += tex2D(_MainTex, IN.uv[i]) * WEIGHTS[i];
				return c;
			}
			ENDCG		
		
		// 0 : Downsample
		Pass {
			CGPROGRAM
			#pragma vertex vertDownsample
			#pragma fragment fragDownsample
			ENDCG
		}
		// 1 : Horizontal Separable Gaussian
		Pass {
			CGPROGRAM
			#pragma vertex vertBlurH
			#pragma fragment fragBlur
			ENDCG
		}
		// 2 : Vertical Separable Gaussian
		Pass {
			CGPROGRAM
			#pragma vertex vertBlurV
			#pragma fragment fragBlur
			ENDCG
		}
	} 
	FallBack Off
}
