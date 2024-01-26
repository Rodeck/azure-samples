# Content
This is a repository containing sample Azure functions, that can be used as reference for creating your own functions.
The function has blob trigger and blob output, so can be used as any kind of blob processing function.

# Setup
This can be used entirely in local dev machine, levreging the Azure Storage Emulator.

## Prerequisites
- Install Azure Extension for VS Code (https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- Install Azurite VS Code extension (https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)
- Launch Azurite (VS Code -> View -> Command Palette -> Azurite: Start)
- Launch Azurite Blob Service (Ctrl + Shift + P -> Azurite Blob Service: Start)
- Launch Azurite Queue Service (Ctrl + Shift + P -> Azurite Queue Service: Start)
- Launch function, either from VS Code or from Azure Functions Core Tools or run (func start)
- Create input and oputput containers in Azurite Blob Service `pdf-templates` and `pdf-templates-results` (Right click on Blob Containers in Azure extension)
- Upload any pdf file to the input container (`pdf-templates`) in Azurite Blob Service (Note: right-click on the container and select upload, do not copy file directly from opened explorer, it will not work)

Function will be triggered and will create a parsed json file in the output container (`pdf-templates-results`)

## Deploy to Azure
- Create a storage account in Azure
- Create a function app in Azure
- Create input and oputput containers in the storage account `pdf-templates` and `pdf-templates-results`
- Set the following environment variables in the function app
  - `AzureWebJobsStorage` - connection string to the storage account
- Deploy the function app to Azure

