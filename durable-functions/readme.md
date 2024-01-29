# Content
This is a repository containing sample Azure durable functions, that can be used as reference for creating your own functions.
The function has http trigger that kicks off orchestrator. As input argument (Body of the http request) it takes a json file with the following structure:
```json
{
    "websites": [
        {
            "name": "website1",
            "url": "https://www.website1.com"
        },
        {
            "name": "website2",
            "url": "https://www.website2.com"
        }
    ]
}
```

Then functions runs activity for each of the websites, downloads it's content and saves it to the blob storage.

# Setup
This can be used entirely in local dev machine, levreging the Azure Storage Emulator.

## Prerequisites
- Install Azure Extension for VS Code (https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- Install Azurite VS Code extension (https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)
- Launch Azurite (VS Code -> View -> Command Palette -> Azurite: Start)
- Launch Azurite Blob Service (Ctrl + Shift + P -> Azurite Blob Service: Start)
- Launch Azurite Queue Service (Ctrl + Shift + P -> Azurite Queue Service: Start)
- Launch function, either from VS Code or from Azure Functions Core Tools or run (func start)
- Create output container in Azurite Blob Service `webpages`(Right click on Blob Containers in Azure extension)
- Execute http request to kick off the orchestrator. Use url that `func start` provides (default: http://localhost:7071/api/PdfFactoryOrch_HttpStart)

Function will be triggered and will create a file for each of the websites provided in the http request body.

## Deploy to Azure
- Create a storage account in Azure
- Create a function app in Azure
- Create output container in the storage account `webpages`
- Set the following environment variables in the function app
  - `AzureWebJobsStorage` - connection string to the storage account
- Deploy the function app to Azure

