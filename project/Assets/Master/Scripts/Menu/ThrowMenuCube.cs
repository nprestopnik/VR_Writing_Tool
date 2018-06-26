//a modified version of the WorkstationBehaviourExample.cs script from the leap examples

using Leap.Unity;
using Leap.Unity.Animation;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
[RequireComponent(typeof(AnchorableBehaviour))]
[AddComponentMenu("")]
public class ThrowMenuCube : MonoBehaviour {

  /// <summary>
    /// If the rigidbody of this object moves faster than this speed and the object
    /// is in workstation mode, it will exit workstation mode.
    /// </summary>
    public const float MAX_SPEED_AS_WORKSTATION = 0.005F;

    /// <summary>
    /// If the rigidbody of this object moves slower than this speed and the object
    /// wants to enter workstation mode, it will first pick a target position and
    /// travel there. (Otherwise, it will open its workstation in-place.)
    /// </summary>
    public const float MIN_SPEED_TO_ACTIVATE_TRAVELING = 0.5F;

    //public TransformTweenBehaviour workstationModeTween;

    private InteractionBehaviour _intObj;
    private AnchorableBehaviour _anchObj;

    private bool _wasKinematicBeforeActivation = false;

    /// <summary>
    /// Gets whether the workstation is currently traveling to a target position to open
    /// in workstation mode. Will return false if it is not traveling due to not being
    /// in workstation mode or if it has already reached its target position.
    /// </summary>
    public bool isTraveling { get { return _travelTween.isValid && _travelTween.isRunning; } }

    void OnValidate() {
      refreshRequiredComponents();
    }

    void Start() {
      refreshRequiredComponents();

      if (!_anchObj.tryAnchorNearestOnGraspEnd) {
        Debug.LogWarning("WorkstationBehaviour expects its AnchorableBehaviour's tryAnchorNearestOnGraspEnd property to be enabled.", this.gameObject);
      }
    }

    void OnDestroy() {
      _anchObj.OnPostTryAnchorOnGraspEnd -= onPostObjectGraspEnd;
    }



    private void refreshRequiredComponents() {
      _intObj = GetComponent<InteractionBehaviour>();
      _anchObj = GetComponent<AnchorableBehaviour>();

      _anchObj.OnPostTryAnchorOnGraspEnd += onPostObjectGraspEnd;
    }

    private void onPostObjectGraspEnd() {
      if (_anchObj.preferredAnchor == null) {
        // Choose a good position and rotation for workstation mode and begin traveling there.
        Vector3 targetPosition = _intObj.rigidbody.position;
		Quaternion targetRotation = _intObj.rigidbody.rotation;

        beginTraveling(_intObj.rigidbody.position, _intObj.rigidbody.velocity,
                       _intObj.rigidbody.rotation, _intObj.rigidbody.angularVelocity,
                       targetPosition, targetRotation);
      }
    }

    #region Traveling

    private const float MAX_TRAVEL_SPEED = 4.00F;

    private Tween _travelTween;

    private float      _initTravelTime = 0F;
    private Vector3    _initTravelPosition = Vector3.zero;
    private Vector3    _initTravelVelocity = Vector3.zero;
    private Quaternion _initTravelRotation = Quaternion.identity;
    private Vector3    _initTravelAngVelocity = Vector3.zero;
    private Vector3    _effGravity = Vector3.zero;

    private Vector3    _travelTargetPosition;
    private Quaternion _travelTargetRotation;

    private Vector2 _minMaxWorkstationTravelTime    = new Vector2(0.05F, 1.00F);
    private Vector2 _minMaxTravelTimeFromThrowSpeed = new Vector2(0.30F, 8.00F);

    private void beginTraveling(Vector3 initPosition, Vector3 initVelocity,
                                Quaternion initRotation, Vector3 initAngVelocity,
                                Vector3 targetPosition, Quaternion targetRotation) {
      _initTravelTime        = Time.time;
      _initTravelPosition    = initPosition;
      _initTravelVelocity    = initVelocity;
      _initTravelRotation    = initRotation;
      _initTravelAngVelocity = initAngVelocity;

      float velMagnitude = _initTravelVelocity.magnitude;
      if (velMagnitude > MAX_TRAVEL_SPEED) {
        float capSpeedMultiplier = MAX_TRAVEL_SPEED / velMagnitude;
        _initTravelVelocity *= capSpeedMultiplier;
      }

      _effGravity            = Vector3.Lerp(Vector3.zero, Physics.gravity, initVelocity.magnitude.Map(0.80F, 3F, 0F, 0.70F));



      _travelTargetPosition = targetPosition;
      _travelTargetRotation = targetRotation;

      // Construct a single-use Tween that will last a specific duration
      // and specify a custom callback as it progresses to update the
      // object's position and rotation.
      _travelTween = Tween.Single()
                          .OverTime(_initTravelVelocity.magnitude.Map(_minMaxTravelTimeFromThrowSpeed.x, _minMaxTravelTimeFromThrowSpeed.y,
                                                                      _minMaxWorkstationTravelTime.x, _minMaxWorkstationTravelTime.y))
                          .OnProgress(onTravelTweenProgress)
                          //.OnReachEnd(ActivateWorkstation) // When the tween is finished, open workstation mode.
                          .Play();
    }

    private void onTravelTweenProgress(float progress) {
      float      curTime = Time.time;
      Vector3    extrapolatedPosition = evaluatePosition(_initTravelPosition, _initTravelVelocity, _effGravity, _initTravelTime, curTime);
      Quaternion extrapolatedRotation = evaluateRotation(_initTravelRotation, _initTravelAngVelocity, _initTravelTime, curTime);
      
      // Interpolate from the position and rotation that the object would naturally have over time
      // (by movement due to inertia and by acceleration due to gravity)
      // to the target position and rotation over the lifetime of the tween.
      _intObj.rigidbody.position =     Vector3.Lerp(extrapolatedPosition, _travelTargetPosition, progress);
      _intObj.rigidbody.rotation = Quaternion.Slerp(extrapolatedRotation, _travelTargetRotation, progress);
    }

    private void cancelTraveling() {
      // In case traveling was halted mid-travel-tween, halt the tween.
      if (_travelTween.isValid) { _travelTween.Stop(); }
    }

    /// <summary>
    /// Evaluates the position of a body over time with initial velocity and acceleration due to gravity.
    /// </summary>
    private Vector3 evaluatePosition(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity, float initialTime, float timeToEvaluate) {
      float t = timeToEvaluate - initialTime;
      return initialPosition + (initialVelocity * t) + (0.5f * gravity * t * t);
    }

    /// <summary>
    /// Evaluates the rotation of a body over time with initial velocity and acceleration due to gravity.
    /// </summary>
    private Quaternion evaluateRotation(Quaternion initialRotation, Vector3 angularVelocity, float initialTime, float timeToEvaluate) {
      float t = timeToEvaluate - initialTime;
      return Quaternion.Euler(angularVelocity * t) * initialRotation;
    }

    #endregion
}
