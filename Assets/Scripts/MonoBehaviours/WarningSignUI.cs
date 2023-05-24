using UnityEngine;

public class WarningSignUI : MonoBehaviour {
   [SerializeField] private Animator animator;

   private const string IS_FLICKERING_ANIMATION = "IsFlickering";
   private bool isWarning;
   public void SetWarning(bool enabled) {
      if (enabled && !isWarning) {
         animator.SetBool(IS_FLICKERING_ANIMATION, true);
      } else if (!enabled && isWarning) {
         animator.SetBool(IS_FLICKERING_ANIMATION, false);
      }
   }

}
