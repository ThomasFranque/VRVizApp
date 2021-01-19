using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splashscreen : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = default;
    [Space]
    [SerializeField] private Image _circle = default;
    [SerializeField] private Image _vrViz = default;
    [SerializeField] private Image _subTitle = default;
    [SerializeField] private Image _rCircle = default;
    [SerializeField] private Image _iCircle = default;

    [SerializeField] private Animator _anim = default;

    private void Awake()
    {
        StartCoroutine(CDelayBeforeAnim());
    }

    private IEnumerator CDelayBeforeAnim()
    {
        yield return new WaitForSeconds(0.8f);
        Play();
    }

    private void Play()
    {
        _anim.SetTrigger("Intro");
    }

    public void SlideIn()
    {

    }

    public void OpenCircle()
    {

    }

    public void SubTitle()
    {

    }

    public void EndOfAnim()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}