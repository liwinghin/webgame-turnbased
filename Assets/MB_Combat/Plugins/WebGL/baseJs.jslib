var unityPlugin = {
    RequestCombatStart: function()
    {
        CallCombatStart();
    },
    RequestCombatClose: function(message)
    {
        CallCombatClose(Pointer_stringify(message));
    }
};

mergeInto(LibraryManager.library, unityPlugin);