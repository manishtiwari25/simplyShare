generateQRUsingText = function(key){
    var selectedText = sessionStorage.getItem(key);
    createQR(selectedText);
}

createQR = function(selectedText){
    new QRCode("qrcode", {
        text: selectedText,
        width: 256,
        height: 256,
        colorDark : "#000000",
        colorLight : "#ffffff",
        correctLevel : QRCode.CorrectLevel.H
    });
}