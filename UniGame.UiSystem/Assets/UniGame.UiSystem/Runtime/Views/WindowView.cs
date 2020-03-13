namespace UniGame.UiSystem.Runtime
{
    using System.Diagnostics;
    using Abstracts;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniUiSystem.Runtime.Utils;
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class WindowView<TWindowModel> : 
        UiView<TWindowModel> 
        where TWindowModel : class, IViewModel
    {
        #region inspector
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        [SerializeField] protected CanvasGroup canvasGroup;
        
#if ODIN_INSPECTOR   
        [Sirenix.OdinInspector.FoldoutGroup(nameof(canvasGroup),false)]  
#endif
        [SerializeField]
        private CanvasGroupState hiddenState = new CanvasGroupState() {
            Alpha         = 0,
            BlockRaycasts = false,
            Interactable  = false
        };
                
#if ODIN_INSPECTOR   
        [Sirenix.OdinInspector.FoldoutGroup(nameof(canvasGroup),false)]  
#endif 
        [SerializeField]
        private CanvasGroupState visibleState = new CanvasGroupState() {
            Alpha         = 1,
            BlockRaycasts = true,
            Interactable  = true
        };
        
        #endregion
        
        protected sealed override void OnInitialize(TWindowModel model)
        {
            IsActive.Where(x => x).
                Subscribe(x => canvasGroup.SetState(visibleState)).
                AddTo(LifeTime);
            
            IsActive.Where(x => !x).
                Subscribe(x => canvasGroup.SetState(hiddenState)).
                AddTo(LifeTime);

            OnWindowInitialize(model);
        }

        protected virtual void OnWindowInitialize(TWindowModel model){}

        protected void Awake()
        {
            canvasGroup = canvasGroup == null ? 
                GetComponent<CanvasGroup>() : 
                canvasGroup;
        }
        
#region editor only     
        
        [Conditional("UNITY_EDITOR")]
        protected void OnValidate()
        {
            canvasGroup = canvasGroup == null ? 
                GetComponent<CanvasGroup>() : 
                canvasGroup;
        }
        
#endregion
    }
}