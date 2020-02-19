using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Banchou.Part {
    public class LocalInputCommandStream : MonoBehaviour, ICommandStream, IMovementInput {
        [Header("Input Axes")]
        [SerializeField] private string _horizontalAxis = "Horizontal";
        [SerializeField] private string _verticalAxis = "Vertical";
        [SerializeField] private string _lightAttack = "Fire1";
        [SerializeField] private string _heavyAttack = "Fire2";
        [SerializeField] private string _holdDirection = "LockOn";
        [SerializeField] private string _jump = "Jump";
        [SerializeField] private string _dodge = "Dodge";

        [Header("Parameters")]
        [SerializeField] private float _attackTapThreshold = 0.25f;
        [SerializeField] private float _jumpTapThreshold = 0.1f;
        [SerializeField] private float _tiltSpeedThreshold = 1f;
        //[SerializeField, Range(0f, 1f)] private float _pushThreshold = 0.7f;
        [SerializeField] private float _pullLag = 0.35f;

        [Header("References")]
        [SerializeField] private Transform _camera = null;
        [SerializeField] private Transform _orientation = null;

        [Header("Debug")]
        [SerializeField] private Vector2 _debugMovement;
        [SerializeField] private float _debugSpeed;

        public IObservable<Command> Commands =>
            LightAttack
                .Merge(LightCharge)
                .Merge(HeavyAttack)
                .Merge(HeavyCharge)
                .Merge(LightChargeRelease)
                .Merge(HeavyChargeRelease)
                .Merge(Jump)
                .Merge(Hop)
                .Merge(Dodge)
                .Where(command => command != Command.None);
        
        public Vector3 MovementDirection {
            get {
                var controlAxes = new Vector2(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis));
                var angle = Mathf.Atan2(controlAxes.y, controlAxes.x);
                var maxX = Mathf.Abs(Mathf.Cos(angle));
                var maxY = Mathf.Abs(Mathf.Sin(angle));
                var restrictedAxes = new Vector2(
                    Mathf.Clamp(Mathf.Abs(controlAxes.x), 0f, maxX) * Mathf.Sign(controlAxes.x),
                    Mathf.Clamp(Mathf.Abs(controlAxes.y), 0f, maxY) * Mathf.Sign(controlAxes.y)
                );
                _debugMovement = restrictedAxes;
                return ViewAxis(restrictedAxes);
            }
        }

        public bool IsBuffered(Command command) {
            switch (command) {
                case Command.NeutralLight:
                case Command.PushLight:
                case Command.PullLight:
                    return Input.GetButton(_lightAttack);
                case Command.NeutralHeavy:
                case Command.PushHeavy:
                case Command.PullHeavy:
                    return Input.GetButton(_heavyAttack);
                default:
                    return true;
            }
        }

        public bool RotateToMovement => !Input.GetButton(_holdDirection);

        private IObservable<Vector3> DirectionLag =>
            this.FixedUpdateAsObservable()
                .Select(_ => MovementDirection)
                .Where(movement => movement != Vector3.zero)
                .Select(movement => movement.normalized)
                .Delay(TimeSpan.FromSeconds(_pullLag));

        // Projects the stick directions into the world based on the camera's perspective
        private Vector3 ViewAxis(Vector2 controlAxes) =>
            Vector3.ProjectOnPlane(_camera.right, _orientation.up).normalized * controlAxes.x +
            Vector3.ProjectOnPlane(_camera.forward, _orientation.up).normalized * controlAxes.y;
        
        #region Input Gestures
        // Capture stick movements that...
        //   1. Hit an extremity. i.e., some axis value greater than `_tiltMagnitudeThreshold`
        //   2. Do so faster than `_tiltSpeed`
        // To capture quick stick tilting motions
        public IObservable<(float Current, float Speed)> Tilt => 
            this.FixedUpdateAsObservable()
                .Select(_ => Vector3.Dot(MovementDirection, _orientation.forward))
                .Where(move => move == 0f || move >= 0.9f || move <= 0.9f)
                .Scan((Current: 0f, OffTime: 0f), (prev, move) => (
                    Current: move,
                    OffTime: move == 0f ? Time.fixedUnscaledTime : prev.OffTime
                ))
                .Select(tuple => (
                    Current: tuple.Current,
                    Speed: Time.fixedUnscaledTime - tuple.OffTime > 0f ? tuple.Current / (Time.fixedUnscaledTime - tuple.OffTime) : 0f
                ));
        
        // Emits the running time a button is held
        private IObservable<float> ButtonHold(string button) =>
            this.UpdateAsObservable()
                .SkipWhile(_ => Input.GetButton(button)) // Omit until button is released
                .Scan(0f, (holdTime, _) => Input.GetButton(button) ? holdTime + Time.unscaledDeltaTime : 0f);
        
        // Emits when the button is held for less than `tapThreshold` seconds
        // Used to trigger quick button taps
        private IObservable<Unit> ButtonTap(string button, float tapThreshold) =>
            ButtonHold(button)
                .Pairwise()
                .Where(pair => pair.Current == 0f && pair.Previous > 0f && pair.Previous <= tapThreshold)
                .Select(pair => Unit.Default);
        
        // Emits when the button is held longer than `tapThreshold` seconds
        // Marks the start of a button charge
        private IObservable<Unit> ButtonChargeStart(string button, float tapThreshold) =>
            ButtonHold(button)
                .Pairwise()
                .Where(pair => pair.Current > tapThreshold && pair.Previous > 0f && pair.Previous <= tapThreshold)
                .Select(pair => Unit.Default);

        // Emits when the button is released after being held for some time longer than `_tapThreshold` seconds
        // Marks the end of a button charge
        private IObservable<Unit> ButtonChargeEnd(string button, float tapThreshold) =>
            ButtonHold(button)
                .Pairwise()
                .Where(pair => pair.Current == 0f && pair.Previous > tapThreshold)
                .Select(pair => Unit.Default);
        
        private IObservable<Command> TiltTap(string button, Command forward, Command neutral, Command back) =>
            ButtonTap(button, _attackTapThreshold)
                .WithLatestFrom(
                    Tilt,
                    (_, tilt) => {
                        if      (tilt.Current > 0f && tilt.Speed >  _tiltSpeedThreshold) { return forward; }
                        else if (tilt.Current < 0f && tilt.Speed < -_tiltSpeedThreshold) { return back; }
                        return neutral;
                    }
                );

        private IObservable<Command> TiltCharge(string button, Command forward, Command neutral, Command back) =>
            ButtonChargeStart(button, _attackTapThreshold)
                .WithLatestFrom(
                    Tilt,
                    (holdTime, tilt) => {
                        if      (tilt.Current > 0f && tilt.Speed >  _tiltSpeedThreshold) { return forward; }
                        else if (tilt.Current < 0f && tilt.Speed < -_tiltSpeedThreshold) { return back; }
                        return neutral;
                    }
                );
        #endregion

        #region Command Detection        
        private IObservable<Command> LightAttack => TiltTap(
            _lightAttack, Command.PushLight, Command.NeutralLight, Command.PullLight
        );
        private IObservable<Command> LightCharge => TiltCharge(
            _lightAttack, Command.PushLightCharge, Command.NeutralLightCharge, Command.PullLightCharge
        );
        private IObservable<Command> HeavyAttack => TiltTap(
            _heavyAttack, Command.PushHeavy, Command.NeutralHeavy, Command.PullHeavy
        );
        private IObservable<Command> HeavyCharge => TiltCharge(
            _heavyAttack, Command.PushHeavyCharge, Command.NeutralHeavyCharge, Command.PullHeavyCharge
        );
        
        private IObservable<Command> LightChargeRelease => ButtonChargeEnd(_lightAttack, _attackTapThreshold).Select(_ => Command.LightChargeRelease);
        private IObservable<Command> HeavyChargeRelease => ButtonChargeEnd(_heavyAttack, _attackTapThreshold).Select(_ => Command.HeavyChargeRelease);

        private IObservable<Command> Jump => ButtonChargeStart(_jump, _jumpTapThreshold).Select(_ => Command.Jump);
        
        private IObservable<Command> Hop => ButtonTap(_jump, _jumpTapThreshold).Select(_ => Command.Hop);
        
        private IObservable<Command> Dodge =>
            this.FixedUpdateAsObservable()
                .Where(_ => Input.GetButtonDown(_dodge))
                .Select(_ => Command.Dodge);
        
        //private IObservable<Command> Dash =>
            
        #endregion
        
        private Vector3 _lastDirection;
        private void Start() {
            DirectionLag.Subscribe(d => _lastDirection = d);
            Tilt.Subscribe(tilt => _debugSpeed = (float)Math.Round((double)tilt.Speed, 1));
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _lastDirection);
        }
    }
}