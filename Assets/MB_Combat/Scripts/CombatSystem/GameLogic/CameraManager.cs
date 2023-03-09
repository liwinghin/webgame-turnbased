using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager m_Instance = null;
        public static CameraManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<CameraManager>();
                return m_Instance;
            }
        }

        public Camera mainCam = null;

        [Header("Shaking Camera")]
        public bool isShaked = false;
        public AnimationCurve curve;
        public float duration = 1f;

        [Header("Attack Motion")]
        public bool isFinished = false;
        public float zoomInSpeed = 0.4f;
        public float zoomOutSpeed = 0.25f;

        public BaseFormationHandler targetTeam = null;
        public CharacterUnit target = null;

        private Vector3 originalPos = new Vector3(3f, 1.2f, 0.7f);
        private Vector3 originalAngles = new Vector3(0, 270, 0);

        public Vector3 pirateAttackPos = new Vector3(3f, 1.2f, 0.7f);
        public Vector3 pirateAttackAngles = new Vector3(0, 270, 0);

        public Vector3 enemyAttackPos = new Vector3(-3f, 1.2f, 0f);
        public Vector3 enemyAttackAngles = new Vector3(0, 75, 0);

        private void Start()
        {
            originalPos = mainCam.transform.localPosition;
            originalAngles = mainCam.transform.localEulerAngles;
        }

        public void ShakeCamera()
        {
            if(isShaked) { return; }
            isShaked = true;
            StartCoroutine(ShakingCamera());
        }

        public void ChasingTarget(BaseFormationHandler team, CharacterUnit character)
        {
            targetTeam = team;
            target = character;
            StartCoroutine(StartAttackCamera());
        }

        IEnumerator ShakingCamera()
        {
            var startMainCamPostion = mainCam.transform.position;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float strengh = curve.Evaluate(elapsedTime / duration);
                mainCam.transform.position = startMainCamPostion + Random.insideUnitSphere * strengh;
                yield return null;
            }
            mainCam.transform.position = startMainCamPostion;
            isShaked = false;
        }

        public IEnumerator StartAttackCamera()
        {
            if (target == null) { yield return null; }

            isFinished = false;
            mainCam.transform.SetParent(target.transform);
            Vector3 offsetPos = (targetTeam.teamType == "Player") ? pirateAttackPos : enemyAttackPos;
            Vector3 offsetAngle = (targetTeam.teamType == "Player") ? pirateAttackAngles : enemyAttackAngles;
            yield return Move(mainCam.transform.localPosition, offsetPos, mainCam.transform.localEulerAngles, offsetAngle, zoomInSpeed);
            yield return new WaitUntil(() => target.IsAttacking == false);
            mainCam.transform.SetParent(null);
            yield return Move(mainCam.transform.localPosition, originalPos, mainCam.transform.localEulerAngles, originalAngles, zoomOutSpeed);
            targetTeam = null;
            target = null;
            yield return new WaitForSeconds(0.25f);
            isFinished = true;
        }

        private IEnumerator Move(Vector3 startPos, Vector3 targetPos, Vector3 startAngle, Vector3 finishAngle, float seconds)
        {
            float elapsedTime = 0;
            mainCam.transform.localPosition = startPos;

            while (elapsedTime < seconds)
            {
                mainCam.transform.localPosition = Vector3.Lerp(startPos, targetPos, (elapsedTime / seconds));
                //mainCam.transform.LookAt(target.transform);
                float xLerp = Mathf.LerpAngle(startAngle.x, finishAngle.x, (elapsedTime / seconds));
                float yLerp = Mathf.LerpAngle(startAngle.y, finishAngle.y, (elapsedTime / seconds));
                float zLerp = Mathf.LerpAngle(startAngle.z, finishAngle.z, (elapsedTime / seconds));

                mainCam.transform.localEulerAngles = new Vector3(xLerp, yLerp, zLerp);
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            mainCam.transform.localPosition = targetPos;
            mainCam.transform.localEulerAngles = finishAngle;
        }
    }
}