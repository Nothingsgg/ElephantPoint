# ElephantPoint

ElephantPoint is a C# module that will use SharePoint token to search and download files.  

Article URL: https://www.lrqa.com/en/insights/articles/elephantpoint-a-sharepoint-enumeration-tool/

## Usage
```
Usage: ElephantPoint.exe [options]
    /help            :  show this menu
    /SPO_url         :  Sharepoint URL ex: expensiverabbit.sharepoint.com
    /file_url        :  will be obtained from the results of the first example
    /save_file       :  save file location and name
    /token           :  access token for SharePoint, can be obtained by ManaCloud
    /max_row         :  number of rows for results (optional)
    /fql             :  takes no args, changes the format (optional)
    /query_file      :  name of file you are looking for
    /ref_filter      :  filtering results (optional)

Example:
    ElephantPoint.exe /query_file:passwords.txt /SPO_url:expensiverabbit.sharepoint.com /token:ey...
    ElephantPoint.exe /file_url:https://expensiverabbit.sharepoint.com/.../passwords.txt /save_file:C:\Users\public\test.txt /token:ey...
    ElephantPoint.exe /file_url:https://expensiverabbit.sharepoint.com/.../passwords.txt /b64 /token:ey...
```

## Credits  
Code is based on the following repo:
- https://github.com/nheiniger/SnaffPoint 
