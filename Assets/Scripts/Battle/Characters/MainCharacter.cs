using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    #region inspector
    public float EnterenceSeconds = 1;
    public float WalkOutSeconds = 1;
    public Animator animator;
    #endregion


    public delegate void MainCharacterNoti();
    public enum STATE
    {
        ENTERENCE,
        WALKOUT
    }

    private void Awake()
    {
    }

    public void SetIdle()
    {
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
    }

    public IEnumerator CharacterEnterence(STATE state)
    {
        Transform targetPosition;
        Vector3 Pos_origin = transform.position;
        float targetSeconds;

        if (state == STATE.ENTERENCE)
        {
            targetPosition = StageManager.instance.MainCharacterIn;
            targetSeconds = EnterenceSeconds;
            animator.SetBool("isRun", true);
            animator.SetBool("isJump", false);

            float deltaTime = 0;
            while (deltaTime < EnterenceSeconds)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(Pos_origin, targetPosition.position, deltaTime / targetSeconds);
                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPosition.position;
        }
        else
        {
            targetPosition = StageManager.instance.MainCharacterOut;
            targetSeconds = WalkOutSeconds;
            animator.SetBool("isJump", true);
            animator.SetBool("isRun", false);

            float length = animator.runtimeAnimatorController.animationClips[0].length;
            yield return new WaitForSeconds(length);
        }
    }
}
