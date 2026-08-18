Precisa da DLL para rodar o executavel

> Comando usado no VS para geração
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```