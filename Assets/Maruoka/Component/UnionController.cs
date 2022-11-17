using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionController : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField]
    private MainLifeController _lifeController = default;
    [SerializeField]
    private MoveBehavior _mover = default;
    [SerializeField]
    private WireAction _wireActioner = default;
    [SerializeField]
    private FlyingSquirrelAction _flyingSquirrelActioner = default;
    [SerializeField]
    private ShootGunBehavior _shootGun = default;
    [SerializeField]
    private SeparationInstruction _separationInstructioner = default;
    [SerializeField]
    private UnionStateController _stateController = default;
    [SerializeField]
    private UnionAnimationController _animationController = default;
    #endregion

    #region Member Variables
    #endregion

    #region Unity Methods
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Process();
    }
    private void OnDrawGizmosSelected()
    {
        OnDrawGizmoShootGunArea();
        OnDrawGizmoFlyingSquirrelFrontCheckArea();
    }
    #endregion

    #region Private Methods
    private void Init()
    {
        var rb2D = GetComponent<Rigidbody2D>();
        _mover.Init(rb2D);
        _stateController.Init(rb2D);
        _animationController.Init(_stateController);
        _lifeController.Init(_mover);
        _shootGun.Init(transform, _stateController);
        _flyingSquirrelActioner.Init(rb2D, _stateController,
            GetComponent<GroundCheck>(), transform, _mover);
    }
    private void Process()
    {
        _mover.Move();
        _wireActioner.Fire();
        _shootGun.Update();
        _flyingSquirrelActioner.Update();
        _stateController.Update();
        _animationController.Update();
        _separationInstructioner.Execution();
    }
    #endregion

    #region Draw Gizmos
    // ギズモ表示メソッド群
    /// <summary>
    /// 銃発砲による攻撃エリアを描画する。
    /// </summary>
    private void OnDrawGizmoShootGunArea()
    {
        if (_shootGun.IsDrawGizmo)
        {
            Gizmos.color = _shootGun.GizmoColor;
            var dir = _stateController.FacingDirection == FacingDirection.RIGHT ?
                Vector3.right : Vector3.left;

            Gizmos.DrawLine(transform.position,
                transform.position + dir * _shootGun.MaxDistance);
        }
    }
    private void OnDrawGizmoFlyingSquirrelFrontCheckArea()
    {
        if (_flyingSquirrelActioner.IsDrawGizmo)
        {
            Gizmos.color = _flyingSquirrelActioner.GizmoColor;

            var xOffset = _flyingSquirrelActioner.ForwardCheckOffset.x * (_stateController.FacingDirection == FacingDirection.RIGHT ? 1f : -1f);
            var pos = transform.position + new Vector3(xOffset, _flyingSquirrelActioner.ForwardCheckOffset.y, 0f);
            // var colliders = Physics2D.OverlapBoxAll(pos, _forwardCheckSize, 0f, _forwardCheckTargetLayer);
            Gizmos.DrawCube(pos, _flyingSquirrelActioner.ForwardCheckSize);
        }
    }
    #endregion

    #region Test
    // テストコード群
    public void TestDamage()
    {
        _lifeController.Damage(1, new Vector2(1, 1), 10f, 1000);
    }
    #endregion
}