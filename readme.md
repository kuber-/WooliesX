# Deploy 

`dotnet publish`
`cp src/Api/app.yaml src/Api/bin/Debug/netcoreapp2.1/publish/`
`gcloud app deploy bin/Debug/netcoreapp2.1/publish/app.yaml`

# Api tests

## Command line

* `npm install -g newman`
* `newman run WooliesX_Answers_Api.postman_collection.json`

## In postman

* Import WooliesX_Answers_Api.postman_collection.json
* Run collection in the Runner.

# Dev

## Watch Web

`dotnet watch --project src/Api/Api.csproj run`
