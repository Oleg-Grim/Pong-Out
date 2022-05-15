using DG.Tweening;
using ME.ECS;
using UnityEngine;
using UnityEngine.UI;

public class UICore : MonoBehaviour
{
   public GlobalEvent TimeSynced;
   public Image _background;

   private void Start()
   {
      TimeSynced.Subscribe(FadeBackground);
   }

   private void FadeBackground(in Entity entity)
   {
      _background.DOFade(0, 2f);
   }

   private void OnDestroy()
   {
      TimeSynced.Unsubscribe(FadeBackground);
   }
}
