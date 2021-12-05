# CSVParser
Generates Report CSV files by parsing and filtering source CSV files.

The file path in App.config to source, destination and Log folder must be changed a per local machine setup.

<configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" /> -- log folder path
</configSections>
  
  <appSettings>
		<add key="SourceFolderPath" value="C:\Users\Fenil\CSV_Parser\CSVParse\Files\"/> -- Source folder path
		<add key="ReportsFolderPath" value="C:\Users\Fenil\CSV_Parser\CSVParse\Reports\"/> -- Destination/Report folder path
	</appSettings>
