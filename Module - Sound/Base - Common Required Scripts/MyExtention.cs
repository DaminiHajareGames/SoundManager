using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CommanOfDamini.Enum;

namespace CommanOfDamini
{
    public static class MyExtention
    {
        #region GAMEOBJECT_RELATED
        /// <summary>
        /// Open the specified component.
        /// </summary>
        /// <param name="obj">Component.</param>
        public static void Open(this GameObject obj)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        /// <summary>
        /// Close the specified component.
        /// </summary>
        /// <param name="component">Component.</param>
        public static void Close(this GameObject component)
        {
            if (component != null)
                component.SetActive(false);
        }


        public static void Open(this Component component)
        {
            if (component != null && component.gameObject != null)
                component.gameObject.SetActive(true);
        }

        /// <summary>
        /// Close the specified component.
        /// </summary>
        /// <param name="component">Component.</param>
        public static void Close(this Component component)
        {
            if (component != null && component.gameObject != null)
                component.gameObject.SetActive(false);
        }
        #endregion

        #region SPRITE_RELATED
        public static Vector2 SizeOfObject(this SpriteRenderer renderer)
        {
            return renderer.bounds.size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="spriteBoundry"></param>
        /// <param name="offsetFromBoundry">"-> positive - inner , negative - outer"</param>
        /// <returns></returns>

        public static Vector3 BoundryPosition(this SpriteRenderer spriteRenderer, Directions2D spriteBoundry, float offsetFromBoundry = 0)
        {
            Vector3 targetPos = Vector3.zero;
            Vector3 sizeOfObj = spriteRenderer.bounds.size;

            switch (spriteBoundry)
            {
                case Directions2D.left:
                    targetPos = spriteRenderer.transform.position + (Vector3.left * ((sizeOfObj.x / 2) - offsetFromBoundry));
                    break;

                case Directions2D.right:
                    targetPos = spriteRenderer.transform.position + (Vector3.right * ((sizeOfObj.x / 2) - offsetFromBoundry));
                    break;

                case Directions2D.up:
                    targetPos = spriteRenderer.transform.position + (Vector3.up * ((sizeOfObj.y / 2) - offsetFromBoundry));
                    break;

                case Directions2D.down:
                    targetPos = spriteRenderer.transform.position + (Vector3.down * ((sizeOfObj.y / 2) - offsetFromBoundry));
                    break;

                case Directions2D.onSamePos:
                    targetPos = spriteRenderer.transform.position;
                    break;
            }
            targetPos = targetPos.SetZ(spriteRenderer.transform.position.z);
            return targetPos;

        }

        public static bool IsNearToPos(this Vector3 pos, Vector3 targetPos, float minDistance = 0){
            if(Vector3.Distance(pos, targetPos) <= minDistance){
                return true;
            }
            return false;
        }
        #endregion

        #region MATHEMATIC_TERM_RELATED
        public static float MappingValue(this float firstValue, float firstMin, float firstMax, float secondMin, float secondMax)
        {
            return Mathf.Lerp(secondMin, secondMax, Mathf.InverseLerp(firstMin, firstMax, firstValue));
            // return ((firstValue / (firstMin + firstMax)) * (secondMin + secondMax));
        }

        public static float MappingValue(this int firstValue, float firstMin, float firstMax, float secondMin, float secondMax)
        {
            return Mathf.Lerp(secondMin, secondMax, Mathf.InverseLerp(firstMin, firstMax, firstValue));
            // return ((firstValue / (firstMin + firstMax)) * (secondMin + secondMax));
        }
        #endregion

        #region DIRECTION_RELATED
        public static Directions CheckRelativeDirectionInX(this Vector3 pos, Vector3 relativePos)
        {
            if (pos.x < relativePos.x)
            {
                return Directions.right;
            }
            else if (pos.x > relativePos.x)
            {
                return Directions.left;
            }
            else
            {
                return Directions.onSamePos;
            }
        }

        public static Directions CheckRelativeDirectionInY(this Vector3 pos, Vector3 relativePos)
        {
            if (pos.y < relativePos.y)
            {
                return Directions.up;
            }
            else if (pos.y > relativePos.y)
            {
                return Directions.down;
            }
            else
            {
                return Directions.onSamePos;
            }
        }

        public static Directions CheckRelativeDirectionInX(this Vector3 pos, float relativePosInX)
        {
            if (pos.x < relativePosInX)
            {
                return Directions.right;
            }
            else if (pos.x > relativePosInX)
            {
                return Directions.left;
            }
            else
            {
                return Directions.onSamePos;
            }
        }

        public static Directions OppDirection(this Directions direction)
        {

            if (direction == Directions.left)
            {
                return Directions.right;
            }
            else if (direction == Directions.right)
            {
                return Directions.left;
            }
            else if (direction == Directions.up)
            {
                return Directions.down;
            }
            else if (direction == Directions.down)
            {
                return Directions.up;
            }
            else if (direction == Directions.forward)
            {
                return Directions.back;
            }
            else if (direction == Directions.back)
            {
                return Directions.forward;
            }
            else if (direction == Directions.onSamePos)
            {
                return Directions.onSamePos;
            }
            return Directions.count;
        }

        public static Vector3 DirectionToVector(this Directions direction)
        {
            if (direction == Directions.left)
            {
                return Vector3.left;
            }
            else if (direction == Directions.right)
            {
                return Vector3.right;
            }
            else if (direction == Directions.up)
            {
                return Vector3.up;
            }
            else if (direction == Directions.down)
            {
                return Vector3.down;
            }
            else if (direction == Directions.forward)
            {
                return Vector3.forward;
            }
            else if (direction == Directions.back)
            {
                return Vector3.back;
            }
            else if (direction == Directions.onSamePos)
            {
                return Vector3.zero;
            }
            return Vector3.zero;
        }
        #endregion

        #region VECTOR_RELATED
        public static Vector3 ConvertToXZero(this Vector3 pos)
        {
            return pos + (Vector3.right * (0 - pos.x));
        }

        public static Vector3 ConvertToYZero(this Vector3 pos)
        {
            return pos + (Vector3.up * (0 - pos.y));
        }

        public static Vector3 ConvertToZZero(this Vector3 pos)
        {
            return pos + (Vector3.forward * (0 - pos.z));
        }

        public static Vector3 SetX(this Vector3 pos, float xValue)
        {
            return pos.ConvertToXZero() + (Vector3.right * xValue);
        }

        public static Vector3 SetY(this Vector3 pos, float yValue)
        {
            return pos.ConvertToYZero() + (Vector3.up * yValue);
        }

        public static Vector3 SetZ(this Vector3 pos, float zValue)
        {
            return pos.ConvertToZZero() + (Vector3.forward * zValue);
        }
        #endregion

        #region LIST_RELATED
        public static List<T> SuffleList<T>(this List<T> list)
        {
            List<int> indexRemaining = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                indexRemaining.Add(i);
            }

            List<T> temp = new List<T>();
            int selectedIndex;
            for (int i = 0; i < list.Count; i++)
            {
                selectedIndex = Random.Range(0, indexRemaining.Count);
                temp.Add(list[indexRemaining[selectedIndex]]);
                indexRemaining.RemoveAt(selectedIndex);
            }
            return temp;
        }

