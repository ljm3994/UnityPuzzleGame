using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UICommons;

public class StageLosePopup : PopupController
{
    #region inspector
    public float StageLoseTime;
    [Space]
    public Text tLabel;
    public Image radialBackGround;
    public RadialSlider timeSlider;
    public Text tLeft;
    public Button adBtn;

    #endregion

    public override void Setup<T>(T t)
    {
        tLabel.text = "Stage Failure";
        tLeft.gameObject.SetActive(true);
        adBtn.gameObject.SetActive(true);
        radialBackGround.gameObject.SetActive(true);
        timeSlider.Value = 0;

        adBtn.onClick.AddListener(
            () =>
            {
                /// TODO: 광고 창 후 스테이지 진행
                ///
                BattleManager.instance.SetLobbyScene();
            });

        StartCoroutine(StageLoseCounter());
    }


    IEnumerator StageLoseCounter()
    {
        float deltaTime = 0;
        while(deltaTime <= StageLoseTime)
        {
            deltaTime += Time.deltaTime;
            timeSlider.Value = deltaTime / StageLoseTime;

            float timeLeft = StageLoseTime - deltaTime;
            tLeft.text = (timeLeft > 0) ? (timeLeft > 10 ? "": "0") +  timeLeft.ToString("F2") : "00.00";
            yield return null;
        }

        /// TODO : 로비 화면으로
        ///
        BattleManager.instance.SetLobbyScene();
    }


    public override void Load()
    {
        
    }
}
