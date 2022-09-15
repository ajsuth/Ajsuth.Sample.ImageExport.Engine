# Sitecore Experience Commerce Sellable Item Image Export Plugin
This is sample plugin to export Sitecore Commerce catalog images and either downloaded into the local file system or uploaded into Azure Storage.

## Supported Sitecore Experience Commerce Versions
- XC 10.1

## Features

* Export sellable item images to local folder
* Export sellable item images to Azure Storage

## Running the Export

The postman collection can be imported from the solution's `/postman` folder. The **_Export Catalog Images_** API uses the following manual configurations to control the exporter functionality.

### Image Settings

The image settings manages behaviour of the product feed generator.

#### Image Settings Example

```
"imageSettings": {
    "@odata.type": "Ajsuth.Sample.ImageExport.Engine.Policies.ImageExportPolicy",
    "ExportStandaloneProducts": true,
    "ExportProductsWithVariants": true,
    "UploadImages": true,
    "OverrideExistingImages": false,
    "LocalImageLocation" : "C:\\Images\\Habitat",
    "ImageConfigurations" : [
        {
            "TargetSize": 300,
            "FileNameSuffix": ""
        },
        {
            "TargetSize": 100,
            "FileNameSuffix": "-s"
        }
    ]
}
```

#### Image Settings Properties

* **`ExportStandaloneProducts`:** When true, sellable items without variants will have their images exported.
* **`ExportProductsWithVariants`:** When true, sellable items with variants will have their images exported.
* **`UploadImages`:** If enabled, the images will be uploaded to Azure Storage, otherwise images will be downloaded to the local file system.
* **`LocalImageLocation`:** The folder path of the local file system where images will be downloaded.
* **`ImageConfigurations`:** An array of image configurations to transform images when uploading to Azure Storage.
  * **`TargetSize`:** The size of image, resized prior to uploading to Azure Storage. If left null, the image will not be resized.
  * **`FileNameSuffix`:** The suffix appended to the image file name to ensure unique files are uploaded.

### Azure Storage Settings

The Azure Storage settings holds the configuration for Azure Storage.

#### Azure Storage Settings Example

```
"azureStorageSettings": {
    "@odata.type": "Ajsuth.Sample.ImageExport.Engine.Policies.AzureStoragePolicy",
    "ConnectionString": "",
    "BaseUrl": "",
    "Container": ""
}
```

#### Azure Storage Settings Properties

* **`ConnectionString`:** The connection string to the storage account.
* **`BaseUrl`:** The product image urls are generated using the following format, `<baseUrl><container>/<sellable item friendly id}/<image file name>`.
* **`Container`:** The product image urls are generated using the following format, `<baseUrl><container>/<sellable item friendly id}/<image file name>`.

## Installation Instructions
1. Download the repository.
2. Add the **Ajsuth.Sample.ImageExport.Engine.csproj** to the _**Sitecore Commerce Engine**_ solution.
3. In the _**Sitecore Commerce Engine**_ project, add a reference to the **Ajsuth.Sample.ImageExport.Engine** project.
4. Run the _**Sitecore Commerce Engine**_ from Visual Studio or deploy the solution and run from IIS.
5. Run the Bootstrap command on the _**Sitecore Commerce Engine**_.

## Known Issues
| Feature                 | Description | Issue |
| ----------------------- | ----------- | ----- |
|                         |             |       |

## Disclaimer
The code provided in this repository is sample code only. It is not intended for production usage and not endorsed by Sitecore.
Both Sitecore and the code author do not take responsibility for any issues caused as a result of using this code.
No guarantee or warranty is provided and code must be used at own risk.
