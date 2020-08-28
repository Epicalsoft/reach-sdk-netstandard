# Reach Api Client SDK for .NET Standard
The Reach Api Client SDK allows you to integrate Reach into your .NET Standard libraries.

Learn more about about the provided samples, documentation, integrating the SDK into your app, accessing source code.

## Getting Started
* Contact [hello@epicalsoft.com](mailto:hello@epicalsoft.com) to create your Reach app.
* Receive your **Client Id** and **Client Secret** and store them in a safe place.
* Initialize the client using

```csharp
ReachClient.Init("[clientId]", "[clientSecret]");
//or
ReachClient.Init("[userKey]");
//or
ReachClient.Init("[clientId]", "[clientSecret]", "[username]", "[password]");
```

## Installation
#### Package Manager
```
PM > Install-Package Epicalsoft.Reach.Api.Client.Net -Version 1.3.22.0
```
#### .NET CLI
```
> dotnet add package Epicalsoft.Reach.Api.Client.Net --version 1.3.22.0
```

## Usage
### 1. `GlobalScopeManager.GetClassificationsAsync()`
#### Invocation
```csharp
var classifications = await GlobalScopeManager.Instance.GetClassificationsAsync();
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

### 2. `GlobalScopeManager.GetRoadTypesAsync()`
#### Invocation
```csharp
var roadTypes = await GlobalScopeManager.Instance.GetRoadTypesAsync();
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

### 3. `GlobalScopeManager.GetCountriesAsync()`
#### Invocation
```csharp
var countries = await GlobalScopeManager.Instance.GetCountriesAsync();
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

### 4. `GlobalScopeManager.GetNearbyIncidentsAsync(double, double, ClassificationGroup)`
#### Invocation
* **ClassificationGroup** Medical Incidents, Public Protection, Human Security, Public Administration
```csharp
var nearbyIncidents = await GlobalScopeManager.Instance.GetNearbyIncidentsAsync(-12.051299, -77.064956, ClassificationGroup.HumanSecurity);
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

### 5. `GlobalScopeManager.GetIncidentDetailAsync(int)`
#### Invocation
```csharp
var incident = await GlobalScopeManager.Instance.GetIncidentDetailAsync(18052014);
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
  "FCP": 2,
  "Evidences": []
}
```

### 6. `GlobalScopeManager.VerifyFacesAsync(VerifyFacesRequest)`
#### Invocation
```csharp
var verifyFacesRequest = new VerifyFacesRequest {
	Data = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsL..."
};

var facesVerificationResult = await GlobalScopeManager.Instance.VerifyFacesAsync(verifyFacesRequest);
```
* **Data** must contain the byte array of an image (preferably .jpg up to 4096x4096) in Base64String.

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
  * Ok (1) indicates that no errors occurred and at least one result was returned.
  * ZeroResults (2)  indicates that the search was successful but returned no results.
  * OverQueryLimit (3) indicates that you are over your quota.
  * InvalidRequest (4) indicates that the data value is wrong or missing.
  * UnknownError (5)  a server-side error.
* **FacesCount** refers to the number of detected faces in the image

### 7. `GlobalScopeManager.SendSOSAlertAsync(SOSAlert)`
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

await GlobalScopeManager.Instance.SendSOSAlertAsync(sosAlert);
```
* **Trusted** refers if the person is trusted.
* **IDN** refers to the Identity Document Number of the person.
* **CNC** refers to the Country Number Code, check `GlobalScopeManager.GetCountriesAsync()`

### 8. `UserScopeManager.UploadMediaFileAsync(MediaFileData)`
#### Invocation
```csharp
var mediaFileData = new MediaFileData
{
    Code = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
    Target = MediaFileTarget.Evidences,
    Kind = MediaFileKind.Image,
    Data = Convert.ToBase64String(byteArray),
    Filename = "incident_evidence_001.jpg"
};

var result = await UserScopeManager.Instance.UploadMediaFileAsync(mediaFileData);
```
* **Target** may contain the following values:
  * Evidences (1) indicates that operation corresponds to an incident's evidence upload.
* **Kind** may contain the following values:
  * Image (1) indicates that media file data corresponds to an image.

#### Response example
```javascript
{
  "MediaFileId": 1,
  "DataCode": "3F2504E0-4F89-11D3-9A0C-0305E82C3301"
}
```

### 9. `GlobalScopeManager.GetReverseGeocode(double, double)`
#### Invocation
```csharp
var address = await GlobalScopeManager.Instance.GetReverseGeocode(lat, lng);
```

## Prerequisites
* NETStandard.Library (>= 2.0.3)
* Newtonsoft.Json (>= 12.0.2)

## Contact
If you need help installing or using the library, please contact Reach Support at hello@epicalsoft.com Reach's Support staff are well-versed in all of the Reach Libraries, and usually reply within 24 hours.

Please report any bugs as issues.
Follow @reachsos on Twitter and /reachsos on Facebook for updates.

## License
Copyright 2019 Epicalsoft Corporation.

Licensed under the Apache License: http://www.apache.org/licenses/LICENSE-2.0
