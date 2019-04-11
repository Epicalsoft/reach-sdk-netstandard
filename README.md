# Reach Api Client SDK for .NET Standard
The Reach Api Client SDK allows you to integrate Reach into your .NET Standard libraries.

Learn more about about the provided samples, documentation, integrating the SDK into your app, accessing source code.

## Getting Started
* Contact [hello@epicalsoft.com](mailto:hello@epicalsoft.com) to create your Reach app.
* Receive your **Client Id** and **Client Secret** and store them in a safe place.
* Initialize the client using

```csharp
ReachClient.Init("[clientId]", "[clientSecret]");
```

## Installation
#### Package Manager
```
PM > Install-Package Epicalsoft.Reach.Api.Client.Net -Version 1.3.12.49
```
#### .NET CLI
```
> dotnet add package Epicalsoft.Reach.Api.Client.Net --version 1.3.12.49
```

## Usage
### 1. `GlobalScopeManager.GetClassifications()`
#### Invocation
```csharp
var classifications = await GlobalScopeManager.Instance.GetClassifications();
```
#### Response example
```javascript
[
  {
    "Code": "300301",
    "Title": "Theft",
    "Lang": "en"
  },
  {
    "Code": "30030101",
    "Title": "Robbery to people",
    "Lang": "en"
  },
  {
    "Code": "30030102",
    "Title": "Theft of vehicles or auto parts",
    "Lang": "en"
  }
]
```

### 2. `GlobalScopeManager.GetRoadTypes()`
#### Invocation
```csharp
var roadTypes = await GlobalScopeManager.Instance.GetRoadTypes();
```
#### Response example
```javascript
[
  {
    "Code": 11,
    "Name": "Bridge"
  },
  {
    "Code": 7,
    "Name": "Commercial place"
  },
  ...
]
```

### 3. `GlobalScopeManager.GetCountries()`
#### Invocation
```csharp
var countries = await GlobalScopeManager.Instance.GetCountries();
```
#### Response example
```javascript
[
  {
    "Name": "Perú",
    "Alpha2Code": "PE",
    "Alpha3Code": "PER",
    "NumericCode": 604,
    "CallingCode": 51,
    "Culture": "es-PE"
  },
  {
    "Name": "México",
    "Alpha2Code": "MX",
    "Alpha3Code": "MEX",
    "NumericCode": 484,
    "CallingCode": 52,
    "Culture": "es-MX"
  }
]
```

### 4. `GlobalScopeManager.GetNearbyIncidents(double lat, double lng, ClassificationGroup group)`
#### Invocation
* **ClassificationGroup** Medical Incidents, Public Protection, Human Security, Public Administration
```csharp
var nearbyIncidents = await GlobalScopeManager.Instance.GetNearbyIncidents(-12.051299, -77.064956, ClassificationGroup.HumanSecurity);
```
#### Response example
```javascript
[
  {
    "Id": 18052014,
    "Kind": "30030101",
    "Lat": -12.0697313823956,
    "Lng": -77.053617797792,
    "UTC": "2017-10-26T23:36:29.753"
  }
]
```

### 5. `GlobalScopeManager.GetIncidentDetail(int id)`
#### Invocation
```csharp
var incident = await GlobalScopeManager.Instance.GetIncidentDetail(18052014);
```
#### Response example
```javascript
{
  "Id": 18052014,
  "UserId": 1630,
  "Description": "Something is happening! Winter is coming!",
  "Thumbnail": null,
  "Abstract": "Something is happening! Winter is... - https://reachsos.com/incidents/details/18052014",
  "Latitude": -12.0697313823956,
  "Longitude": -77.053617797792,
  "UTC": "2018-08-17T23:36:29.753",
  "HasEvidence": false,
  "HighlightsCount": 11,
  "CommentsCount": 7,
  "AlertedCount": 13,
  "RoadType": {
    "Code": 0,
    "Name": "Public road"
  },
  "Classification": {
    "Code": "30030101",
    "Name": "Robbery to people",
    "Lang": "en"
  },
  "Nickname": "Atreyu",
  "Trusted": true,
  "Verified": false
}
```

### 6. `GlobalScopeManager.VerifyFaces(VerifyFacesRequest verifyFacesRequest)`
#### Invocation
```csharp
var verifyFacesRequest = new VerifyFacesRequest { Data = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsL..." };
```
* **Data** must contain the byte array of an image (preferably .jpg up to 1024x1024) in Base64String.
```csharp
var facesVerificationResult = await GlobalScopeManager.Instance.VerifyFaces(verifyFacesRequest);
```
#### Response example
```javascript
{
  "Status": 1,
  "Matches": [
    {
      "Confidence": 0.6149,
      "IncidentId": 122018
    },
    {
      "Confidence": 0.5827,
      "IncidentId": 93710
    }
  ],
  "FacesCount": 3
}
```
* **Status** may contain the following values:
  * OK (1) indicates that no errors occurred and at least one result was returned.
  * ZERO_RESULTS (2)  indicates that the search was successful but returned no results.
  * OVER_QUERY_LIMIT (3) indicates that you are over your quota.
  * INVALID_REQUEST(4) indicates that the data value is wrong or missing.
  * UNKNOWN_ERROR(5)  a server-side error.
* **FacesCount** refers to the number of detected faces in the image

### 7. `GlobalScopeManager.SendSOSAlert(SOSAlert alert)`
#### Invocation
```csharp
var sosAlert = new SOSAlert
{
  Sender = new User {
    Trusted = true,
    FullName = "Reach",
    Nickname = "Citizen Security Social Network",
    CountryCode = "51",
    PhoneNumber = "999999999",
    IDN = "11111111"
  },
  CNC = 604,
  Location = new GPSLocation {
    Lat = -12.115032,
    Lng = -77.046044,
    UTC = DateTime.UtcNow
  }
};
```
* **Trusted** refers if the person is trusted.
* **IDN** refers to the Identity Document Number of the person.
* **CNC** refers to the Country Number Code, check `GlobalScopeManager.GetCountries()`
```csharp
await GlobalScopeManager.Instance.SendSOSAlert(sosAlert);
```

## Prerequisites
* NETStandard.Library (>= 2.0.3)
* akavache (>= 6.5.1)
* Newtonsoft.Json (>= 12.0.1)

## Contact
If you need help installing or using the library, please contact Reach Support at hello@epicalsoft.com Reach's Support staff are well-versed in all of the Reach Libraries, and usually reply within 24 hours.

Please report any bugs as issues.
Follow @reachsos on Twitter and /reachsos on Facebook for updates.

## License
Copyright 2018 Epicalsoft Corporation.

Licensed under the Apache License: http://www.apache.org/licenses/LICENSE-2.0
