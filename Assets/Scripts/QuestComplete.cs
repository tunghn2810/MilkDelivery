using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestComplete : MonoBehaviour
{
	private Animator _anim;

    private readonly int SlideIn = Animator.StringToHash("SlideIn");
    private readonly int SlideOut = Animator.StringToHash("SlideOut");

    private Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CoroutineCoordinator());
    }

    private IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (_coroutineQueue.Count > 0)
                yield return StartCoroutine(_coroutineQueue.Dequeue());
            yield return null;
        }
    }

    private IEnumerator CompleteQuest()
    {
        _anim.CrossFade(SlideIn, 0, 0);

        yield return new WaitForSeconds(3f);

        _anim.CrossFade(SlideOut, 0, 0);

        yield return new WaitForSeconds(1f);
    }

    public void QuestCompleted()
    {
        _coroutineQueue.Enqueue(CompleteQuest());
    }
}
