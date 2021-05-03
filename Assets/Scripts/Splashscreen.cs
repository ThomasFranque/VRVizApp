using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TMP_Text _loading = default;

    [SerializeField] private Animator _anim = default;

    private void Awake()
    {
        StartCoroutine(CDelayBeforeAnim());
    }

    private IEnumerator CDelayBeforeAnim()
    {
        yield return new WaitForSeconds(0.8f);
        Play();

        // while (true)
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         _loading.text = "Loading Resources";
        //         string dots = "";
        //         for (int j = 0; j <= i; j++)
        //         {
        //             dots += ".";
        //         }
        //         _loading.text = _loading.text + dots + "\n<size=50%> This might take a few minutes";
        //         Debug.Log(dots);
        //         yield return new WaitForSeconds(0.8f);
        //     }
        //     yield return new WaitForSeconds(0.8f);
        // }
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