# Version Update Checklist

Perform the following tasks when publishing a new version of *Dan's ShopTools Desktop*.

 - [ ] Update **ReadMe.docx**, if necessary. Close the document and run Scripts/CreateReadMe.cmd to update README.md.
 - [ ] Make sure the project is in **Debug** mode.
 - [ ] Update the version number in **ShopTools.csproj**.
 - [ ] Compile and test all changes to the application.
 - [ ] Check the GitHub project online to make sure any issues to be addressed in this version have been completed.
 - [ ] Switch the project mode to **Release** and compile.
 - [ ] Open **Scripts/ShopToolsDocumentation.shfbproj** and update the version number in **HelpFileVersion**.
 - [ ] Open **Scripts/ShopToolsDocumentation.shfbproj** in SHFB and compile the new version of the SDK.
 - [ ] Check in SDK documentation on **danielanywhere.github.io**. Use the summary 'ShopTools SDK updates for version {Version}'.
 - [ ] Check in source changes on **danielanywhere/ShopTools**. Use the summary 'Updates for version {Version}'.
 - [ ] Update or close any associated GitHub issues.

