using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataLoading : MonoBehaviour
{
    [SerializeField]

    Image image;

    AsyncOperation Operation;
    bool Reyurnb = false;
    float m_Percent = 0.0f;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        StartCoroutine(StartLoad("Lobby"));
    }

    // Update is called once per frame
    void Update()
    {

    }
    void PercentAction(float percent)
    {
        m_Percent = percent;
    }

    [System.Obsolete]
    public IEnumerator StartLoad(string strSceneName)
    {
        Reyurnb = GameDataBase.Instance.LoadData(PercentAction);

        float DelayTime = 0.0f;

        while (!Reyurnb)
        {
            yield return false;

            DelayTime += Time.deltaTime;

            if (m_Percent < 0.9f)
            {
                image.fillAmount = Mathf.Lerp(image.fillAmount, m_Percent, DelayTime);

                if(image.fillAmount >= m_Percent)
                {
                    DelayTime = 0f;
                }
            }
            else
            {
                image.fillAmount = Mathf.Lerp(image.fillAmount, 1f, DelayTime);

                if(image.fillAmount == 1.0f)
                {
                    SceneManager.LoadScene(strSceneName);
                    yield return true;
                }
            }
        }
    }
}
