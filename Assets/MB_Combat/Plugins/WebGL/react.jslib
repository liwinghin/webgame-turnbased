var unityPlugin = {
    RequestCombatStart: function()
    {
        window.dispatchReactUnityEvent("RequestCombatStart");
    },
    RequestCombatClose: function(message)
    {
        window.dispatchReactUnityEvent("RequestCombatClose", Pointer_stringify(message));
    }
};

mergeInto(LibraryManager.library, unityPlugin);