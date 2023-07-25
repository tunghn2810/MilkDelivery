using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCard : MonoBehaviour
{
    private Animator _anim;

    private readonly int Idle = Animator.StringToHash("Idle");
    private readonly int PopIn = Animator.StringToHash("PopIn");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private IEnumerator CompleteGame()
    {
        _anim.CrossFade(Idle, 0, 0);

        yield return new WaitForSeconds(1f);

        _anim.CrossFade(PopIn, 0, 0);
    }

    public void GameCompleted()
    {
        StartCoroutine(CompleteGame());
    }
}
