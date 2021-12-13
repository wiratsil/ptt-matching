var plugin = {
    OpenInTab : function(url)
    {
        url = Pointer_stringify(url);
        window.open(url,'_self');
    },
};
mergeInto(LibraryManager.library, plugin);