using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpPower;
    [SerializeField] PresentPresenter _presentPresenter;

    [SerializeField] RectTransform _resultPanel;
    [SerializeField] Text _goalText;
    [SerializeField] float _resultDuration = 0.2f;
    [SerializeField] float _textDuration = 0.1f;
    [SerializeField] float _tweenDelay = 0.5f;
    [SerializeField] Ease _textEase = Ease.OutQuint;

    Rigidbody2D _rb;
    bool _isGoaled;

    public bool IsGoaled => _isGoaled;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");

        Vector2 dir = new Vector2(h, v);

        _rb.velocity = dir.normalized * _speed + new Vector2(0, _rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    private void ChangeBool()
    {
        _isGoaled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_presentPresenter.BoolChange() && collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Goal");
            var seq = DOTween.Sequence();
            seq.Append(_resultPanel.DOScale(Vector3.one, _resultDuration).SetEase(Ease.OutBounce)).SetDelay(_textDuration)
               .Append(_goalText.DOFade(1f, _textDuration)).SetEase(_textEase)
               .OnComplete(() => ChangeBool());
        }
    }
}
