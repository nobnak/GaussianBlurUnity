using UnityEngine;
using System.Collections;

namespace Gaussian {

	public class GaussianFilter : MonoBehaviour {
		public int nIterations = 3;
		public int lod = 1;
		public Material gaussianMat;

		void OnRenderImage(RenderTexture src, RenderTexture dst) {
			src = DownSample(src, lod, gaussianMat);
			Blur (src, dst, nIterations, gaussianMat);
			RenderTexture.ReleaseTemporary(src);
		}

		public static void Blur (RenderTexture src, RenderTexture dst, int nIterations, Material gaussianMat) {
			var tmp0 = RenderTexture.GetTemporary (src.width, src.height, 0, src.format);
			var tmp1 = RenderTexture.GetTemporary (src.width, src.height, 0, src.format);
			var iters = Mathf.Clamp (nIterations, 0, 10);
			Graphics.Blit (src, tmp0);
			for (var i = 0; i < iters; i++) {
				for (var pass = 1; pass < 3; pass++) {
					tmp1.DiscardContents ();
					tmp0.filterMode = FilterMode.Bilinear;
					Graphics.Blit (tmp0, tmp1, gaussianMat, pass);
					var tmpSwap = tmp0;
					tmp0 = tmp1;
					tmp1 = tmpSwap;
				}
			}
			Graphics.Blit (tmp0, dst);
			RenderTexture.ReleaseTemporary (tmp0);
			RenderTexture.ReleaseTemporary (tmp1);
		}

		public static RenderTexture DownSample(RenderTexture src, int lod, Material gaussianMat) {
			var dst = RenderTexture.GetTemporary(src.width, src.height, 0 , src.format);
			src.filterMode = FilterMode.Bilinear;
			Graphics.Blit(src, dst);

			for (var i = 0; i < lod; i++) {
				var tmp = RenderTexture.GetTemporary(dst.width >> 1, dst.height >> 1, 0, dst.format);
				dst.filterMode = FilterMode.Bilinear;
				Graphics.Blit(dst, tmp, gaussianMat, 0);
				RenderTexture.ReleaseTemporary(dst);
				dst = tmp;
			}
			return dst;
		}
	}
}