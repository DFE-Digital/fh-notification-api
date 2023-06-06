# fh-notification-api
Provides a standardised api for notifications

## Requirements

* DotNet Core 7.0 and any supported IDE for DEV running.

## About

This API provide a standard interface to send email and other notifications to users

## Local running

In the appsetting.json file you set the Database to be Sql Server or SQLite


The startup project is: FamilyHubs.Notification.Api
Starting the API will then show the swagger definition with the available operations.

## Migrations

To Add Migration

<br />
 dotnet ef migrations add CreateIntialSchema --project ..\FamilyHubs.Notification.Data\FamilyHubs.Notification.Data.csproj
<br />

To Apply Latest Schema Manually

<br />
 dotnet ef database update --project ..\FamilyHubs.Notification.Data\FamilyHubs.Notification.Data.csproj
<br />

## cypress tests
Run the API (debug or non debug both fine)
open powershell at ..\fh-notification-api\tests\cypress

<br />
 npx cypress open 
<br />