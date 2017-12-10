# Reach SDK for .NET Standard
This open-source library allows you to integrate Reach into your .NET Standard libraries.

Learn more about about the provided samples, documentation, integrating the SDK into your app, accessing source code.

## Getting Started
* Contact [hello@epicalsoft.com](mailto:hello@epicalsoft.com) to create your Reach app.
* Receive your **Client Id** and **Client Secret** and store them in a safe place.
* Initialize the client using `var reachClient = new ReachClient("[ClientId]", "[ClientSecret]");`

## Usage
### 1. Get Incident Types - `GlobalContext.GetIncidentTypes()`
#### Invocation
```csharp
var incidentTypes = await reachClient.GlobalContext.GetIncidentTypes();
```
#### Response
```javascript
[
  {
    "Code": 118,
    "Name": "Abuse of authority",
    "GroupId": 1,
    "IconUrl": "https://reachsos.com/assets/icons/incidents/ic_abuso.png"
  },
  ...
]
```

### 2. Get Road Types - `GlobalContext.GetRoadTypes()`
#### Invocation
```csharp
var roadTypes = await reachClient.GlobalContext.GetRoadTypes();
```
#### Response
```javascript
[
  {
    "Code": 11,
    "Name": "Bridge"
  },
  ...
]
```

### 3. Get Countries - `GlobalContext.GetCountries()`
#### Invocation
```csharp
var countries = await reachClient.GlobalContext.GetCountries();
```
#### Response
```javascript
[
  {
    "Name": "Afghanistan",
    "Alpha3": "AFG",
    "CNC": 4
  },
  ...
]
```

### 4. Get Nearby Incidents - `GlobalContext.GetNearbyIncidents(double lat, double lng, byte groupId)`
#### Invocation
* **GroupId** 1: Security, 2: Public Incidents, 3: Civil Protection, 4: Medical Incidents
```csharp
var nearbyIncidents = await reachClient.GlobalContext.GetNearbyIncidents(-12.051299, -77.064956, 1);
```
#### Response
```javascript
[
  {
    "Id": 270609,
    "Type": 121,
    "Lat": -12.0697313823956,
    "Lng": -77.053617797792,
    "UTC": "2017-10-26T23:36:29.753"
  },
  ...
]
```

### 5. Get Incident Detail - `GlobalContext.GetIncidentDetail(int id)`
#### Invocation
```csharp
var incident = await reachClient.GlobalContext.GetIncidentDetail(270609);
```
#### Response
```javascript
{
  "Id": 270609,
  "UserId": 1630,
  "Description": "Something is happening! Winter is coming!",
  "Thumbnail": null,
  "Abstract": "Something is happening! Winter is...",
  "Latitude": -12.0697313823956,
  "Longitude": -77.053617797792,
  "UTC": "2017-10-26T23:36:29.753",
  "HasEvidence": false,
  "HighlightsCount": 27,
  "CommentsCount": 6,
  "InterventionsCount": 9,
  "RoadType": {
    "Code": 0,
    "Name": "Public road"
  },
  "IncidentType": {
    "Code": 103,
    "Name": "Kidnapping"
  },
  "Nickname": "Tsaheylu"
}
```

### 6. Verify Suspect Face - `GlobalContext.VerifySuspect(Face face)`
#### Invocation
```javascript
{
  "Data": "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsL...",
  "HasLocation": false
}
```
* **Data** must contain the byte array of an image (preferably .jpg up to 1024x1024) in Base64String.
```csharp
var suspectVerifyResult = await reachClient.GlobalContext.VerifySuspect(face);
```
#### Response
```javascript
{
  "IsSuspect": true,
  "Confidence": 0.53689,
  "IncidentId": 270609
}
```

### 7. Register SOS Alert - `GlobalContext.RegisterSOSAlert(SOSAlert alert)`
#### Invocation
```javascript
{
  "Sender": {
    "Trusted": true,
    "FullName": "Reach",
    "Nickname": "Citizen Security Social Network",
    "CountryCode": "51",
    "PhoneNumber": "999999999",
    "IDN": "11111111"
  },
  "CNC": 604,
  "Location": {
    "Lat": -12.115032,
    "Lng": -77.046044,
    "UTC": "2017-10-26T23:36:29.753"
  }
}
```
* **Trusted** refers if the person is trusted.
* **IDN** refers to the Identity Document Number of the person.
* **CNC** refers to the Country Number Code, check `GlobalContext.GetCountries()`
```csharp
await reachClient.GlobalContext.RegisterSOSAlert(alert);
```

## Prerequisites
* Newtonsoft.Json >= 10.0.3

## Contact
If you need help installing or using the library, please contact Reach Support at hello@epicalsoft.com Reach's Support staff are well-versed in all of the Reach Libraries, and usually reply within 24 hours.

Please report any bugs as issues.
Follow @reachsos on Twitter and /reachsos on Facebook for updates.

## License
Copyright 2017 Epicalsoft, Inc.

Licensed under the Apache License: http://www.apache.org/licenses/LICENSE-2.0
