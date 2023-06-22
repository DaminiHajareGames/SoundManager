using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FP11.LudoGame
{
    public class ButtonSoundGenerator : MonoBehaviour
    {
        public Transform parentOfAllHirarchyObj;

        public void GenrateSoundForGenralButton()
        {
            SoundManager.instance.GenrateSound(SoundManager.SoundTypes.genralButtonSound);
        }

#if UNITY_EDITOR
        [ContextMenu("GiveButtonSoundEffectToAllButton")]
         void GiveButtonSoundEffectToAllButton()
         {
             FindAllObjectsInChild(parentOfAllHirarchyObj);

             parentOfAllHirarchyObj = null;
         }


         void FindAllObjectsInChild(Transform parent)
         {
             GameObject obj;
             for (int i = 0; i < parent.childCount; i++)
             {
                 obj = parent.GetChild(i).gameObject;
                 if (obj.GetComponent<Button>() != null)
                 {
                     UnityEditor.Events.UnityEventTools.AddPersistentListener(obj.GetComponent<Button>().onClick, GenrateSoundForGenralButton);
                 }

                 if (obj.transform.childCount != 0)
                 {
                     FindAllObjectsInChild(obj.transform);
                 }
             }
         }
#endif
    }
}
