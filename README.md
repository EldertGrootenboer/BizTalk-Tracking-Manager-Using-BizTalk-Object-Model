# BizTalk Tracking Manager Using BizTalk Object Model
Anyone who has used the BizTalk Server Administration console to update their tracking settings knows how tedious this becomes if we have check or update multiple applications. This tool uses the BizTalk Object Model to allow mass updating of the BizTalk tracking settings. The BizTalk Tracking Manager allows us to get a quick overview of tracking for all our applications, and update these settings in bulk.

## Description
This sample uses the BizTalk Object Model to retrieve all the applications installed, and retrieves the tracking settings for ports, orchestrations and pipelines. Before starting the application, make sure to update the connectionstring for your environment in the config file.

```XML
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
    <configSections> 
    </configSections> 
    <connectionStrings> 
        <add name="BizTalk_Tracking_Manager.Properties.Settings.BizTalkManagmentDatabaseConnectionString" 
            connectionString="Server=localhost;Database=BizTalkMgmtDb;Integrated Security=SSPI;" /> 
    </connectionStrings> 
    <startup>  
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" /> 
    </startup> 
</configuration>
```

The BizTalk Object Model makes working with our BizTalk objects very easy, which makes it easy for us in making tools like this.

```C#
/// <summary>  
/// Catalog explorer for working with BizTalk objects.  
/// </summary>  
private static BtsCatalogExplorer _btsCatalogExplorer;  
 
# region Caching  
 
private static ApplicationCollection _applications;  
private static IEnumerable<BtsOrchestration> _orchestrations;  
private static IEnumerable<Microsoft.BizTalk.ExplorerOM.ReceivePort> _receivePorts;  
private static IEnumerable<Microsoft.BizTalk.ExplorerOM.SendPort> _sendPorts;  
private static IEnumerable<Microsoft.BizTalk.ExplorerOM.Pipeline> _pipelines;  
 
#endregion  
 
/// <summary>  
/// Constructor.  
/// </summary>  
public BizTalkObjectModelHelper()  
{  
    _btsCatalogExplorer = new BtsCatalogExplorer { ConnectionString = Settings.Default.BizTalkManagmentDatabaseConnectionString };  
}  
 
/// <summary>  
/// Get all BizTalk applications.  
/// </summary>  
internal ApplicationCollection GetApplications()  
{  
    return _applications ?? (_applications = _btsCatalogExplorer.Applications);  
}  
 
/// <summary>  
/// Save changes to the catalog explorer.  
/// </summary>  
public void SaveChanges()  
{  
    _btsCatalogExplorer.SaveChanges();  
}  
 
/// <summary>  
/// Get orchestrations for all applications.  
/// </summary>  
public IEnumerable<BtsOrchestration> GetOrchestrations()  
{  
    return _orchestrations ?? (_orchestrations = GetApplications().Cast<Application>().SelectMany(application => application.Orchestrations.Cast<BtsOrchestration>()));  
}  
 
/// <summary>  
/// Get receive ports for all applications.  
/// </summary>  
public IEnumerable<Microsoft.BizTalk.ExplorerOM.ReceivePort> GetReceivePorts()  
{  
    return _receivePorts ?? (_receivePorts = GetApplications().Cast<Application>().SelectMany(application => application.ReceivePorts.Cast<Microsoft.BizTalk.ExplorerOM.ReceivePort>()));  
}  
  
/// <summary>  
/// Get send ports for all applications.  
/// </summary>  
public IEnumerable<Microsoft.BizTalk.ExplorerOM.SendPort> GetSendPorts()  
{  
    return _sendPorts ?? (_sendPorts = GetApplications().Cast<Application>().SelectMany(application => application.SendPorts.Cast<Microsoft.BizTalk.ExplorerOM.SendPort>()));  
}  
  
/// <summary>  
/// Get pipelines for all applications.  
/// </summary>  
public IEnumerable<Microsoft.BizTalk.ExplorerOM.Pipeline> GetPipelines()  
{  
    return _pipelines ?? (_pipelines = GetApplications().Cast<Application>().SelectMany(application => application.Pipelines.Cast<Microsoft.BizTalk.ExplorerOM.Pipeline>()));  
}
```

The settings are displayed and can be changed one at a time (by clicking the individual check boxes), per application (by clicking the row header) or per tracking setting (by clicking the column header). The changed tracking settings can be easily identified by the color of the fields. One-way ports are also recognized and the unused tracking settings disabled.

![](https://i1.code.msdn.s-msft.com/biztalk-tracking-manager-33bce27c/image/file/152507/1/capture.png)

Once all tracking has been set as required it can be saved by clicking the Save button, which will again use the BizTalk Object Model to save the changes.

```C#
case ArtifactType.Orchestration: 
// Set no tracking 
OrchestrationTrackingTypes orchestrationTracking = 0; 
 
// Loop through checkbox cells which have been checked 
foreach (var cell in row.Cells.Cast<DataGridViewCell>().Where(cell => cell.ColumnIndex != 0)) 
{ 
    // Check if cell is ticked 
    if (Convert.ToBoolean(cell.Value)) 
    { 
        // Used to parse tracking 
        OrchestrationTrackingTypes columnTracking; 
 
        // Parse tracking 
        Enum.TryParse(cell.OwningColumn.Name, out columnTracking); 
 
        // Update tracking 
        orchestrationTracking = orchestrationTracking | columnTracking; 
    } 
 
    // Reset cell 
    SetCellNotEdited(cell); 
    cell.Tag = null; 
}
 
// Queue tracking to be saved 
_controller.QueueOrchestrationTracking(row.Cells[0].Value.ToString(), orchestrationTracking); 
break;
```

```C#
/// <summary> 
/// Queue saving tracking to the specified artifact. 
/// </summary> 
public void QueueOrchestrationTracking(string orchestrationName, OrchestrationTrackingTypes tracking) 
{ 
    _bizTalkObjectModelHelper.GetOrchestrations().First(orchestration => orchestration.FullName == orchestrationName).Tracking = tracking; 
}
```
