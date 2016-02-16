using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Displacement/Fisheye")]
    public class Fisheye : PostEffectsBase
	{
        public static bool Active = false;

        [Range(0.0f, 1.5f)]
        public float strengthX = 0.05f;
        [Range(0.0f, 1.5f)]
        public float strengthY = 0.05f;

        public Shader fishEyeShader = null;
        private Material fisheyeMaterial = null;

        private bool initi = false;

        private void InitializeFish() {
            iTween.ValueTo( gameObject, iTween.Hash( "from", strengthX, "to", Random.Range( 0.25f, 1.5f ), "time", 3.5f, "onupdate", "UpdateX", "oncomplete", "CompleteX" ) );
            iTween.ValueTo( gameObject, iTween.Hash( "from", strengthY, "to", Random.Range( 0.25f, 1.5f ), "time", 3.5f, "onupdate", "UpdateY", "oncomplete", "CompleteY" ) );
        }

        private void UpdateX(float value) {
            strengthX = value;
        }

        private void CompleteX() {
            iTween.ValueTo( gameObject, iTween.Hash( "from", strengthX, "to", 0, "time", 1.5f, "onupdate", "UpdateX" ) );
        }

        private void CompleteY() {
            iTween.ValueTo( gameObject, iTween.Hash( "from", strengthY, "to", 0, "time", 1.5f, "onupdate", "UpdateY" ) );
        }

        private void UpdateY(float value ) {
            strengthY = value;
        }

        public override bool CheckResources ()
		{
            CheckSupport (false);
            fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader,fisheyeMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if ( !Active ) {
                if ( initi ) initi = false;
                return;
            }
            if ( !initi ) {
                initi = true;
                InitializeFish();
            }

            if (CheckResources()==false)
			{
                Graphics.Blit (source, destination);
                return;
            }

            float oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of fisheye

            float ar = (source.width * 1.0f) / (source.height * 1.0f);

            fisheyeMaterial.SetVector ("intensity", new Vector4 (strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize, strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize));
            Graphics.Blit (source, destination, fisheyeMaterial);
        }
    }
}
