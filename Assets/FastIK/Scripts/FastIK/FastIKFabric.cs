#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace DitzelGames.FastIK
{
    /// <summary>
    /// Fabrik IK Solver
    /// </summary>
    public class FastIKFabric : MonoBehaviour
    {
        /// <summary>
        /// Chain length of bones
        /// </summary>
        [FormerlySerializedAs("ChainLength")] public int chainLength = 2;

        /// <summary>
        /// Target the chain should bent to
        /// </summary>
        [FormerlySerializedAs("Target")] public Transform target;
        [FormerlySerializedAs("Pole")] public Transform pole;

        /// <summary>
        /// Solver iterations per update
        /// </summary>
        [FormerlySerializedAs("Iterations")] [Header("Solver Parameters")]
        public int iterations = 10;

        /// <summary>
        /// Distance when the solver stops
        /// </summary>
        [FormerlySerializedAs("Delta")] public float delta = 0.001f;

        /// <summary>
        /// Strength of going back to the start position.
        /// </summary>
        [FormerlySerializedAs("SnapBackStrength")] [Range(0, 1)]
        public float snapBackStrength = 1f;


        private float[] _bonesLength; //Target to Origin
        private float _completeLength;
        private Transform[] _bones;
        private Vector3[] _positions;
        private Vector3[] _startDirectionSucc;
        private Quaternion[] _startRotationBone;
        private Quaternion _startRotationTarget;
        private Transform _root;


        // Start is called before the first frame update
        void Awake()
        {
            Init();
        }

        void Init()
        {
            //initial array
            _bones = new Transform[chainLength + 1];
            _positions = new Vector3[chainLength + 1];
            _bonesLength = new float[chainLength];
            _startDirectionSucc = new Vector3[chainLength + 1];
            _startRotationBone = new Quaternion[chainLength + 1];

            //find root
            _root = transform;
            for (var i = 0; i <= chainLength; i++)
            {
                if (_root == null)
                    throw new UnityException("The chain value is longer than the ancestor chain!");
                _root = _root.parent;
            }

            //init target
            if (target == null)
            {
                target = new GameObject(gameObject.name + " Target").transform;
                SetPositionRootSpace(target, GetPositionRootSpace(transform));
            }
            _startRotationTarget = GetRotationRootSpace(target);


            //init data
            var current = transform;
            _completeLength = 0;
            for (var i = _bones.Length - 1; i >= 0; i--)
            {
                _bones[i] = current;
                _startRotationBone[i] = GetRotationRootSpace(current);

                if (i == _bones.Length - 1)
                {
                    //leaf
                    _startDirectionSucc[i] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
                }
                else
                {
                    //mid bone
                    _startDirectionSucc[i] = GetPositionRootSpace(_bones[i + 1]) - GetPositionRootSpace(current);
                    _bonesLength[i] = _startDirectionSucc[i].magnitude;
                    _completeLength += _bonesLength[i];
                }

                current = current.parent;
            }



        }

        // Update is called once per frame
        void LateUpdate()
        {
            ResolveIK();
        }

        private void ResolveIK()
        {
            if (target == null)
                return;

            if (_bonesLength.Length != chainLength)
                Init();

            //Fabric

            //  root
            //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
            //   x--------------------x--------------------x---...

            //get position
            for (int i = 0; i < _bones.Length; i++)
                _positions[i] = GetPositionRootSpace(_bones[i]);

            var targetPosition = GetPositionRootSpace(target);
            var targetRotation = GetRotationRootSpace(target);

            //1st is possible to reach?
            if ((targetPosition - GetPositionRootSpace(_bones[0])).sqrMagnitude >= _completeLength * _completeLength)
            {
                //just strech it
                var direction = (targetPosition - _positions[0]).normalized;
                //set everything after root
                for (int i = 1; i < _positions.Length; i++)
                    _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
            }
            else
            {
                for (int i = 0; i < _positions.Length - 1; i++)
                    _positions[i + 1] = Vector3.Lerp(_positions[i + 1], _positions[i] + _startDirectionSucc[i], snapBackStrength);

                for (int iteration = 0; iteration < iterations; iteration++)
                {
                    //https://www.youtube.com/watch?v=UNoX65PRehA
                    //back
                    for (int i = _positions.Length - 1; i > 0; i--)
                    {
                        if (i == _positions.Length - 1)
                            _positions[i] = targetPosition; //set it to target
                        else
                            _positions[i] = _positions[i + 1] + (_positions[i] - _positions[i + 1]).normalized * _bonesLength[i]; //set in line on distance
                    }

                    //forward
                    for (int i = 1; i < _positions.Length; i++)
                        _positions[i] = _positions[i - 1] + (_positions[i] - _positions[i - 1]).normalized * _bonesLength[i - 1];

                    //close enough?
                    if ((_positions[_positions.Length - 1] - targetPosition).sqrMagnitude < delta * delta)
                        break;
                }
            }

            //move towards pole
            if (pole != null)
            {
                var polePosition = GetPositionRootSpace(pole);
                for (int i = 1; i < _positions.Length - 1; i++)
                {
                    var plane = new Plane(_positions[i + 1] - _positions[i - 1], _positions[i - 1]);
                    var projectedPole = plane.ClosestPointOnPlane(polePosition);
                    var projectedBone = plane.ClosestPointOnPlane(_positions[i]);
                    var angle = Vector3.SignedAngle(projectedBone - _positions[i - 1], projectedPole - _positions[i - 1], plane.normal);
                    _positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (_positions[i] - _positions[i - 1]) + _positions[i - 1];
                }
            }

            //set position & rotation
            for (int i = 0; i < _positions.Length; i++)
            {
                if (i == _positions.Length - 1)
                    SetRotationRootSpace(_bones[i], Quaternion.Inverse(targetRotation) * _startRotationTarget * Quaternion.Inverse(_startRotationBone[i]));
                else
                    SetRotationRootSpace(_bones[i], Quaternion.FromToRotation(_startDirectionSucc[i], _positions[i + 1] - _positions[i]) * Quaternion.Inverse(_startRotationBone[i]));
                SetPositionRootSpace(_bones[i], _positions[i]);
            }
        }

        private Vector3 GetPositionRootSpace(Transform current)
        {
            if (_root == null)
                return current.position;
            else
                return Quaternion.Inverse(_root.rotation) * (current.position - _root.position);
        }

        private void SetPositionRootSpace(Transform current, Vector3 position)
        {
            if (_root == null)
                current.position = position;
            else
                current.position = _root.rotation * position + _root.position;
        }

        private Quaternion GetRotationRootSpace(Transform current)
        {
            //inverse(after) * before => rot: before -> after
            if (_root == null)
                return current.rotation;
            else
                return Quaternion.Inverse(current.rotation) * _root.rotation;
        }

        private void SetRotationRootSpace(Transform current, Quaternion rotation)
        {
            if (_root == null)
                current.rotation = rotation;
            else
                current.rotation = _root.rotation * rotation;
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            var current = this.transform;
            for (var i = 0; i < chainLength && current != null && current.parent != null; i++)
            {
                var parentPosition = current.parent.position;
                var currentPosition = current.position;
                var scale = Vector3.Distance(currentPosition, parentPosition) * 0.1f;
                Handles.matrix = Matrix4x4.TRS(currentPosition, Quaternion.FromToRotation(Vector3.up, parentPosition - currentPosition), new Vector3(scale, Vector3.Distance(parentPosition, currentPosition), scale));
                Handles.color = Color.green;
                Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
                current = current.parent;
            }
#endif
        }

    }
}