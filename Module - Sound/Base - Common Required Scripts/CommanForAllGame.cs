using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Networking;
using CommanOfDamini.Enum;

namespace CommanOfDamini
{
    public delegate void ActionInFuns();

    public class
        CommanForAllGame : MonoBehaviour
    {
        public static CommanForAllGame instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            SafeDontDestroyOnLoad(gameObject);
        }

        #region COROUTINE

        //<For BlendShape..................................................................................
        public IEnumerator AnimationForBlanderShape(SkinnedMeshRenderer skinnedMeshRenderer, float startingWaitTime, int indexToChangeMesh, float minValue, float maxVelue, float completeInTime)
        {
            yield return new WaitForSeconds(startingWaitTime);
            float timer = 0;
            while (timer <= 1)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(indexToChangeMesh, Mathf.Lerp(minValue, maxVelue, timer));
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            skinnedMeshRenderer.SetBlendShapeWeight(indexToChangeMesh, maxVelue);
        }
        //>For BlendShape..................................................................................

        //<For Mesh..................................................................................
        public IEnumerator GlowMesh(Material material, float completeInTime, Color firstColor, Color secondColor, float timingForFirstColor, float timingForSecondColor, ActionInFuns actionAfterCompletion)
        {
            float startTime = Time.time;
            WaitForSeconds waitForFirstColor = new WaitForSeconds(timingForFirstColor);
            WaitForSeconds waitForSecondColor = new WaitForSeconds(timingForSecondColor);
            while (Time.time - startTime <= completeInTime)
            {
                material.color = firstColor;
                yield return waitForFirstColor;
                material.color = secondColor;
                yield return waitForSecondColor;
            }
            actionAfterCompletion();
        }

        public IEnumerator ChangeOpecity(Material[] material, float startingWaitTime, float completeInTime, float startingOpecity, float targetOpecity, float completeWaitTime = 0, ActionInFuns actionAfterCompletion = null)
        {
            yield return new WaitForSeconds(startingWaitTime);
            float timer = 0;
            Color color;
            while (timer <= 1)
            {
                for (int i = 0; i < material.Length; i++)
                {
                    color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, timer);
                }
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            if (actionAfterCompletion != null)
            {
                yield return new WaitForSeconds(completeWaitTime);
                actionAfterCompletion();
            }

        }

