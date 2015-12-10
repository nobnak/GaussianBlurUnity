using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Gaussian;

namespace First {

	public class UI : MonoBehaviour {
		public const string FORMAT = "{0:d}";

		public GaussianFilter gaussianFilter;

		public Slider lodSlider;
		public Text lodValue;
		public Slider itrSlider;
		public Text itrValue;

		void Start () {
			var lod = gaussianFilter.lod;
			var iterations = gaussianFilter.nIterations;

			lodSlider.value = lod;
			lodSlider.onValueChanged.AddListener (OnChangeLod);
			lodValue.text = string.Format (FORMAT, lod);

			itrSlider.value = iterations;
			itrSlider.onValueChanged.AddListener (OnChangeIterations);
			itrValue.text = string.Format (FORMAT, iterations);
		}

		public void OnChangeLod(float value) {
			gaussianFilter.lod = (int)value;
			lodValue.text = string.Format (FORMAT, gaussianFilter.lod);
		}
		public void OnChangeIterations(float value) {
			gaussianFilter.nIterations = (int)value;
			itrValue.text = string.Format (FORMAT, gaussianFilter.nIterations);
		}
	}
}