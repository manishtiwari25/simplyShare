const box = "<li><div class='comment-main-level'><div class='comment-box'><div class='comment-head'><span>{0}</span></div><div class='comment-content'>{1}</div></div></div></li>"

const qr = document.getElementById("scan");
const maincontainer = document.getElementById("comments-list");
const divcontainer = document.getElementById("divcontainer");

document.addEventListener('DOMContentLoaded', function() {
  document.getElementById("sendData").addEventListener("click", sendData);
});

if (!String.prototype.format) {
    String.prototype.format = function() {
      var args = arguments;
      return this.replace(/{(\d+)}/g, function(match, number) { 
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
      });
    };
}

chrome.identity.getAuthToken({ 'interactive': true }, function(token) {
  chrome.storage.sync.set({token: token}, function() {
    console.log('token is set  ' + token);
  });
});

chrome.storage.sync.get(['token'], function(result) {
  localStorage.setItem('googleAccessToken',result.token);
});


  const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:44358/usermessagehub",{ accessTokenFactory: () => localStorage.getItem('googleAccessToken') })
  .configureLogging(signalR.LogLevel.Error)
  .build();

  connection.start().then(function () {
    console.log("connected");
    connection.invoke('getConnectionId')
        .then(function (connectionId) {
            sessionStorage.setItem('conectionId', connectionId);
            generateQRUsingText("conectionId");
        }).catch(err => console.error(err.toString()));
    });

    connection.on("Negotiate", function (message) {
      var signalRMessage = JSON.parse(message);
      if(signalRMessage.IsNegotiation)
      {
          sessionStorage.setItem('mobileConectionId', signalRMessage.Message);
          qr.hidden = true;
          divcontainer.hidden = false;
      }
  });
  
  connection.on("Send", function (message) {
      var signalRMessage = JSON.parse(message);
      console.log(signalRMessage);
      if(signalRMessage.Flow === "Incoming"){
          var newMsg = box.format(new Date(),signalRMessage.Message);
          maincontainer.innerHTML += newMsg;
      }    
  });  

function sendData(){
    const textArea = document.getElementById("sendSomething");
    var value = textArea.value;
    var connectionId =sessionStorage.getItem('mobileConectionId');
    var signalRMessage = JSON.stringify(createSignalRMessage(value));
    connection.invoke("Send", connectionId, signalRMessage).catch(err => {debugger;console.error(err.toString());});
}

function createSignalRMessage(message){
    var signalRMessage = {IsNegotiation : false, Message : message, Type : "String", DateTime :new Date(),Flow:"OutGoing" };
    return signalRMessage;
}

