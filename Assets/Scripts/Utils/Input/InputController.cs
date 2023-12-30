using UnityEngine;
using Zenject;

namespace Utils.Input {
public class InputController : MonoBehaviour {
    [Inject] new Camera camera;

    Log log;
    
    bool isMobile;
    TouchPhase phase;
    Vector3 prevPosition;
    Vector3 currentPosition;
    
    #region destop input hanlding
    bool touchedBefore;
    bool touching;
    #endregion

    public bool paused;

    void Awake() {
        log = new(GetType(),false);
        isMobile = Application.isMobilePlatform;
    }

    void Update() {
        if (paused) return;
        #region determine touch phase
        if (isMobile) {
            if (UnityEngine.Input.touchCount != 1) return;
            var touch = UnityEngine.Input.GetTouch(0);
            phase = touch.phase;
            prevPosition = currentPosition;
            currentPosition = camera.ScreenToWorldPoint(touch.position);
        } else {
            touching = UnityEngine.Input.GetMouseButton(0);
            prevPosition = currentPosition;
            currentPosition = camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            if (!touchedBefore && touching) {
                phase = TouchPhase.Began;
                touchedBefore = true;
            } else if (touchedBefore && touching) {
                if (prevPosition.x - currentPosition.x != 0 || prevPosition.y - currentPosition.y != 0) {
                    phase = TouchPhase.Moved;
                } else {
                    phase = TouchPhase.Stationary;
                }
            } else if (touchedBefore && !touching) {
                phase = TouchPhase.Ended;
                touchedBefore = false;
            } else {
                return;
            }
        }
        #endregion
        
        log.log($"{phase}, prevPos {prevPosition}, currentPos {currentPosition}");
        switch (phase) {
            case TouchPhase.Began:
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
        }
    }
}
}