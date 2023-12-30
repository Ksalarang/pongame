using GameInstaller;
using GameScene;
using UnityEngine;
using Utils;
using Zenject;

namespace Controllers {
public class InputController : MonoBehaviour {
    [Inject] GameSettings settings;
    [Inject] new Camera camera;
    [Inject(Id = StickControllerId.Stick1)] StickController stickController;
    
    Log log;
    
    bool isMobile;
    TouchPhase phase;
    Vector3 prevPosition;
    Vector3 currentPosition;
    
    #region destop input hanlding
    bool touchedBefore;
    bool touching;
    #endregion

    [HideInInspector] public bool paused;

    void Awake() {
        log = new(GetType(), settings.log.inputController);
        isMobile = Application.isMobilePlatform;
    }

    void Update() {
        if (paused) return;
        #region determine touch phase
        if (isMobile) {
            if (Input.touchCount != 1) return;
            var touch = Input.GetTouch(0);
            phase = touch.phase;
            prevPosition = currentPosition;
            currentPosition = camera.ScreenToWorldPoint(touch.position);
        } else {
            touching = Input.GetMouseButton(0);
            prevPosition = currentPosition;
            currentPosition = camera.ScreenToWorldPoint(Input.mousePosition);
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
                if (!settings.debug.autoPlay) {
                    stickController.moveStick(currentPosition.x - prevPosition.x);
                }
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
        }
    }
}
}