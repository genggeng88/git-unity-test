using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour {
    private Camera _camera;

    void Start() {
        _camera = GetComponent<Camera>();

        // hide the mouse cursor at the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)){
            Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            // notify the target when hit, the target will react when notified
            if (Physics.Raycast(ray, out hit)){
                // retrieve the object the ray hit
                GameObject hitObject = hit.transform.gameObject;
                // check for the ReactiveTarget component on the object
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null) {
                    target.ReactToHit();
                } else {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }

    void OnGUI() {
        int size = 12;
        float posX = _camera.pixelWidth/2 - size/4;
        float posY = _camera.pixelHeight/2 - size/4;
        // GUI.Label() displays text on screen
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }
}