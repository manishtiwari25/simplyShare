chrome.contextMenus.create({
        id: "simplyReceive",
        title: "simplyShare",
        contexts:["all"],  // ContextType
        onclick: connect // A callback function
});

function connect(){
    chrome.tabs.create({
        url: chrome.extension.getURL('../../html/connect.html'),
        active: false
    });
};
