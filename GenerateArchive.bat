copy /y SolastaMods\bin\Release\net481\SolastaMods.dll SolastaMods.dll
copy /y SolastaMods\bin\Release\net481\SolastaMods.pdb SolastaMods.pdb
copy /y SolastaMods\Info.json Info.json

tar.exe -acf SolastaMods.zip SolastaMods.dll SolastaMods.pdb Info.json

del SolastaMods.dll
del SolastaMods.pdb
del Info.json

..\UnityModManagerInstaller\UnityModManager.exe