# Cliente SDK Reach Api para .NET Standard

El Cliente SDK API de Reach te permite integrar Reach en tus librerías .NEt Standard

Aprende más sobre los ejemplos proporcionados, la documentación, la integración del SDK en su aplicación y el acceso al código fuente.

## Para Empezar
* Contacto [hello@epicalsoft.com](mailto:hello@epicalsoft.com) para crear tu Reach app.
* Reciba su **Client Id** y **Cliente Secret** y guárdelos en un lugar seguro.
* Inicializar el cliente utilizando `var reachClient = new ReachClient("[ClientId]", "[ClientSecret]");`

## Instalación
#### Nuget Package Manager
```
PM > Install-Package Epicalsoft.Reach.Api.Client.Net -Version 1.3.9.24
```
#### .NET CLI
```
> dotnet add package Epicalsoft.Reach.Api.Client.Net --version 1.3.9.24
```

## Uso
### 1. Obtener Clasificaciones (Get Classifications) - `GlobalContext.GetClassifications()`
#### Invocación
```csharp
var classifications = await reachClient.GlobalContext.GetClassifications();
```
#### Response
```javascript
[
  {
    "Code": "30030101",
    "Title": "Robo a personas",
    "Lang": "es"
  },
  ...
]
```

### 2. Obtener tipos de caminos (Get Road Types) - `GlobalContext.GetRoadTypes()`
#### Invocación
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

### 3. Obtener Países (Get Countries) - `GlobalContext.GetCountries()`
#### Invocación
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

### 4. Incidentes cercanos (Get Nearby Incidents) - `GlobalContext.GetNearbyIncidents(double lat, double lng, byte groupId)`
#### Invocación
* **GroupId** 1: Security, 2: Public Incidents, 3: Civil Protection, 4: Medical Incidents
```csharp
var nearbyIncidents = await reachClient.GlobalContext.GetNearbyIncidents(-12.051299, -77.064956, 1);
```
#### Response
```javascript
[
  {
    "Id": 270609,
    "Kind": "30030101",
    "Lat": -12.0697313823956,
    "Lng": -77.053617797792,
    "UTC": "2017-10-26T23:36:29.753"
  },
  ...
]
```

### 5. Obtener Detalles del Incidente (Get Incident Detail) - `GlobalContext.GetIncidentDetail(int id)`
#### Invocación
```csharp
var incident = await reachClient.GlobalContext.GetIncidentDetail(270609);
```
#### Response
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
  "HighlightsCount": 27,
  "CommentsCount": 6,
  "AlertedCount": 9,
  "RoadType": {
    "Code": 0,
    "Name": "Public road"
  },
  "Classification": {
    "Code": "30030101",
    "Name": "Robo a personas",
    "Lang": "es"
  },
  "Nickname": "Atreyu",
  "Trusted": true,
  "Verified": false
}
```

### 6. Verificar Caras (Verify Faces) - `GlobalContext.VerifyFaces(VerifyFacesRequest verifyFacesRequest)`
#### Invocación
```javascript
{
  "Data": "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsL..."
}
```
* **Data** debe contener la matriz de bytes de una imagen (preferiblemente .jpg hasta 1024x1024) in Base64String.
```csharp
var facesVerificationResult = await reachClient.GlobalContext.VerifyFaces(verifyFacesRequest);
```
#### Response
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

### 7. Registrar Alerta SOS (Register SOS Alert) - `GlobalContext.RegisterSOSAlert(SOSAlert alert)`
#### Invocación
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

## Prerequisitos
* NETStandard.Library >= 2.0.3
* Newtonsoft.Json >= 11.0.2

## Contacto
Si necesita ayuda para instalar o utilizar la biblioteca, póngase en contacto con el servicio de asistencia técnica de Reach en hello@epicalsoft.com El personal de asistencia técnica de Reach conoce bien todas las bibliotecas de Reach y, por lo general, responde en un plazo de 24 horas.

Por favor, informe de cualquier error como problema.
Sigue a @reachsos en Twitter y /reachsos en Facebook para actualizaciones.

## Licencia
Copyright 2017 Epicalsoft, Inc.

Licenciado bajo la licencia de Apache: http://www.apache.org/licenses/LICENSE-2.0
