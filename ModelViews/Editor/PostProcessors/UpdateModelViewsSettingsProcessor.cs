﻿namespace UniGame.UiSystem.ModelViews.Editor.PostProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModelViewsMap.Runtime.Settings;
    using UiSystem.Runtime;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.Core.EditorTools.Editor.AssetOperations;
    using UniModules.UniGame.UISystem.Runtime.Abstract;
    using UnityEditor;

    public class UpdateModelViewsSettingsProcessor : AssetModificationProcessor
    {
        private const string cacheKey = "";
        
        public static Func<object,List<ModelViewsModuleSettings>> settingsCache = MemorizeTool.
            Create<object,List<ModelViewsModuleSettings>>(x => AssetEditorTools.
            GetAssets<ModelViewsModuleSettings>());
        
        public static string[] OnWillSaveAssets(string[] paths)
        {
            var settingsAssets = settingsCache(cacheKey);
            
            foreach (var asset in settingsAssets) {
                if(!ValidateTarget(asset,paths))
                    continue;
                Rebuild(asset);
            }
            
            return paths;
        }        
        
        [MenuItem("UniGame/View System/Rebuild ModelsViewsSettings")]
        public static void Rebuild()
        {
            var settings = AssetEditorTools.
                GetAssets<ModelViewsModuleSettings>();
            
            foreach (var setting in settings) {
                Rebuild(setting);
                EditorUtility.SetDirty(setting);
            }
        }
        
        public static void Rebuild(ModelViewsModuleSettings settings)
        {
            settings.CleanUp();
            
            var baseViewType  = typeof(IUiView<>);
            var baseModelType = typeof(IViewModel);
            
            var modelTypes = baseModelType.GetAssignableTypes();
            var typeArs    = new Type[1];
            //get all views
            foreach (var modelType in modelTypes) {
                typeArs[0] = modelType;
                var targetType = baseViewType.MakeGenericType(typeArs);
                var viewTypes  = targetType.GetAssignableTypes();

                settings.UpdateValue(modelType,viewTypes);

            }
            
            EditorUtility.SetDirty(settings);
        }

        private static bool ValidateTarget(ModelViewsModuleSettings asset, string[] paths)
        {
            if (!asset || asset.isRebuildActive == false) return false;
            if (asset.updateTargets.Count == 0) return true;
            
            foreach (var targetPath in asset.updateTargets) {
                if (paths.Any(x => x.IndexOf(targetPath, StringComparison.OrdinalIgnoreCase) >= 0)) {
                    return true;
                }
            }

            return false;
        }

    }
}
