

## C2B Register Urls Request
```c#
var RegisterC2BUrlObject = new CustomerToBusinessRegisterUrlDto(
	"https://blablabala/api/confirm",
	"https://blablabala/api/validate",
	"Cancelled",
	"603047"
);

var c2bRegisterUrlrequest = await _mpesaClient.RegisterC2BUrlAsync(RegisterC2BUrlObject, accesstoken, RequestEndPoint.RegisterC2BUrl);
```

## C2B Payment Request
```c#
//C2B Object
Var CustomerToBusinessSimulateObject = new CustomerToBusinessSimulateDto
(
	"603047",
	"10",
	"account",
	"254708374149",
	"CustomerPayBillOnline"
);

var c2brequest = await _mpesaClient.MakeC2BPaymentAsync(CustomerToBusinessSimulateObject, accesstoken, RequestEndPoint.CustomerToBusinessSimulate);
```
