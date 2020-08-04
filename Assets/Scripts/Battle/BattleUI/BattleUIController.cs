using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    #region Inspector
    public Text mNumLinkText;
    public GameObject StageTextPrefab;
    public Button MenuButton;
    public GaugeBarController StageGaugeBar;
    #endregion

    public Dictionary<int, Button> unitButtons = new Dictionary<int, Button>();

    private void Start()
    {
        StageGaugeBar.showText = false;
        StageGaugeBar.Max = (BattleManager.instance.STAGE_MAX + 1)*60;
        StageManager.instance.stage_angle.AddNoti(
            (Vector3 val) => {
                StageGaugeBar.Value = Mathf.Abs(val.y);
            });

        BattleManager.instance.numLink.AddNoti(
            (int val) => {
                mNumLinkText.text = val.ToString();
            });

        MenuButton.onClick.AddListener(
            () =>
            {
                BattleUIManager.instance.Popup("MenuPopup", 0);
            });
    }

    public void AddUnitButton(int i,Button button)
    {
        unitButtons.Add(i, button);
        button.onClick.AddListener(
            () => {
                BattleUIManager.instance.Popup("UnitButtonPopup", i);
            }
            );
    }

    public void ShowStageText(string s, StageFontController.StageFontFinishAction fontFinishAction)
    {
        StageFontController sfc = GameObject.Instantiate(StageTextPrefab, transform).GetComponent<StageFontController>();
        sfc.AddStageFontAction(fontFinishAction);
        sfc.SetText(s);
    }
}
