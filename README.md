# simplyShare
Hassel Free Text Transfer (PC &lt;--> Mobile)

This project has 4 component.
1. chrome extension
2. back end API/signalR hub
3. Android Application
4. Azure services

Why I tried to create this app.
I usually switch my reading work from mobile to pc or vice versa, but there isn't any way to send a URL from pc to phone. 
so for solving this problem, I created this app, before that I used whats app for the same purpose.

How this works->
- on the android app, it will ask to log in, for that, I have used azure AD B2c. 
- after log it, the app will scan a QR code. 
- on chrome, right-click on the text you want to send, select simpyShare, it will generate a QR code, scan it from the phone, and done...

The same process for Phone to PC, on pc open simplyShare chrome extension, it will generate a QR code, scan it, and send the clipboard data.

you can say, it's a small version of Microsoft Phone companion app. 