        public static void ShiftList<T>(this List<T> list, int oldIndex, int newIndex)
        {
            List<T> tList = new List<T>(list.Count);
            int index = newIndex;

            /*for (int i = ; i < tList.Count; i++)
            {

            }*/
        }
        #endregion

        #region RECTTRANSFORM_RELATED
        public static Vector2 RectPosToWorldPos(this RectTransform rectTransform, Camera camera){
            return camera.ScreenToWorldPoint(RectTransformUtility.WorldToScreenPoint(camera, rectTransform.transform.position));
        }

        public static bool IsRectPartiallyInScreenView(this RectTransform rectTransform)
        {
            Rect screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            foreach (Vector3 corner in objectCorners)
            {
                if (screenRect.Contains(corner))
                {
                    return true;
                }
            }
            return false;
        }



        public static bool IsRectContainsPoint(this RectTransform rectTransform, Vector2 objScreenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, objScreenPoint);
        }
        #endregion

            #region ACTION_IN_FUNS_RELATED
            public static void CheckNullAndInvoke(this ActionInFuns actionInFuns)
        {
            if (actionInFuns != null)
            {
                actionInFuns();
            }
        }

        public static void Clear(this ActionInFuns actionInFuns)
        {
            actionInFuns = null;
        }
        #endregion


        #region CLASS_RELATED
        public static void CopyAllTo<T>(this T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
            foreach (var sourceField in type.GetFields())
            {
                var targetField = type.GetField(sourceField.Name);
                targetField.SetValue(target, sourceField.GetValue(source));
            }

        }
        #endregion

        #region TIME_RELATED
        public static string FloatToTime(this float toConvert, string format)
        {
            int hours = (int)(toConvert / 3600);
            switch (format)
            {
                case "00.0":
                    return string.Format("{0:00}:{1:0}",
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 10) % 10));//miliseconds
                case "#0.0":
                    return string.Format("{0:#0}:{1:0}",
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 10) % 10));//miliseconds
                case "00.00":
                    return string.Format("{0:00}:{1:00}",
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 100) % 100));//miliseconds
                case "00.000":
                    return string.Format("{0:00}:{1:000}",
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                case "#00.000":
                    return string.Format("{0:#00}:{1:000}",
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                case "#0:00":
                    return string.Format("{0:#0}:{1:00}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60);//seconds
                case "#00:00":
                    return string.Format("{0:#00}:{1:00}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60);//seconds
                case "0:00.0":
                    return string.Format("{0:0}:{1:00}.{2:0}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 10) % 10));//miliseconds
                case "#0:00.0":
                    return string.Format("{0:#0}:{1:00}.{2:0}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 10) % 10));//miliseconds
                case "0:00.00":
                    return string.Format("{0:0}:{1:00}.{2:00}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 100) % 100));//miliseconds
                case "#0:00.00":
                    return string.Format("{0:#0}:{1:00}.{2:00}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 100) % 100));//miliseconds
                case "0:00.000":
                    return string.Format("{0:0}:{1:00}.{2:000}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                case "#0:00.000":
                    return string.Format("{0:#0}:{1:00}.{2:000}",
                         Mathf.Floor(toConvert / 60),//minutes
                         Mathf.Floor(toConvert) % 60,//seconds
                         Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                case "00:00:00":
                    return System.String.Format("{0}:{1:00}:{2:00}", hours, Mathf.Floor((toConvert / 60f) %60), Mathf.Floor(toConvert) % 60);
            }
        
            return "error";
        }

        public static string FloatToTime(this double toConvert, string format)
        {
            return ((float)toConvert).FloatToTime(format);
        }
        #endregion


        #region TEXTURE_TO_CONVERSIONS
        public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
            float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                                  incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }

        //public static string TextureToBase64Key(this Texture2D mytexture)
        //{
        //    byte[] bytes;
        //    bytes = mytexture.EncodeToPNG();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        bf.Serialize(ms, mytexture);
        //        bytes = ms.ToArray();
        //    }

        //    string enc = Convert.ToBase64String(bytes);
        //}
        #endregion
    }
}