        public IEnumerator ChangeOpecity(SpriteRenderer spriteRenderer, float startingWaitTime, float completeInTime, float startingOpecity, float targetOpecity, float waitTimeForCallingAction = 0f, ActionInFuns actionAfterCompletion = null)
        {
            yield return new WaitForSeconds(startingWaitTime);
            float timer = 0;
            Color color;
            while (timer <= 1)
            {
                color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(startingOpecity, targetOpecity, timer));
                spriteRenderer.color = color;
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetOpecity);
            if (actionAfterCompletion != null)
            {
                yield return new WaitForSeconds(waitTimeForCallingAction);
                actionAfterCompletion();
            }
        }

        public IEnumerator ChangeOpecity(CanvasGroup canvasGroup, float startingWaitTime, float completeInTime, float startingOpecity, float targetOpecity, float waitTimeForCallingAction, ActionInFuns actionAfterCompletion)
        {
            Debug.Log("ChangeOpecity");
            yield return new WaitForSeconds(startingWaitTime);
            float timer = startingOpecity;
            int sign = (int)Mathf.Sign(targetOpecity - startingOpecity);
            Color color;
            Debug.Log("((((((ChangeOpecity");

            while ((sign == 1 && timer <= targetOpecity) || (sign == -1 && timer >= targetOpecity))
            {
                canvasGroup.alpha = timer;
                timer += (Time.deltaTime / completeInTime) * sign;
                yield return null;
            }
            yield return new WaitForSeconds(waitTimeForCallingAction);
            if (actionAfterCompletion != null)
            {
                actionAfterCompletion();
            }
        }

        public IEnumerator ChangeOpecity(Image image, float startingWaitTime, float completeInTime, float startingOpecity, float targetOpecity, ActionInFuns actionAfterCompletion)
        {
            yield return new WaitForSeconds(startingWaitTime);
            float timer = 0;
            Color color;
            while (timer <= 1)
            {
                color = new Color(image.color.r, image.color.g, image.color.b, timer);
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            actionAfterCompletion.CheckNullAndInvoke();
        }
        //>For Mesh..................................................................................

        //<For Move Pos..................................................................................
        public IEnumerator MovePos(GameObject obj, float startingWaitTime, float completeInTime, Vector3 startingPos, Vector3 targetPos, Action actionAfterCompletion)
        {

            yield return new WaitForSeconds(startingWaitTime);
            float timer = 0;
            while (timer <= 1)
            {
                obj.transform.position = Vector3.Lerp(startingPos, targetPos, timer);
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            obj.transform.position = targetPos;
            actionAfterCompletion?.Invoke();
        }

        public IEnumerator PingPongBetweenTwoPos(Transform givenTransform, Vector3 lookAtAxis, Vector3 startPos, Vector3 targetPos, float onePhaseTime, float startWaitTime, float numberOfCycles = Mathf.Infinity, Sign isFaceInRight = Sign.positive, float delayedCompleteTime = 0, ActionInFuns completeActionFun = null)
        {
            float timer = 0;
            int sign = 1;
            Vector3 currentStartPos = givenTransform.position;
            Vector3 currentTargetPos = startPos;
            float currentNumberOfCycle = 0f;
            float twiceNumberOfCycle = numberOfCycles * 2;

            //For only first time phase time will be different because first lerp between given transform to target pos.
            float startingReachTimeToStartPos = Trirashi(Vector3.Distance(startPos, targetPos), onePhaseTime, Vector3.Distance(currentStartPos, currentTargetPos));

            yield return new WaitForSeconds(startWaitTime);

            timer = 0;
            float startTime = Time.time;
            while (timer <= 1)
            {
                givenTransform.position = Vector3.Lerp(currentStartPos, currentTargetPos, timer);
                givenTransform.rotation = Custom2DLookAt(givenTransform.position, currentTargetPos, lookAtAxis, isFaceInRight);
                timer += Time.deltaTime / startingReachTimeToStartPos;
                yield return null;
            }
            currentStartPos = startPos;
            currentTargetPos = targetPos;
            timer = 0f;

            while (true)
            {
                givenTransform.position = Vector3.Lerp(currentStartPos, currentTargetPos, timer);

                givenTransform.rotation = Custom2DLookAt(givenTransform.position, currentTargetPos, lookAtAxis, isFaceInRight);

                if (timer >= 1)
                {
                    if (sign == 1)
                    {
                        sign = -1;
                        currentStartPos = targetPos;
                        currentTargetPos = startPos;
                    }
                    else
                    {
                        sign = 1;
                        currentStartPos = startPos;
                        currentTargetPos = targetPos;
                    }
                    timer = 0;
                }
                timer += Time.deltaTime / onePhaseTime;
                currentNumberOfCycle += Time.deltaTime / onePhaseTime;
                if (currentNumberOfCycle >= twiceNumberOfCycle) //*2 because we want full cycle with both side pingpong
                {
                    break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(delayedCompleteTime);
            completeActionFun();
        }

        public IEnumerator FollowGivenPathAndMoveOntoIt(GameObject movingObject, Vector3[] path, float startingWaitTime, float completeInTime, float atEachPointWaitTime, bool isBabyStep = false, Action afterOneStepAction = null, float waitTImeBeforeCompleteAction = 0, Action completeAction = null)
        {
            if (startingWaitTime != 0)
            {
                yield return new WaitForSeconds(startingWaitTime);
            }

            float[] timeToEachPointToCover;
            float[] distanceAtEachPoint;
            float totalDistance = 0;
            float timer = 0;
            Vector3 startScale = movingObject.transform.localScale;
            Vector3 peakPointScale = movingObject.transform.localScale + (movingObject.transform.localScale / 4);

            timeToEachPointToCover = new float[path.Length];
            distanceAtEachPoint = new float[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                if (i == path.Length - 1)
                {
                    distanceAtEachPoint[i] = Vector3.Distance(path[i], path[0]);
                }
                else
                {
                    distanceAtEachPoint[i] = Vector3.Distance(path[i], path[i + 1]);
                }
                totalDistance += distanceAtEachPoint[i];
            }

            for (int i = 0; i < path.Length; i++)
            {
                timeToEachPointToCover[i] = ((distanceAtEachPoint[i] * completeInTime) / totalDistance) + atEachPointWaitTime;
            }

            int nextIndex;

            for (int i = 0; i < path.Length - 1; i++)
            {
                timer = 0;
                while (timer < 1)
                {
                    //if (i == path.Length - 1)
                    //{
                    //    nextIndex = 0;
                    //}
                    //else
                    //{
                    //    nextIndex = i + 1;
                    //}
                    movingObject.transform.position = Vector3.Lerp(path[i], path[i + 1], timer);
                    if (isBabyStep)
                    {
                        if (timer <= 0.5f)
                        {
                            movingObject.transform.localScale = Vector3.Lerp(startScale, peakPointScale, timer.MappingValue(0, 0.5f, 0, 1));
                        }
                        else
                        {
                            movingObject.transform.localScale = Vector3.Lerp(peakPointScale, startScale, timer.MappingValue(0.5f, 1f, 0, 1));
                        }
                    }
                    timer += Time.deltaTime / timeToEachPointToCover[i];
                    yield return null;
                }

                afterOneStepAction?.Invoke();

                if (isBabyStep)
                {
                    movingObject.transform.localScale = startScale;
                }
            }
            movingObject.transform.position = path[path.Length - 1];

            if (isBabyStep)
            {
                movingObject.transform.localScale = startScale;
            }

            if (waitTImeBeforeCompleteAction != 0)
            {
                yield return new WaitForSeconds(waitTImeBeforeCompleteAction);
            }
            completeAction?.Invoke();
        }

        public IEnumerator PingPongBetweenTwoColor(Image image, Color startColor, Color targetColor, float onePhaseTime, float startWaitTime)
        {
            float timer = 0;
            int sign = 1;

            image.color = startColor;
            yield return new WaitForSeconds(startWaitTime);


            while (true)
            {

                image.color = Color.Lerp(startColor, targetColor, timer);
                if (timer >= 1)
                {
                    sign = -1;
                }
                else if (timer < 0)
                {
                    sign = 1;
                }

                timer += sign * Time.deltaTime / onePhaseTime;
                yield return null;
            }
        }

        public IEnumerator PingPongBetweenTwoSize(Transform obj, Vector3 startSize, Vector3 targetSize, float onePhaseTime, float startWaitTime)
        {
            float timer = 0;
            int sign = 1;

            obj.transform.localScale = startSize;
            yield return new WaitForSeconds(startWaitTime);


            while (true)
            {

                obj.transform.localScale = Vector3.Lerp(startSize, targetSize, timer);
                if (timer >= 1)
                {
                    sign = -1;
                }
                else if (timer < 0)
                {
                    sign = 1;
                }

                timer += sign * Time.deltaTime / onePhaseTime;
                yield return null;
            }
        }

        public IEnumerator PingPongBetweenTwoImages(Image image, Sprite startSprite, Sprite targetSprite, float onePhaseTime, float startWaitTime)
        {
            float timer = 0;
            int sign = 1;
            Sprite currentSprite = startSprite;
            image.sprite = targetSprite;
            yield return new WaitForSeconds(startWaitTime);


            while (true)
            {

                image.sprite = currentSprite;
                if (timer >= 1)
                {
                    currentSprite = startSprite;
                    sign = -1;
                }
                else if (timer < 0)
                {
                    currentSprite = targetSprite;
                    sign = 1;
                }

                timer += sign * Time.deltaTime / onePhaseTime;
                yield return null;
            }
        }


        public IEnumerator LerpAllTransformValues(Transform passedTransform, Transform targetTransform, float waitTime, float completeInTime, Action actionOnCompletion)
        {
            yield return new WaitForSeconds(waitTime);
            float timer = 0;
            while (timer <= 1)
            {
                passedTransform.position = Vector3.Lerp(passedTransform.position, targetTransform.position, timer);
                passedTransform.localScale = Vector3.Lerp(passedTransform.localScale, targetTransform.lossyScale, timer);
                passedTransform.rotation = Quaternion.Slerp(passedTransform.rotation, targetTransform.rotation, timer);
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            passedTransform.position = targetTransform.position;
            passedTransform.localScale = targetTransform.lossyScale;
            passedTransform.rotation = targetTransform.rotation;
            actionOnCompletion();
        }
        //<For Move Pos..................................................................................


        //<For Scalling..................................................................................
        public IEnumerator Scaling(GameObject obj, float startingWaitTime, float completeInTime, Vector3 startingSize, Vector3 targetSize, float waitTimeToCallCompleteAction, Action actionAfterCompletion)
        {
            float timer = 0;
            if (startingWaitTime != 0)
            {
                yield return new WaitForSeconds(startingWaitTime);
            }
            while (timer <= 1)
            {
                obj.transform.localScale = Vector3.Lerp(startingSize, targetSize, timer);
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            if (waitTimeToCallCompleteAction != 0)
            {
                yield return new WaitForSeconds(waitTimeToCallCompleteAction);
            }
            actionAfterCompletion?.Invoke();
        }

        IEnumerator PingPongBetweenTwoSizes(GameObject obj, float minDuration, float maxDuration, Vector3 startingSize, Vector3 targetSize)
        {
            float timer = 0;
            float currentOneCycleDuration = UnityEngine.Random.Range(minDuration, maxDuration);
            float sign = 1;
            bool shouldResetCommanValues = false;

            while (true)
            {
                obj.transform.localScale = Vector3.Lerp(startingSize, targetSize, timer);
                timer = timer + (sign * (Time.deltaTime / currentOneCycleDuration));
                if (sign == 1 && timer > 1)
                {
                    timer = 0;
                    shouldResetCommanValues = true;
                }
                else if (sign == -1 && timer < 0)
                {
                    timer = 0;
                    shouldResetCommanValues = true;
                }

                if (shouldResetCommanValues)
                {
                    sign = -1 * sign;
                    currentOneCycleDuration = UnityEngine.Random.Range(minDuration, maxDuration);
                    shouldResetCommanValues = false;
                }
                yield return null;
            }
        }
        //>For Scalling.....................................................................................

        public IEnumerator BlinkAccordingToTime(Image image, float startWaitTime, float completeInTime, float startTimeForDarkColor, float targetTimeForDarkColor, float startTimeForLightColor, float targetTimeForLightColor, Color darkColor, Color lightColor, float completeWaitTime = 0, ActionInFuns actionAfterCompletion = null)
        {
            //yield return null;
            Color currentColor;
            bool isDrakColor = true;
            float currentTimeForDarkColor = startTimeForDarkColor;
            float currentTimeForLightColor = startTimeForLightColor;
            float currentTime = 0;
            float timer = 0;
            if (isDrakColor)
            {
                currentColor = darkColor;
                currentTime = currentTimeForDarkColor;
            }
            else
            {
                currentColor = lightColor;
                currentTime = currentTimeForLightColor;
            }

            float startTime = Time.time;
            while (timer <= 1)
            {
                image.color = currentColor;
                currentTimeForDarkColor = Mathf.Lerp(startTimeForDarkColor, targetTimeForDarkColor, timer);
                currentTimeForLightColor = Mathf.Lerp(startTimeForLightColor, targetTimeForLightColor, timer);
                if (Time.time - startTime > currentTime)
                {
                    isDrakColor = !isDrakColor;
                    if (isDrakColor)
                    {
                        currentColor = darkColor;
                        currentTime = currentTimeForDarkColor;
                    }
                    else
                    {
                        currentColor = lightColor;
                        currentTime = currentTimeForLightColor;
                    }
                    startTime = Time.time;
                }
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            yield return new WaitForSeconds(completeWaitTime);
            actionAfterCompletion();
        }

        public IEnumerator BlinkAccordingToTime(SpriteRenderer spriteRenderer, float startWaitTime, float completeInTime, float startTimeForDarkColor, float targetTimeForDarkColor, float startTimeForLightColor, float targetTimeForLightColor, Color darkColor, Color lightColor, float completeWaitTime = 0, ActionInFuns actionAfterCompletion = null)
        {

            //yield return null;
            Color currentColor;
            bool isDrakColor = true;
            float currentTimeForDarkColor = startTimeForDarkColor;
            float currentTimeForLightColor = startTimeForLightColor;
            float currentTime = 0;
            float timer = 0;
            if (isDrakColor)
            {
                currentColor = darkColor;
                currentTime = currentTimeForDarkColor;
            }
            else
            {
                currentColor = lightColor;
                currentTime = currentTimeForLightColor;
            }

            float startTime = Time.time;
            while (timer <= 1)
            {
                spriteRenderer.color = currentColor;
                currentTimeForDarkColor = Mathf.Lerp(startTimeForDarkColor, targetTimeForDarkColor, timer);
                currentTimeForLightColor = Mathf.Lerp(startTimeForLightColor, targetTimeForLightColor, timer);
                if (Time.time - startTime > currentTime)
                {
                    isDrakColor = !isDrakColor;
                    if (isDrakColor)
                    {
                        currentColor = darkColor;
                        currentTime = currentTimeForDarkColor;
                    }
                    else
                    {
                        currentColor = lightColor;
                        currentTime = currentTimeForLightColor;
                    }
                    startTime = Time.time;
                }
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            spriteRenderer.color = darkColor;
            yield return new WaitForSeconds(completeWaitTime);
            actionAfterCompletion();
        }


        public IEnumerator BlinkAccordingToTime(SpriteRenderer spriteRenderer, float startWaitTime, float completeInTime, float startTimeForDarkColor, float targetTimeForDarkColor, float startTimeForLightColor, float targetTimeForLightColor, Sprite darkColorSprite, Sprite lightColorSprite, ActionInFuns actionAfterCompletion)
        {

            //yield return null;
            Sprite currentSprite;
            bool isDrakColor = true;
            float currentTimeForDarkColor = startTimeForDarkColor;
            float currentTimeForLightColor = startTimeForLightColor;
            float currentTime = 0;
            float timer = 0;
            if (isDrakColor)
            {
                currentSprite = darkColorSprite;
                currentTime = currentTimeForDarkColor;
            }
            else
            {
                currentSprite = lightColorSprite;
                currentTime = currentTimeForLightColor;
            }

            float startTime = Time.time;
            while (timer <= 1)
            {
                spriteRenderer.sprite = currentSprite;
                currentTimeForDarkColor = Mathf.Lerp(startTimeForDarkColor, targetTimeForDarkColor, timer);
                currentTimeForLightColor = Mathf.Lerp(startTimeForLightColor, targetTimeForLightColor, timer);
                if (Time.time - startTime > currentTime)
                {
                    isDrakColor = !isDrakColor;
                    if (isDrakColor)
                    {
                        currentSprite = darkColorSprite;
                        currentTime = currentTimeForDarkColor;
                    }
                    else
                    {
                        currentSprite = lightColorSprite;
                        currentTime = currentTimeForLightColor;
                    }
                    startTime = Time.time;
                }
                timer += Time.deltaTime / completeInTime;
                yield return null;
            }
            actionAfterCompletion();
        }
        #endregion

        #region UI_RELATED
        public IEnumerator FillProgressBar(Image image, float startFromDuration, float duration, float startWaitTime, bool increasing = true, float completeWaitTime = 0, Action onCompleteAction = null)
        {
            float timer = 0;
            image.fillAmount = timer;
            timer = startFromDuration.MappingValue(0, duration, 0, 1);
            Debug.Log("SstartFromDuration = " + startFromDuration);
            Debug.Log("Start timer = " + timer);

            yield return new WaitForSeconds(startWaitTime);

            while (timer <= 1)
            {
                if (increasing)
                    image.fillAmount = timer;
                else
                    image.fillAmount = (1 - timer);
                timer += Time.deltaTime / duration;
                yield return null;
            }
            image.fillAmount = 1;
            yield return new WaitForSeconds(completeWaitTime);
            onCompleteAction?.Invoke();
        }
        #endregion

        #region RotationRelated
        public Quaternion Custom2DLookAt(Vector3 fromPos, Vector3 toPos, Vector3 axisToRotate, Sign isFaceInRight = Sign.positive)
        {
            var dir = (toPos - fromPos) * (int)isFaceInRight;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, axisToRotate);
        }

        public IEnumerator LerpRotation(GameObject obj, Vector3 startRotation, Vector3 targetRotation, float duration, float waitTime = 0f, float completeWaitTime = 0, ActionInFuns completeAction = null)
        {
            float timer = 0;
            if (waitTime != 0)
            {
                yield return new WaitForSeconds(waitTime);
            }
            while (timer <= 1)
            {
                obj.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, timer));
                timer += Time.deltaTime / duration;
                yield return null;
            }
            obj.transform.rotation = Quaternion.Euler(targetRotation);
            if (completeAction != null)
            {
                yield return new WaitForSeconds(completeWaitTime);
                completeAction();
            }
        }
        #endregion

        #region Mapping
        #endregion

        #region Shaking
        public IEnumerator Shake(GameObject shakingObject, float startingWaitTime, float duration, float magnitude, float completeWaitTime = 0, ActionInFuns completeAction = null)
        {
            Vector3 orignalPosition = shakingObject.transform.position;
            float startTime = 0f;

            if (startingWaitTime != 0)
            {
                yield return new WaitForSeconds(startingWaitTime);
            }

            startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                float y = UnityEngine.Random.Range(-1f, 1f) * (magnitude / 4);

                shakingObject.transform.position = new Vector3(orignalPosition.x + x, orignalPosition.y + y, shakingObject.transform.position.z);
                yield return null;
            }
            shakingObject.transform.position = orignalPosition;

            if (completeAction != null)
            {
                yield return new WaitForSeconds(completeWaitTime);
                completeAction();
            }
        }

        public IEnumerator ShakeWithRotation(GameObject shakingObject, float startingWaitTime, float duration, float magintude, float completeWaitTime = 0, ActionInFuns completeAction = null)
        {

            if (startingWaitTime != 0)
            {
                yield return new WaitForSeconds(startingWaitTime);
            }

            float startTime = Time.time;
            float currentRotaion;
            float startRotation;
            int currentSignRotation = 1;
            Vector3 currentAngle = Vector3.zero;
            Quaternion orginalRotation = shakingObject.transform.rotation;

            startRotation = shakingObject.transform.eulerAngles.z;
            currentRotaion = startRotation + (currentSignRotation * magintude);
            currentAngle = new Vector3(shakingObject.transform.eulerAngles.x, shakingObject.transform.eulerAngles.y, currentRotaion);
            currentAngle = currentAngle.SetZ(currentRotaion);

            while (Time.time - startTime < duration)
            {
                currentRotaion = startRotation + (currentSignRotation * magintude);
                currentAngle = new Vector3(shakingObject.transform.eulerAngles.x, shakingObject.transform.eulerAngles.y, currentRotaion);
                currentSignRotation = -1 * currentSignRotation;
                shakingObject.transform.eulerAngles = currentAngle;
                yield return null;
            }

            shakingObject.transform.rotation = orginalRotation;
            if (completeAction != null)
            {
                yield return new WaitForSeconds(completeWaitTime);
                completeAction();
            }
        }

        public IEnumerator ShakeWithRotationWithRandom(GameObject shakingObject, float startingWaitTime, float duration, float magintude, float completeWaitTime, ActionInFuns completeAction, bool loop = false)
        {

            if (startingWaitTime != 0)
            {
                yield return new WaitForSeconds(startingWaitTime);
            }

            float startTime;
            float currentRotaion;
            float startRotation;
            Vector3 currentAngle;
            Quaternion orginalRotation;
            orginalRotation = shakingObject.transform.rotation;
            WaitForSeconds waitForSeconds = new WaitForSeconds(duration / 20f);

            do
            {

                startTime = Time.time;
                currentAngle = Vector3.zero;

                startRotation = orginalRotation.z;
                currentRotaion = startRotation + (UnityEngine.Random.Range(-1, 1) * magintude);
                currentAngle = new Vector3(orginalRotation.eulerAngles.x, orginalRotation.eulerAngles.y, currentRotaion);
                currentAngle = currentAngle.SetZ(currentRotaion);

                while (Time.time - startTime < duration)
                {
                    currentRotaion = startRotation + (UnityEngine.Random.Range(-1, 1) * magintude);
                    currentAngle = new Vector3(shakingObject.transform.eulerAngles.x, shakingObject.transform.eulerAngles.y, currentRotaion);
                    shakingObject.transform.eulerAngles = currentAngle;
                    yield return null;
                }
            } while (loop);

            shakingObject.transform.rotation = orginalRotation;
            if (completeAction != null)
            {
                yield return new WaitForSeconds(completeWaitTime);
                completeAction();
            }
        }
        #endregion

        #region MATHEMATIC_TERM_RELATED
        public float Trirashi(float oneValue, float relativeValue, float secondValue)
        {
            return (secondValue * relativeValue) / oneValue;
        }

        public Vector3 GetDirectionThroughAngle(Vector3 angle, float distance)
        {
            var rotation = Quaternion.Euler(angle);
            var forward = Vector3.forward * distance;
            return rotation * forward;
        }
        #endregion

        #region Sprite
        public Vector2 MaintainObjectSizeAccordingToNewSprite(Vector2 oldTextureActualSize, Vector2 newTextureActualSize, Vector2 objOldScaleSize)
        {
            Vector2 objNewScaleSize;
            float x, y;
            x = (oldTextureActualSize.x * objOldScaleSize.x) / newTextureActualSize.x;
            y = (oldTextureActualSize.y * objOldScaleSize.y) / newTextureActualSize.y;
            objNewScaleSize = new Vector3(x, y);
            return objNewScaleSize;
        }
        #endregion

        #region Texture

        public Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;
            if (System.IO.File.Exists(FilePath))
            {
                FileData = System.IO.File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;

        }
        #endregion

        #region Video
        public IEnumerator PlayVideo(VideoPlayer videoPlayer, AudioSource audioSource, VideoClip videoToPlay, RawImage rawImage, Color colorAtStarting, ActionInFuns Skip)
        {
            //Add VideoPlayer to the GameObject
            if (!videoPlayer.isPlaying)
            {

                Color startingColorOfRawImage = rawImage.color;
                rawImage.color = colorAtStarting;

                //Disable Play on Awake for both Video and Audio
                videoPlayer.playOnAwake = false;
                audioSource.playOnAwake = false;

                //We want to play from video clip not from url
                videoPlayer.source = VideoSource.VideoClip;

                //Set Audio Output to AudioSource
                videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

                //Assign the Audio from Video to AudioSource to be played
                videoPlayer.EnableAudioTrack(0, true);
                videoPlayer.SetTargetAudioSource(0, audioSource);

                //Set video To Play then prepare Audio to prevent Buffering
                videoPlayer.clip = videoToPlay;
                videoPlayer.Prepare();

                //Wait until video is prepared
                while (!videoPlayer.isPrepared)
                {
                    yield return null;
                }

                //Assign the Texture from Video to RawImage to be displayed
                rawImage.texture = videoPlayer.texture;

                rawImage.color = startingColorOfRawImage;

                //Play Video
                videoPlayer.Play();

                //Play Sound
                audioSource.Play();

                while (videoPlayer.isPlaying)
                {
                    yield return null;
                }

                Skip();
            }
        }

        #endregion

        #region InputRelated
        public IEnumerator CheckOneTimeInputTouch(ActionInFuns onTouchEvent)
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    onTouchEvent();
                    break;
                }
                yield return null;
            }
        }

        public bool IsPointerOverUIObject()
        {
            //            return EventSystem.current.IsPointerOverGameObject();
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public bool IsPointerOverUIObject(Vector2 screenPos)
        {

            //            return EventSystem.current.IsPointerOverGameObject();
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(screenPos.x, screenPos.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        #endregion

        #region ParabolaMoving
        public IEnumerator ProjectileMotion(GameObject obj, Vector3 targetPosition, float hopHeight, float startWaitTime, float duration, float completeActionWaitTime, Action completeAction)
        {
            yield return new WaitForSeconds(startWaitTime);
            Vector3 startingPos = obj.transform.position;
            float timer = 0;
            float height = 0;

            while (timer <= 1)
            {
                height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
                obj.transform.position = Vector3.Lerp(startingPos, targetPosition, timer) + (Vector3.up * height);
                timer += Time.deltaTime / duration;
                yield return null;
            }

            Debug.Log("Completed");
            obj.transform.position = targetPosition;
            yield return new WaitForSeconds(completeActionWaitTime);
            completeAction?.Invoke();
        }
        #endregion

        #region Random
        public Vector3 RandomOfTwoDuration(Vector3 minTime, Vector3 maxPos)
        {
            return Vector3.Lerp(minTime, maxPos, UnityEngine.Random.Range(0f, 1f));
        }
        #endregion

        #region ListRelated
        #endregion

        #region COLOR_RELATED
        public void ChangeColor(ref Color color)
        {
            color = Color.blue;
        }
        #endregion

        #region Positions
        public void GetInBetweenPositions(List<Vector2> posList, Vector2 startPos, Vector2 endPos, int noOfParts)
        {
            noOfParts = Mathf.Clamp(noOfParts, 2, int.MaxValue);
            float onePartLerpPart = 1f / noOfParts;
            float timer = 0;

            while (timer < 1)
            {
                posList.Add(Vector2.Lerp(startPos, endPos, timer));
                timer += onePartLerpPart;
            }
            posList.Add(endPos);
        }

        public void InstantiateInBetweenObjects(GameObject prefab, List<Transform> listOfGameObject, Transform parentTransform, Vector2 startPos, Vector2 endPos, int noOfParts)
        {
            noOfParts = Mathf.Clamp(noOfParts, 2, int.MaxValue);
            float onePartLerpPart = 1f / noOfParts;
            float timer = 0;

            while (timer < 1)
            {
                listOfGameObject.Add(Instantiate(prefab, Vector2.Lerp(startPos, endPos, timer), prefab.transform.rotation, parentTransform.transform).transform);
                timer += onePartLerpPart;
            }
            listOfGameObject.Add(Instantiate(prefab, endPos, prefab.transform.rotation, parentTransform.transform).transform);
        }
        #endregion

        #region TIMER_RELATED
        public IEnumerator TimerUpdate(double requiredTime, DateTime startTime, Action<string, double, TimeSpan> getTimeDiffAction, bool isDecreasingOrder = true, string format = "#00:00", Action onCompleteAction = null)
        {
            double currentTime;
            double timeDiff;
            string timeDiffString;

            TimeSpan timeSpan = (DateTime.Now - startTime);
            currentTime = timeSpan.TotalSeconds;
            Debug.Log("current time = " + currentTime);

            timeDiff = requiredTime - currentTime;
            Debug.Log("timer diff = " + timeDiff);
            do
            {
                if (timeDiff <= 0)
                {
                    Debug.Log("Timer finish");
                    onCompleteAction?.Invoke();
                    yield break;
                }

                if (isDecreasingOrder)
                {
                    timeDiffString = timeDiff.FloatToTime(format);
                    getTimeDiffAction?.Invoke(timeDiffString, timeDiff, timeSpan);
                }
                else
                {
                    timeDiffString = (requiredTime - (timeDiff)).FloatToTime(format);
                    getTimeDiffAction?.Invoke(timeDiffString, requiredTime - (timeDiff), timeSpan);
                }


                timeSpan = (DateTime.Now - startTime);
                currentTime = timeSpan.TotalSeconds;
                timeDiff = requiredTime - currentTime;
                yield return null;
            } while (true);
        }
       
        #endregion

        #region DOWNLOAD_RELATED
        public Coroutine DownloadTexture(string url, Action<Texture2D> textureRecivedAction, Action<string> errorAction)
        {
            return StartCoroutine(DownloadTextureCoroutine(url, textureRecivedAction, errorAction));
        }

        IEnumerator DownloadTextureCoroutine(string url, Action<Texture2D> textureRecivedAction, Action<string> errorAction)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                    errorAction?.Invoke(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    textureRecivedAction?.Invoke(texture);
                }
            }
        }
        #endregion

        #region Coroutine_Related
        public void StopPassedCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        //<Simple Coroutine...............................................................................
        public Coroutine CallEventAfterSomeTime(float waitTime, Action action)
        {
           // Debug.LogWarning($" CallEventAfterSomeTime action: {action.Method.Name}");
            return StartCoroutine(CallEventAfterSomeTimeCoroutine(waitTime, ()=> { action?.Invoke(); }));
        }

        public IEnumerator CallEventAfterSomeTimeCoroutine(float waitTime, ActionInFuns action)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.LogWarning($" CallEventAfterSomeTimeCoroutine action: {action.Method.Name}");
            action();
        }

        public void CallEventAtEndOfFrame(ActionInFuns action)
        {
            StartCoroutine(CallEventAtEndOfFrameCoroutine(action));
        }

        IEnumerator CallEventAtEndOfFrameCoroutine(ActionInFuns action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }
        //>Simple Coroutine...............................................................................

        #endregion


        #region DESTROY_RELATED
        public static void SafeDontDestroyOnLoad(GameObject obj)
        {
            obj.transform.parent = null;
            DontDestroyOnLoad(obj);
        }
        #endregion
    }

    public class HandleQueueFuns
    {
        bool isLastOneCallingAlreadyWorking = false;
        float delayTimeForLastOne = 0f;
        ActionInFuns callLastOneOnlyAction;

        public IEnumerator CallOnlyLastOneFunAfterDelay(float delayTime, ActionInFuns passedfunToCallAfterDelay)
        {
            delayTimeForLastOne = delayTime;
            callLastOneOnlyAction = passedfunToCallAfterDelay;
            if (!isLastOneCallingAlreadyWorking)
            {
                isLastOneCallingAlreadyWorking = true;
                float startTime = Time.time;
                while (Time.time - startTime < delayTimeForLastOne)
                {
                    yield return null;
                }
                callLastOneOnlyAction();
                isLastOneCallingAlreadyWorking = false;
            }
        }
    }
}