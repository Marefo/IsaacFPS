using System;
using _CodeBase.Logging;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.UI
{
  public class LoadingCurtain : MonoBehaviour
  {
    public event Action BecameInvisible;
		
		public bool Invisible { get; private set; }
		public bool Active => _curtain.gameObject.activeSelf;
		
		[SerializeField] private CanvasGroup _curtain;

		private string _defaultFieldValue;
		private Coroutine _loadingEffectCoroutine;
		private Tweener _fadeIn;
		private Tweener _fadeOut;

		private void Awake() => Hide();

		public void Show()
		{
			Invisible = false;
			_curtain.gameObject.SetActive(true);
			_curtain.alpha = 1;
		}
    
		public void Hide()
		{
			Invisible = true;
			BecameInvisible?.Invoke();
			
			_curtain.gameObject.SetActive(false);
		}

		public void FadeInAndOut(float duration = 1, Action onFadeInComplete = null) => 
			FadeIn(duration / 2, () => FadeOutWithAction(duration / 2, onFadeInComplete));

		public void FadeOutWithAction(float duration = 1, Action action = null)
		{
			action?.Invoke();
			FadeOut(duration);
		}
		
		public void FadeIn(float duration = 1, Action onComplete = null)
		{
			_fadeOut?.Kill();
			_curtain.alpha = 0;
			_curtain.gameObject.SetActive(true);

			_fadeIn = _curtain.DOFade(1, duration)
				.OnComplete(() => OnFadeInComplete(onComplete)).SetLink(gameObject);
		}

		public void FadeOut(float duration = 1, Action onComplete = null)
		{
			_fadeIn?.Kill();
			_curtain.alpha = 1;
			
			_fadeOut = _curtain.DOFade(0, duration)
				.OnUpdate(OnFadeOutUpdate)
				.OnComplete(() => OnFadeOutComplete(onComplete)).SetLink(gameObject);
		}

		private void OnFadeInComplete(Action onComplete = null)
		{
			Invisible = false;
			onComplete?.Invoke();
		}

		private void OnFadeOutUpdate()
		{
			if (Invisible == false && _curtain.alpha <= 0.8f)
			{
				Invisible = true;
				BecameInvisible?.Invoke();
			}
		}

		private void OnFadeOutComplete(Action callback = null)
		{
			callback?.Invoke();
			_curtain.gameObject.SetActive(false);
		}
  }
}