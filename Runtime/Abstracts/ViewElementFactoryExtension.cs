﻿namespace UniGame.UiSystem.Runtime
{
    using Abstracts;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UnityEngine;

    public static class ViewElementFactoryExtension
    {
        
        public static async UniTask<T> Create<T>( 
            this IViewElementFactory factory,
            IViewModel viewModel,
            ILifeTime lifeTime,
            Transform parent = null,
            bool stayWoorldPosition = false) where T : class, IView
        {
            var view = await factory.Create(viewModel, typeof(T), string.Empty, parent, string.Empty,stayWoorldPosition) as T;
            lifeTime.AddCleanUpAction(() => view?.Close());
            return view;
        }
        
        public static async UniTask<T> Create<T>( 
            this IViewElementFactory factory,
            IViewModel viewModel,
            ILifeTime lifeTime,
            string skinTag = "",
            Transform parent = null,
            string viewName = "", 
            bool stayWorld = false) where T : class, IView
        {
            var view = await factory.Create(viewModel, typeof(T), skinTag, parent, viewName,stayWorld) as T;
            lifeTime.AddCleanUpAction(() => view?.Close());
            return view;
        }
        
        public static async UniTask<T> Create<T>( 
            this IViewElementFactory factory,
            IViewModel viewModel,
            string skinTag = "",
            Transform parent = null,
            string viewName = null) where T : class, IView
        {
            var view = await factory.Create(viewModel, typeof(T), skinTag, parent, viewName) as T;
            return view;
        }
    }
}