using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class LightOverlay : MonoBehaviour {

  private const int MAX_LIGHTS = 20;

  private Material cutMaterial;
  private Material overlayMaterial;

  private NBLight[] lights = new NBLight[MAX_LIGHTS];

  private Camera cam;

  void Start() {
    cutMaterial = new Material(Shader.Find("NBLightSystem/CutLights"));
    overlayMaterial = new Material(Shader.Find("Sprites/Default"));
    cam = GetComponent<Camera>();
  }

  void OnRenderImage(RenderTexture source, RenderTexture destination) {
    RenderTexture shadow = RenderTexture.GetTemporary(Screen.width, Screen.height);

    RenderTexture.active = shadow;
    GL.Begin(GL.TRIANGLES);
    GL.Clear(true, true, new Color(0, 0, 0, 1));
    GL.End();

    cutMaterial.SetFloat("_ScreenWidth", Screen.width);
    cutMaterial.SetFloat("_ScreenHeight", Screen.height);
    float worldToPixel = (cam.WorldToScreenPoint(Vector3.right) - cam.WorldToScreenPoint(Vector3.zero)).magnitude;

    for (int i = 0; i < MAX_LIGHTS && lights[i] != null; i++) {
      Vector3 screenPos = cam.WorldToScreenPoint(lights[i].transform.position);
      cutMaterial.SetFloat("_ScreenX", screenPos.x);
      cutMaterial.SetFloat("_ScreenY", screenPos.y);
      cutMaterial.SetFloat("_GradStartDist", lights[i].gradientStart*worldToPixel);
      cutMaterial.SetFloat("_GradEndDist", lights[i].gradientEnd*worldToPixel);
      Graphics.Blit(null, shadow, cutMaterial, 0);
    }

    Graphics.Blit(source, destination);
    Graphics.Blit(shadow, destination, overlayMaterial);

    RenderTexture.ReleaseTemporary(shadow);
  }

  public void AddLight(NBLight light) {
    for (int i = 0; i < MAX_LIGHTS; i++) {
      if (lights[i] == null) {
        lights[i] = light;
        return;
      }
      if (lights[i] == light)
        return;
    }
    Debug.LogError("We've got too many lights atm, increase MAX_LIGHTS or remove a light or two");
  }

  public void RemoveLight(NBLight light) {
    int idx = -1;
    int last = MAX_LIGHTS-1;
    for (int i = 0; i < MAX_LIGHTS; i++) {
      if (lights[i] == null) {
        if (idx == -1)
          return;
        last = i-1;
        break;
      }
      if (lights[i] == light)
        idx = i;
    }
    lights[idx] = lights[last];
    lights[last] = null;
  }
}
