# MpesaLib
MPESA API LIBRARY For C# Developers

* This Library is work in progress.
* Pull requests are accepted!
* Library is based on .NET Standard 2.0
* Will be deployed as a NUGET Package in the near future...
* This is a pet project, and is fully open source

Explore All existing MPESA APIs and how to generate your API Keys at Daraja - [Safaricom's Developer Portal](https://developer.safaricom.co.ke/apis-explorer)

## Note
* Access tokens generated by the [Authclient](https://github.com/ayiemba/MpesaLib/blob/master/src/MpesaLib/Clients/AuthClient.cs) expire after an hour (this is documented in Daraja). Users of this library should ensure they handle token expriration in their code. An quick solution would be to check for 403 errors then request a new token to use with the other apis.
