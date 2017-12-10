# Reach SDK for .NET Standard
This open-source library allows you to integrate Reach into your .NET Standard libraries.

Learn more about about the provided samples, documentation, integrating the SDK into your app, accessing source code.

## Getting Started
* Contact [hello@epicalsoft.com](mailto:hello@epicalsoft.com) to create your Reach app.
* Receive your **Client Id** and **Client Secret** and store them in a safe place.
* Initialize the client using `var reachClient = new ReachClient("[ClientId]", "[ClientSecret]");`

## Usage
### Get Nearby Incidents
```csharp
GlobalContext.GetNearbyIncidents(double lat, double lng, byte groupId)
```
* **lat** Latitude
* **lng** Longitude
* **groupId** 1: Security, 2: Public Incidents, 3: Civil Protection, 4: Medical Incidents
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
  {
    "Id": 221212,
    "Type": 108,
    "Lat": -12.0526251642248,
    "Lng": -77.0815892145038,
    "UTC": "2017-10-27T15:50:04.023"
  }
]
```

### Get Incident Detail
```csharp
GlobalContext.GetIncidentDetail(int id)
```
* **id** Incident Id
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
  "HighlightsCount": 0,
  "CommentsCount": 0,
  "InterventionsCount": 0,
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

## Prerequisites
* NETStandard.Library >= 2.0.0
* Newtonsoft.Json >= 10.0.3

## Contact
If you need help installing or using the library, please contact Reach Support at hello@epicalsoft.com Reach's Support staff are well-versed in all of the Reach Libraries, and usually reply within 24 hours.

Please report any bugs as issues.
Follow @reachsos on Twitter and /reachsos on Facebook for updates.

## License
Copyright 2017 Epicalsoft, Inc.

Licensed under the Apache License: http://www.apache.org/licenses/LICENSE-2.0
