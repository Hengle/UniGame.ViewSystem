﻿namespace UniGame.UiSystem.Runtime.Abstracts
{
    using System.Collections.Generic;
    using UniGreenModules.UniGame.UiSystem.Runtime;

    public interface IViewLayoutContainer : IReadOnlyViewLayoutContainer
    {
        IEnumerable<IViewLayout> Controllers { get; }

        /// <summary>
        /// get controller of target view type 
        /// </summary>
        IViewLayout GetLayout(ViewType type);
    }
}