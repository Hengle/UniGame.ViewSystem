﻿
namespace UniGame.UiSystem.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstracts;
    using UniGreenModules.UniGame.UiSystem.Runtime;
    using UnityEngine;

    public class ViewStackLayoutsContainer : IViewLayoutContainer
    {

        private IDictionary<ViewType, IViewLayout> _viewControllers;

        private IViewLayout _dummyController;


        public ViewStackLayoutsContainer(IDictionary<ViewType, IViewLayout> layoutMap)
        {
            _viewControllers = layoutMap;
            //empty object controller
            _dummyController = new ViewLayout();
        }


        public IReadOnlyViewLayout this[ViewType type] => GetLayout(type);

        
        public TView Get<TView>()  where TView : Component, IView
        {
            foreach (var viewLayout in _viewControllers) {
                var layout = viewLayout.Value;
                var view = layout.Get<TView>();
                if (view) return view;
            }

            return null;
        }

        public IEnumerable<IViewLayout> Controllers => _viewControllers.Values;

        public IViewLayout GetLayout(ViewType type)
        {
            return _viewControllers.TryGetValue(type, out var controller) ? 
                controller : 
                _dummyController;
        }

    }
}