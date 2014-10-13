@script RequireComponent (ReflectionRenderTexture)

var renderTextureSize = 256;

private var renderTexture : RenderTexture;

function Start() {
    if( !RenderTexture.enabled ) {
        print("Render textures are not available. Disabling mirror script");
        enabled = false;
        return;
    }
    
    renderTexture = new RenderTexture( renderTextureSize, renderTextureSize, 16 );
    renderTexture.isPowerOfTwo = true;
    
    gameObject.AddComponent("Camera");
    var cam : Camera = camera;
    var mainCam = Camera.main;
    cam.targetTexture = renderTexture;
    cam.clearFlags = mainCam.clearFlags;
    cam.backgroundColor = mainCam.backgroundColor;
    cam.nearClipPlane = mainCam.nearClipPlane;
    cam.farClipPlane = mainCam.farClipPlane;
    cam.fieldOfView = mainCam.fieldOfView;
    
    renderer.material.SetTexture("_ReflectionTex", renderTexture);
    
    var reflScript : ReflectionRenderTexture = GetComponent(ReflectionRenderTexture);
    reflScript.m_ReflectUpperSide = true;
}

function OnDisable() {
    Destroy(renderTexture);
}
